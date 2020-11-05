namespace pkNX.WinForms
{
    partial class SSWE
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.CB_Location = new System.Windows.Forms.ComboBox();
            this.L_Hash = new System.Windows.Forms.Label();
            this.B_Save = new System.Windows.Forms.Button();
            this.L_Type = new System.Windows.Forms.Label();
            this.TC_Types = new System.Windows.Forms.TabControl();
            this.Tab_Normal = new System.Windows.Forms.TabPage();
            this.SL_0 = new pkNX.WinForms.EncounterList8();
            this.Tab_Overcast = new System.Windows.Forms.TabPage();
            this.SL_1 = new pkNX.WinForms.EncounterList8();
            this.Tab_Raining = new System.Windows.Forms.TabPage();
            this.SL_2 = new pkNX.WinForms.EncounterList8();
            this.Tab_Thunder = new System.Windows.Forms.TabPage();
            this.SL_3 = new pkNX.WinForms.EncounterList8();
            this.Tab_Sun = new System.Windows.Forms.TabPage();
            this.SL_4 = new pkNX.WinForms.EncounterList8();
            this.ST_Snowing = new System.Windows.Forms.TabPage();
            this.SL_5 = new pkNX.WinForms.EncounterList8();
            this.ST_Snowstorm = new System.Windows.Forms.TabPage();
            this.SL_6 = new pkNX.WinForms.EncounterList8();
            this.ST_Sandstorm = new System.Windows.Forms.TabPage();
            this.SL_7 = new pkNX.WinForms.EncounterList8();
            this.ST_Fog = new System.Windows.Forms.TabPage();
            this.SL_8 = new pkNX.WinForms.EncounterList8();
            this.ST_Shaking = new System.Windows.Forms.TabPage();
            this.SL_9 = new pkNX.WinForms.EncounterList8();
            this.ST_Fishing = new System.Windows.Forms.TabPage();
            this.SL_10 = new pkNX.WinForms.EncounterList8();
            this.Tab_Rand = new System.Windows.Forms.TabPage();
            this.NUD_LevelBoost = new System.Windows.Forms.NumericUpDown();
            this.CHK_Level = new System.Windows.Forms.CheckBox();
            this.CHK_FillEmpty = new System.Windows.Forms.CheckBox();
            this.B_RandAll = new System.Windows.Forms.Button();
            this.PG_Species = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.TC_Types.SuspendLayout();
            this.Tab_Normal.SuspendLayout();
            this.Tab_Overcast.SuspendLayout();
            this.Tab_Raining.SuspendLayout();
            this.Tab_Thunder.SuspendLayout();
            this.Tab_Sun.SuspendLayout();
            this.ST_Snowing.SuspendLayout();
            this.ST_Snowstorm.SuspendLayout();
            this.ST_Sandstorm.SuspendLayout();
            this.ST_Fog.SuspendLayout();
            this.ST_Shaking.SuspendLayout();
            this.ST_Fishing.SuspendLayout();
            this.Tab_Rand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CB_Location);
            this.splitContainer1.Panel1.Controls.Add(this.L_Hash);
            this.splitContainer1.Panel1.Controls.Add(this.B_Save);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.L_Type);
            this.splitContainer1.Panel2.Controls.Add(this.TC_Types);
            this.splitContainer1.Size = new System.Drawing.Size(457, 686);
            this.splitContainer1.SplitterDistance = 42;
            this.splitContainer1.TabIndex = 14;
            // 
            // CB_Location
            // 
            this.CB_Location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_Location.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CB_Location.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Location.FormattingEnabled = true;
            this.CB_Location.Location = new System.Drawing.Point(12, 13);
            this.CB_Location.Name = "CB_Location";
            this.CB_Location.Size = new System.Drawing.Size(220, 21);
            this.CB_Location.TabIndex = 3;
            this.CB_Location.SelectedIndexChanged += new System.EventHandler(this.CB_Location_SelectedIndexChanged);
            // 
            // L_Hash
            // 
            this.L_Hash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.L_Hash.AutoSize = true;
            this.L_Hash.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.L_Hash.Location = new System.Drawing.Point(238, 16);
            this.L_Hash.Name = "L_Hash";
            this.L_Hash.Size = new System.Drawing.Size(77, 14);
            this.L_Hash.TabIndex = 11;
            this.L_Hash.Text = "L_ZoneHash";
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(370, 11);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(75, 23);
            this.B_Save.TabIndex = 12;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // L_Type
            // 
            this.L_Type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.L_Type.AutoSize = true;
            this.L_Type.ForeColor = System.Drawing.Color.Red;
            this.L_Type.Location = new System.Drawing.Point(2, 624);
            this.L_Type.Name = "L_Type";
            this.L_Type.Size = new System.Drawing.Size(41, 13);
            this.L_Type.TabIndex = 12;
            this.L_Type.Text = "Source";
            // 
            // TC_Types
            // 
            this.TC_Types.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.TC_Types.Controls.Add(this.Tab_Normal);
            this.TC_Types.Controls.Add(this.Tab_Overcast);
            this.TC_Types.Controls.Add(this.Tab_Raining);
            this.TC_Types.Controls.Add(this.Tab_Thunder);
            this.TC_Types.Controls.Add(this.Tab_Sun);
            this.TC_Types.Controls.Add(this.ST_Snowing);
            this.TC_Types.Controls.Add(this.ST_Snowstorm);
            this.TC_Types.Controls.Add(this.ST_Sandstorm);
            this.TC_Types.Controls.Add(this.ST_Fog);
            this.TC_Types.Controls.Add(this.ST_Shaking);
            this.TC_Types.Controls.Add(this.ST_Fishing);
            this.TC_Types.Controls.Add(this.Tab_Rand);
            this.TC_Types.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TC_Types.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TC_Types.ItemSize = new System.Drawing.Size(32, 100);
            this.TC_Types.Location = new System.Drawing.Point(0, 0);
            this.TC_Types.Multiline = true;
            this.TC_Types.Name = "TC_Types";
            this.TC_Types.SelectedIndex = 0;
            this.TC_Types.Size = new System.Drawing.Size(457, 640);
            this.TC_Types.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TC_Types.TabIndex = 11;
            this.TC_Types.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TC_Tables_DrawItem);
            // 
            // Tab_Normal
            // 
            this.Tab_Normal.Controls.Add(this.SL_0);
            this.Tab_Normal.Location = new System.Drawing.Point(104, 4);
            this.Tab_Normal.Name = "Tab_Normal";
            this.Tab_Normal.Size = new System.Drawing.Size(349, 632);
            this.Tab_Normal.TabIndex = 0;
            this.Tab_Normal.Text = "Normal";
            this.Tab_Normal.UseVisualStyleBackColor = true;
            // 
            // SL_0
            // 
            this.SL_0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_0.Location = new System.Drawing.Point(0, 0);
            this.SL_0.Margin = new System.Windows.Forms.Padding(0);
            this.SL_0.Name = "SL_0";
            this.SL_0.Size = new System.Drawing.Size(349, 632);
            this.SL_0.TabIndex = 0;
            // 
            // Tab_Overcast
            // 
            this.Tab_Overcast.Controls.Add(this.SL_1);
            this.Tab_Overcast.Location = new System.Drawing.Point(104, 4);
            this.Tab_Overcast.Name = "Tab_Overcast";
            this.Tab_Overcast.Size = new System.Drawing.Size(349, 632);
            this.Tab_Overcast.TabIndex = 1;
            this.Tab_Overcast.Text = "Overcast";
            this.Tab_Overcast.UseVisualStyleBackColor = true;
            // 
            // SL_1
            // 
            this.SL_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_1.Location = new System.Drawing.Point(0, 0);
            this.SL_1.Margin = new System.Windows.Forms.Padding(0);
            this.SL_1.Name = "SL_1";
            this.SL_1.Size = new System.Drawing.Size(349, 632);
            this.SL_1.TabIndex = 1;
            // 
            // Tab_Raining
            // 
            this.Tab_Raining.Controls.Add(this.SL_2);
            this.Tab_Raining.Location = new System.Drawing.Point(104, 4);
            this.Tab_Raining.Name = "Tab_Raining";
            this.Tab_Raining.Size = new System.Drawing.Size(349, 632);
            this.Tab_Raining.TabIndex = 2;
            this.Tab_Raining.Text = "Raining";
            this.Tab_Raining.UseVisualStyleBackColor = true;
            // 
            // SL_2
            // 
            this.SL_2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_2.Location = new System.Drawing.Point(0, 0);
            this.SL_2.Margin = new System.Windows.Forms.Padding(0);
            this.SL_2.Name = "SL_2";
            this.SL_2.Size = new System.Drawing.Size(349, 632);
            this.SL_2.TabIndex = 1;
            // 
            // Tab_Thunder
            // 
            this.Tab_Thunder.Controls.Add(this.SL_3);
            this.Tab_Thunder.Location = new System.Drawing.Point(104, 4);
            this.Tab_Thunder.Name = "Tab_Thunder";
            this.Tab_Thunder.Size = new System.Drawing.Size(349, 632);
            this.Tab_Thunder.TabIndex = 3;
            this.Tab_Thunder.Text = "Thunder";
            this.Tab_Thunder.UseVisualStyleBackColor = true;
            // 
            // SL_3
            // 
            this.SL_3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_3.Location = new System.Drawing.Point(0, 0);
            this.SL_3.Margin = new System.Windows.Forms.Padding(0);
            this.SL_3.Name = "SL_3";
            this.SL_3.Size = new System.Drawing.Size(349, 632);
            this.SL_3.TabIndex = 1;
            // 
            // Tab_Sun
            // 
            this.Tab_Sun.Controls.Add(this.SL_4);
            this.Tab_Sun.Location = new System.Drawing.Point(104, 4);
            this.Tab_Sun.Name = "Tab_Sun";
            this.Tab_Sun.Size = new System.Drawing.Size(349, 632);
            this.Tab_Sun.TabIndex = 4;
            this.Tab_Sun.Text = "Intense Sun";
            this.Tab_Sun.UseVisualStyleBackColor = true;
            // 
            // SL_4
            // 
            this.SL_4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_4.Location = new System.Drawing.Point(0, 0);
            this.SL_4.Margin = new System.Windows.Forms.Padding(0);
            this.SL_4.Name = "SL_4";
            this.SL_4.Size = new System.Drawing.Size(349, 632);
            this.SL_4.TabIndex = 2;
            // 
            // ST_Snowing
            // 
            this.ST_Snowing.Controls.Add(this.SL_5);
            this.ST_Snowing.Location = new System.Drawing.Point(104, 4);
            this.ST_Snowing.Name = "ST_Snowing";
            this.ST_Snowing.Size = new System.Drawing.Size(349, 632);
            this.ST_Snowing.TabIndex = 11;
            this.ST_Snowing.Text = "Snowing";
            this.ST_Snowing.UseVisualStyleBackColor = true;
            // 
            // SL_5
            // 
            this.SL_5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_5.Location = new System.Drawing.Point(0, 0);
            this.SL_5.Margin = new System.Windows.Forms.Padding(0);
            this.SL_5.Name = "SL_5";
            this.SL_5.Size = new System.Drawing.Size(349, 632);
            this.SL_5.TabIndex = 1;
            // 
            // ST_Snowstorm
            // 
            this.ST_Snowstorm.Controls.Add(this.SL_6);
            this.ST_Snowstorm.Location = new System.Drawing.Point(104, 4);
            this.ST_Snowstorm.Name = "ST_Snowstorm";
            this.ST_Snowstorm.Size = new System.Drawing.Size(349, 632);
            this.ST_Snowstorm.TabIndex = 5;
            this.ST_Snowstorm.Text = "Snowstorm";
            this.ST_Snowstorm.UseVisualStyleBackColor = true;
            // 
            // SL_6
            // 
            this.SL_6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_6.Location = new System.Drawing.Point(0, 0);
            this.SL_6.Margin = new System.Windows.Forms.Padding(0);
            this.SL_6.Name = "SL_6";
            this.SL_6.Size = new System.Drawing.Size(349, 632);
            this.SL_6.TabIndex = 1;
            // 
            // ST_Sandstorm
            // 
            this.ST_Sandstorm.Controls.Add(this.SL_7);
            this.ST_Sandstorm.Location = new System.Drawing.Point(104, 4);
            this.ST_Sandstorm.Name = "ST_Sandstorm";
            this.ST_Sandstorm.Size = new System.Drawing.Size(349, 632);
            this.ST_Sandstorm.TabIndex = 9;
            this.ST_Sandstorm.Text = "Sandstorm";
            this.ST_Sandstorm.UseVisualStyleBackColor = true;
            // 
            // SL_7
            // 
            this.SL_7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_7.Location = new System.Drawing.Point(0, 0);
            this.SL_7.Margin = new System.Windows.Forms.Padding(0);
            this.SL_7.Name = "SL_7";
            this.SL_7.Size = new System.Drawing.Size(349, 632);
            this.SL_7.TabIndex = 1;
            // 
            // ST_Fog
            // 
            this.ST_Fog.Controls.Add(this.SL_8);
            this.ST_Fog.Location = new System.Drawing.Point(104, 4);
            this.ST_Fog.Name = "ST_Fog";
            this.ST_Fog.Size = new System.Drawing.Size(349, 632);
            this.ST_Fog.TabIndex = 8;
            this.ST_Fog.Text = "Heavy Fog";
            this.ST_Fog.UseVisualStyleBackColor = true;
            // 
            // SL_8
            // 
            this.SL_8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_8.Location = new System.Drawing.Point(0, 0);
            this.SL_8.Margin = new System.Windows.Forms.Padding(0);
            this.SL_8.Name = "SL_8";
            this.SL_8.Size = new System.Drawing.Size(349, 632);
            this.SL_8.TabIndex = 1;
            // 
            // ST_Shaking
            // 
            this.ST_Shaking.Controls.Add(this.SL_9);
            this.ST_Shaking.Location = new System.Drawing.Point(104, 4);
            this.ST_Shaking.Name = "ST_Shaking";
            this.ST_Shaking.Size = new System.Drawing.Size(349, 632);
            this.ST_Shaking.TabIndex = 10;
            this.ST_Shaking.Text = "Shaking Trees";
            this.ST_Shaking.UseVisualStyleBackColor = true;
            // 
            // SL_9
            // 
            this.SL_9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_9.Location = new System.Drawing.Point(0, 0);
            this.SL_9.Margin = new System.Windows.Forms.Padding(0);
            this.SL_9.Name = "SL_9";
            this.SL_9.Size = new System.Drawing.Size(349, 632);
            this.SL_9.TabIndex = 1;
            // 
            // ST_Fishing
            // 
            this.ST_Fishing.Controls.Add(this.SL_10);
            this.ST_Fishing.Location = new System.Drawing.Point(104, 4);
            this.ST_Fishing.Name = "ST_Fishing";
            this.ST_Fishing.Size = new System.Drawing.Size(349, 632);
            this.ST_Fishing.TabIndex = 7;
            this.ST_Fishing.Text = "Fishing";
            this.ST_Fishing.UseVisualStyleBackColor = true;
            // 
            // SL_10
            // 
            this.SL_10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SL_10.Location = new System.Drawing.Point(0, 0);
            this.SL_10.Margin = new System.Windows.Forms.Padding(0);
            this.SL_10.Name = "SL_10";
            this.SL_10.Size = new System.Drawing.Size(349, 632);
            this.SL_10.TabIndex = 1;
            // 
            // Tab_Rand
            // 
            this.Tab_Rand.Controls.Add(this.NUD_LevelBoost);
            this.Tab_Rand.Controls.Add(this.CHK_Level);
            this.Tab_Rand.Controls.Add(this.CHK_FillEmpty);
            this.Tab_Rand.Controls.Add(this.B_RandAll);
            this.Tab_Rand.Controls.Add(this.PG_Species);
            this.Tab_Rand.Location = new System.Drawing.Point(104, 4);
            this.Tab_Rand.Name = "Tab_Rand";
            this.Tab_Rand.Padding = new System.Windows.Forms.Padding(3);
            this.Tab_Rand.Size = new System.Drawing.Size(349, 632);
            this.Tab_Rand.TabIndex = 6;
            this.Tab_Rand.Text = "Randomization";
            this.Tab_Rand.UseVisualStyleBackColor = true;
            // 
            // NUD_LevelBoost
            // 
            this.NUD_LevelBoost.DecimalPlaces = 2;
            this.NUD_LevelBoost.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.NUD_LevelBoost.Location = new System.Drawing.Point(298, 15);
            this.NUD_LevelBoost.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.NUD_LevelBoost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NUD_LevelBoost.Name = "NUD_LevelBoost";
            this.NUD_LevelBoost.Size = new System.Drawing.Size(43, 20);
            this.NUD_LevelBoost.TabIndex = 303;
            this.NUD_LevelBoost.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CHK_Level
            // 
            this.CHK_Level.AutoSize = true;
            this.CHK_Level.Location = new System.Drawing.Point(169, 16);
            this.CHK_Level.Name = "CHK_Level";
            this.CHK_Level.Size = new System.Drawing.Size(130, 17);
            this.CHK_Level.TabIndex = 302;
            this.CHK_Level.Text = "Multiply PKM Level by";
            this.CHK_Level.UseVisualStyleBackColor = true;
            // 
            // CHK_FillEmpty
            // 
            this.CHK_FillEmpty.Location = new System.Drawing.Point(80, 9);
            this.CHK_FillEmpty.Name = "CHK_FillEmpty";
            this.CHK_FillEmpty.Size = new System.Drawing.Size(83, 31);
            this.CHK_FillEmpty.TabIndex = 8;
            this.CHK_FillEmpty.Text = "Fill All Empty Slots";
            this.CHK_FillEmpty.UseVisualStyleBackColor = true;
            // 
            // B_RandAll
            // 
            this.B_RandAll.Location = new System.Drawing.Point(6, 6);
            this.B_RandAll.Name = "B_RandAll";
            this.B_RandAll.Size = new System.Drawing.Size(68, 34);
            this.B_RandAll.TabIndex = 7;
            this.B_RandAll.Text = "Randomize All Species";
            this.B_RandAll.UseVisualStyleBackColor = true;
            this.B_RandAll.Click += new System.EventHandler(this.B_RandAll_Click);
            // 
            // PG_Species
            // 
            this.PG_Species.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PG_Species.Location = new System.Drawing.Point(0, 46);
            this.PG_Species.Name = "PG_Species";
            this.PG_Species.Size = new System.Drawing.Size(349, 586);
            this.PG_Species.TabIndex = 6;
            this.PG_Species.ToolbarVisible = false;
            // 
            // SSWE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 686);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.Name = "SSWE";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wild Editor";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.TC_Types.ResumeLayout(false);
            this.Tab_Normal.ResumeLayout(false);
            this.Tab_Overcast.ResumeLayout(false);
            this.Tab_Raining.ResumeLayout(false);
            this.Tab_Thunder.ResumeLayout(false);
            this.Tab_Sun.ResumeLayout(false);
            this.ST_Snowing.ResumeLayout(false);
            this.ST_Snowstorm.ResumeLayout(false);
            this.ST_Sandstorm.ResumeLayout(false);
            this.ST_Fog.ResumeLayout(false);
            this.ST_Shaking.ResumeLayout(false);
            this.ST_Fishing.ResumeLayout(false);
            this.Tab_Rand.ResumeLayout(false);
            this.Tab_Rand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_LevelBoost)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox CB_Location;
        private System.Windows.Forms.Label L_Hash;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl TC_Types;
        private System.Windows.Forms.TabPage Tab_Normal;
        private EncounterList8 SL_0;
        private System.Windows.Forms.TabPage Tab_Overcast;
        private EncounterList8 SL_1;
        private System.Windows.Forms.TabPage Tab_Raining;
        private EncounterList8 SL_2;
        private System.Windows.Forms.TabPage Tab_Thunder;
        private EncounterList8 SL_3;
        private System.Windows.Forms.TabPage Tab_Sun;
        private EncounterList8 SL_5;
        private System.Windows.Forms.TabPage ST_Snowing;
        private EncounterList8 SL_4;
        private System.Windows.Forms.TabPage ST_Snowstorm;
        private EncounterList8 SL_6;
        private System.Windows.Forms.TabPage ST_Sandstorm;
        private EncounterList8 SL_7;
        private System.Windows.Forms.TabPage ST_Fog;
        private EncounterList8 SL_8;
        private System.Windows.Forms.TabPage ST_Shaking;
        private EncounterList8 SL_9;
        private System.Windows.Forms.TabPage ST_Fishing;
        private EncounterList8 SL_10;
        private System.Windows.Forms.TabPage Tab_Rand;
        private System.Windows.Forms.CheckBox CHK_FillEmpty;
        private System.Windows.Forms.Button B_RandAll;
        private System.Windows.Forms.PropertyGrid PG_Species;
        private System.Windows.Forms.Label L_Type;
        private System.Windows.Forms.NumericUpDown NUD_LevelBoost;
        private System.Windows.Forms.CheckBox CHK_Level;
    }
}