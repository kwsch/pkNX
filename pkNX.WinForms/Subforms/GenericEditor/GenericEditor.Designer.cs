namespace pkNX.WinForms
{
    partial class GenericEditor<T>
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
            B_Save = new System.Windows.Forms.Button();
            CB_EntryName = new System.Windows.Forms.ComboBox();
            B_Dump = new System.Windows.Forms.Button();
            B_Rand = new System.Windows.Forms.Button();
            B_AddEntry = new System.Windows.Forms.Button();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            Grid = new System.Windows.Forms.PropertyGrid();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // B_Save
            // 
            B_Save.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            B_Save.Location = new System.Drawing.Point(482, 3);
            B_Save.Name = "B_Save";
            B_Save.Size = new System.Drawing.Size(117, 44);
            B_Save.TabIndex = 1;
            B_Save.Text = "Save";
            B_Save.UseVisualStyleBackColor = true;
            B_Save.Click += B_Save_Click;
            // 
            // CB_EntryName
            // 
            CB_EntryName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CB_EntryName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            CB_EntryName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            CB_EntryName.FormattingEnabled = true;
            CB_EntryName.Location = new System.Drawing.Point(8, 8);
            CB_EntryName.Margin = new System.Windows.Forms.Padding(8);
            CB_EntryName.Name = "CB_EntryName";
            CB_EntryName.Size = new System.Drawing.Size(463, 33);
            CB_EntryName.TabIndex = 2;
            CB_EntryName.SelectedIndexChanged += CB_EntryName_SelectedIndexChanged;
            // 
            // B_Dump
            // 
            B_Dump.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            B_Dump.Location = new System.Drawing.Point(605, 3);
            B_Dump.Name = "B_Dump";
            B_Dump.Size = new System.Drawing.Size(117, 44);
            B_Dump.TabIndex = 3;
            B_Dump.Text = "Dump";
            B_Dump.UseVisualStyleBackColor = true;
            B_Dump.Click += B_Dump_Click;
            // 
            // B_Rand
            // 
            B_Rand.Location = new System.Drawing.Point(728, 3);
            B_Rand.Name = "B_Rand";
            B_Rand.Size = new System.Drawing.Size(117, 44);
            B_Rand.TabIndex = 4;
            B_Rand.Text = "Randomize";
            B_Rand.UseVisualStyleBackColor = true;
            B_Rand.Visible = false;
            // 
            // B_AddEntry
            // 
            B_AddEntry.Location = new System.Drawing.Point(851, 3);
            B_AddEntry.Name = "B_AddEntry";
            B_AddEntry.Size = new System.Drawing.Size(117, 44);
            B_AddEntry.TabIndex = 5;
            B_AddEntry.Text = "Add Entry";
            B_AddEntry.UseVisualStyleBackColor = true;
            B_AddEntry.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(CB_EntryName);
            flowLayoutPanel1.Controls.Add(B_Save);
            flowLayoutPanel1.Controls.Add(B_Dump);
            flowLayoutPanel1.Controls.Add(B_Rand);
            flowLayoutPanel1.Controls.Add(B_AddEntry);
            flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new System.Drawing.Size(983, 50);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // Grid
            // 
            Grid.BackColor = System.Drawing.SystemColors.Control;
            Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            Grid.Location = new System.Drawing.Point(0, 50);
            Grid.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            Grid.Name = "Grid";
            Grid.Size = new System.Drawing.Size(983, 419);
            Grid.TabIndex = 7;
            // 
            // GenericEditor
            // 
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            ClientSize = new System.Drawing.Size(983, 469);
            Controls.Add(Grid);
            Controls.Add(flowLayoutPanel1);
            Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            MinimumSize = new System.Drawing.Size(735, 525);
            Name = "GenericEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "GenericEditor";
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ComboBox CB_EntryName;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.Button B_Rand;
        private System.Windows.Forms.Button B_AddEntry;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PropertyGrid Grid;
    }
}
