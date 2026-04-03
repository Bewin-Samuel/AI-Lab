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
            btnValidate = new Button();
            txtKey = new TextBox();
            label2 = new Label();
            cmbKeyType = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            lblKeyDetails = new Label();
            btnKeyDetails = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnKeyDetails);
            groupBox1.Controls.Add(btnValidate);
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
            // btnValidate
            // 
            btnValidate.BackColor = SystemColors.ScrollBar;
            btnValidate.Location = new Point(51, 116);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(75, 23);
            btnValidate.TabIndex = 4;
            btnValidate.Text = "&Validate";
            btnValidate.UseVisualStyleBackColor = false;
            btnValidate.Click += OnKeyValidateClick;
            // 
            // txtKey
            // 
            txtKey.Location = new Point(129, 74);
            txtKey.Name = "txtKey";
            txtKey.PasswordChar = '•';
            txtKey.Size = new Size(168, 23);
            txtKey.TabIndex = 3;
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
            // cmbKeyType
            // 
            cmbKeyType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKeyType.FormattingEnabled = true;
            cmbKeyType.Location = new Point(129, 32);
            cmbKeyType.Name = "cmbKeyType";
            cmbKeyType.Size = new Size(168, 23);
            cmbKeyType.TabIndex = 1;
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
            // groupBox2
            // 
            groupBox2.Controls.Add(lblKeyDetails);
            groupBox2.Location = new Point(21, 201);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(313, 150);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Key Details";
            // 
            // lblKeyDetails
            // 
            lblKeyDetails.Dock = DockStyle.Fill;
            lblKeyDetails.Location = new Point(3, 19);
            lblKeyDetails.Name = "lblKeyDetails";
            lblKeyDetails.Size = new Size(307, 128);
            lblKeyDetails.TabIndex = 0;
            // 
            // btnKeyDetails
            // 
            btnKeyDetails.BackColor = SystemColors.ScrollBar;
            btnKeyDetails.Location = new Point(141, 116);
            btnKeyDetails.Name = "btnKeyDetails";
            btnKeyDetails.Size = new Size(75, 23);
            btnKeyDetails.TabIndex = 4;
            btnKeyDetails.Text = "Key &Details";
            btnKeyDetails.UseVisualStyleBackColor = false;
            btnKeyDetails.Click += OnKeyDetailsClick;
            // 
            // KevValidator
            // 
            AcceptButton = btnValidate;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(357, 363);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "KevValidator";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Key Validator";
            Load += OnFormLoad;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnValidate;
        private TextBox txtKey;
        private Label label2;
        private ComboBox cmbKeyType;
        private Label label1;
        private GroupBox groupBox2;
        private Label lblKeyDetails;
        private Button btnKeyDetails;
    }
}