using Shell.Common;
using Shell.Core;

namespace Shell.Tools
{
    public partial class Analyzer : BaseForm
    {
        private readonly ModelProvider _modelProvider = new();
        private IReadOnlyList<ModelInfo> _loadedModels = [];

        private const string SummarizationPrompt = """
            You are an Expert in understanding & Summerizing the given content.
            Always format your response exactly as follows:

            Summary:
            <provide a concise summary>

            Actions:
              • <list actionable items> - <Name if available>
            """;

        private const string SentimentPrompt = """
            You are an Expert in analyzing the sentiment of the given content.
            Always format your response exactly as follows:

            Sentiments:
            <Precisely describe the overall sentiment not exeeding 20 - 30 words>
            """;

        public Analyzer()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadTypeDropdown();
        }

        private void OnAnalysisOptionChanged(object? sender, EventArgs e)
        {
            btnAnalyze.Enabled = chkSummerize.Checked || chkSentiment.Checked;
        }

        private async void OnAnalyzeClick(object sender, EventArgs e)
        {
            var content = rtbContent.Text;

            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Please enter content to analyze.", "Analyze", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbContent.Focus();
                return;
            }

            var apiKey = txtApiKey.Text.Trim();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Please enter an API Key.", "Analyze", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApiKey.Focus();
                return;
            }

            if (cmbModels.SelectedIndex < 0 || cmbModels.SelectedIndex >= _loadedModels.Count)
            {
                MessageBox.Show("Please load and select a model.", "Analyze", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbModels.Focus();
                return;
            }

            var provider = cmbModelProviders.SelectedItem?.ToString() ?? string.Empty;
            var modelId = _loadedModels[cmbModels.SelectedIndex].Id;
            var baseUrl = txtBaseUrl.Text.Trim();

            btnAnalyze.Enabled = false;
            Cursor = Cursors.WaitCursor;
            rtbResult.Clear();

            try
            {
                var chatProvider = ChatProviderFactory.Create(provider, apiKey, modelId, baseUrl);
                var results = await ExecuteSelectedAnalysesAsync(chatProvider, content);
                rtbResult.Text = string.Join(Environment.NewLine + Environment.NewLine, results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to analyze content: {ex.Message}", "Analyze", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAnalyze.Enabled = chkSummerize.Checked || chkSentiment.Checked;
                Cursor = Cursors.Default;
            }
        }

        private async Task<List<string>> ExecuteSelectedAnalysesAsync(IChatCompletionProvider chatProvider, string content)
        {
            var results = new List<string>();

            if (chkSummerize.Checked)
                results.Add(await chatProvider.CompleteChatAsync(SummarizationPrompt, content));

            if (chkSentiment.Checked)
                results.Add(await chatProvider.CompleteChatAsync(SentimentPrompt, content));

            return results;
        }

        private void LoadTypeDropdown()
        {
            cmbModelProviders.Items.Clear();

            cmbModelProviders.Items.Add("Azure Open AI");
            cmbModelProviders.Items.Add("Gemini");
            cmbModelProviders.Items.Add("GitHub");
            cmbModelProviders.Items.Add("Hugging Face");
            cmbModelProviders.Items.Add("Open AI");
            cmbModelProviders.Items.Add("Open Router");

            cmbModelProviders.Sorted = true;

            cmbModelProviders.SelectedIndex = cmbModelProviders.Items.Count -1;
        }

        private void OnProviderChanged(object? sender, EventArgs e)
        {
            txtApiKey.Clear();
            txtBaseUrl.Clear();
            cmbModels.Items.Clear();
            _loadedModels = [];
        }

        private async void OnLoadModelsClick(object sender, EventArgs e)
        {
            var apiKey = txtApiKey.Text;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                MessageBox.Show("Please enter an API Key.", "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApiKey.Focus();
                return;
            }

            var provider = cmbModelProviders.SelectedItem?.ToString() ?? string.Empty;

            btnLoad.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var isValid = await _modelProvider.ValidateKeyAsync(provider, apiKey.Trim(), txtBaseUrl.Text.Trim());

                if (!isValid)
                {
                    MessageBox.Show("Key validation failed. Please check your key and try again.",
                        "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var models = await _modelProvider.GetModelsAsync(provider, apiKey.Trim(), txtBaseUrl.Text.Trim());
                PopulateModelsDropdown(models);
            }
            catch
            {
                MessageBox.Show("Unable to load models.", "Load Models", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLoad.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void PopulateModelsDropdown(IReadOnlyList<ModelInfo> models)
        {
            cmbModels.Items.Clear();
            _loadedModels = models;

            foreach (var model in models)
                cmbModels.Items.Add(model.DisplayName);

            if (cmbModels.Items.Count > 0)
                cmbModels.SelectedIndex = 0;
        }

        private async void OnFileUpload(object sender, EventArgs e)
        {
            var path = txtFileOrUrlPath.Text.Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                using var dialog = new OpenFileDialog
                {
                    Title = "Select a file to analyze",
                    Filter = ContentReader.FileDialogFilter,
                    FilterIndex = 1
                };

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                path = dialog.FileName;
                txtFileOrUrlPath.Text = path;
            }

            btnUploadFile.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                rtbContent.Text = await ContentReader.ReadAsync(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load content: {ex.Message}", "Load Content", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnUploadFile.Enabled = true;
                Cursor = Cursors.Default;
            }
        }
    }
}
