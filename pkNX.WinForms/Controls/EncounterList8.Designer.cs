namespace pkNX.WinForms
{
    partial class EncounterList8
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.NUD_Min = new System.Windows.Forms.NumericUpDown();
            this.L_LevelMin = new System.Windows.Forms.Label();
            this.L_LevelMax = new System.Windows.Forms.Label();
            this.NUD_Max = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Max)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv.Location = new System.Drawing.Point(0, 29);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv.ShowCellErrors = false;
            this.dgv.ShowCellToolTips = false;
            this.dgv.ShowEditingIcon = false;
            this.dgv.ShowRowErrors = false;
            this.dgv.Size = new System.Drawing.Size(300, 291);
            this.dgv.TabIndex = 2;
            this.dgv.CurrentCellDirtyStateChanged += new System.EventHandler(this.CurrentCellDirtyStateChanged);
            // 
            // NUD_Min
            // 
            this.NUD_Min.Location = new System.Drawing.Point(62, 3);
            this.NUD_Min.Name = "NUD_Min";
            this.NUD_Min.Size = new System.Drawing.Size(38, 20);
            this.NUD_Min.TabIndex = 3;
            this.NUD_Min.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_LevelMin
            // 
            this.L_LevelMin.AutoSize = true;
            this.L_LevelMin.Location = new System.Drawing.Point(3, 5);
            this.L_LevelMin.Name = "L_LevelMin";
            this.L_LevelMin.Size = new System.Drawing.Size(53, 13);
            this.L_LevelMin.TabIndex = 4;
            this.L_LevelMin.Text = "Level Min";
            // 
            // L_LevelMax
            // 
            this.L_LevelMax.AutoSize = true;
            this.L_LevelMax.Location = new System.Drawing.Point(113, 5);
            this.L_LevelMax.Name = "L_LevelMax";
            this.L_LevelMax.Size = new System.Drawing.Size(56, 13);
            this.L_LevelMax.TabIndex = 5;
            this.L_LevelMax.Text = "Level Max";
            // 
            // NUD_Max
            // 
            this.NUD_Max.Location = new System.Drawing.Point(172, 3);
            this.NUD_Max.Name = "NUD_Max";
            this.NUD_Max.Size = new System.Drawing.Size(38, 20);
            this.NUD_Max.TabIndex = 6;
            this.NUD_Max.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // EncounterList8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NUD_Max);
            this.Controls.Add(this.L_LevelMax);
            this.Controls.Add(this.L_LevelMin);
            this.Controls.Add(this.NUD_Min);
            this.Controls.Add(this.dgv);
            this.Name = "EncounterList8";
            this.Size = new System.Drawing.Size(300, 320);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Max)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label L_LevelMin;
        private System.Windows.Forms.Label L_LevelMax;
        public System.Windows.Forms.NumericUpDown NUD_Min;
        public System.Windows.Forms.NumericUpDown NUD_Max;
    }
}
