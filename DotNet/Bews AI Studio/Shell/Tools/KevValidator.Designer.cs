namespace Shell.Tools
{
    partial class KevValidator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            label1 = new Label();
            cmbKeyType = new ComboBox();
            label2 = new Label();
            txtKey = new TextBox();
            btnSubmit = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSubmit);
            groupBox1.Controls.Add(txtKey);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cmbKeyType);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(21, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(313, 159);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Key Details";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 36);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 0;
            label1.Text = "Type";
            // 
            // cmbKeyType
            // 
            cmbKeyType.FormattingEnabled = true;
            cmbKeyType.Location = new Point(129, 32);
            cmbKeyType.Name = "cmbKeyType";
            cmbKeyType.Size = new Size(168, 23);
            cmbKeyType.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 78);
            label2.Name = "label2";
            label2.Size = new Size(26, 15);
            label2.TabIndex = 2;
            label2.Text = "Key";
            // 
            // txtKey
            // 
            txtKey.Location = new Point(129, 74);
            txtKey.Name = "txtKey";
            txtKey.PasswordChar = '•';
            txtKey.Size = new Size(168, 23);
            txtKey.TabIndex = 3;
            // 
            // btnSubmit
            // 
            btnSubmit.BackColor = SystemColors.ScrollBar;
            btnSubmit.Location = new Point(117, 121);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(75, 23);
            btnSubmit.TabIndex = 4;
            btnSubmit.Text = "&Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            // 
            // KevValidator
            // 
            AcceptButton = btnSubmit;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 209);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "KevValidator";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Key Validator";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnSubmit;
        private TextBox txtKey;
        private Label label2;
        private ComboBox cmbKeyType;
        private Label label1;
    }
}