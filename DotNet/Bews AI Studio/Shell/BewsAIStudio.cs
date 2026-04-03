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
            var keyValidator = new KevValidator
            {
                TopLevel = false,
                MdiParent = this,
            };

            keyValidator.Show();
        }
    }
}
