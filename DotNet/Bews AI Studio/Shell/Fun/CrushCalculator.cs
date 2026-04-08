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
                if (flameResult == null)
                {
                    MessageBox.Show("Please calculate the flame result first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var modelFacilitator = new ModelFacilitator(GetModelData());
                lblFlameResult.Text = FlameResultFormatter.Format(flameResult) + Environment.NewLine + Environment.NewLine;
                lblFlameResult.AppendText(await modelFacilitator.GetHaikuAsync(flameResult.Crush1, flameResult.Crush2, flameResult.FinalResult));
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred while generating the haiku. {Environment.NewLine}{exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static ModelData GetModelData()
        {
            return new ModelData
            {
                ApiKey = "8fVyELafcYZ38uEymeFqbAoIReaWnLciDnozcTmbRUy3cUFsXwzIJQQJ99CDACYeBjFXJ3w3AAABACOGdYJn",
                BaseUri = new Uri("https://bews-instance.openai.azure.com/"),
                DeploymentName = "gpt-4o"
            };
        }
    }
}