using Shell.Fun;
using Shell.Tools;

namespace Shell
{
    public partial class BewsAIStudio : Form
    {
        public BewsAIStudio()
        {
            InitializeComponent();
        }

        private void OnKeyValidatorClick(object sender, EventArgs e)
        {
            ShowMdiChild(new KeyValidator());
        }

        private void ShowMdiChild(Form form)
        {
            form.MdiParent = this;
            form.Show();
        }

        private void OnCrushCalcClick(object sender, EventArgs e)
        {
            ShowMdiChild(new CrushCalculator());
        }

        private void OnExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnSummerizerClick(object sender, EventArgs e)
        {
            ShowMdiChild(new Summerizer());
        }
    }
}