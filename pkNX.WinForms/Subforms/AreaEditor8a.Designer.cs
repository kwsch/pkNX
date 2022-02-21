namespace pkNX.WinForms.Subforms
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
            this.CB_Area = new System.Windows.Forms.ComboBox();
            this.TC_Editor = new System.Windows.Forms.TabControl();
            this.Tab_Encounters = new System.Windows.Forms.TabPage();
            this.Tab_Regular = new System.Windows.Forms.TabPage();
            this.Tab_Wormhole = new System.Windows.Forms.TabPage();
            this.Tab_Landmarks = new System.Windows.Forms.TabPage();
            this.Tab_Randomize = new System.Windows.Forms.TabPage();
            this.B_Save = new System.Windows.Forms.Button();
            this.PG_RandSettings = new System.Windows.Forms.PropertyGrid();
            this.Edit_Encounters = new pkNX.WinForms.Controls.EncounterTableEditor8a();
            this.Edit_RegularSpawners = new pkNX.WinForms.Controls.PlacementSpawnerEditor8a();
            this.Edit_WormholeSpawners = new pkNX.WinForms.Controls.PlacementSpawnerEditor8a();
            this.Edit_LandmarkSpawns = new pkNX.WinForms.Controls.LandmarkEditor8a();
            this.B_Randomize = new System.Windows.Forms.Button();
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
            this.CB_Area.Location = new System.Drawing.Point(12, 12);
            this.CB_Area.Name = "CB_Area";
            this.CB_Area.Size = new System.Drawing.Size(212, 21);
            this.CB_Area.TabIndex = 1;
            this.CB_Area.SelectedIndexChanged += new System.EventHandler(this.CB_Map_SelectedIndexChanged);
            // 
            // TC_Editor
            // 
            this.TC_Editor.Controls.Add(this.Tab_Encounters);
            this.TC_Editor.Controls.Add(this.Tab_Regular);
            this.TC_Editor.Controls.Add(this.Tab_Wormhole);
            this.TC_Editor.Controls.Add(this.Tab_Landmarks);
            this.TC_Editor.Controls.Add(this.Tab_Randomize);
            this.TC_Editor.Location = new System.Drawing.Point(12, 39);
            this.TC_Editor.Name = "TC_Editor";
            this.TC_Editor.SelectedIndex = 0;
            this.TC_Editor.Size = new System.Drawing.Size(685, 481);
            this.TC_Editor.TabIndex = 2;
            // 
            // Tab_Encounters
            // 
            this.Tab_Encounters.Controls.Add(this.Edit_Encounters);
            this.Tab_Encounters.Location = new System.Drawing.Point(4, 22);
            this.Tab_Encounters.Name = "Tab_Encounters";
            this.Tab_Encounters.Size = new System.Drawing.Size(677, 455);
            this.Tab_Encounters.TabIndex = 0;
            this.Tab_Encounters.Text = "Encounters";
            this.Tab_Encounters.UseVisualStyleBackColor = true;
            // 
            // Tab_Regular
            // 
            this.Tab_Regular.Controls.Add(this.Edit_RegularSpawners);
            this.Tab_Regular.Location = new System.Drawing.Point(4, 22);
            this.Tab_Regular.Name = "Tab_Regular";
            this.Tab_Regular.Size = new System.Drawing.Size(677, 455);
            this.Tab_Regular.TabIndex = 1;
            this.Tab_Regular.Text = "*Regular";
            this.Tab_Regular.UseVisualStyleBackColor = true;
            // 
            // Tab_Wormhole
            // 
            this.Tab_Wormhole.Controls.Add(this.Edit_WormholeSpawners);
            this.Tab_Wormhole.Location = new System.Drawing.Point(4, 22);
            this.Tab_Wormhole.Name = "Tab_Wormhole";
            this.Tab_Wormhole.Size = new System.Drawing.Size(677, 455);
            this.Tab_Wormhole.TabIndex = 2;
            this.Tab_Wormhole.Text = "*Wormhole";
            this.Tab_Wormhole.UseVisualStyleBackColor = true;
            // 
            // Tab_Landmarks
            // 
            this.Tab_Landmarks.Controls.Add(this.Edit_LandmarkSpawns);
            this.Tab_Landmarks.Location = new System.Drawing.Point(4, 22);
            this.Tab_Landmarks.Name = "Tab_Landmarks";
            this.Tab_Landmarks.Size = new System.Drawing.Size(677, 455);
            this.Tab_Landmarks.TabIndex = 3;
            this.Tab_Landmarks.Text = "*Landmarks";
            this.Tab_Landmarks.UseVisualStyleBackColor = true;
            // 
            // Tab_Randomize
            // 
            this.Tab_Randomize.Controls.Add(this.B_Randomize);
            this.Tab_Randomize.Controls.Add(this.PG_RandSettings);
            this.Tab_Randomize.Location = new System.Drawing.Point(4, 22);
            this.Tab_Randomize.Name = "Tab_Randomize";
            this.Tab_Randomize.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Randomize.Size = new System.Drawing.Size(677, 455);
            this.Tab_Randomize.TabIndex = 4;
            this.Tab_Randomize.Text = "Randomize";
            this.Tab_Randomize.UseVisualStyleBackColor = true;
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(633, 1);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 3;
            this.B_Save.Text = "Save All";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // PG_RandSettings
            // 
            this.PG_RandSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PG_RandSettings.Location = new System.Drawing.Point(3, 3);
            this.PG_RandSettings.Name = "PG_RandSettings";
            this.PG_RandSettings.Size = new System.Drawing.Size(671, 449);
            this.PG_RandSettings.TabIndex = 0;
            // 
            // Edit_Encounters
            // 
            this.Edit_Encounters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_Encounters.Location = new System.Drawing.Point(0, 0);
            this.Edit_Encounters.Name = "Edit_Encounters";
            this.Edit_Encounters.Size = new System.Drawing.Size(677, 455);
            this.Edit_Encounters.TabIndex = 0;
            // 
            // Edit_RegularSpawners
            // 
            this.Edit_RegularSpawners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_RegularSpawners.Location = new System.Drawing.Point(0, 0);
            this.Edit_RegularSpawners.Name = "Edit_RegularSpawners";
            this.Edit_RegularSpawners.Size = new System.Drawing.Size(677, 455);
            this.Edit_RegularSpawners.TabIndex = 1;
            // 
            // Edit_WormholeSpawners
            // 
            this.Edit_WormholeSpawners.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_WormholeSpawners.Location = new System.Drawing.Point(0, 0);
            this.Edit_WormholeSpawners.Name = "Edit_WormholeSpawners";
            this.Edit_WormholeSpawners.Size = new System.Drawing.Size(677, 455);
            this.Edit_WormholeSpawners.TabIndex = 2;
            // 
            // Edit_LandmarkSpawns
            // 
            this.Edit_LandmarkSpawns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Edit_LandmarkSpawns.Location = new System.Drawing.Point(0, 0);
            this.Edit_LandmarkSpawns.Name = "Edit_LandmarkSpawns";
            this.Edit_LandmarkSpawns.Size = new System.Drawing.Size(677, 455);
            this.Edit_LandmarkSpawns.TabIndex = 0;
            // 
            // B_Randomize
            // 
            this.B_Randomize.Location = new System.Drawing.Point(431, 3);
            this.B_Randomize.Name = "B_Randomize";
            this.B_Randomize.Size = new System.Drawing.Size(243, 23);
            this.B_Randomize.TabIndex = 5;
            this.B_Randomize.Text = "Randomize Current Map Encounters";
            this.B_Randomize.UseVisualStyleBackColor = true;
            this.B_Randomize.Click += new System.EventHandler(this.B_Randomize_Click);
            // 
            // AreaEditor8a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 532);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.TC_Editor);
            this.Controls.Add(this.CB_Area);
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