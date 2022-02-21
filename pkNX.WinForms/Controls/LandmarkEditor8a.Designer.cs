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
            this.B_HighEncounterChance = new System.Windows.Forms.Button();
            this.PG_Encounters = new System.Windows.Forms.PropertyGrid();
            this.CB_Encounters = new System.Windows.Forms.ComboBox();
            this.L_ConfigName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // B_HighEncounterChance
            // 
            this.B_HighEncounterChance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_HighEncounterChance.Location = new System.Drawing.Point(614, 0);
            this.B_HighEncounterChance.Name = "B_HighEncounterChance";
            this.B_HighEncounterChance.Size = new System.Drawing.Size(148, 23);
            this.B_HighEncounterChance.TabIndex = 8;
            this.B_HighEncounterChance.Text = "High Encounter Chance";
            this.B_HighEncounterChance.UseVisualStyleBackColor = true;
            this.B_HighEncounterChance.Click += new System.EventHandler(this.B_HighEncounterChance_Click);
            // 
            // PG_Encounters
            // 
            this.PG_Encounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PG_Encounters.Location = new System.Drawing.Point(3, 38);
            this.PG_Encounters.Name = "PG_Encounters";
            this.PG_Encounters.Size = new System.Drawing.Size(759, 548);
            this.PG_Encounters.TabIndex = 7;
            // 
            // CB_Encounters
            // 
            this.CB_Encounters.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_Encounters.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_Encounters.FormattingEnabled = true;
            this.CB_Encounters.Location = new System.Drawing.Point(3, 15);
            this.CB_Encounters.Name = "CB_Encounters";
            this.CB_Encounters.Size = new System.Drawing.Size(174, 21);
            this.CB_Encounters.TabIndex = 6;
            this.CB_Encounters.SelectedIndexChanged += new System.EventHandler(this.CB_Encounters_SelectedIndexChanged);
            // 
            // L_ConfigName
            // 
            this.L_ConfigName.AutoSize = true;
            this.L_ConfigName.Location = new System.Drawing.Point(3, 1);
            this.L_ConfigName.Name = "L_ConfigName";
            this.L_ConfigName.Size = new System.Drawing.Size(110, 13);
            this.L_ConfigName.TabIndex = 9;
            this.L_ConfigName.Text = "{Configured File Path}";
            // 
            // EncounterTableEditor8a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.L_ConfigName);
            this.Controls.Add(this.B_HighEncounterChance);
            this.Controls.Add(this.PG_Encounters);
            this.Controls.Add(this.CB_Encounters);
            this.Name = "EncounterTableEditor8a";
            this.Size = new System.Drawing.Size(765, 589);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_HighEncounterChance;
        private System.Windows.Forms.PropertyGrid PG_Encounters;
        private System.Windows.Forms.ComboBox CB_Encounters;
        private System.Windows.Forms.Label L_ConfigName;
    }
}
