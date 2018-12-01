namespace pkNX.WinForms
{
    partial class ShinyRate
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
            this.B_Save = new System.Windows.Forms.Button();
            this.RB_Default = new System.Windows.Forms.RadioButton();
            this.RB_Fixed = new System.Windows.Forms.RadioButton();
            this.RB_Always = new System.Windows.Forms.RadioButton();
            this.GB_Rerolls = new System.Windows.Forms.GroupBox();
            this.NUD_Rerolls = new System.Windows.Forms.NumericUpDown();
            this.L_Overall = new System.Windows.Forms.Label();
            this.GB_RerollHelper = new System.Windows.Forms.GroupBox();
            this.NUD_Rate = new System.Windows.Forms.NumericUpDown();
            this.L_RerollOverall = new System.Windows.Forms.Label();
            this.L_RerollCount = new System.Windows.Forms.Label();
            this.L_Note = new System.Windows.Forms.Label();
            this.GB_Rerolls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).BeginInit();
            this.GB_RerollHelper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rate)).BeginInit();
            this.SuspendLayout();
            //
            // B_Save
            //
            this.B_Save.Location = new System.Drawing.Point(239, 215);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(60, 23);
            this.B_Save.TabIndex = 1;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            //
            // RB_Default
            //
            this.RB_Default.AutoSize = true;
            this.RB_Default.Location = new System.Drawing.Point(122, 96);
            this.RB_Default.Name = "RB_Default";
            this.RB_Default.Size = new System.Drawing.Size(59, 17);
            this.RB_Default.TabIndex = 3;
            this.RB_Default.TabStop = true;
            this.RB_Default.Text = "Default";
            this.RB_Default.UseVisualStyleBackColor = true;
            this.RB_Default.CheckedChanged += new System.EventHandler(this.ChangeSelection);
            //
            // RB_Fixed
            //
            this.RB_Fixed.AutoSize = true;
            this.RB_Fixed.Location = new System.Drawing.Point(122, 119);
            this.RB_Fixed.Name = "RB_Fixed";
            this.RB_Fixed.Size = new System.Drawing.Size(111, 17);
            this.RB_Fixed.TabIndex = 4;
            this.RB_Fixed.TabStop = true;
            this.RB_Fixed.Text = "Fixed Reroll Count";
            this.RB_Fixed.UseVisualStyleBackColor = true;
            this.RB_Fixed.CheckedChanged += new System.EventHandler(this.ChangeSelection);
            //
            // RB_Always
            //
            this.RB_Always.AutoSize = true;
            this.RB_Always.Location = new System.Drawing.Point(122, 142);
            this.RB_Always.Name = "RB_Always";
            this.RB_Always.Size = new System.Drawing.Size(87, 17);
            this.RB_Always.TabIndex = 5;
            this.RB_Always.TabStop = true;
            this.RB_Always.Text = "Always Shiny";
            this.RB_Always.UseVisualStyleBackColor = true;
            this.RB_Always.CheckedChanged += new System.EventHandler(this.ChangeSelection);
            //
            // GB_Rerolls
            //
            this.GB_Rerolls.Controls.Add(this.NUD_Rerolls);
            this.GB_Rerolls.Controls.Add(this.L_Overall);
            this.GB_Rerolls.ForeColor = System.Drawing.Color.Red;
            this.GB_Rerolls.Location = new System.Drawing.Point(12, 87);
            this.GB_Rerolls.Name = "GB_Rerolls";
            this.GB_Rerolls.Size = new System.Drawing.Size(105, 76);
            this.GB_Rerolls.TabIndex = 15;
            this.GB_Rerolls.TabStop = false;
            this.GB_Rerolls.Text = "PID Generation Loop Rerolls";
            //
            // NUD_Rerolls
            //
            this.NUD_Rerolls.Location = new System.Drawing.Point(16, 33);
            this.NUD_Rerolls.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NUD_Rerolls.Name = "NUD_Rerolls";
            this.NUD_Rerolls.Size = new System.Drawing.Size(65, 20);
            this.NUD_Rerolls.TabIndex = 1;
            this.NUD_Rerolls.Value = new decimal(new int[] {
            125,
            0,
            0,
            0});
            this.NUD_Rerolls.ValueChanged += new System.EventHandler(this.ChangeRerollCount);
            //
            // L_Overall
            //
            this.L_Overall.AutoSize = true;
            this.L_Overall.ForeColor = System.Drawing.SystemColors.ControlText;
            this.L_Overall.Location = new System.Drawing.Point(53, 56);
            this.L_Overall.Name = "L_Overall";
            this.L_Overall.Size = new System.Drawing.Size(28, 13);
            this.L_Overall.TabIndex = 2;
            this.L_Overall.Text = "PCT";
            //
            // GB_RerollHelper
            //
            this.GB_RerollHelper.Controls.Add(this.NUD_Rate);
            this.GB_RerollHelper.Controls.Add(this.L_RerollOverall);
            this.GB_RerollHelper.Controls.Add(this.L_RerollCount);
            this.GB_RerollHelper.Location = new System.Drawing.Point(12, 175);
            this.GB_RerollHelper.Name = "GB_RerollHelper";
            this.GB_RerollHelper.Size = new System.Drawing.Size(169, 63);
            this.GB_RerollHelper.TabIndex = 14;
            this.GB_RerollHelper.TabStop = false;
            this.GB_RerollHelper.Text = "Reroll Helper";
            //
            // NUD_Rate
            //
            this.NUD_Rate.DecimalPlaces = 2;
            this.NUD_Rate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_Rate.Location = new System.Drawing.Point(81, 13);
            this.NUD_Rate.Name = "NUD_Rate";
            this.NUD_Rate.Size = new System.Drawing.Size(65, 20);
            this.NUD_Rate.TabIndex = 7;
            this.NUD_Rate.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NUD_Rate.ValueChanged += new System.EventHandler(this.ChangePercent);
            //
            // L_RerollOverall
            //
            this.L_RerollOverall.AutoSize = true;
            this.L_RerollOverall.Location = new System.Drawing.Point(24, 15);
            this.L_RerollOverall.Name = "L_RerollOverall";
            this.L_RerollOverall.Size = new System.Drawing.Size(51, 13);
            this.L_RerollOverall.TabIndex = 9;
            this.L_RerollOverall.Text = "Overall%:";
            //
            // L_RerollCount
            //
            this.L_RerollCount.AutoSize = true;
            this.L_RerollCount.Location = new System.Drawing.Point(24, 37);
            this.L_RerollCount.Name = "L_RerollCount";
            this.L_RerollCount.Size = new System.Drawing.Size(47, 13);
            this.L_RerollCount.TabIndex = 8;
            this.L_RerollCount.Text = "Count: 0";
            //
            // L_Note
            //
            this.L_Note.Location = new System.Drawing.Point(12, 7);
            this.L_Note.Name = "L_Note";
            this.L_Note.Size = new System.Drawing.Size(295, 77);
            this.L_Note.TabIndex = 13;
            this.L_Note.Text = "Note: \r\nChanging the rate only changes the amount of PID rerolls.\r\nChanging the r" +
    "ate does not alter the \"IsShiny\" determination.\r\n\r\nThink of it like a frozen sup" +
    "er-Shiny Charm.";
            //
            // ShinyRate
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 247);
            this.Controls.Add(this.GB_Rerolls);
            this.Controls.Add(this.GB_RerollHelper);
            this.Controls.Add(this.L_Note);
            this.Controls.Add(this.RB_Always);
            this.Controls.Add(this.RB_Fixed);
            this.Controls.Add(this.RB_Default);
            this.Controls.Add(this.B_Save);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ShinyRate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shiny Rate";
            this.GB_Rerolls.ResumeLayout(false);
            this.GB_Rerolls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rerolls)).EndInit();
            this.GB_RerollHelper.ResumeLayout(false);
            this.GB_RerollHelper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Rate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.RadioButton RB_Default;
        private System.Windows.Forms.RadioButton RB_Fixed;
        private System.Windows.Forms.RadioButton RB_Always;
        private System.Windows.Forms.GroupBox GB_Rerolls;
        private System.Windows.Forms.NumericUpDown NUD_Rerolls;
        private System.Windows.Forms.Label L_Overall;
        private System.Windows.Forms.GroupBox GB_RerollHelper;
        private System.Windows.Forms.NumericUpDown NUD_Rate;
        private System.Windows.Forms.Label L_RerollOverall;
        private System.Windows.Forms.Label L_RerollCount;
        private System.Windows.Forms.Label L_Note;
    }
}