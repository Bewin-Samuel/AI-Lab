namespace Shell.Tools
{
    partial class Summerizer
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
            btnSentiment = new Button();
            btnSummerize = new Button();
            rtbContent = new RichTextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            rtbResult = new RichTextBox();
            lblResult = new Label();
            groupBox3 = new GroupBox();
            btnLoad = new Button();
            txtBaseUrl = new TextBox();
            label3 = new Label();
            txtApiKey = new TextBox();
            label2 = new Label();
            cmbModels = new ComboBox();
            label5 = new Label();
            txtProviders = new ComboBox();
            label4 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSentiment);
            groupBox1.Controls.Add(btnSummerize);
            groupBox1.Controls.Add(rtbContent);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(19, 172);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(769, 347);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // btnSentiment
            // 
            btnSentiment.BackColor = SystemColors.ScrollBar;
            btnSentiment.Location = new Point(392, 297);
            btnSentiment.Name = "btnSentiment";
            btnSentiment.Size = new Size(85, 40);
            btnSentiment.TabIndex = 4;
            btnSentiment.Text = "S&entiment Analysis";
            btnSentiment.UseVisualStyleBackColor = false;
            // 
            // btnSummerize
            // 
            btnSummerize.BackColor = SystemColors.ScrollBar;
            btnSummerize.Location = new Point(292, 297);
            btnSummerize.Name = "btnSummerize";
            btnSummerize.Size = new Size(85, 40);
            btnSummerize.TabIndex = 4;
            btnSummerize.Text = "&Summerizer";
            btnSummerize.UseVisualStyleBackColor = false;
            btnSummerize.Click += OnSummerize;
            // 
            // rtbContent
            // 
            rtbContent.Location = new Point(24, 45);
            rtbContent.Name = "rtbContent";
            rtbContent.Size = new Size(726, 246);
            rtbContent.TabIndex = 1;
            rtbContent.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(9, 20);
            label1.Name = "label1";
            label1.Size = new Size(53, 15);
            label1.TabIndex = 0;
            label1.Text = "Content:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rtbResult);
            groupBox2.Controls.Add(lblResult);
            groupBox2.Location = new Point(19, 539);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(762, 227);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            // 
            // rtbResult
            // 
            rtbResult.Location = new Point(24, 45);
            rtbResult.Name = "rtbResult";
            rtbResult.Size = new Size(726, 189);
            rtbResult.TabIndex = 1;
            rtbResult.Text = "";
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Location = new Point(9, 20);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(53, 15);
            lblResult.TabIndex = 0;
            lblResult.Text = "Content:";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnLoad);
            groupBox3.Controls.Add(txtBaseUrl);
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(txtApiKey);
            groupBox3.Controls.Add(label2);
            groupBox3.Controls.Add(cmbModels);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(txtProviders);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new Point(19, 7);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(769, 159);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Key Details";
            // 
            // btnLoad
            // 
            btnLoad.BackColor = SystemColors.ScrollBar;
            btnLoad.Location = new Point(507, 90);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(85, 61);
            btnLoad.TabIndex = 5;
            btnLoad.Text = "&Load Models";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += OnLoadModelsClick;
            // 
            // txtBaseUrl
            // 
            txtBaseUrl.Location = new Point(129, 90);
            txtBaseUrl.Name = "txtBaseUrl";
            txtBaseUrl.Size = new Size(358, 23);
            txtBaseUrl.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 94);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 2;
            label3.Text = "Base Url";
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new Point(129, 52);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.PasswordChar = '•';
            txtApiKey.Size = new Size(358, 23);
            txtApiKey.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 56);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 2;
            label2.Text = "Api Key";
            // 
            // cmbModels
            // 
            cmbModels.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModels.FormattingEnabled = true;
            cmbModels.Location = new Point(129, 128);
            cmbModels.Name = "cmbModels";
            cmbModels.Size = new Size(358, 23);
            cmbModels.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 132);
            label5.Name = "label5";
            label5.Size = new Size(46, 15);
            label5.TabIndex = 0;
            label5.Text = "Models";
            // 
            // txtProviders
            // 
            txtProviders.DropDownStyle = ComboBoxStyle.DropDownList;
            txtProviders.FormattingEnabled = true;
            txtProviders.Location = new Point(129, 14);
            txtProviders.Name = "txtProviders";
            txtProviders.Size = new Size(358, 23);
            txtProviders.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 18);
            label4.Name = "label4";
            label4.Size = new Size(56, 15);
            label4.TabIndex = 0;
            label4.Text = "Providers";
            // 
            // Summerizer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 853);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Summerizer";
            ShowInTaskbar = false;
            Text = "Summerizer";
            Load += OnFormLoad;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private RichTextBox rtbContent;
        private Button btnSentiment;
        private Button btnSummerize;
        private GroupBox groupBox2;
        private RichTextBox rtbResult;
        private Label lblResult;
        private GroupBox groupBox3;
        private TextBox txtBaseUrl;
        private Label label3;
        private TextBox txtApiKey;
        private Label label2;
        private ComboBox txtProviders;
        private Label label4;
        private ComboBox cmbModels;
        private Label label5;
        private Button btnLoad;
    }
}