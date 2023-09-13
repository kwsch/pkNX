namespace pkNX.WinForms.Subforms
{
    partial class MapViewer9
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
            pictureBox1 = new System.Windows.Forms.PictureBox();
            CB_Map = new System.Windows.Forms.ComboBox();
            CB_Field = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBox1.Location = new System.Drawing.Point(14, 14);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(597, 591);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseMove += Map_MouseMove;
            // 
            // CB_Map
            // 
            CB_Map.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CB_Map.FormattingEnabled = true;
            CB_Map.Location = new System.Drawing.Point(619, 43);
            CB_Map.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CB_Map.Name = "CB_Map";
            CB_Map.Size = new System.Drawing.Size(140, 23);
            CB_Map.TabIndex = 1;
            CB_Map.SelectedIndexChanged += CB_Map_SelectedIndexChanged;
            // 
            // CB_Field
            // 
            CB_Field.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            CB_Field.FormattingEnabled = true;
            CB_Field.Location = new System.Drawing.Point(619, 14);
            CB_Field.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CB_Field.Name = "CB_Field";
            CB_Field.Size = new System.Drawing.Size(140, 23);
            CB_Field.TabIndex = 2;
            CB_Field.SelectedIndexChanged += CB_Field_SelectedIndexChanged;
            // 
            // MapViewer9
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(827, 614);
            Controls.Add(CB_Field);
            Controls.Add(CB_Map);
            Controls.Add(pictureBox1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MapViewer9";
            Text = "MapViewer9";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox CB_Map;
        private System.Windows.Forms.ComboBox CB_Field;
    }
}
