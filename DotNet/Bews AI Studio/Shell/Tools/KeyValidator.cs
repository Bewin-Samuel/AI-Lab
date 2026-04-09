using Shell.Common;
using Shell.Core;
using System.Net.Http.Headers;

namespace Shell.Tools
{
    public partial class KeyValidator : BaseForm
    {
        private static readonly HttpClient HttpClient = new();
        private readonly ModelProvider _modelProvider = new();
        private List<ModelInfo> _allModels = [];
        private IReadOnlyList<ModelInfo> _loadedModels = [];

        public KeyValidator()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadTypeDropdown();
        }

        private void LoadTypeDropdown()
        {
            cmbKeyType.Items.Clear();

            cmbKeyType.Items.Add("Azure Open AI");
            cmbKeyType.Items.Add("Open Router");
            cmbKeyType.Items.Add("Open AI");
            cmbKeyType.Items.Add("Hugging Face");
            cmbKeyType.Items.Add("Gemini");
            cmbKeyType.Items.Add("GitHub");

            cmbKeyType.Sorted = true;

            cmbKeyType.SelectedIndex = 0;
        }

        private void ClearKeyDetails()
        {
            lblKeyDetails.Text = string.Empty;
            dgvModels.Rows.Clear();
            _allModels.Clear();
            cmbModels.Items.Clear();
            _loadedModels = [];
        }

        private async void OnKeyValidateClick(object? sender, EventArgs e)
        {
            ClearKeyDetails();

            var key = txtKey.Text;

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Please enter a key.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKey.Focus();
                return;
            }

            var keyType = cmbKeyType.SelectedItem?.ToString() ?? string.Empty;
            btnValidate.Enabled = false;
            Cursor = Cursors.WaitCursor;

            bool isValid;

            try
            {
                isValid = await _modelProvider.ValidateKeyAsync(keyType, key, txtBaseUrl.Text.Trim());
            }
            finally
            {
                btnValidate.Enabled = true;
                Cursor = Cursors.Default;
            }

            MessageBox.Show(
                isValid ? "Key validated successfully." : "Key validation failed.",
                "Validation",
                MessageBoxButtons.OK,
                isValid ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        private async void OnKeyDetailsClick(object sender, EventArgs e)
        {
            ClearKeyDetails();
            var key = txtKey.Text;

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Please enter a key.", "Key Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKey.Focus();
                return;
            }

            var keyType = cmbKeyType.SelectedItem?.ToString() ?? string.Empty;

            btnKeyDetails.Enabled = false;
            btnValidate.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var details = await GetKeyDetailsAsync(keyType, key.Trim());
                lblKeyDetails.Text = BuildKeyDetailsText(details);
            }
            catch
            {
                lblKeyDetails.Text = "Unable to fetch key details.";
            }
            finally
            {
                btnKeyDetails.Enabled = true;
                btnValidate.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private static async Task<KeyDetailsInfo> GetKeyDetailsAsync(string keyType, string key)
        {
            return keyType switch
            {
                "Open Router" => await GetOpenRouterKeyDetailsAsync(key),
                "Open AI" => await GetOpenAiKeyDetailsAsync(key),
                "Hugging Face" => await GetHuggingFaceKeyDetailsAsync(key),
                "Gemini" => await GetGeminiKeyDetailsAsync(key),
                "GitHub" => await GetGitHubKeyDetailsAsync(key),
                _ => new KeyDetailsInfo()
            };
        }

        private static async Task<KeyDetailsInfo> GetOpenAiKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/models");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            details.Active = response.IsSuccessStatusCode;

            if (!response.IsSuccessStatusCode)
            {
                return details;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);

            if (document.RootElement.TryGetProperty("data", out var models) &&
                models.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                details.TotalModelsAccessible = models.GetArrayLength();
            }

            return details;
        }

        private static async Task<KeyDetailsInfo> GetOpenRouterKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://openrouter.ai/api/v1/auth/key"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                using var response = await HttpClient.SendAsync(request);
                details.Active = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
                    if (document.RootElement.TryGetProperty("data", out var data))
                    {
                        var usage = GetDecimalProperty(data, "usage");
                        var limit = GetDecimalProperty(data, "limit");
                        if (usage.HasValue && limit.HasValue)
                        {
                            details.AvailableCredits = limit.Value - usage.Value;
                        }

                        details.CreatedDate = GetDateProperty(data, "created_at");
                        details.ExpiryDate = GetDateProperty(data, "expiry") ?? GetDateProperty(data, "expires_at");
                    }
                }
            }

            using (var modelRequest = new HttpRequestMessage(HttpMethod.Get, "https://openrouter.ai/api/v1/models"))
            {
                modelRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
                using var modelResponse = await HttpClient.SendAsync(modelRequest);

                if (modelResponse.IsSuccessStatusCode)
                {
                    using var stream = await modelResponse.Content.ReadAsStreamAsync();
                    using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);

                    if (document.RootElement.TryGetProperty("data", out var models) &&
                        models.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        var totalModels = 0;
                        var totalFreeModels = 0;

                        foreach (var model in models.EnumerateArray())
                        {
                            totalModels++;

                            if (model.TryGetProperty("pricing", out var pricing))
                            {
                                var prompt = GetDecimalProperty(pricing, "prompt");
                                var completion = GetDecimalProperty(pricing, "completion");

                                if (prompt.GetValueOrDefault() <= 0m && completion.GetValueOrDefault() <= 0m)
                                {
                                    totalFreeModels++;
                                }
                            }
                        }

                        details.TotalModelsAccessible = totalModels;
                        details.TotalFreeModels = totalFreeModels;
                    }
                }
            }

            return details;
        }

        private static async Task<KeyDetailsInfo> GetGeminiKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models?key={Uri.EscapeDataString(key)}";
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            using var response = await HttpClient.SendAsync(request);

            details.Active = response.IsSuccessStatusCode;

            if (!response.IsSuccessStatusCode)
            {
                return details;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);

            if (document.RootElement.TryGetProperty("models", out var models) &&
                models.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                details.TotalModelsAccessible = models.GetArrayLength();
            }

            return details;
        }

        private static async Task<KeyDetailsInfo> GetHuggingFaceKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://huggingface.co/api/whoami-v2");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            details.Active = response.IsSuccessStatusCode;
            return details;
        }

        private static async Task<KeyDetailsInfo> GetGitHubKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", key);
            request.Headers.UserAgent.ParseAdd("BewsAIStudio/1.0");
            request.Headers.Accept.ParseAdd("application/vnd.github+json");

            using var response = await HttpClient.SendAsync(request);
            details.Active = response.IsSuccessStatusCode;

            if (response.Headers.TryGetValues("github-authentication-token-expiration", out var values))
            {
                var expiry = values.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(expiry) && DateTimeOffset.TryParse(expiry, out var expiryDate))
                {
                    details.ExpiryDate = expiryDate.DateTime;
                }
            }

            return details;
        }

        private static string BuildKeyDetailsText(KeyDetailsInfo details)
        {
            return string.Join(Environment.NewLine, new[]
            {
                $"• Active = {details.Active}",
                $"• Key Created Date: {FormatDate(details.CreatedDate)}",
                $"• Key Expiry Date: {FormatDate(details.ExpiryDate)}",
                $"• Available Credits: {FormatDecimal(details.AvailableCredits)}",
                $"• Total Number of Models Accessible by this Key: {FormatInt(details.TotalModelsAccessible)}",
                $"• Total Number of Free Models: {FormatInt(details.TotalFreeModels)}"
            });
        }

        private static string FormatDate(DateTime? date) => date?.ToString("dd-MMM-yy") ?? "N/A";

        private static string FormatDecimal(decimal? value) => value?.ToString("0.##") ?? "N/A";

        private static string FormatInt(int? value) => value?.ToString() ?? "N/A";

        private static decimal? GetDecimalProperty(System.Text.Json.JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var value))
            {
                return null;
            }

            if (value.ValueKind == System.Text.Json.JsonValueKind.Number && value.TryGetDecimal(out var numberValue))
            {
                return numberValue;
            }

            if (value.ValueKind == System.Text.Json.JsonValueKind.String &&
                decimal.TryParse(value.GetString(), out var stringValue))
            {
                return stringValue;
            }

            return null;
        }

        private static DateTime? GetDateProperty(System.Text.Json.JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var value) || value.ValueKind != System.Text.Json.JsonValueKind.String)
            {
                return null;
            }

            var stringValue = value.GetString();
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return null;
            }

            if (DateTimeOffset.TryParse(stringValue, out var dateValue))
            {
                return dateValue.DateTime;
            }

            return null;
        }

        private sealed class KeyDetailsInfo
        {
            public bool Active { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public decimal? AvailableCredits { get; set; }
            public int? TotalModelsAccessible { get; set; }
            public int? TotalFreeModels { get; set; }
        }

        private async void OnModelDetailsClick(object sender, EventArgs e)
        {
            var key = txtKey.Text;

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Please enter a key.", "Model Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKey.Focus();
                return;
            }

            var keyType = cmbKeyType.SelectedItem?.ToString() ?? string.Empty;

            btnModelDetails.Enabled = false;
            btnLoadModels.Enabled = false;
            btnKeyDetails.Enabled = false;
            btnValidate.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                InitializeModelGridColumns();
                _allModels = [.. await _modelProvider.GetModelsAsync(keyType, key.Trim())];
                ApplyModelFilter();
            }
            catch
            {
                _allModels = [];
                dgvModels.Rows.Clear();
            }
            finally
            {
                btnModelDetails.Enabled = true;
                btnLoadModels.Enabled = true;
                btnKeyDetails.Enabled = true;
                btnValidate.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void InitializeModelGridColumns()
        {
            if (dgvModels.Columns.Count > 0)
            {
                return;
            }

            dgvModels.AutoGenerateColumns = false;
            dgvModels.AllowUserToAddRows = false;
            dgvModels.RowHeadersVisible = false;

            dgvModels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSNo",
                HeaderText = "S.No",
                Width = 50,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });

            dgvModels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelName",
                HeaderText = "Model Name",
                Width = 120,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });

            dgvModels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelId",
                HeaderText = "Model ID",
                Width = 170,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });

            dgvModels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colModelType",
                HeaderText = "Model Type",
                Width = 80,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });

            dgvModels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colParams",
                HeaderText = "Params",
                Width = 90,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });
        }

        private void OnProviderChanged(object? sender, EventArgs e)
        {
            txtKey.Clear();
            txtBaseUrl.Clear();
            ClearKeyDetails();
        }

        private async void OnLoadModelsClick(object? sender, EventArgs e)
        {
            var key = txtKey.Text;

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("Please enter a key.", "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKey.Focus();
                return;
            }

            var provider = cmbKeyType.SelectedItem?.ToString() ?? string.Empty;

            SetAllButtonsEnabled(false);
            Cursor = Cursors.WaitCursor;

            try
            {
                var isValid = await _modelProvider.ValidateKeyAsync(provider, key.Trim(), txtBaseUrl.Text.Trim());

                if (!isValid)
                {
                    MessageBox.Show("Key validation failed. Please check your key and try again.",
                        "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _loadedModels = await _modelProvider.GetModelsAsync(provider, key.Trim(), txtBaseUrl.Text.Trim());
                PopulateModelsDropdown(_loadedModels);
            }
            catch
            {
                MessageBox.Show("Unable to load models.", "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetAllButtonsEnabled(true);
                Cursor = Cursors.Default;
            }
        }

        private void PopulateModelsDropdown(IReadOnlyList<ModelInfo> models)
        {
            cmbModels.Items.Clear();
            foreach (var model in models)
                cmbModels.Items.Add(model.DisplayName);

            if (cmbModels.Items.Count > 0)
                cmbModels.SelectedIndex = 0;
        }

        private void SetAllButtonsEnabled(bool enabled)
        {
            btnValidate.Enabled      = enabled;
            btnKeyDetails.Enabled    = enabled;
            btnModelDetails.Enabled  = enabled;
            btnLoadModels.Enabled    = enabled;
        }

        private void OnModelSearchTextChanged(object? sender, EventArgs e)
        {
            ApplyModelFilter();
        }

        private void ApplyModelFilter()
        {
            var query = txtModelSearch.Text.Trim();
            IEnumerable<ModelInfo> filtered = _allModels;

            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = _allModels.Where(model =>
                    ContainsIgnoreCase(model.DisplayName, query) ||
                    ContainsIgnoreCase(model.Id, query) ||
                    ContainsIgnoreCase(model.ModelType, query) ||
                    ContainsIgnoreCase(model.Params, query));
            }

            BindModelGrid(filtered.ToList());
        }

        private void BindModelGrid(List<ModelInfo> models)
        {
            dgvModels.Rows.Clear();

            for (var i = 0; i < models.Count; i++)
            {
                var model = models[i];
                dgvModels.Rows.Add(i + 1, model.DisplayName, model.Id, model.ModelType, model.Params);
            }
        }

        private static bool ContainsIgnoreCase(string source, string value) =>
            source.Contains(value, StringComparison.OrdinalIgnoreCase);

    }
}
