namespace Shell
{
    partial class BewsAIStudio
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
            menuStrip1 = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            keyValidatorToolStripMenuItem = new ToolStripMenuItem();
            summerizerToolStripMenuItem = new ToolStripMenuItem();
            sentimentAnalyzerToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pythonToolStripMenuItem = new ToolStripMenuItem();
            funToolStripMenuItem = new ToolStripMenuItem();
            crushCalcToolStripMenuItem = new ToolStripMenuItem();
            quizToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem, pythonToolStripMenuItem, funToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { keyValidatorToolStripMenuItem, summerizerToolStripMenuItem, sentimentAnalyzerToolStripMenuItem, toolStripMenuItem1, exitToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(47, 20);
            toolsToolStripMenuItem.Text = "&Tools";
            // 
            // keyValidatorToolStripMenuItem
            // 
            keyValidatorToolStripMenuItem.Name = "keyValidatorToolStripMenuItem";
            keyValidatorToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.K;
            keyValidatorToolStripMenuItem.ShowShortcutKeys = false;
            keyValidatorToolStripMenuItem.Size = new Size(180, 22);
            keyValidatorToolStripMenuItem.Text = "&Key Validator";
            keyValidatorToolStripMenuItem.Click += OnKeyValidatorClick;
            // 
            // summerizerToolStripMenuItem
            // 
            summerizerToolStripMenuItem.Name = "summerizerToolStripMenuItem";
            summerizerToolStripMenuItem.Size = new Size(180, 22);
            summerizerToolStripMenuItem.Text = "&Summerizer";
            summerizerToolStripMenuItem.Click += OnSummerizerClick;
            // 
            // sentimentAnalyzerToolStripMenuItem
            // 
            sentimentAnalyzerToolStripMenuItem.Name = "sentimentAnalyzerToolStripMenuItem";
            sentimentAnalyzerToolStripMenuItem.Size = new Size(180, 22);
            sentimentAnalyzerToolStripMenuItem.Text = "S&entiment Analyzer";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(180, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += OnExitClick;
            // 
            // pythonToolStripMenuItem
            // 
            pythonToolStripMenuItem.Name = "pythonToolStripMenuItem";
            pythonToolStripMenuItem.Size = new Size(57, 20);
            pythonToolStripMenuItem.Text = "&Python";
            // 
            // funToolStripMenuItem
            // 
            funToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { crushCalcToolStripMenuItem, quizToolStripMenuItem });
            funToolStripMenuItem.Name = "funToolStripMenuItem";
            funToolStripMenuItem.Size = new Size(39, 20);
            funToolStripMenuItem.Text = "&Fun";
            // 
            // crushCalcToolStripMenuItem
            // 
            crushCalcToolStripMenuItem.Name = "crushCalcToolStripMenuItem";
            crushCalcToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            crushCalcToolStripMenuItem.ShowShortcutKeys = false;
            crushCalcToolStripMenuItem.Size = new Size(180, 22);
            crushCalcToolStripMenuItem.Text = "&Crush Calc";
            crushCalcToolStripMenuItem.Click += OnCrushCalcClick;
            // 
            // quizToolStripMenuItem
            // 
            quizToolStripMenuItem.Name = "quizToolStripMenuItem";
            quizToolStripMenuItem.Size = new Size(180, 22);
            quizToolStripMenuItem.Text = "&Quiz";
            // 
            // BewsAIStudio
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "BewsAIStudio";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bews AI Studio";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolsToolStripMenuItem;
        private ToolStripMenuItem keyValidatorToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem pythonToolStripMenuItem;
        private ToolStripMenuItem funToolStripMenuItem;
        private ToolStripMenuItem crushCalcToolStripMenuItem;
        private ToolStripMenuItem quizToolStripMenuItem;
        private ToolStripMenuItem summerizerToolStripMenuItem;
        private ToolStripMenuItem sentimentAnalyzerToolStripMenuItem;
    }
}
