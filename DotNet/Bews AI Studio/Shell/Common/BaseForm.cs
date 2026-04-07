using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Shell.Common
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            this.TopLevel = false;
            this.ShowInTaskbar = false;
            ShowFormInTheCenterOfMdi();
        }

        private void ShowFormInTheCenterOfMdi()
        {
            if (this.MdiParent != null)
            {
                int x = (this.MdiParent.ClientSize.Width - this.Width) / 2;
                int y = (this.MdiParent.ClientSize.Height - this.Height) / 2;
                this.Location = new Point(x < 0 ? 0 : x, y < 0 ? 0 : y);
            }
        }
    }
}
