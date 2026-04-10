namespace Shell.Tools
{
    partial class Analyzer
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
            cmbModelProviders = new ComboBox();
            label4 = new Label();
            rtbContent = new RichTextBox();
            btnAnalyze = new Button();
            chkSummerize = new CheckBox();
            chkSentiment = new CheckBox();
            label6 = new Label();
            txtFileOrUrlPath = new TextBox();
            txtSummerizeLength = new TextBox();
            btnUploadFile = new Button();
            txtSentimentLength = new TextBox();
            groupBox1 = new GroupBox();
            btnTxtLoad = new Button();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rtbResult);
            groupBox2.Controls.Add(lblResult);
            groupBox2.Location = new Point(19, 550);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(762, 275);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            // 
            // rtbResult
            // 
            rtbResult.Location = new Point(24, 45);
            rtbResult.Name = "rtbResult";
            rtbResult.Size = new Size(726, 224);
            rtbResult.TabIndex = 1;
            rtbResult.Text = "";
            // 
            // lblResult
            // 
            lblResult.AutoSize = true;
            lblResult.Location = new Point(9, 20);
            lblResult.Name = "lblResult";
            lblResult.Size = new Size(128, 15);
            lblResult.TabIndex = 0;
            lblResult.Text = "Summary && Sentiment";
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
            groupBox3.Controls.Add(cmbModelProviders);
            groupBox3.Controls.Add(label4);
            groupBox3.Location = new Point(19, 7);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(624, 184);
            groupBox3.TabIndex = 6;
            groupBox3.TabStop = false;
            groupBox3.Text = "Key Details";
            // 
            // btnLoad
            // 
            btnLoad.BackColor = SystemColors.ScrollBar;
            btnLoad.Location = new Point(512, 24);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(85, 137);
            btnLoad.TabIndex = 5;
            btnLoad.Text = "&Load Models";
            btnLoad.UseVisualStyleBackColor = false;
            btnLoad.Click += OnLoadModelsClick;
            // 
            // txtBaseUrl
            // 
            txtBaseUrl.Location = new Point(129, 100);
            txtBaseUrl.Name = "txtBaseUrl";
            txtBaseUrl.Size = new Size(358, 23);
            txtBaseUrl.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 104);
            label3.Name = "label3";
            label3.Size = new Size(49, 15);
            label3.TabIndex = 2;
            label3.Text = "Base Url";
            // 
            // txtApiKey
            // 
            txtApiKey.Location = new Point(129, 62);
            txtApiKey.Name = "txtApiKey";
            txtApiKey.PasswordChar = '•';
            txtApiKey.Size = new Size(358, 23);
            txtApiKey.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 66);
            label2.Name = "label2";
            label2.Size = new Size(47, 15);
            label2.TabIndex = 2;
            label2.Text = "Api Key";
            // 
            // cmbModels
            // 
            cmbModels.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModels.FormattingEnabled = true;
            cmbModels.Location = new Point(129, 138);
            cmbModels.Name = "cmbModels";
            cmbModels.Size = new Size(358, 23);
            cmbModels.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 142);
            label5.Name = "label5";
            label5.Size = new Size(46, 15);
            label5.TabIndex = 0;
            label5.Text = "Models";
            // 
            // cmbModelProviders
            // 
            cmbModelProviders.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbModelProviders.FormattingEnabled = true;
            cmbModelProviders.Location = new Point(129, 24);
            cmbModelProviders.Name = "cmbModelProviders";
            cmbModelProviders.Size = new Size(358, 23);
            cmbModelProviders.TabIndex = 0;
            cmbModelProviders.SelectedIndexChanged += OnProviderChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 28);
            label4.Name = "label4";
            label4.Size = new Size(56, 15);
            label4.TabIndex = 0;
            label4.Text = "Providers";
            // 
            // rtbContent
            // 
            rtbContent.Location = new Point(16, 22);
            rtbContent.Name = "rtbContent";
            rtbContent.Size = new Size(734, 252);
            rtbContent.TabIndex = 1;
            rtbContent.Text = "";
            // 
            // btnAnalyze
            // 
            btnAnalyze.BackColor = SystemColors.ScrollBar;
            btnAnalyze.Location = new Point(544, 283);
            btnAnalyze.Name = "btnAnalyze";
            btnAnalyze.Size = new Size(70, 50);
            btnAnalyze.TabIndex = 4;
            btnAnalyze.Text = "&Analyze";
            btnAnalyze.UseVisualStyleBackColor = false;
            btnAnalyze.Click += OnAnalyzeClick;
            // 
            // chkSummerize
            // 
            chkSummerize.AutoSize = true;
            chkSummerize.Checked = true;
            chkSummerize.CheckState = CheckState.Checked;
            chkSummerize.Location = new Point(629, 283);
            chkSummerize.Name = "chkSummerize";
            chkSummerize.Size = new Size(85, 19);
            chkSummerize.TabIndex = 6;
            chkSummerize.Text = "S&ummerize";
            chkSummerize.UseVisualStyleBackColor = true;
            chkSummerize.CheckedChanged += OnAnalysisOptionChanged;
            // 
            // chkSentiment
            // 
            chkSentiment.AutoSize = true;
            chkSentiment.Location = new Point(629, 312);
            chkSentiment.Name = "chkSentiment";
            chkSentiment.Size = new Size(80, 19);
            chkSentiment.TabIndex = 7;
            chkSentiment.Text = "&Sentiment";
            chkSentiment.UseVisualStyleBackColor = true;
            chkSentiment.CheckedChanged += OnAnalysisOptionChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(24, 301);
            label6.Name = "label6";
            label6.Size = new Size(84, 15);
            label6.TabIndex = 8;
            label6.Text = "FilePath or Url:";
            // 
            // txtFileOrUrlPath
            // 
            txtFileOrUrlPath.Location = new Point(114, 297);
            txtFileOrUrlPath.Name = "txtFileOrUrlPath";
            txtFileOrUrlPath.Size = new Size(183, 23);
            txtFileOrUrlPath.TabIndex = 9;
            // 
            // txtSummerizeLength
            // 
            txtSummerizeLength.Location = new Point(715, 281);
            txtSummerizeLength.Name = "txtSummerizeLength";
            txtSummerizeLength.Size = new Size(35, 23);
            txtSummerizeLength.TabIndex = 9;
            // 
            // btnUploadFile
            // 
            btnUploadFile.BackColor = SystemColors.ScrollBar;
            btnUploadFile.Location = new Point(303, 295);
            btnUploadFile.Name = "btnUploadFile";
            btnUploadFile.Size = new Size(31, 26);
            btnUploadFile.TabIndex = 10;
            btnUploadFile.Text = "...";
            btnUploadFile.UseVisualStyleBackColor = false;
            btnUploadFile.Click += OnFileUpload;
            // 
            // txtSentimentLength
            // 
            txtSentimentLength.Location = new Point(715, 310);
            txtSentimentLength.Name = "txtSentimentLength";
            txtSentimentLength.Size = new Size(35, 23);
            txtSentimentLength.TabIndex = 11;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnTxtLoad);
            groupBox1.Controls.Add(txtSentimentLength);
            groupBox1.Controls.Add(btnUploadFile);
            groupBox1.Controls.Add(txtSummerizeLength);
            groupBox1.Controls.Add(txtFileOrUrlPath);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(chkSentiment);
            groupBox1.Controls.Add(chkSummerize);
            groupBox1.Controls.Add(btnAnalyze);
            groupBox1.Controls.Add(rtbContent);
            groupBox1.Location = new Point(19, 197);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(769, 347);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Content:";
            // 
            // btnTxtLoad
            // 
            btnTxtLoad.BackColor = SystemColors.ScrollBar;
            btnTxtLoad.Location = new Point(340, 295);
            btnTxtLoad.Name = "btnTxtLoad";
            btnTxtLoad.Size = new Size(57, 26);
            btnTxtLoad.TabIndex = 12;
            btnTxtLoad.Text = "&Load";
            btnTxtLoad.UseVisualStyleBackColor = false;
            // 
            // Analyzer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 837);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Analyzer";
            ShowInTaskbar = false;
            Text = "Analyzer";
            Load += OnFormLoad;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox2;
        private RichTextBox rtbResult;
        private Label lblResult;
        private GroupBox groupBox3;
        private TextBox txtBaseUrl;
        private Label label3;
        private TextBox txtApiKey;
        private Label label2;
        private ComboBox cmbModelProviders;
        private Label label4;
        private ComboBox cmbModels;
        private Label label5;
        private Button btnLoad;
        private RichTextBox rtbContent;
        private Button btnAnalyze;
        private CheckBox chkSummerize;
        private CheckBox chkSentiment;
        private Label label6;
        private TextBox txtFileOrUrlPath;
        private TextBox txtSummerizeLength;
        private Button btnUploadFile;
        private TextBox txtSentimentLength;
        private GroupBox groupBox1;
        private Button btnTxtLoad;
    }
}