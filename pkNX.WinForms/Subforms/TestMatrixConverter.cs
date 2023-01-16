using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms;


public partial class TestMatrixConverter : Form
{
    public TestMatrixConverter()
    {
        InitializeComponent();
    }

    private void B_ConvertA_Click(object sender, EventArgs e)
    {
        Transform transform = new()
        {
            Scale = new PackedVec3f()
            {
                X = float.Parse(TB_ScaleX.Text),
                Y = float.Parse(TB_ScaleY.Text),
                Z = float.Parse(TB_ScaleZ.Text),
            },
            Rotate = new PackedVec3f()
            {
                X = float.Parse(TB_RotationX.Text),
                Y = float.Parse(TB_RotationY.Text),
                Z = float.Parse(TB_RotationZ.Text),
            },
            Translate = new PackedVec3f()
            {
                X = float.Parse(TB_TranslateX.Text),
                Y = float.Parse(TB_TranslateY.Text),
                Z = float.Parse(TB_TranslateZ.Text),
            }
        };

        Matrix4x3f matrix = new()
        {
            AxisX = null,
            AxisY = null,
            AxisZ = null,
            AxisW = null
        };


        TB_Result.Text = matrix.ToString();
    }

    private void B_Decompose_Click(object sender, EventArgs e)
    {

    }
}
