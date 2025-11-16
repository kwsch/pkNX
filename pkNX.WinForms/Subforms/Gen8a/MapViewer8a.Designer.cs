namespace pkNX.WinForms.Subforms
{
    partial class MapViewer8a
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapViewer8a));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CB_Map = new System.Windows.Forms.ComboBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            this.L_CoordinateMouse = new System.Windows.Forms.Label();
            this.L_SpawnDump = new System.Windows.Forms.Label();
            this.NUD_Tolerance = new System.Windows.Forms.NumericUpDown();
            this.L_Tolerance = new System.Windows.Forms.Label();
            this.L_SpeciesFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Tolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapViewer8a_MouseMove);
            // 
            // CB_Map
            // 
            this.CB_Map.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Map.FormattingEnabled = true;
            this.CB_Map.Location = new System.Drawing.Point(530, 12);
            this.CB_Map.Name = "CB_Map";
            this.CB_Map.Size = new System.Drawing.Size(121, 21);
            this.CB_Map.TabIndex = 1;
            this.CB_Map.SelectedIndexChanged += new System.EventHandler(this.CB_Map_SelectedIndexChanged);
            // 
            // CB_Species
            // 
            this.CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Species.FormattingEnabled = true;
            this.CB_Species.Location = new System.Drawing.Point(530, 138);
            this.CB_Species.Name = "CB_Species";
            this.CB_Species.Size = new System.Drawing.Size(121, 21);
            this.CB_Species.TabIndex = 2;
            this.CB_Species.SelectedIndexChanged += new System.EventHandler(this.CB_Species_SelectedIndexChanged);
            // 
            // L_CoordinateMouse
            // 
            this.L_CoordinateMouse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_CoordinateMouse.AutoSize = true;
            this.L_CoordinateMouse.Location = new System.Drawing.Point(527, 510);
            this.L_CoordinateMouse.Name = "L_CoordinateMouse";
            this.L_CoordinateMouse.Size = new System.Drawing.Size(153, 13);
            this.L_CoordinateMouse.TabIndex = 3;
            this.L_CoordinateMouse.Text = "Hover over img for Coordinates";
            // 
            // L_SpawnDump
            // 
            this.L_SpawnDump.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.L_SpawnDump.Location = new System.Drawing.Point(530, 162);
            this.L_SpawnDump.Name = "L_SpawnDump";
            this.L_SpawnDump.Size = new System.Drawing.Size(176, 348);
            this.L_SpawnDump.TabIndex = 5;
            this.L_SpawnDump.Text = "~";
            this.L_SpawnDump.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // NUD_Tolerance
            // 
            this.NUD_Tolerance.Location = new System.Drawing.Point(612, 93);
            this.NUD_Tolerance.Name = "NUD_Tolerance";
            this.NUD_Tolerance.Size = new System.Drawing.Size(40, 20);
            this.NUD_Tolerance.TabIndex = 6;
            this.NUD_Tolerance.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // L_Tolerance
            // 
            this.L_Tolerance.AutoSize = true;
            this.L_Tolerance.Location = new System.Drawing.Point(530, 77);
            this.L_Tolerance.Name = "L_Tolerance";
            this.L_Tolerance.Size = new System.Drawing.Size(122, 13);
            this.L_Tolerance.TabIndex = 7;
            this.L_Tolerance.Text = "Mouse Hover Tolerance";
            // 
            // L_SpeciesFilter
            // 
            this.L_SpeciesFilter.AutoSize = true;
            this.L_SpeciesFilter.Location = new System.Drawing.Point(530, 122);
            this.L_SpeciesFilter.Name = "L_SpeciesFilter";
            this.L_SpeciesFilter.Size = new System.Drawing.Size(70, 13);
            this.L_SpeciesFilter.TabIndex = 8;
            this.L_SpeciesFilter.Text = "Species Filter";
            // 
            // MapViewer8a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 532);
            this.Controls.Add(this.L_SpeciesFilter);
            this.Controls.Add(this.L_Tolerance);
            this.Controls.Add(this.NUD_Tolerance);
            this.Controls.Add(this.L_SpawnDump);
            this.Controls.Add(this.L_CoordinateMouse);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.CB_Map);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapViewer8a";
            this.Text = "MapViewer8a";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_Tolerance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox CB_Map;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_CoordinateMouse;
        private System.Windows.Forms.Label L_SpawnDump;
        private System.Windows.Forms.NumericUpDown NUD_Tolerance;
        private System.Windows.Forms.Label L_Tolerance;
        private System.Windows.Forms.Label L_SpeciesFilter;
    }
}