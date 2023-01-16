namespace pkNX.WinForms
{
    partial class TestMatrixConverter
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label9;
            this.TB_Result = new System.Windows.Forms.TextBox();
            this.TB_TranslateX = new System.Windows.Forms.TextBox();
            this.TB_TranslateY = new System.Windows.Forms.TextBox();
            this.TB_TranslateZ = new System.Windows.Forms.TextBox();
            this.TB_RotationZ = new System.Windows.Forms.TextBox();
            this.TB_RotationY = new System.Windows.Forms.TextBox();
            this.TB_RotationX = new System.Windows.Forms.TextBox();
            this.TB_ScaleZ = new System.Windows.Forms.TextBox();
            this.TB_ScaleY = new System.Windows.Forms.TextBox();
            this.TB_ScaleX = new System.Windows.Forms.TextBox();
            this.B_ConvertA = new System.Windows.Forms.Button();
            this.B_Decompose = new System.Windows.Forms.Button();
            this.TB_m02 = new System.Windows.Forms.TextBox();
            this.TB_m01 = new System.Windows.Forms.TextBox();
            this.TB_m00 = new System.Windows.Forms.TextBox();
            this.TB_m12 = new System.Windows.Forms.TextBox();
            this.TB_m11 = new System.Windows.Forms.TextBox();
            this.TB_m10 = new System.Windows.Forms.TextBox();
            this.TB_m22 = new System.Windows.Forms.TextBox();
            this.TB_m21 = new System.Windows.Forms.TextBox();
            this.TB_m20 = new System.Windows.Forms.TextBox();
            this.TB_ResultMat = new System.Windows.Forms.TextBox();
            this.TB_m32 = new System.Windows.Forms.TextBox();
            this.TB_m31 = new System.Windows.Forms.TextBox();
            this.TB_m30 = new System.Windows.Forms.TextBox();
            this.TB_m33 = new System.Windows.Forms.TextBox();
            this.TB_m03 = new System.Windows.Forms.TextBox();
            this.TB_m13 = new System.Windows.Forms.TextBox();
            this.TB_m23 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(435, 65);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(26, 25);
            label1.TabIndex = 10;
            label1.Text = "S:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(435, 102);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(27, 25);
            label2.TabIndex = 11;
            label2.Text = "R:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(435, 139);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(25, 25);
            label3.TabIndex = 12;
            label3.Text = "T:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(397, 176);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(63, 25);
            label4.TabIndex = 13;
            label4.Text = "Result:";
            // 
            // TB_Result
            // 
            this.TB_Result.Location = new System.Drawing.Point(467, 173);
            this.TB_Result.Name = "TB_Result";
            this.TB_Result.ReadOnly = true;
            this.TB_Result.Size = new System.Drawing.Size(153, 31);
            this.TB_Result.TabIndex = 9;
            this.TB_Result.TabStop = false;
            // 
            // TB_TranslateX
            // 
            this.TB_TranslateX.Location = new System.Drawing.Point(467, 136);
            this.TB_TranslateX.Name = "TB_TranslateX";
            this.TB_TranslateX.Size = new System.Drawing.Size(47, 31);
            this.TB_TranslateX.TabIndex = 6;
            this.TB_TranslateX.Text = "0.0";
            // 
            // TB_TranslateY
            // 
            this.TB_TranslateY.Location = new System.Drawing.Point(520, 136);
            this.TB_TranslateY.Name = "TB_TranslateY";
            this.TB_TranslateY.Size = new System.Drawing.Size(47, 31);
            this.TB_TranslateY.TabIndex = 7;
            this.TB_TranslateY.Text = "0.0";
            // 
            // TB_TranslateZ
            // 
            this.TB_TranslateZ.Location = new System.Drawing.Point(573, 136);
            this.TB_TranslateZ.Name = "TB_TranslateZ";
            this.TB_TranslateZ.Size = new System.Drawing.Size(47, 31);
            this.TB_TranslateZ.TabIndex = 8;
            this.TB_TranslateZ.Text = "0.0";
            // 
            // TB_RotationZ
            // 
            this.TB_RotationZ.Location = new System.Drawing.Point(573, 99);
            this.TB_RotationZ.Name = "TB_RotationZ";
            this.TB_RotationZ.Size = new System.Drawing.Size(47, 31);
            this.TB_RotationZ.TabIndex = 5;
            this.TB_RotationZ.Text = "0.0";
            // 
            // TB_RotationY
            // 
            this.TB_RotationY.Location = new System.Drawing.Point(520, 99);
            this.TB_RotationY.Name = "TB_RotationY";
            this.TB_RotationY.Size = new System.Drawing.Size(47, 31);
            this.TB_RotationY.TabIndex = 4;
            this.TB_RotationY.Text = "0.0";
            // 
            // TB_RotationX
            // 
            this.TB_RotationX.Location = new System.Drawing.Point(467, 99);
            this.TB_RotationX.Name = "TB_RotationX";
            this.TB_RotationX.Size = new System.Drawing.Size(47, 31);
            this.TB_RotationX.TabIndex = 3;
            this.TB_RotationX.Text = "0.0";
            // 
            // TB_ScaleZ
            // 
            this.TB_ScaleZ.Location = new System.Drawing.Point(573, 62);
            this.TB_ScaleZ.Name = "TB_ScaleZ";
            this.TB_ScaleZ.Size = new System.Drawing.Size(47, 31);
            this.TB_ScaleZ.TabIndex = 2;
            this.TB_ScaleZ.Text = "1.0";
            // 
            // TB_ScaleY
            // 
            this.TB_ScaleY.Location = new System.Drawing.Point(520, 62);
            this.TB_ScaleY.Name = "TB_ScaleY";
            this.TB_ScaleY.Size = new System.Drawing.Size(47, 31);
            this.TB_ScaleY.TabIndex = 1;
            this.TB_ScaleY.Text = "1.0";
            // 
            // TB_ScaleX
            // 
            this.TB_ScaleX.Location = new System.Drawing.Point(467, 62);
            this.TB_ScaleX.Name = "TB_ScaleX";
            this.TB_ScaleX.Size = new System.Drawing.Size(47, 31);
            this.TB_ScaleX.TabIndex = 0;
            this.TB_ScaleX.Text = "1.0";
            // 
            // B_ConvertA
            // 
            this.B_ConvertA.Location = new System.Drawing.Point(508, 210);
            this.B_ConvertA.Name = "B_ConvertA";
            this.B_ConvertA.Size = new System.Drawing.Size(112, 34);
            this.B_ConvertA.TabIndex = 14;
            this.B_ConvertA.Text = "Convert A";
            this.B_ConvertA.UseVisualStyleBackColor = true;
            this.B_ConvertA.Click += new System.EventHandler(this.B_ConvertA_Click);
            // 
            // B_Decompose
            // 
            this.B_Decompose.Location = new System.Drawing.Point(185, 244);
            this.B_Decompose.Name = "B_Decompose";
            this.B_Decompose.Size = new System.Drawing.Size(118, 34);
            this.B_Decompose.TabIndex = 29;
            this.B_Decompose.Text = "Decompose";
            this.B_Decompose.UseVisualStyleBackColor = true;
            this.B_Decompose.Click += new System.EventHandler(this.B_Decompose_Click);
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(27, 210);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(63, 25);
            label5.TabIndex = 28;
            label5.Text = "Result:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(65, 136);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(26, 25);
            label6.TabIndex = 27;
            label6.Text = "2:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(65, 99);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(26, 25);
            label7.TabIndex = 26;
            label7.Text = "1:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(65, 62);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(26, 25);
            label8.TabIndex = 25;
            label8.Text = "0:";
            // 
            // TB_m02
            // 
            this.TB_m02.Location = new System.Drawing.Point(203, 59);
            this.TB_m02.Name = "TB_m02";
            this.TB_m02.Size = new System.Drawing.Size(47, 31);
            this.TB_m02.TabIndex = 17;
            this.TB_m02.Text = "1.0";
            // 
            // TB_m01
            // 
            this.TB_m01.Location = new System.Drawing.Point(150, 59);
            this.TB_m01.Name = "TB_m01";
            this.TB_m01.Size = new System.Drawing.Size(47, 31);
            this.TB_m01.TabIndex = 16;
            this.TB_m01.Text = "1.0";
            // 
            // TB_m00
            // 
            this.TB_m00.Location = new System.Drawing.Point(97, 59);
            this.TB_m00.Name = "TB_m00";
            this.TB_m00.Size = new System.Drawing.Size(47, 31);
            this.TB_m00.TabIndex = 15;
            this.TB_m00.Text = "1.0";
            // 
            // TB_m12
            // 
            this.TB_m12.Location = new System.Drawing.Point(203, 96);
            this.TB_m12.Name = "TB_m12";
            this.TB_m12.Size = new System.Drawing.Size(47, 31);
            this.TB_m12.TabIndex = 20;
            this.TB_m12.Text = "0.0";
            // 
            // TB_m11
            // 
            this.TB_m11.Location = new System.Drawing.Point(150, 96);
            this.TB_m11.Name = "TB_m11";
            this.TB_m11.Size = new System.Drawing.Size(47, 31);
            this.TB_m11.TabIndex = 19;
            this.TB_m11.Text = "0.0";
            // 
            // TB_m10
            // 
            this.TB_m10.Location = new System.Drawing.Point(97, 96);
            this.TB_m10.Name = "TB_m10";
            this.TB_m10.Size = new System.Drawing.Size(47, 31);
            this.TB_m10.TabIndex = 18;
            this.TB_m10.Text = "0.0";
            // 
            // TB_m22
            // 
            this.TB_m22.Location = new System.Drawing.Point(203, 133);
            this.TB_m22.Name = "TB_m22";
            this.TB_m22.Size = new System.Drawing.Size(47, 31);
            this.TB_m22.TabIndex = 23;
            this.TB_m22.Text = "0.0";
            // 
            // TB_m21
            // 
            this.TB_m21.Location = new System.Drawing.Point(150, 133);
            this.TB_m21.Name = "TB_m21";
            this.TB_m21.Size = new System.Drawing.Size(47, 31);
            this.TB_m21.TabIndex = 22;
            this.TB_m21.Text = "0.0";
            // 
            // TB_m20
            // 
            this.TB_m20.Location = new System.Drawing.Point(97, 133);
            this.TB_m20.Name = "TB_m20";
            this.TB_m20.Size = new System.Drawing.Size(47, 31);
            this.TB_m20.TabIndex = 21;
            this.TB_m20.Text = "0.0";
            // 
            // TB_ResultMat
            // 
            this.TB_ResultMat.Location = new System.Drawing.Point(97, 207);
            this.TB_ResultMat.Name = "TB_ResultMat";
            this.TB_ResultMat.ReadOnly = true;
            this.TB_ResultMat.Size = new System.Drawing.Size(206, 31);
            this.TB_ResultMat.TabIndex = 24;
            this.TB_ResultMat.TabStop = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(65, 173);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(26, 25);
            label9.TabIndex = 33;
            label9.Text = "3:";
            // 
            // TB_m32
            // 
            this.TB_m32.Location = new System.Drawing.Point(203, 170);
            this.TB_m32.Name = "TB_m32";
            this.TB_m32.Size = new System.Drawing.Size(47, 31);
            this.TB_m32.TabIndex = 32;
            this.TB_m32.Text = "0.0";
            // 
            // TB_m31
            // 
            this.TB_m31.Location = new System.Drawing.Point(150, 170);
            this.TB_m31.Name = "TB_m31";
            this.TB_m31.Size = new System.Drawing.Size(47, 31);
            this.TB_m31.TabIndex = 31;
            this.TB_m31.Text = "0.0";
            // 
            // TB_m30
            // 
            this.TB_m30.Location = new System.Drawing.Point(97, 170);
            this.TB_m30.Name = "TB_m30";
            this.TB_m30.Size = new System.Drawing.Size(47, 31);
            this.TB_m30.TabIndex = 30;
            this.TB_m30.Text = "0.0";
            // 
            // TB_m33
            // 
            this.TB_m33.Location = new System.Drawing.Point(256, 170);
            this.TB_m33.Name = "TB_m33";
            this.TB_m33.Size = new System.Drawing.Size(47, 31);
            this.TB_m33.TabIndex = 37;
            this.TB_m33.Text = "0.0";
            // 
            // TB_m03
            // 
            this.TB_m03.Location = new System.Drawing.Point(256, 59);
            this.TB_m03.Name = "TB_m03";
            this.TB_m03.Size = new System.Drawing.Size(47, 31);
            this.TB_m03.TabIndex = 34;
            this.TB_m03.Text = "1.0";
            // 
            // TB_m13
            // 
            this.TB_m13.Location = new System.Drawing.Point(256, 96);
            this.TB_m13.Name = "TB_m13";
            this.TB_m13.Size = new System.Drawing.Size(47, 31);
            this.TB_m13.TabIndex = 35;
            this.TB_m13.Text = "0.0";
            // 
            // TB_m23
            // 
            this.TB_m23.Location = new System.Drawing.Point(256, 133);
            this.TB_m23.Name = "TB_m23";
            this.TB_m23.Size = new System.Drawing.Size(47, 31);
            this.TB_m23.TabIndex = 36;
            this.TB_m23.Text = "0.0";
            // 
            // TestMatrixConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TB_m33);
            this.Controls.Add(this.TB_m03);
            this.Controls.Add(this.TB_m13);
            this.Controls.Add(this.TB_m23);
            this.Controls.Add(label9);
            this.Controls.Add(this.TB_m32);
            this.Controls.Add(this.TB_m31);
            this.Controls.Add(this.TB_m30);
            this.Controls.Add(this.B_Decompose);
            this.Controls.Add(label5);
            this.Controls.Add(label6);
            this.Controls.Add(label7);
            this.Controls.Add(label8);
            this.Controls.Add(this.TB_m02);
            this.Controls.Add(this.TB_m01);
            this.Controls.Add(this.TB_m00);
            this.Controls.Add(this.TB_m12);
            this.Controls.Add(this.TB_m11);
            this.Controls.Add(this.TB_m10);
            this.Controls.Add(this.TB_m22);
            this.Controls.Add(this.TB_m21);
            this.Controls.Add(this.TB_m20);
            this.Controls.Add(this.TB_ResultMat);
            this.Controls.Add(this.B_ConvertA);
            this.Controls.Add(label4);
            this.Controls.Add(label3);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.TB_ScaleZ);
            this.Controls.Add(this.TB_ScaleY);
            this.Controls.Add(this.TB_ScaleX);
            this.Controls.Add(this.TB_RotationZ);
            this.Controls.Add(this.TB_RotationY);
            this.Controls.Add(this.TB_RotationX);
            this.Controls.Add(this.TB_TranslateZ);
            this.Controls.Add(this.TB_TranslateY);
            this.Controls.Add(this.TB_TranslateX);
            this.Controls.Add(this.TB_Result);
            this.Name = "TestMatrixConverter";
            this.Text = "TestMatrixConverter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TB_Result;
        private System.Windows.Forms.TextBox TB_TranslateX;
        private System.Windows.Forms.TextBox TB_TranslateY;
        private System.Windows.Forms.TextBox TB_TranslateZ;
        private System.Windows.Forms.TextBox TB_RotationZ;
        private System.Windows.Forms.TextBox TB_RotationY;
        private System.Windows.Forms.TextBox TB_RotationX;
        private System.Windows.Forms.TextBox TB_ScaleZ;
        private System.Windows.Forms.TextBox TB_ScaleY;
        private System.Windows.Forms.TextBox TB_ScaleX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button B_ConvertA;
        private System.Windows.Forms.Button B_Decompose;
        private System.Windows.Forms.TextBox TB_m02;
        private System.Windows.Forms.TextBox TB_m01;
        private System.Windows.Forms.TextBox TB_m00;
        private System.Windows.Forms.TextBox TB_m12;
        private System.Windows.Forms.TextBox TB_m11;
        private System.Windows.Forms.TextBox TB_m10;
        private System.Windows.Forms.TextBox TB_m22;
        private System.Windows.Forms.TextBox TB_m21;
        private System.Windows.Forms.TextBox TB_m20;
        private System.Windows.Forms.TextBox TB_ResultMat;
        private System.Windows.Forms.TextBox TB_m32;
        private System.Windows.Forms.TextBox TB_m31;
        private System.Windows.Forms.TextBox TB_m30;
        private System.Windows.Forms.TextBox TB_m33;
        private System.Windows.Forms.TextBox TB_m03;
        private System.Windows.Forms.TextBox TB_m13;
        private System.Windows.Forms.TextBox TB_m23;
    }
}
