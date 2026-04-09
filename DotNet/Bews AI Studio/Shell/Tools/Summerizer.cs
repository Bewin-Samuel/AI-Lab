using Shell.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Shell.Tools
{
    public partial class Summerizer : BaseForm
    {
        public Summerizer()
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

        private void OnLoadModelsClick(object sender, EventArgs e)
        {

        }
    }
}
