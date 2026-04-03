namespace Shell.Tools
{
    public partial class KevValidator : Form
    {
        private static readonly HttpClient HttpClient = new();
        private List<ModelDetailInfo> _allModels = new();

        public KevValidator()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            FormInCenterOfMdi();
            LoadTypeDropdown();
        }

        private void FormInCenterOfMdi()
        {
            if (this.MdiParent != null)
            {
                int x = (this.MdiParent.ClientSize.Width - this.Width) / 2;
                int y = (this.MdiParent.ClientSize.Height - this.Height) / 2;
                this.Location = new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
            }
        }

        private void LoadTypeDropdown()
        {
            cmbKeyType.Items.Clear();

            cmbKeyType.Items.Add("Open Router");
            cmbKeyType.Items.Add("Open AI");
            cmbKeyType.Items.Add("Hugging Face");
            cmbKeyType.Items.Add("Gemini");
            cmbKeyType.Items.Add("GitHub");

            cmbKeyType.Sorted = true;

            cmbKeyType.SelectedIndex = 0;
        }

        private async void OnKeyValidateClick(object? sender, EventArgs e)
        {
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
                isValid = await ValidateKeyAsync(keyType, key);
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

        private static async Task<bool> ValidateKeyAsync(string keyType, string key)
        {
            var normalizedKey = key.Trim();

            if (string.IsNullOrWhiteSpace(normalizedKey))
            {
                return false;
            }

            return keyType switch
            {
                "Open Router" => await ValidateOpenRouterKeyAsync(normalizedKey),
                "Open AI" => await ValidateOpenAiKeyAsync(normalizedKey),
                "Hugging Face" => await ValidateHuggingFaceKeyAsync(normalizedKey),
                "Gemini" => await ValidateGeminiKeyAsync(normalizedKey),
                "GitHub" => await ValidateGitHubKeyAsync(normalizedKey),
                _ => false
            };
        }

        private static async Task<bool> ValidateOpenAiKeyAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/models");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<bool> ValidateOpenRouterKeyAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://openrouter.ai/api/v1/auth/key");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<bool> ValidateGeminiKeyAsync(string key)
        {
            var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models?key={Uri.EscapeDataString(key)}";
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using var response = await HttpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<bool> ValidateHuggingFaceKeyAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://huggingface.co/api/whoami-v2");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private static async Task<bool> ValidateGitHubKeyAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            request.Headers.UserAgent.ParseAdd("BewsAIStudio/1.0");
            request.Headers.Accept.ParseAdd("application/vnd.github+json");

            using var response = await HttpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        private async void OnKeyDetailsClick(object sender, EventArgs e)
        {
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
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

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
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
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
                modelRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
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
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            details.Active = response.IsSuccessStatusCode;
            return details;
        }

        private static async Task<KeyDetailsInfo> GetGitHubKeyDetailsAsync(string key)
        {
            var details = new KeyDetailsInfo();

            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
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
            btnKeyDetails.Enabled = false;
            btnValidate.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                InitializeModelGridColumns();
                _allModels = await GetModelDetailsAsync(keyType, key.Trim());
                ApplyModelFilter();
            }
            catch
            {
                _allModels = new List<ModelDetailInfo>();
                dgvModels.Rows.Clear();
            }
            finally
            {
                btnModelDetails.Enabled = true;
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

        private static async Task<List<ModelDetailInfo>> GetModelDetailsAsync(string keyType, string key)
        {
            return keyType switch
            {
                "Open Router" => await GetOpenRouterModelsAsync(key),
                "Open AI" => await GetOpenAiModelsAsync(key),
                "Gemini" => await GetGeminiModelsAsync(key),
                "Hugging Face" => await GetHuggingFaceModelsAsync(key),
                "GitHub" => await GetGitHubModelsAsync(key),
                _ => new List<ModelDetailInfo>()
            };
        }

        private static async Task<List<ModelDetailInfo>> GetOpenAiModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/models");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ModelDetailInfo>();
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
            var models = new List<ModelDetailInfo>();

            if (!document.RootElement.TryGetProperty("data", out var data) ||
                data.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return models;
            }

            foreach (var model in data.EnumerateArray())
            {
                var modelId = model.TryGetProperty("id", out var idElement) ? idElement.GetString() ?? string.Empty : string.Empty;
                var modelName = modelId;

                models.Add(new ModelDetailInfo
                {
                    ModelName = modelName,
                    ModelId = modelId,
                    ModelType = "N/A",
                    Params = ExtractParamsFromName(modelId, modelName)
                });
            }

            return models;
        }

        private static async Task<List<ModelDetailInfo>> GetOpenRouterModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://openrouter.ai/api/v1/models");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ModelDetailInfo>();
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
            var models = new List<ModelDetailInfo>();

            if (!document.RootElement.TryGetProperty("data", out var data) ||
                data.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return models;
            }

            foreach (var model in data.EnumerateArray())
            {
                var modelId = model.TryGetProperty("id", out var idElement) ? idElement.GetString() ?? string.Empty : string.Empty;
                var modelName = model.TryGetProperty("name", out var nameElement) ? nameElement.GetString() ?? modelId : modelId;
                var modelType = "Paid";

                if (model.TryGetProperty("pricing", out var pricing))
                {
                    var prompt = GetDecimalProperty(pricing, "prompt");
                    var completion = GetDecimalProperty(pricing, "completion");
                    modelType = prompt.GetValueOrDefault() <= 0m && completion.GetValueOrDefault() <= 0m ? "Free" : "Paid";
                }

                models.Add(new ModelDetailInfo
                {
                    ModelName = modelName,
                    ModelId = modelId,
                    ModelType = modelType,
                    Params = ExtractParamsFromName(modelId, modelName)
                });
            }

            return models;
        }

        private static async Task<List<ModelDetailInfo>> GetGeminiModelsAsync(string key)
        {
            var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models?key={Uri.EscapeDataString(key)}";
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ModelDetailInfo>();
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
            var models = new List<ModelDetailInfo>();

            if (!document.RootElement.TryGetProperty("models", out var modelArray) ||
                modelArray.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return models;
            }

            foreach (var model in modelArray.EnumerateArray())
            {
                var modelId = model.TryGetProperty("name", out var idElement) ? idElement.GetString() ?? string.Empty : string.Empty;
                var modelName = model.TryGetProperty("displayName", out var nameElement) ? nameElement.GetString() ?? modelId : modelId;

                models.Add(new ModelDetailInfo
                {
                    ModelName = modelName,
                    ModelId = modelId,
                    ModelType = "N/A",
                    Params = ExtractParamsFromName(modelId, modelName)
                });
            }

            return models;
        }

        private static async Task<List<ModelDetailInfo>> GetHuggingFaceModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://huggingface.co/api/models?limit=100");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);

            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ModelDetailInfo>();
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
            var models = new List<ModelDetailInfo>();

            if (document.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return models;
            }

            foreach (var model in document.RootElement.EnumerateArray())
            {
                var modelId = model.TryGetProperty("id", out var idElement) ? idElement.GetString() ?? string.Empty : string.Empty;
                var hfParams = TryGetHuggingFaceParams(model);
                models.Add(new ModelDetailInfo
                {
                    ModelName = modelId,
                    ModelId = modelId,
                    ModelType = "N/A",
                    Params = hfParams ?? ExtractParamsFromName(modelId)
                });
            }

            return models;
        }

        private static async Task<List<ModelDetailInfo>> GetGitHubModelsAsync(string key)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://models.inference.ai.azure.com/models");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            request.Headers.UserAgent.ParseAdd("BewsAIStudio/1.0");

            using var response = await HttpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ModelDetailInfo>();
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var document = await System.Text.Json.JsonDocument.ParseAsync(stream);
            var models = new List<ModelDetailInfo>();

            if (document.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return models;
            }

            foreach (var model in document.RootElement.EnumerateArray())
            {
                var modelId = model.TryGetProperty("id", out var idElement) ? idElement.GetString() ?? string.Empty : string.Empty;
                var modelName = model.TryGetProperty("name", out var nameElement) ? nameElement.GetString() ?? modelId : modelId;

                models.Add(new ModelDetailInfo
                {
                    ModelName = modelName,
                    ModelId = modelId,
                    ModelType = "N/A",
                    Params = ExtractParamsFromName(modelId, modelName)
                });
            }

            return models;
        }

        private sealed class ModelDetailInfo
        {
            public string ModelName { get; set; } = string.Empty;
            public string ModelId { get; set; } = string.Empty;
            public string ModelType { get; set; } = string.Empty;
            public string Params { get; set; } = "N/A";
        }

        private static string ExtractParamsFromName(params string[] names)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"(\d+\.?\d*)[xX]?(\d+\.?\d*)?[\s\-_]?[bB]", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                var match = regex.Match(name);
                if (match.Success)
                {
                    return match.Value.Trim('-', '_', ' ').ToUpperInvariant();
                }
            }

            return "N/A";
        }

        private static string? TryGetHuggingFaceParams(System.Text.Json.JsonElement model)
        {
            if (!model.TryGetProperty("safetensors", out var safetensors) ||
                safetensors.ValueKind != System.Text.Json.JsonValueKind.Object)
            {
                return null;
            }

            if (!safetensors.TryGetProperty("total", out var total) ||
                total.ValueKind != System.Text.Json.JsonValueKind.Number ||
                !total.TryGetInt64(out var totalParams) ||
                totalParams <= 0)
            {
                return null;
            }

            return FormatParamCount(totalParams);
        }

        private static string FormatParamCount(long count)
        {
            return count switch
            {
                >= 1_000_000_000 => $"{count / 1_000_000_000d:0.##}B",
                >= 1_000_000 => $"{count / 1_000_000d:0.##}M",
                >= 1_000 => $"{count / 1_000d:0.##}K",
                _ => count.ToString()
            };
        }

        private void OnModelSearchTextChanged(object? sender, EventArgs e)
        {
            ApplyModelFilter();
        }

        private void ApplyModelFilter()
        {
            var query = txtModelSearch.Text.Trim();
            IEnumerable<ModelDetailInfo> filtered = _allModels;

            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = _allModels.Where(model =>
                    ContainsIgnoreCase(model.ModelName, query) ||
                    ContainsIgnoreCase(model.ModelId, query) ||
                    ContainsIgnoreCase(model.ModelType, query) ||
                    ContainsIgnoreCase(model.Params, query));
            }

            BindModelGrid(filtered.ToList());
        }

        private void BindModelGrid(List<ModelDetailInfo> models)
        {
            dgvModels.Rows.Clear();

            for (var i = 0; i < models.Count; i++)
            {
                var model = models[i];
                dgvModels.Rows.Add(i + 1, model.ModelName, model.ModelId, model.ModelType, model.Params);
            }
        }

        private static bool ContainsIgnoreCase(string source, string value) =>
            source.Contains(value, StringComparison.OrdinalIgnoreCase);

    }
}
