namespace pkNX.WinForms
{
    partial class EvolutionRow
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NUD_Level = new System.Windows.Forms.NumericUpDown();
            this.NUD_Form = new System.Windows.Forms.NumericUpDown();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.CB_Arg = new System.Windows.Forms.ComboBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.CB_Method = new System.Windows.Forms.ComboBox();
            this.L_Method = new System.Windows.Forms.Label();
            this.L_Species = new System.Windows.Forms.Label();
            this.L_Form = new System.Windows.Forms.Label();
            this.L_Level = new System.Windows.Forms.Label();
            this.L_Arg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.SuspendLayout();
            // 
            // NUD_Level
            // 
            this.NUD_Level.Location = new System.Drawing.Point(469, 41);
            this.NUD_Level.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NUD_Level.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.NUD_Level.Name = "NUD_Level";
            this.NUD_Level.Size = new System.Drawing.Size(38, 20);
            this.NUD_Level.TabIndex = 72;
            this.NUD_Level.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // NUD_Form
            // 
            this.NUD_Form.Location = new System.Drawing.Point(429, 41);
            this.NUD_Form.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NUD_Form.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.NUD_Form.Name = "NUD_Form";
            this.NUD_Form.Size = new System.Drawing.Size(38, 20);
            this.NUD_Form.TabIndex = 71;
            this.NUD_Form.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // PB_Preview
            // 
            this.PB_Preview.Location = new System.Drawing.Point(0, 5);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(68, 56);
            this.PB_Preview.TabIndex = 70;
            this.PB_Preview.TabStop = false;
            // 
            // CB_Arg
            // 
            this.CB_Arg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Arg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Arg.FormattingEnabled = true;
            this.CB_Arg.Location = new System.Drawing.Point(308, 40);
            this.CB_Arg.Name = "CB_Arg";
            this.CB_Arg.Size = new System.Drawing.Size(120, 21);
            this.CB_Arg.TabIndex = 69;
            this.CB_Arg.Visible = false;
            // 
            // CB_Species
            // 
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(220, 40);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(87, 21);
            this.CB_Species.TabIndex = 68;
            // 
            // CB_Method
            // 
            this.CB_Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Method.DropDownWidth = 250;
            this.CB_Method.FormattingEnabled = true;
            this.CB_Method.Location = new System.Drawing.Point(69, 40);
            this.CB_Method.MaxDropDownItems = 10;
            this.CB_Method.Name = "CB_Method";
            this.CB_Method.Size = new System.Drawing.Size(150, 21);
            this.CB_Method.TabIndex = 66;
            // 
            // L_Method
            // 
            this.L_Method.AutoSize = true;
            this.L_Method.Location = new System.Drawing.Point(68, 24);
            this.L_Method.Name = "L_Method";
            this.L_Method.Size = new System.Drawing.Size(43, 13);
            this.L_Method.TabIndex = 73;
            this.L_Method.Text = "Method";
            // 
            // L_Species
            // 
            this.L_Species.AutoSize = true;
            this.L_Species.Location = new System.Drawing.Point(219, 24);
            this.L_Species.Name = "L_Species";
            this.L_Species.Size = new System.Drawing.Size(66, 13);
            this.L_Species.TabIndex = 74;
            this.L_Species.Text = "Evolves Into";
            // 
            // L_Form
            // 
            this.L_Form.AutoSize = true;
            this.L_Form.Location = new System.Drawing.Point(428, 24);
            this.L_Form.Name = "L_Form";
            this.L_Form.Size = new System.Drawing.Size(30, 13);
            this.L_Form.TabIndex = 75;
            this.L_Form.Text = "Form";
            // 
            // L_Level
            // 
            this.L_Level.AutoSize = true;
            this.L_Level.Location = new System.Drawing.Point(468, 24);
            this.L_Level.Name = "L_Level";
            this.L_Level.Size = new System.Drawing.Size(33, 13);
            this.L_Level.TabIndex = 76;
            this.L_Level.Text = "Level";
            // 
            // L_Arg
            // 
            this.L_Arg.AutoSize = true;
            this.L_Arg.Location = new System.Drawing.Point(307, 24);
            this.L_Arg.Name = "L_Arg";
            this.L_Arg.Size = new System.Drawing.Size(52, 13);
            this.L_Arg.TabIndex = 77;
            this.L_Arg.Text = "Argument";
            // 
            // EvolutionRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.L_Arg);
            this.Controls.Add(this.L_Level);
            this.Controls.Add(this.L_Form);
            this.Controls.Add(this.L_Species);
            this.Controls.Add(this.L_Method);
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.NUD_Form);
            this.Controls.Add(this.PB_Preview);
            this.Controls.Add(this.CB_Arg);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.CB_Method);
            this.Name = "EvolutionRow";
            this.Size = new System.Drawing.Size(530, 66);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.ComboBox CB_Arg;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.ComboBox CB_Method;
        private System.Windows.Forms.Label L_Method;
        private System.Windows.Forms.Label L_Species;
        private System.Windows.Forms.Label L_Form;
        private System.Windows.Forms.Label L_Level;
        private System.Windows.Forms.Label L_Arg;
    }
}
