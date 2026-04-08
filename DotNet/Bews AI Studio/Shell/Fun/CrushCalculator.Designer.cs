namespace Shell.Fun
{
    partial class CrushCalculator
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
            btnCalculate = new Button();
            txtCrush2 = new TextBox();
            lblCrush2 = new Label();
            txtCrush1 = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            lblFlameResult = new RichTextBox();
            btnAskAI = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnAskAI);
            groupBox1.Controls.Add(btnCalculate);
            groupBox1.Controls.Add(txtCrush2);
            groupBox1.Controls.Add(lblCrush2);
            groupBox1.Controls.Add(txtCrush1);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(360, 157);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Name";
            // 
            // btnCalculate
            // 
            btnCalculate.BackColor = SystemColors.ScrollBar;
            btnCalculate.Location = new Point(97, 96);
            btnCalculate.Name = "btnCalculate";
            btnCalculate.Size = new Size(75, 43);
            btnCalculate.TabIndex = 5;
            btnCalculate.Text = "&Calculate";
            btnCalculate.UseVisualStyleBackColor = false;
            btnCalculate.Click += OnCalculateClick;
            // 
            // txtCrush2
            // 
            txtCrush2.Location = new Point(89, 55);
            txtCrush2.Name = "txtCrush2";
            txtCrush2.Size = new Size(251, 23);
            txtCrush2.TabIndex = 1;
            txtCrush2.Text = "Shalini";
            // 
            // lblCrush2
            // 
            lblCrush2.AutoSize = true;
            lblCrush2.Location = new Point(14, 59);
            lblCrush2.Name = "lblCrush2";
            lblCrush2.Size = new Size(50, 15);
            lblCrush2.TabIndex = 0;
            lblCrush2.Text = "Crush 2:";
            // 
            // txtCrush1
            // 
            txtCrush1.Location = new Point(89, 26);
            txtCrush1.Name = "txtCrush1";
            txtCrush1.Size = new Size(251, 23);
            txtCrush1.TabIndex = 1;
            txtCrush1.Text = "Bewin";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 30);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 0;
            label1.Text = "Crush 1:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblFlameResult);
            groupBox2.Location = new Point(12, 174);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(360, 310);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Calculations...";
            // 
            // lblFlameResult
            // 
            lblFlameResult.BackColor = SystemColors.Control;
            lblFlameResult.BorderStyle = BorderStyle.None;
            lblFlameResult.Dock = DockStyle.Fill;
            lblFlameResult.Location = new Point(3, 19);
            lblFlameResult.Name = "lblFlameResult";
            lblFlameResult.ReadOnly = true;
            lblFlameResult.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            lblFlameResult.Size = new Size(354, 288);
            lblFlameResult.TabIndex = 0;
            lblFlameResult.TabStop = false;
            // 
            // btnAskAI
            // 
            btnAskAI.BackColor = SystemColors.ScrollBar;
            btnAskAI.Location = new Point(189, 96);
            btnAskAI.Name = "btnAskAI";
            btnAskAI.Size = new Size(75, 43);
            btnAskAI.TabIndex = 5;
            btnAskAI.Text = "&Ask AI";
            btnAskAI.UseVisualStyleBackColor = false;
            btnAskAI.Click += OnAskAI;
            // 
            // CrushCalculator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(389, 500);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "CrushCalculator";
            ShowInTaskbar = false;
            Text = "♥ Crush Calculator ♥";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private TextBox txtCrush1;
        private TextBox txtCrush2;
        private Label lblCrush2;
        private GroupBox groupBox2;
        private Button btnCalculate;
        private RichTextBox lblFlameResult;
        private Button btnAskAI;
    }
}