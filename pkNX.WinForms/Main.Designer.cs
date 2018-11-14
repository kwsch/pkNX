namespace pkNX.WinForms
{
    partial class Main
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
            this.TB_Path = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu_File = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Options = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Language = new System.Windows.Forms.ToolStripMenuItem();
            this.CB_Lang = new System.Windows.Forms.ToolStripComboBox();
            this.randomizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_SetRNGSeed = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_Restore = new System.Windows.Forms.ToolStripMenuItem();
            this.FLP_Controls = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TB_Path
            // 
            this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Path.Location = new System.Drawing.Point(124, 4);
            this.TB_Path.Name = "TB_Path";
            this.TB_Path.ReadOnly = true;
            this.TB_Path.Size = new System.Drawing.Size(277, 20);
            this.TB_Path.TabIndex = 10;
            this.TB_Path.Text = "No Game Loaded";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_File,
            this.Menu_Options});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(413, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu_File
            // 
            this.Menu_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Open,
            this.Menu_Exit});
            this.Menu_File.Name = "Menu_File";
            this.Menu_File.Size = new System.Drawing.Size(37, 20);
            this.Menu_File.Text = "File";
            // 
            // Menu_Open
            // 
            this.Menu_Open.Name = "Menu_Open";
            this.Menu_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.Menu_Open.Size = new System.Drawing.Size(155, 22);
            this.Menu_Open.Text = "&Open...";
            this.Menu_Open.Click += new System.EventHandler(this.Menu_Open_Click);
            // 
            // Menu_Exit
            // 
            this.Menu_Exit.Name = "Menu_Exit";
            this.Menu_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.Menu_Exit.Size = new System.Drawing.Size(155, 22);
            this.Menu_Exit.Text = "&Exit";
            this.Menu_Exit.Click += new System.EventHandler(this.Menu_Exit_Click);
            // 
            // Menu_Options
            // 
            this.Menu_Options.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_Language,
            this.randomizationToolStripMenuItem,
            this.Menu_Restore});
            this.Menu_Options.Name = "Menu_Options";
            this.Menu_Options.Size = new System.Drawing.Size(61, 20);
            this.Menu_Options.Text = "Options";
            // 
            // Menu_Language
            // 
            this.Menu_Language.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CB_Lang});
            this.Menu_Language.Name = "Menu_Language";
            this.Menu_Language.Size = new System.Drawing.Size(184, 22);
            this.Menu_Language.Text = "Language";
            // 
            // CB_Lang
            // 
            this.CB_Lang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Lang.Items.AddRange(new object[] {
            "カタカナ",
            "漢字",
            "English",
            "Français",
            "Italiano",
            "Deutsch",
            "Español",
            "한국",
            "汉字简化方案",
            "漢字簡化方案"});
            this.CB_Lang.Name = "CB_Lang";
            this.CB_Lang.Size = new System.Drawing.Size(121, 23);
            this.CB_Lang.SelectedIndexChanged += new System.EventHandler(this.ChangeLanguage);
            // 
            // randomizationToolStripMenuItem
            // 
            this.randomizationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu_SetRNGSeed});
            this.randomizationToolStripMenuItem.Name = "randomizationToolStripMenuItem";
            this.randomizationToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.randomizationToolStripMenuItem.Text = "Randomization";
            // 
            // Menu_SetRNGSeed
            // 
            this.Menu_SetRNGSeed.Name = "Menu_SetRNGSeed";
            this.Menu_SetRNGSeed.Size = new System.Drawing.Size(146, 22);
            this.Menu_SetRNGSeed.Text = "Set int32 seed";
            this.Menu_SetRNGSeed.Click += new System.EventHandler(this.Menu_SetRNGSeed_Click);
            // 
            // Menu_Restore
            // 
            this.Menu_Restore.Enabled = false;
            this.Menu_Restore.Name = "Menu_Restore";
            this.Menu_Restore.Size = new System.Drawing.Size(184, 22);
            this.Menu_Restore.Text = "Restore Original Files";
            // 
            // FLP_Controls
            // 
            this.FLP_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FLP_Controls.Location = new System.Drawing.Point(0, 24);
            this.FLP_Controls.Name = "FLP_Controls";
            this.FLP_Controls.Size = new System.Drawing.Size(413, 238);
            this.FLP_Controls.TabIndex = 13;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 262);
            this.Controls.Add(this.FLP_Controls);
            this.Controls.Add(this.TB_Path);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Main";
            this.Text = "pkNX";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TB_Path;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Menu_File;
        private System.Windows.Forms.ToolStripMenuItem Menu_Open;
        private System.Windows.Forms.ToolStripMenuItem Menu_Exit;
        private System.Windows.Forms.ToolStripMenuItem Menu_Options;
        private System.Windows.Forms.ToolStripMenuItem Menu_Language;
        private System.Windows.Forms.ToolStripComboBox CB_Lang;
        private System.Windows.Forms.ToolStripMenuItem randomizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Menu_SetRNGSeed;
        private System.Windows.Forms.ToolStripMenuItem Menu_Restore;
        private System.Windows.Forms.FlowLayoutPanel FLP_Controls;
    }
}

