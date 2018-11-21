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
            this.L_Method = new System.Windows.Forms.Label();
            this.CB_Method = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            this.SuspendLayout();
            //
            // NUD_Level
            //
            this.NUD_Level.Location = new System.Drawing.Point(556, 13);
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
            this.NUD_Level.Size = new System.Drawing.Size(45, 20);
            this.NUD_Level.TabIndex = 72;
            //
            // NUD_Form
            //
            this.NUD_Form.Location = new System.Drawing.Point(505, 13);
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
            this.NUD_Form.Size = new System.Drawing.Size(45, 20);
            this.NUD_Form.TabIndex = 71;
            //
            // PB_Preview
            //
            this.PB_Preview.Location = new System.Drawing.Point(69, 5);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(40, 30);
            this.PB_Preview.TabIndex = 70;
            this.PB_Preview.TabStop = false;
            //
            // CB_Arg
            //
            this.CB_Arg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Arg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Arg.FormattingEnabled = true;
            this.CB_Arg.Location = new System.Drawing.Point(378, 12);
            this.CB_Arg.Name = "CB_Arg";
            this.CB_Arg.Size = new System.Drawing.Size(121, 21);
            this.CB_Arg.TabIndex = 69;
            //
            // CB_Species
            //
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(271, 12);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(101, 21);
            this.CB_Species.TabIndex = 68;
            //
            // L_Method
            //
            this.L_Method.Location = new System.Drawing.Point(3, 11);
            this.L_Method.Name = "L_Method";
            this.L_Method.Size = new System.Drawing.Size(60, 20);
            this.L_Method.TabIndex = 67;
            this.L_Method.Text = "Method 1:";
            this.L_Method.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // CB_Method
            //
            this.CB_Method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Method.DropDownWidth = 200;
            this.CB_Method.FormattingEnabled = true;
            this.CB_Method.Location = new System.Drawing.Point(115, 12);
            this.CB_Method.MaxDropDownItems = 10;
            this.CB_Method.Name = "CB_Method";
            this.CB_Method.Size = new System.Drawing.Size(150, 21);
            this.CB_Method.TabIndex = 66;
            //
            // EvolutionRow
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NUD_Level);
            this.Controls.Add(this.NUD_Form);
            this.Controls.Add(this.PB_Preview);
            this.Controls.Add(this.CB_Arg);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.L_Method);
            this.Controls.Add(this.CB_Method);
            this.Name = "EvolutionRow";
            this.Size = new System.Drawing.Size(611, 40);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown NUD_Level;
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.ComboBox CB_Arg;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_Method;
        private System.Windows.Forms.ComboBox CB_Method;
    }
}
