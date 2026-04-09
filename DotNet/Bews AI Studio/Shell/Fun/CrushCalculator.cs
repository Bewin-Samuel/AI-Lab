using Microsoft.Extensions.Configuration;
using Shell.Common;

namespace Shell.Fun
{
    public partial class CrushCalculator : BaseForm
    {
        FlameCalculationResult? flameResult;

        public CrushCalculator()
        {
            InitializeComponent();
        }

        private void OnCalculateClick(object sender, EventArgs e)
        {
            flameResult = new FlameCalculator().Calculate(txtCrush1.Text, txtCrush2.Text);
            lblFlameResult.Text = FlameResultFormatter.Format(flameResult);
        }

        private async void OnAskAI(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (flameResult == null)
                {
                    MessageBox.Show("Please calculate the flame result first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var modelFacilitator = new ModelFacilitator(GetModelData());
                lblFlameResult.Text = FlameResultFormatter.Format(flameResult) + Environment.NewLine + Environment.NewLine;

                if(chkHaiku.Checked)
                {
                    lblFlameResult.AppendText(await modelFacilitator.GetHaikuAsync(flameResult.Crush1, flameResult.Crush2, flameResult.FinalResult));
                }

                if (chkStory.Checked)
                {
                    lblFlameResult.AppendText(await modelFacilitator.GetShortStory(flameResult.Crush1, flameResult.Crush2, flameResult.FinalResult));
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while generating the haiku. {Environment.NewLine}{exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private static ModelData GetModelData()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<CrushCalculator>()
                .Build();

            return new ModelData
            {
                ApiKey = config["AzureOpenAI:ApiKey"]!,
                BaseUri = new Uri(config["AzureOpenAI:BaseUri"]!),
                DeploymentName = config["AzureOpenAI:DeploymentName"]!
            };
        }
    }
}