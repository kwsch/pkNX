using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Sprites;
using pkNX.Structures;

namespace pkNX.WinForms
{
    public partial class EvolutionRow : UserControl
    {
        public EvolutionRow()
        {
            InitializeComponent();

            CB_Method.Items.AddRange(EvoMethods);
            CB_Species.Items.AddRange(species);

            CB_Species.SelectedIndexChanged += (_, __) => ChangeSpecies(CB_Species.SelectedIndex, (int)NUD_Form.Value);
            NUD_Form.ValueChanged += (_, __) => ChangeSpecies(CB_Species.SelectedIndex, (int)NUD_Form.Value);

            CB_Arg.Items.AddRange(None);
            CB_Method.SelectedIndexChanged += (s, e) =>
            {
                var index = (EvolutionType)CB_Method.SelectedIndex;
                var type = index.GetArgType();
                if (type == oldMethod)
                    return;

                if (type < EvolutionTypeArgumentType.Items)
                {
                    CB_Arg.Visible = false;
                    return;
                }

                CB_Arg.Visible = true;
                oldMethod = type;
                CB_Arg.Items.Clear();
                var vals = GetArgs(type);
                CB_Arg.Items.AddRange(vals);
                CB_Arg.SelectedIndex = 0;
            };
        }

        private void ChangeSpecies(int spec, int form) => PB_Preview.Image = SpriteBuilder.GetSprite(spec, form, 0, 0, false, false);

        private EvolutionMethod current;
        private EvolutionTypeArgumentType oldMethod;

        public void LoadEvolution(EvolutionMethod s)
        {
            var evo = current = s;
            CB_Species.SelectedIndex = evo.Species;
            NUD_Form.Value = evo.Form;
            NUD_Level.Value = evo.Level;
            CB_Method.SelectedIndex = (int)evo.Method;
            CB_Arg.SelectedIndex = evo.Argument;
        }

        public void SaveEvolution()
        {
            var evo = current;
            evo.Species = CB_Species.SelectedIndex;
            evo.Form = (int)NUD_Form.Value;
            evo.Level = (int)NUD_Level.Value;
            evo.Method = (EvolutionType)CB_Method.SelectedIndex;
            evo.Argument = CB_Arg.SelectedIndex;
        }

        public static string[] items;
        public static string[] movelist;
        public static string[] species;
        public static string[] types;

        private static readonly string[] EvoMethods = Enum.GetNames(typeof(EvolutionType));
        private static readonly string[] Levels = Enumerable.Range(0, 100 + 1).Select(z => z.ToString()).ToArray();
        private static readonly string[] Stats = Enumerable.Range(0, 255 + 1).Select(z => z.ToString()).ToArray();
        private static readonly string[] None = { "" };

        private string[] GetArgs(EvolutionTypeArgumentType type)
        {
            switch (type)
            {
                case EvolutionTypeArgumentType.NoArg: return None;
                case EvolutionTypeArgumentType.Level: return Levels;
                case EvolutionTypeArgumentType.Items: return items;
                case EvolutionTypeArgumentType.Moves: return movelist;
                case EvolutionTypeArgumentType.Species: return species;
                case EvolutionTypeArgumentType.Stat: return Stats;
                case EvolutionTypeArgumentType.Type: return types;
                case EvolutionTypeArgumentType.Version: return Stats;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
