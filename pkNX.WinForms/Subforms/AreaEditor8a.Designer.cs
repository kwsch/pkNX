﻿namespace pkNX.WinForms.Subforms
{
    partial class AreaEditor8a
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AreaEditor8a));
            this.CB_Area = new System.Windows.Forms.ComboBox();
            this.TC_Editor = new System.Windows.Forms.TabControl();
            this.Tab_Encounters = new System.Windows.Forms.TabPage();
            this.Edit_Encounters = new pkNX.WinForms.Controls.EncounterTableEditor8a();
            this.Tab_Regular = new System.Windows.Forms.TabPage();
            this.Edit_RegularSpawners = new pkNX.WinForms.Controls.PlacementSpawnerEditor8a();
            this.Tab_Wormhole = new System.Windows.Forms.TabPage();
            this.Edit_WormholeSpawners = new pkNX.WinForms.Controls.PlacementSpawnerEditor8a();
            this.Tab_Landmarks = new System.Windows.Forms.TabPage();
            this.Edit_LandmarkSpawns = new pkNX.WinForms.Controls.LandmarkEditor8a();
            this.Tab_Randomize = new System.Windows.Forms.TabPage();
            this.B_Randomize = new System.Windows.Forms.Button();
            this.PG_RandSettings = new System.Windows.Forms.PropertyGrid();
            this.B_Save = new System.Windows.Forms.Button();
            this.TC_Editor.SuspendLayout();
            this.Tab_Encounters.SuspendLayout();
            this.Tab_Regular.SuspendLayout();
            this.Tab_Wormhole.SuspendLayout();
            this.Tab_Landmarks.SuspendLayout();
            this.Tab_Randomize.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_Area
            // 
            this.CB_Area.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Area.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Area.FormattingEnabled = true;
            this.CB_Area.Location = new System.Drawing.Point(13, 14);
            this.CB_Area.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CB_Area.Name = "CB_Area";
            this.CB_Area.Size = new System.Drawing.Size(316, 28);
            this.CB_Area.TabIndex = 1;
            this.CB_Area.SelectedIndexChanged += new System.EventHandler(this.CB_Map_SelectedIndexChanged);
            // 
            // TC_Editor
            // 
            this.TC_Editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TC_Editor.Controls.Add(this.Tab_Encounters);
            this.TC_Editor.Controls.Add(this.Tab_Regular);
            this.TC_Editor.Controls.Add(this.Tab_Wormhole);
            this.TC_Editor.Controls.Add(this.Tab_Landmarks);
            this.TC_Editor.Controls.Add(this.Tab_Randomize);
            this.TC_Editor.Location = new System.Drawing.Point(13, 60);
            this.TC_Editor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TC_Editor.MinimumSize = new System.Drawing.Size(773, 474);
            this.TC_Editor.Name = "TC_Editor";
            this.TC_Editor.SelectedIndex = 0;
            this.TC_Editor.Size = new System.Drawing.Size(1252, 750);
            this.TC_Editor.TabIndex = 2;
            // 
            // Tab_Encounters
            // 
            this.Tab_Encounters.Controls.Add(this.Edit_Encounters);
            this.Tab_Encounters.Location = new System.Drawing.Point(4, 29);
            this.Tab_Encounters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Encounters.Name = "Tab_Encounters";
            this.Tab_Encounters.Size = new System.Drawing.Size(1244, 717);
            this.Tab_Encounters.TabIndex = 0;
            this.Tab_Encounters.Text = "Encounters";
            this.Tab_Encounters.UseVisualStyleBackColor = true;
            // 
            // Edit_Encounters
            // 
            this.Edit_Encounters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_Encounters.Location = new System.Drawing.Point(0, 0);
            this.Edit_Encounters.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Edit_Encounters.Name = "Edit_Encounters";
            this.Edit_Encounters.Size = new System.Drawing.Size(1244, 717);
            this.Edit_Encounters.TabIndex = 0;
            // 
            // Tab_Regular
            // 
            this.Tab_Regular.Controls.Add(this.Edit_RegularSpawners);
            this.Tab_Regular.Location = new System.Drawing.Point(4, 29);
            this.Tab_Regular.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Regular.Name = "Tab_Regular";
            this.Tab_Regular.Size = new System.Drawing.Size(1244, 717);
            this.Tab_Regular.TabIndex = 1;
            this.Tab_Regular.Text = "*Regular";
            this.Tab_Regular.UseVisualStyleBackColor = true;
            // 
            // Edit_RegularSpawners
            // 
            this.Edit_RegularSpawners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_RegularSpawners.Location = new System.Drawing.Point(0, 0);
            this.Edit_RegularSpawners.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Edit_RegularSpawners.Name = "Edit_RegularSpawners";
            this.Edit_RegularSpawners.Size = new System.Drawing.Size(1244, 717);
            this.Edit_RegularSpawners.TabIndex = 1;
            // 
            // Tab_Wormhole
            // 
            this.Tab_Wormhole.Controls.Add(this.Edit_WormholeSpawners);
            this.Tab_Wormhole.Location = new System.Drawing.Point(4, 29);
            this.Tab_Wormhole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Wormhole.Name = "Tab_Wormhole";
            this.Tab_Wormhole.Size = new System.Drawing.Size(1244, 717);
            this.Tab_Wormhole.TabIndex = 2;
            this.Tab_Wormhole.Text = "*Wormhole";
            this.Tab_Wormhole.UseVisualStyleBackColor = true;
            // 
            // Edit_WormholeSpawners
            // 
            this.Edit_WormholeSpawners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_WormholeSpawners.Location = new System.Drawing.Point(0, 0);
            this.Edit_WormholeSpawners.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Edit_WormholeSpawners.Name = "Edit_WormholeSpawners";
            this.Edit_WormholeSpawners.Size = new System.Drawing.Size(1244, 717);
            this.Edit_WormholeSpawners.TabIndex = 2;
            // 
            // Tab_Landmarks
            // 
            this.Tab_Landmarks.Controls.Add(this.Edit_LandmarkSpawns);
            this.Tab_Landmarks.Location = new System.Drawing.Point(4, 29);
            this.Tab_Landmarks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Landmarks.Name = "Tab_Landmarks";
            this.Tab_Landmarks.Size = new System.Drawing.Size(1244, 717);
            this.Tab_Landmarks.TabIndex = 3;
            this.Tab_Landmarks.Text = "*Landmarks";
            this.Tab_Landmarks.UseVisualStyleBackColor = true;
            // 
            // Edit_LandmarkSpawns
            // 
            this.Edit_LandmarkSpawns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_LandmarkSpawns.Location = new System.Drawing.Point(0, 0);
            this.Edit_LandmarkSpawns.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Edit_LandmarkSpawns.Name = "Edit_LandmarkSpawns";
            this.Edit_LandmarkSpawns.Size = new System.Drawing.Size(1244, 717);
            this.Edit_LandmarkSpawns.TabIndex = 0;
            // 
            // Tab_Randomize
            // 
            this.Tab_Randomize.Controls.Add(this.B_Randomize);
            this.Tab_Randomize.Controls.Add(this.PG_RandSettings);
            this.Tab_Randomize.Location = new System.Drawing.Point(4, 29);
            this.Tab_Randomize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Randomize.Name = "Tab_Randomize";
            this.Tab_Randomize.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Tab_Randomize.Size = new System.Drawing.Size(1244, 717);
            this.Tab_Randomize.TabIndex = 4;
            this.Tab_Randomize.Text = "Randomize";
            this.Tab_Randomize.UseVisualStyleBackColor = true;
            // 
            // B_Randomize
            // 
            this.B_Randomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Randomize.Location = new System.Drawing.Point(872, 5);
            this.B_Randomize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.B_Randomize.Name = "B_Randomize";
            this.B_Randomize.Size = new System.Drawing.Size(364, 35);
            this.B_Randomize.TabIndex = 5;
            this.B_Randomize.Text = "Randomize Current Map Encounters";
            this.B_Randomize.UseVisualStyleBackColor = true;
            this.B_Randomize.Click += new System.EventHandler(this.B_Randomize_Click);
            // 
            // PG_RandSettings
            // 
            this.PG_RandSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_RandSettings.Location = new System.Drawing.Point(4, 5);
            this.PG_RandSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PG_RandSettings.Name = "PG_RandSettings";
            this.PG_RandSettings.Size = new System.Drawing.Size(1236, 707);
            this.PG_RandSettings.TabIndex = 0;
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(1153, 11);
            this.B_Save.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(112, 35);
            this.B_Save.TabIndex = 3;
            this.B_Save.Text = "Save All";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // AreaEditor8a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 824);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.TC_Editor);
            this.Controls.Add(this.CB_Area);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(823, 624);
            this.Name = "AreaEditor8a";
            this.Text = "Area Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AreaEditor8a_FormClosing);
            this.TC_Editor.ResumeLayout(false);
            this.Tab_Encounters.ResumeLayout(false);
            this.Tab_Regular.ResumeLayout(false);
            this.Tab_Wormhole.ResumeLayout(false);
            this.Tab_Landmarks.ResumeLayout(false);
            this.Tab_Randomize.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox CB_Area;
        private System.Windows.Forms.TabControl TC_Editor;
        private System.Windows.Forms.TabPage Tab_Encounters;
        private System.Windows.Forms.TabPage Tab_Regular;
        private System.Windows.Forms.TabPage Tab_Wormhole;
        private System.Windows.Forms.TabPage Tab_Landmarks;
        private Controls.EncounterTableEditor8a Edit_Encounters;
        private System.Windows.Forms.TabPage Tab_Randomize;
        private System.Windows.Forms.Button B_Save;
        private Controls.PlacementSpawnerEditor8a Edit_RegularSpawners;
        private Controls.PlacementSpawnerEditor8a Edit_WormholeSpawners;
        private System.Windows.Forms.PropertyGrid PG_RandSettings;
        private Controls.LandmarkEditor8a Edit_LandmarkSpawns;
        private System.Windows.Forms.Button B_Randomize;
    }
}