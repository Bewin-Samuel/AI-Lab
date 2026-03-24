namespace QuizMaster
{
    partial class QuizMaster
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            btnStart = new Button();
            txtQnsCount = new TextBox();
            cmbQuizDifficultyLevel = new ComboBox();
            cmbQuizCategories = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnStart);
            groupBox1.Controls.Add(txtQnsCount);
            groupBox1.Controls.Add(cmbQuizDifficultyLevel);
            groupBox1.Controls.Add(cmbQuizCategories);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(18, 16);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(371, 174);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Inputs";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(144, 145);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 3;
            btnStart.Text = "&Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += this.OnStartClickAsync;
            // 
            // txtQnsCount
            // 
            txtQnsCount.Location = new Point(165, 55);
            txtQnsCount.Name = "txtQnsCount";
            txtQnsCount.Size = new Size(180, 23);
            txtQnsCount.TabIndex = 2;
            // 
            // cmbQuizDifficultyLevel
            // 
            cmbQuizDifficultyLevel.FormattingEnabled = true;
            cmbQuizDifficultyLevel.Location = new Point(165, 93);
            cmbQuizDifficultyLevel.Name = "cmbQuizDifficultyLevel";
            cmbQuizDifficultyLevel.Size = new Size(180, 23);
            cmbQuizDifficultyLevel.TabIndex = 1;
            // 
            // cmbQuizCategories
            // 
            cmbQuizCategories.FormattingEnabled = true;
            cmbQuizCategories.Location = new Point(165, 23);
            cmbQuizCategories.Name = "cmbQuizCategories";
            cmbQuizCategories.Size = new Size(180, 23);
            cmbQuizCategories.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 96);
            label3.Name = "label3";
            label3.Size = new Size(88, 15);
            label3.TabIndex = 0;
            label3.Text = "Difficulty Level:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 58);
            label2.Name = "label2";
            label2.Size = new Size(124, 15);
            label2.TabIndex = 0;
            label2.Text = "Number of Questions:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 26);
            label1.Name = "label1";
            label1.Size = new Size(93, 15);
            label1.TabIndex = 0;
            label1.Text = "Quiz Categories:";
            // 
            // QuizMaster
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(427, 450);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            Name = "QuizMaster";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quiz Master";
            Load += OnFormLoad;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cmbQuizCategories;
        private Label label1;
        private Label label2;
        private TextBox txtQnsCount;
        private Button btnStart;
        private ComboBox cmbQuizDifficultyLevel;
        private Label label3;
    }
}
