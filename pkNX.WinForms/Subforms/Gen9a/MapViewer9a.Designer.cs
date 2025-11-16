namespace pkNX.WinForms.Subforms
{
    partial class MapViewer9a
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
            PB_Map = new System.Windows.Forms.PictureBox();
            CB_AreaIndex = new System.Windows.Forms.ComboBox();
            CB_Map = new System.Windows.Forms.ComboBox();
            CB_Species = new System.Windows.Forms.ComboBox();
            L_SpeciesFilter = new System.Windows.Forms.Label();
            B_ExportSpawnsToClipboard = new System.Windows.Forms.Button();
            TB_ObjectName = new System.Windows.Forms.TextBox();
            L_FilterName = new System.Windows.Forms.Label();
            L_PointName = new System.Windows.Forms.Label();
            TB_PointName = new System.Windows.Forms.TextBox();
            CHK_ExportImages = new System.Windows.Forms.CheckBox();
            B_ExportImage = new System.Windows.Forms.Button();
            NUD_CollisionTransparency = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            RTB_Analysis = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)PB_Map).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NUD_CollisionTransparency).BeginInit();
            SuspendLayout();
            // 
            // PB_Map
            // 
            PB_Map.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PB_Map.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            PB_Map.Location = new System.Drawing.Point(0, 0);
            PB_Map.Margin = new System.Windows.Forms.Padding(0);
            PB_Map.Name = "PB_Map";
            PB_Map.Size = new System.Drawing.Size(512, 512);
            PB_Map.TabIndex = 0;
            PB_Map.TabStop = false;
            PB_Map.MouseMove += Map_MouseMove;
            // 
            // CB_AreaIndex
            // 
            CB_AreaIndex.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CB_AreaIndex.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            CB_AreaIndex.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            CB_AreaIndex.DropDownWidth = 300;
            CB_AreaIndex.FormattingEnabled = true;
            CB_AreaIndex.Location = new System.Drawing.Point(516, 33);
            CB_AreaIndex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CB_AreaIndex.Name = "CB_AreaIndex";
            CB_AreaIndex.Size = new System.Drawing.Size(296, 25);
            CB_AreaIndex.TabIndex = 1;
            CB_AreaIndex.SelectedIndexChanged += ChangeAreaIndex;
            // 
            // CB_Map
            // 
            CB_Map.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CB_Map.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CB_Map.FormattingEnabled = true;
            CB_Map.Location = new System.Drawing.Point(516, 0);
            CB_Map.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CB_Map.Name = "CB_Map";
            CB_Map.Size = new System.Drawing.Size(140, 25);
            CB_Map.TabIndex = 2;
            CB_Map.SelectedIndexChanged += ChangeMap;
            // 
            // CB_Species
            // 
            CB_Species.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CB_Species.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            CB_Species.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            CB_Species.DropDownWidth = 300;
            CB_Species.FormattingEnabled = true;
            CB_Species.Location = new System.Drawing.Point(517, 109);
            CB_Species.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CB_Species.Name = "CB_Species";
            CB_Species.Size = new System.Drawing.Size(295, 25);
            CB_Species.TabIndex = 3;
            CB_Species.SelectedValueChanged += ChangeSpawnFilter;
            // 
            // L_SpeciesFilter
            // 
            L_SpeciesFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            L_SpeciesFilter.AutoSize = true;
            L_SpeciesFilter.Location = new System.Drawing.Point(517, 89);
            L_SpeciesFilter.Name = "L_SpeciesFilter";
            L_SpeciesFilter.Size = new System.Drawing.Size(55, 17);
            L_SpeciesFilter.TabIndex = 4;
            L_SpeciesFilter.Text = "Species:";
            // 
            // B_ExportSpawnsToClipboard
            // 
            B_ExportSpawnsToClipboard.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            B_ExportSpawnsToClipboard.Location = new System.Drawing.Point(664, 437);
            B_ExportSpawnsToClipboard.Name = "B_ExportSpawnsToClipboard";
            B_ExportSpawnsToClipboard.Size = new System.Drawing.Size(148, 75);
            B_ExportSpawnsToClipboard.TabIndex = 5;
            B_ExportSpawnsToClipboard.Text = "Export Spawners To Clipboard";
            B_ExportSpawnsToClipboard.UseVisualStyleBackColor = true;
            B_ExportSpawnsToClipboard.Click += B_ExportSpawnsToClipboard_Click;
            // 
            // TB_ObjectName
            // 
            TB_ObjectName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TB_ObjectName.Location = new System.Drawing.Point(516, 157);
            TB_ObjectName.Name = "TB_ObjectName";
            TB_ObjectName.Size = new System.Drawing.Size(296, 25);
            TB_ObjectName.TabIndex = 6;
            TB_ObjectName.TextChanged += ChangeSpawnFilter;
            // 
            // L_FilterName
            // 
            L_FilterName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            L_FilterName.AutoSize = true;
            L_FilterName.Location = new System.Drawing.Point(516, 137);
            L_FilterName.Name = "L_FilterName";
            L_FilterName.Size = new System.Drawing.Size(88, 17);
            L_FilterName.TabIndex = 7;
            L_FilterName.Text = "Object Name:";
            // 
            // L_PointName
            // 
            L_PointName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            L_PointName.AutoSize = true;
            L_PointName.Location = new System.Drawing.Point(516, 185);
            L_PointName.Name = "L_PointName";
            L_PointName.Size = new System.Drawing.Size(79, 17);
            L_PointName.TabIndex = 9;
            L_PointName.Text = "Point Name:";
            // 
            // TB_PointName
            // 
            TB_PointName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TB_PointName.Location = new System.Drawing.Point(516, 205);
            TB_PointName.Name = "TB_PointName";
            TB_PointName.Size = new System.Drawing.Size(296, 25);
            TB_PointName.TabIndex = 8;
            TB_PointName.TextChanged += ChangeSpawnFilter;
            // 
            // CHK_ExportImages
            // 
            CHK_ExportImages.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            CHK_ExportImages.AutoSize = true;
            CHK_ExportImages.Location = new System.Drawing.Point(518, 418);
            CHK_ExportImages.Name = "CHK_ExportImages";
            CHK_ExportImages.Size = new System.Drawing.Size(54, 21);
            CHK_ExportImages.TabIndex = 10;
            CHK_ExportImages.Text = "Auto";
            CHK_ExportImages.UseVisualStyleBackColor = true;
            // 
            // B_ExportImage
            // 
            B_ExportImage.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            B_ExportImage.Location = new System.Drawing.Point(516, 437);
            B_ExportImage.Name = "B_ExportImage";
            B_ExportImage.Size = new System.Drawing.Size(148, 75);
            B_ExportImage.TabIndex = 11;
            B_ExportImage.Text = "Export Current Image";
            B_ExportImage.UseVisualStyleBackColor = true;
            B_ExportImage.Click += B_ExportImage_Click;
            // 
            // NUD_CollisionTransparency
            // 
            NUD_CollisionTransparency.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            NUD_CollisionTransparency.Location = new System.Drawing.Point(761, 64);
            NUD_CollisionTransparency.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            NUD_CollisionTransparency.Name = "NUD_CollisionTransparency";
            NUD_CollisionTransparency.Size = new System.Drawing.Size(51, 25);
            NUD_CollisionTransparency.TabIndex = 12;
            NUD_CollisionTransparency.Value = new decimal(new int[] { 34, 0, 0, 0 });
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label1.Location = new System.Drawing.Point(618, 64);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(139, 25);
            label1.TabIndex = 13;
            label1.Text = "Transparency:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RTB_Analysis
            // 
            RTB_Analysis.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            RTB_Analysis.Location = new System.Drawing.Point(517, 236);
            RTB_Analysis.Name = "RTB_Analysis";
            RTB_Analysis.ReadOnly = true;
            RTB_Analysis.Size = new System.Drawing.Size(295, 176);
            RTB_Analysis.TabIndex = 15;
            RTB_Analysis.Text = "";
            // 
            // MapViewer9a
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(816, 512);
            Controls.Add(RTB_Analysis);
            Controls.Add(label1);
            Controls.Add(NUD_CollisionTransparency);
            Controls.Add(B_ExportImage);
            Controls.Add(CHK_ExportImages);
            Controls.Add(L_PointName);
            Controls.Add(TB_PointName);
            Controls.Add(L_FilterName);
            Controls.Add(TB_ObjectName);
            Controls.Add(B_ExportSpawnsToClipboard);
            Controls.Add(L_SpeciesFilter);
            Controls.Add(CB_Species);
            Controls.Add(CB_Map);
            Controls.Add(CB_AreaIndex);
            Controls.Add(PB_Map);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MapViewer9a";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Pokémon Legends: Z-A Map Viewer (2D)";
            ((System.ComponentModel.ISupportInitialize)PB_Map).EndInit();
            ((System.ComponentModel.ISupportInitialize)NUD_CollisionTransparency).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox PB_Map;
        private System.Windows.Forms.ComboBox CB_AreaIndex;
        private System.Windows.Forms.ComboBox CB_Map;
        private System.Windows.Forms.ComboBox CB_Species;
        private System.Windows.Forms.Label L_SpeciesFilter;
        private System.Windows.Forms.Button B_ExportSpawnsToClipboard;
        private System.Windows.Forms.TextBox TB_ObjectName;
        private System.Windows.Forms.Label L_FilterName;
        private System.Windows.Forms.Label L_PointName;
        private System.Windows.Forms.TextBox TB_PointName;
        private System.Windows.Forms.CheckBox CHK_ExportImages;
        private System.Windows.Forms.Button B_ExportImage;
        private System.Windows.Forms.NumericUpDown NUD_CollisionTransparency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox RTB_Analysis;
    }
}
