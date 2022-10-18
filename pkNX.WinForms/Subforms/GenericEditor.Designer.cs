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
            this.Grid = new System.Windows.Forms.PropertyGrid();
            this.B_Save = new System.Windows.Forms.Button();
            this.CB_EntryName = new System.Windows.Forms.ComboBox();
            this.B_Dump = new System.Windows.Forms.Button();
            this.B_Rand = new System.Windows.Forms.Button();
            this.B_AddEntry = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Grid.Location = new System.Drawing.Point(0, 15);
            this.Grid.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(1043, 1229);
            this.Grid.TabIndex = 0;
            // 
            // B_Save
            // 
            this.B_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Save.Location = new System.Drawing.Point(343, 0);
            this.B_Save.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(100, 44);
            this.B_Save.TabIndex = 1;
            this.B_Save.Text = "Save";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // CB_EntryName
            // 
            this.CB_EntryName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CB_EntryName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CB_EntryName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CB_EntryName.FormattingEnabled = true;
            this.CB_EntryName.Location = new System.Drawing.Point(123, 15);
            this.CB_EntryName.Margin = new System.Windows.Forms.Padding(5, 6, 12, 6);
            this.CB_EntryName.Name = "CB_EntryName";
            this.CB_EntryName.Size = new System.Drawing.Size(459, 33);
            this.CB_EntryName.TabIndex = 2;
            this.CB_EntryName.SelectedIndexChanged += new System.EventHandler(this.CB_EntryName_SelectedIndexChanged);
            // 
            // B_Dump
            // 
            this.B_Dump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Dump.Location = new System.Drawing.Point(249, 0);
            this.B_Dump.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.B_Dump.Name = "B_Dump";
            this.B_Dump.Size = new System.Drawing.Size(88, 44);
            this.B_Dump.TabIndex = 3;
            this.B_Dump.Text = "Dump";
            this.B_Dump.UseVisualStyleBackColor = true;
            this.B_Dump.Click += new System.EventHandler(this.B_Dump_Click);
            // 
            // B_Rand
            // 
            this.B_Rand.Location = new System.Drawing.Point(126, 0);
            this.B_Rand.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.B_Rand.Name = "B_Rand";
            this.B_Rand.Size = new System.Drawing.Size(117, 44);
            this.B_Rand.TabIndex = 4;
            this.B_Rand.Text = "Randomize";
            this.B_Rand.UseVisualStyleBackColor = true;
            this.B_Rand.Visible = false;
            // 
            // B_AddEntry
            // 
            this.B_AddEntry.Location = new System.Drawing.Point(3, 0);
            this.B_AddEntry.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.B_AddEntry.Name = "B_AddEntry";
            this.B_AddEntry.Size = new System.Drawing.Size(117, 44);
            this.B_AddEntry.TabIndex = 5;
            this.B_AddEntry.Text = "Add Entry";
            this.B_AddEntry.UseVisualStyleBackColor = true;
            this.B_AddEntry.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.B_Save);
            this.flowLayoutPanel1.Controls.Add(this.B_Dump);
            this.flowLayoutPanel1.Controls.Add(this.B_Rand);
            this.flowLayoutPanel1.Controls.Add(this.B_AddEntry);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(597, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(446, 44);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // GenericEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 1244);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.CB_EntryName);
            this.Controls.Add(this.Grid);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MinimumSize = new System.Drawing.Size(735, 525);
            this.Name = "GenericEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GenericEditor";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid Grid;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.ComboBox CB_EntryName;
        private System.Windows.Forms.Button B_Dump;
        private System.Windows.Forms.Button B_Rand;
        private System.Windows.Forms.Button B_AddEntry;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}