using System.Windows.Forms;
using pkNX.Game;

namespace pkNX.WinForms.Controls
{
    internal class EditorUU : EditorBase
    {
        protected internal EditorUU(GameManager rom) : base(rom) { }

        public void TestButton()
        {
            using var form = new Form();
            form.ShowDialog();
        }
    }
}