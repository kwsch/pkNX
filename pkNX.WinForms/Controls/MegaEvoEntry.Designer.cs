namespace pkNX.WinForms
{
    partial class MegaEvoEntry
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
            this.NUD_Form = new System.Windows.Forms.NumericUpDown();
            this.PB_Preview = new System.Windows.Forms.PictureBox();
            this.L_Method = new System.Windows.Forms.Label();
            this.CB_Arg = new System.Windows.Forms.ComboBox();
            this.PB_Base = new System.Windows.Forms.PictureBox();
            this.L_Into = new System.Windows.Forms.Label();
            this.CB_Method = new System.Windows.Forms.ComboBox();
            this.GB_Name = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Base)).BeginInit();
            this.GB_Name.SuspendLayout();
            this.SuspendLayout();
            // 
            // NUD_Form
            // 
            this.NUD_Form.Location = new System.Drawing.Point(82, 73);
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
            this.PB_Preview.Location = new System.Drawing.Point(87, 99);
            this.PB_Preview.Name = "PB_Preview";
            this.PB_Preview.Size = new System.Drawing.Size(68, 56);
            this.PB_Preview.TabIndex = 70;
            this.PB_Preview.TabStop = false;
            // 
            // L_Method
            // 
            this.L_Method.Location = new System.Drawing.Point(16, 73);
            this.L_Method.Name = "L_Method";
            this.L_Method.Size = new System.Drawing.Size(60, 20);
            this.L_Method.TabIndex = 67;
            this.L_Method.Text = "Form:";
            this.L_Method.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Arg
            // 
            this.CB_Arg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Arg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Arg.FormattingEnabled = true;
            this.CB_Arg.Location = new System.Drawing.Point(6, 46);
            this.CB_Arg.Name = "CB_Arg";
            this.CB_Arg.Size = new System.Drawing.Size(121, 21);
            this.CB_Arg.TabIndex = 72;
            // 
            // PB_Base
            // 
            this.PB_Base.Location = new System.Drawing.Point(6, 99);
            this.PB_Base.Name = "PB_Base";
            this.PB_Base.Size = new System.Drawing.Size(68, 56);
            this.PB_Base.TabIndex = 73;
            this.PB_Base.TabStop = false;
            // 
            // L_Into
            // 
            this.L_Into.Location = new System.Drawing.Point(27, 125);
            this.L_Into.Name = "L_Into";
            this.L_Into.Size = new System.Drawing.Size(60, 20);
            this.L_Into.TabIndex = 74;
            this.L_Into.Text = "Into ->";
            this.L_Into.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CB_Method
            // 
            this.CB_Method.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Method.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Method.FormattingEnabled = true;
            this.CB_Method.Location = new System.Drawing.Point(6, 19);
            this.CB_Method.Name = "CB_Method";
            this.CB_Method.Size = new System.Drawing.Size(121, 21);
            this.CB_Method.TabIndex = 75;
            // 
            // GB_Name
            // 
            this.GB_Name.Controls.Add(this.PB_Base);
            this.GB_Name.Controls.Add(this.CB_Method);
            this.GB_Name.Controls.Add(this.L_Into);
            this.GB_Name.Controls.Add(this.L_Method);
            this.GB_Name.Controls.Add(this.CB_Arg);
            this.GB_Name.Controls.Add(this.PB_Preview);
            this.GB_Name.Controls.Add(this.NUD_Form);
            this.GB_Name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GB_Name.Location = new System.Drawing.Point(0, 0);
            this.GB_Name.Name = "GB_Name";
            this.GB_Name.Size = new System.Drawing.Size(160, 160);
            this.GB_Name.TabIndex = 76;
            this.GB_Name.TabStop = false;
            this.GB_Name.Text = "Mega";
            // 
            // MegaEvoEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GB_Name);
            this.Name = "MegaEvoEntry";
            this.Size = new System.Drawing.Size(160, 160);
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Form)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Preview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Base)).EndInit();
            this.GB_Name.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown NUD_Form;
        private System.Windows.Forms.PictureBox PB_Preview;
        private System.Windows.Forms.Label L_Method;
        private System.Windows.Forms.ComboBox CB_Arg;
        private System.Windows.Forms.PictureBox PB_Base;
        private System.Windows.Forms.Label L_Into;
        private System.Windows.Forms.ComboBox CB_Method;
        private System.Windows.Forms.GroupBox GB_Name;
    }
}
