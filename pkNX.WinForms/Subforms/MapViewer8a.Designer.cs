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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CB_Map = new System.Windows.Forms.ComboBox();
            this.CB_Species = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            // MapViewer8a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 532);
            this.Controls.Add(this.CB_Species);
            this.Controls.Add(this.CB_Map);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MapViewer8a";
            this.Text = "MapViewer8a";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox CB_Map;
        private System.Windows.Forms.ComboBox CB_Species;
    }
}