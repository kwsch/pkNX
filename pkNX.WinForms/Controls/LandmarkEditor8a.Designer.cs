namespace pkNX.WinForms.Controls
{
    partial class LandmarkEditor8a
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
            this.components = new System.ComponentModel.Container();
            this.B_HighEncounterChance = new System.Windows.Forms.Button();
            this.PG_Encounters = new System.Windows.Forms.PropertyGrid();
            this.CB_Encounters = new System.Windows.Forms.ComboBox();
            this.L_ConfigName = new System.Windows.Forms.Label();
            this.TT_ButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // B_HighEncounterChance
            // 
            this.B_HighEncounterChance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_HighEncounterChance.Location = new System.Drawing.Point(730, 7);
            this.B_HighEncounterChance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_HighEncounterChance.Name = "B_HighEncounterChance";
            this.B_HighEncounterChance.Size = new System.Drawing.Size(222, 35);
            this.B_HighEncounterChance.TabIndex = 8;
            this.B_HighEncounterChance.Text = "High Encounter Chance";
            this.TT_ButtonToolTip.SetToolTip(this.B_HighEncounterChance, "Set all ActivationRate\'s of all spawners in this area to 75");
            this.B_HighEncounterChance.UseVisualStyleBackColor = true;
            this.B_HighEncounterChance.Click += new System.EventHandler(this.B_HighEncounterChance_Click);
            // 
            // PG_Encounters
            // 
            this.PG_Encounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PG_Encounters.Location = new System.Drawing.Point(8, 91);
            this.PG_Encounters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PG_Encounters.Name = "PG_Encounters";
            this.PG_Encounters.Size = new System.Drawing.Size(944, 620);
            this.PG_Encounters.TabIndex = 7;
            // 
            // CB_Encounters
            // 
            this.CB_Encounters.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Encounters.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Encounters.FormattingEnabled = true;
            this.CB_Encounters.Location = new System.Drawing.Point(8, 52);
            this.CB_Encounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 6);
            this.CB_Encounters.Name = "CB_Encounters";
            this.CB_Encounters.Size = new System.Drawing.Size(259, 28);
            this.CB_Encounters.TabIndex = 6;
            this.CB_Encounters.SelectedIndexChanged += new System.EventHandler(this.CB_Encounters_SelectedIndexChanged);
            // 
            // L_ConfigName
            // 
            this.L_ConfigName.AutoSize = true;
            this.L_ConfigName.Location = new System.Drawing.Point(8, 7);
            this.L_ConfigName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 11);
            this.L_ConfigName.Name = "L_ConfigName";
            this.L_ConfigName.Padding = new System.Windows.Forms.Padding(4, 7, 4, 0);
            this.L_ConfigName.Size = new System.Drawing.Size(171, 27);
            this.L_ConfigName.TabIndex = 9;
            this.L_ConfigName.Text = "{Configured File Path}";
            this.L_ConfigName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // LandmarkEditor8a
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.L_ConfigName);
            this.Controls.Add(this.B_HighEncounterChance);
            this.Controls.Add(this.PG_Encounters);
            this.Controls.Add(this.CB_Encounters);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(773, 454);
            this.Name = "LandmarkEditor8a";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(960, 720);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_HighEncounterChance;
        private System.Windows.Forms.PropertyGrid PG_Encounters;
        private System.Windows.Forms.ComboBox CB_Encounters;
        private System.Windows.Forms.Label L_ConfigName;
        private System.Windows.Forms.ToolTip TT_ButtonToolTip;
    }
}
