namespace pkNX.WinForms
{
    partial class EncounterList
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
            this.NUD_SpawnRate = new System.Windows.Forms.NumericUpDown();
            this.L_SpawnRate = new System.Windows.Forms.Label();
            this.NUD_Duration = new System.Windows.Forms.NumericUpDown();
            this.L_Duration = new System.Windows.Forms.Label();
            this.L_Count = new System.Windows.Forms.Label();
            this.NUD_Count = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SpawnRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Duration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Count)).BeginInit();
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
            this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv.Location = new System.Drawing.Point(0, 53);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv.ShowCellErrors = false;
            this.dgv.ShowCellToolTips = false;
            this.dgv.ShowEditingIcon = false;
            this.dgv.ShowRowErrors = false;
            this.dgv.Size = new System.Drawing.Size(300, 267);
            this.dgv.TabIndex = 2;
            this.dgv.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_CurrentCellDirtyStateChanged);
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
            this.L_LevelMax.Location = new System.Drawing.Point(3, 26);
            this.L_LevelMax.Name = "L_LevelMax";
            this.L_LevelMax.Size = new System.Drawing.Size(56, 13);
            this.L_LevelMax.TabIndex = 5;
            this.L_LevelMax.Text = "Level Max";
            // 
            // NUD_Max
            // 
            this.NUD_Max.Location = new System.Drawing.Point(62, 24);
            this.NUD_Max.Name = "NUD_Max";
            this.NUD_Max.Size = new System.Drawing.Size(38, 20);
            this.NUD_Max.TabIndex = 6;
            this.NUD_Max.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // NUD_SpawnRate
            // 
            this.NUD_SpawnRate.Location = new System.Drawing.Point(142, 24);
            this.NUD_SpawnRate.Name = "NUD_SpawnRate";
            this.NUD_SpawnRate.Size = new System.Drawing.Size(38, 20);
            this.NUD_SpawnRate.TabIndex = 12;
            this.NUD_SpawnRate.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // L_SpawnRate
            // 
            this.L_SpawnRate.AutoSize = true;
            this.L_SpawnRate.Location = new System.Drawing.Point(114, 5);
            this.L_SpawnRate.Name = "L_SpawnRate";
            this.L_SpawnRate.Size = new System.Drawing.Size(66, 13);
            this.L_SpawnRate.TabIndex = 11;
            this.L_SpawnRate.Text = "Spawn Rate";
            // 
            // NUD_Duration
            // 
            this.NUD_Duration.Location = new System.Drawing.Point(255, 24);
            this.NUD_Duration.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NUD_Duration.Name = "NUD_Duration";
            this.NUD_Duration.Size = new System.Drawing.Size(38, 20);
            this.NUD_Duration.TabIndex = 16;
            this.NUD_Duration.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUD_Duration.Visible = false;
            // 
            // L_Duration
            // 
            this.L_Duration.AutoSize = true;
            this.L_Duration.Location = new System.Drawing.Point(196, 26);
            this.L_Duration.Name = "L_Duration";
            this.L_Duration.Size = new System.Drawing.Size(47, 13);
            this.L_Duration.TabIndex = 15;
            this.L_Duration.Text = "Duration";
            this.L_Duration.Visible = false;
            // 
            // L_Count
            // 
            this.L_Count.AutoSize = true;
            this.L_Count.Location = new System.Drawing.Point(196, 5);
            this.L_Count.Name = "L_Count";
            this.L_Count.Size = new System.Drawing.Size(58, 13);
            this.L_Count.TabIndex = 14;
            this.L_Count.Text = "Max Count";
            // 
            // NUD_Count
            // 
            this.NUD_Count.Location = new System.Drawing.Point(255, 3);
            this.NUD_Count.Name = "NUD_Count";
            this.NUD_Count.Size = new System.Drawing.Size(38, 20);
            this.NUD_Count.TabIndex = 13;
            this.NUD_Count.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            // 
            // EncounterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NUD_Duration);
            this.Controls.Add(this.L_Duration);
            this.Controls.Add(this.L_Count);
            this.Controls.Add(this.NUD_Count);
            this.Controls.Add(this.NUD_SpawnRate);
            this.Controls.Add(this.L_SpawnRate);
            this.Controls.Add(this.NUD_Max);
            this.Controls.Add(this.L_LevelMax);
            this.Controls.Add(this.L_LevelMin);
            this.Controls.Add(this.NUD_Min);
            this.Controls.Add(this.dgv);
            this.Name = "EncounterList";
            this.Size = new System.Drawing.Size(300, 320);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_SpawnRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Duration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Count)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Label L_LevelMin;
        private System.Windows.Forms.Label L_LevelMax;
        private System.Windows.Forms.Label L_SpawnRate;
        private System.Windows.Forms.Label L_Duration;
        private System.Windows.Forms.Label L_Count;
        public System.Windows.Forms.NumericUpDown NUD_Min;
        public System.Windows.Forms.NumericUpDown NUD_Max;
        public System.Windows.Forms.NumericUpDown NUD_SpawnRate;
        public System.Windows.Forms.NumericUpDown NUD_Duration;
        public System.Windows.Forms.NumericUpDown NUD_Count;
    }
}
