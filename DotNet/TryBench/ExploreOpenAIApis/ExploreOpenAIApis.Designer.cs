namespace ExploreOpenAIApis
{
    partial class ExploreOpenAIApis
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
            btnGo = new Button();
            txtPrompt = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            txtResponse = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnGo);
            groupBox1.Controls.Add(txtPrompt);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(23, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(416, 100);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Ask";
            // 
            // btnGo
            // 
            btnGo.BackColor = SystemColors.ActiveBorder;
            btnGo.Location = new Point(356, 26);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(54, 62);
            btnGo.TabIndex = 2;
            btnGo.Text = "&Go";
            btnGo.UseVisualStyleBackColor = false;
            btnGo.Click += OnGoClickAsync;
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new Point(74, 26);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(276, 62);
            txtPrompt.TabIndex = 1;
            txtPrompt.Text = "Who is the God of Christianity?";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 26);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 0;
            label1.Text = "Prompt:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtResponse);
            groupBox2.Location = new Point(23, 140);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(416, 298);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Response";
            // 
            // txtResponse
            // 
            txtResponse.Location = new Point(18, 26);
            txtResponse.Multiline = true;
            txtResponse.Name = "txtResponse";
            txtResponse.ScrollBars = ScrollBars.Both;
            txtResponse.Size = new Size(392, 266);
            txtResponse.TabIndex = 0;
            // 
            // ExploreOpenAIApis
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(452, 450);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "ExploreOpenAIApis";
            Text = "My Gpt";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtPrompt;
        private Label label1;
        private GroupBox groupBox2;
        private TextBox txtResponse;
        private Button btnGo;
    }
}
