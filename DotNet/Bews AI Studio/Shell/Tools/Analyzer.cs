using Shell.Common;
using Shell.Core;

namespace Shell.Tools
{
    public partial class Analyzer : BaseForm
    {
        private readonly ModelProvider _modelProvider = new();

        public Analyzer()
        {
            InitializeComponent();
        }

        private void OnSummerize(object sender, EventArgs e)
        {

        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadTypeDropdown();
        }

        private void LoadTypeDropdown()
        {
            txtProviders.Items.Clear();

            txtProviders.Items.Add("Azure Open AI");
            txtProviders.Items.Add("Open Router");
            txtProviders.Items.Add("Open AI");
            txtProviders.Items.Add("Hugging Face");
            txtProviders.Items.Add("Gemini");
            txtProviders.Items.Add("GitHub");

            txtProviders.Sorted = true;

            txtProviders.SelectedIndex = 0;
        }

        private void OnProviderChanged(object? sender, EventArgs e)
        {
            txtApiKey.Clear();
            txtBaseUrl.Clear();
            cmbModels.Items.Clear();
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

            var provider = txtProviders.SelectedItem?.ToString() ?? string.Empty;

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

            foreach (var model in models)
                cmbModels.Items.Add(model.DisplayName);

            if (cmbModels.Items.Count > 0)
                cmbModels.SelectedIndex = 0;
        }
    }
}
