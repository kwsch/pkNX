using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls
{
    public abstract class EditorBase
    {
        protected readonly GameManager ROM;

        public GameVersion Game => ROM.Game;
        public int Language { get => ROM.Language; set => ROM.Language = value; }

        protected EditorBase(GameManager rom) => ROM = rom;
        public string Location { get; internal set; }

        public IEnumerable<Button> GetControls(int width, int height)
        {
            var type = GetType();
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (var m in methods)
            {
                const string prefix = "Edit";
                if (!m.Name.StartsWith(prefix))
                    continue;

                var name = m.Name.Substring(prefix.Length);
                var b = new Button
                {
                    Width = width,
                    Height = height,
                    Name = $"B_{name}",
                    Text = m.Name.Substring(prefix.Length),
                };
                b.Click += (s, e) => m.Invoke(this, null);
                yield return b;
            }
        }

        public void Close() => ROM.SaveAll(true);

        private static EditorBase GetEditor(GameManager ROM)
        {
            var g = ROM.Game;
            if (GameVersion.XY.Contains(g)) return new EditorXY(ROM);
            if (g == GameVersion.ORASDEMO) return new EditorORASDEMO(ROM);
            if (GameVersion.ORAS.Contains(g)) return new EditorAO(ROM);
            if (g == GameVersion.SMDEMO) return new EditorSMDEMO(ROM);
            if (GameVersion.SM.Contains(g)) return new EditorSM(ROM);
            if (GameVersion.USUM.Contains(g)) return new EditorUU(ROM);
            if (GameVersion.GG.Contains(g)) return new EditorGG(ROM);
            if (GameVersion.SWSH.Contains(g)) return new EditorGG(ROM);
            return null;
        }

        public static EditorBase GetEditor(string loc, int language)
        {
            var gl = GameLocation.GetGame(loc);
            if (gl == null)
                return null;
            GameManager gm = GameManager.GetManager(gl, language);
            EditorBase editor = GetEditor(gm);
            editor.Location = loc;
            return editor;
        }
    }
}
