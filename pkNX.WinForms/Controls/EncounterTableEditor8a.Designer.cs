namespace pkNX.WinForms.Controls
{
    partial class EncounterTableEditor8a
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
            this.B_NoShinyLocks = new System.Windows.Forms.Button();
            this.PG_Encounters = new System.Windows.Forms.PropertyGrid();
            this.CB_Encounters = new System.Windows.Forms.ComboBox();
            this.L_ConfigName = new System.Windows.Forms.Label();
            this.B_CloneTableEntry = new System.Windows.Forms.Button();
            this.B_ConfigureAsAlpha = new System.Windows.Forms.Button();
            this.B_RemoveCondition = new System.Windows.Forms.Button();
            this.TT_ButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // B_NoShinyLocks
            // 
            this.B_NoShinyLocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_NoShinyLocks.Location = new System.Drawing.Point(730, 7);
            this.B_NoShinyLocks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_NoShinyLocks.Name = "B_NoShinyLocks";
            this.B_NoShinyLocks.Size = new System.Drawing.Size(222, 35);
            this.B_NoShinyLocks.TabIndex = 8;
            this.B_NoShinyLocks.Text = "Remove All Shiny Locks";
            this.TT_ButtonToolTip.SetToolTip(this.B_NoShinyLocks, "Changes all ShinyLock properties from Never to Random for all encounters in the c" +
        "urrent area");
            this.B_NoShinyLocks.UseVisualStyleBackColor = true;
            this.B_NoShinyLocks.Click += new System.EventHandler(this.B_NoShinyLocks_Click);
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
            this.PG_Encounters.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PG_Encounters_SelectedGridItemChanged);
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
            // B_CloneTableEntry
            // 
            this.B_CloneTableEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_CloneTableEntry.Enabled = false;
            this.B_CloneTableEntry.Location = new System.Drawing.Point(626, 48);
            this.B_CloneTableEntry.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_CloneTableEntry.Name = "B_CloneTableEntry";
            this.B_CloneTableEntry.Size = new System.Drawing.Size(156, 35);
            this.B_CloneTableEntry.TabIndex = 10;
            this.B_CloneTableEntry.Text = "Clone Table Entry";
            this.TT_ButtonToolTip.SetToolTip(this.B_CloneTableEntry, "Clone the selected encounter slot and add it to the bottom of the list");
            this.B_CloneTableEntry.UseVisualStyleBackColor = true;
            this.B_CloneTableEntry.Click += new System.EventHandler(this.B_CloneTableEntry_Click);
            // 
            // B_ConfigureAsAlpha
            // 
            this.B_ConfigureAsAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_ConfigureAsAlpha.Enabled = false;
            this.B_ConfigureAsAlpha.Location = new System.Drawing.Point(456, 48);
            this.B_ConfigureAsAlpha.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_ConfigureAsAlpha.Name = "B_ConfigureAsAlpha";
            this.B_ConfigureAsAlpha.Size = new System.Drawing.Size(162, 35);
            this.B_ConfigureAsAlpha.TabIndex = 11;
            this.B_ConfigureAsAlpha.Text = "Configure as Alpha";
            this.TT_ButtonToolTip.SetToolTip(this.B_ConfigureAsAlpha, "Configures the values that are often used for alpha pokemon on the selected encou" +
        "ter slot");
            this.B_ConfigureAsAlpha.UseVisualStyleBackColor = true;
            this.B_ConfigureAsAlpha.Click += new System.EventHandler(this.B_ConfigureAsAlpha_Click);
            // 
            // B_RemoveCondition
            // 
            this.B_RemoveCondition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_RemoveCondition.Enabled = false;
            this.B_RemoveCondition.Location = new System.Drawing.Point(790, 48);
            this.B_RemoveCondition.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.B_RemoveCondition.Name = "B_RemoveCondition";
            this.B_RemoveCondition.Size = new System.Drawing.Size(162, 35);
            this.B_RemoveCondition.TabIndex = 12;
            this.B_RemoveCondition.Text = "Remove Condition";
            this.TT_ButtonToolTip.SetToolTip(this.B_RemoveCondition, "Sets the ConditionID and ConditionTypeID to None for the selected encounter slot");
            this.B_RemoveCondition.UseVisualStyleBackColor = true;
            this.B_RemoveCondition.Click += new System.EventHandler(this.B_RemoveCondition_Click);
            // 
            // EncounterTableEditor8a
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.B_RemoveCondition);
            this.Controls.Add(this.B_ConfigureAsAlpha);
            this.Controls.Add(this.B_CloneTableEntry);
            this.Controls.Add(this.L_ConfigName);
            this.Controls.Add(this.B_NoShinyLocks);
            this.Controls.Add(this.PG_Encounters);
            this.Controls.Add(this.CB_Encounters);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(773, 454);
            this.Name = "EncounterTableEditor8a";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Size = new System.Drawing.Size(960, 720);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_NoShinyLocks;
        private System.Windows.Forms.PropertyGrid PG_Encounters;
        private System.Windows.Forms.ComboBox CB_Encounters;
        private System.Windows.Forms.Label L_ConfigName;
        private System.Windows.Forms.Button B_CloneTableEntry;
        private System.Windows.Forms.Button B_ConfigureAsAlpha;
        private System.Windows.Forms.Button B_RemoveCondition;
        private System.Windows.Forms.ToolTip TT_ButtonToolTip;
    }
}
