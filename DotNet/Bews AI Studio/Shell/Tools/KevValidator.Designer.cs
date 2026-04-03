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
            btnModelDetails = new Button();
            btnKeyDetails = new Button();
            btnValidate = new Button();
            txtKey = new TextBox();
            label2 = new Label();
            cmbKeyType = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            lblKeyDetails = new Label();
            groupBox3 = new GroupBox();
            txtModelSearch = new TextBox();
            lblModelSearch = new Label();
            dgvModels = new DataGridView();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvModels).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnModelDetails);
            groupBox1.Controls.Add(btnKeyDetails);
            groupBox1.Controls.Add(btnValidate);
            groupBox1.Controls.Add(txtKey);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cmbKeyType);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 10);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(508, 159);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Key Details";
            // 
            // btnModelDetails
            // 
            btnModelDetails.BackColor = SystemColors.ScrollBar;
            btnModelDetails.Location = new Point(320, 104);
            btnModelDetails.Name = "btnModelDetails";
            btnModelDetails.Size = new Size(75, 43);
            btnModelDetails.TabIndex = 4;
            btnModelDetails.Text = "Model &Details";
            btnModelDetails.UseVisualStyleBackColor = false;
            btnModelDetails.Click += OnModelDetailsClick;
            // 
            // btnKeyDetails
            // 
            btnKeyDetails.BackColor = SystemColors.ScrollBar;
            btnKeyDetails.Location = new Point(217, 104);
            btnKeyDetails.Name = "btnKeyDetails";
            btnKeyDetails.Size = new Size(75, 43);
            btnKeyDetails.TabIndex = 4;
            btnKeyDetails.Text = "Key &Details";
            btnKeyDetails.UseVisualStyleBackColor = false;
            btnKeyDetails.Click += OnKeyDetailsClick;
            // 
            // btnValidate
            // 
            btnValidate.BackColor = SystemColors.ScrollBar;
            btnValidate.Location = new Point(114, 104);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(75, 43);
            btnValidate.TabIndex = 4;
            btnValidate.Text = "&Validate";
            btnValidate.UseVisualStyleBackColor = false;
            btnValidate.Click += OnKeyValidateClick;
            // 
            // txtKey
            // 
            txtKey.Location = new Point(129, 68);
            txtKey.Name = "txtKey";
            txtKey.PasswordChar = '•';
            txtKey.Size = new Size(304, 23);
            txtKey.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 72);
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
            cmbKeyType.Size = new Size(304, 23);
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
            groupBox2.Location = new Point(12, 184);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(508, 150);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Key Details";
            // 
            // lblKeyDetails
            // 
            lblKeyDetails.Dock = DockStyle.Fill;
            lblKeyDetails.Location = new Point(3, 19);
            lblKeyDetails.Name = "lblKeyDetails";
            lblKeyDetails.Size = new Size(502, 128);
            lblKeyDetails.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtModelSearch);
            groupBox3.Controls.Add(lblModelSearch);
            groupBox3.Controls.Add(dgvModels);
            groupBox3.Location = new Point(12, 343);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(508, 403);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Model Details";
            // 
            // txtModelSearch
            // 
            txtModelSearch.Location = new Point(85, 23);
            txtModelSearch.Name = "txtModelSearch";
            txtModelSearch.Size = new Size(417, 23);
            txtModelSearch.TabIndex = 2;
            txtModelSearch.TextChanged += OnModelSearchTextChanged;
            // 
            // lblModelSearch
            // 
            lblModelSearch.AutoSize = true;
            lblModelSearch.Location = new Point(6, 27);
            lblModelSearch.Name = "lblModelSearch";
            lblModelSearch.Size = new Size(42, 15);
            lblModelSearch.TabIndex = 1;
            lblModelSearch.Text = "Search";
            // 
            // dgvModels
            // 
            dgvModels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvModels.Location = new Point(6, 56);
            dgvModels.Name = "dgvModels";
            dgvModels.Size = new Size(496, 341);
            dgvModels.TabIndex = 0;
            // 
            // KevValidator
            // 
            AcceptButton = btnValidate;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(531, 752);
            Controls.Add(groupBox3);
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
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvModels).EndInit();
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
        private GroupBox groupBox3;
        private DataGridView dgvModels;
        private Button btnModelDetails;
        private TextBox txtModelSearch;
        private Label lblModelSearch;
    }
}