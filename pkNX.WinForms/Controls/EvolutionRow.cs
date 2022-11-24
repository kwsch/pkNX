using System;
using System.Linq;
using System.Windows.Forms;
using pkNX.Structures;
using PKHeX.Drawing.PokeSprite;

namespace pkNX.WinForms;

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
            L_Method.Visible = L_Species.Visible = L_Arg.Visible = L_Form.Visible = L_Level.Visible = index > 0;
            L_Arg.Visible = CB_Arg.Visible = type >= EvolutionTypeArgumentType.Items;
            if (type == oldMethod)
                return;

            if (type < EvolutionTypeArgumentType.Items)
                return;

            CB_Arg.Visible = true;
            oldMethod = type;
            CB_Arg.Items.Clear();
            var vals = GetArgs(type);
            CB_Arg.Items.AddRange(vals);
            CB_Arg.SelectedIndex = 0;
        };
    }

    private void ChangeSpecies(int spec, int form) => PB_Preview.Image = SpriteUtil.GetSprite((ushort)spec, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never);

    private EvolutionMethod? current;
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
        if (evo == null)
            return;
        evo.Species = (ushort)CB_Species.SelectedIndex;
        evo.Form = (byte)NUD_Form.Value;
        evo.Level = (byte)NUD_Level.Value;
        evo.Method = (EvolutionType)CB_Method.SelectedIndex;
        evo.Argument = (ushort)CB_Arg.SelectedIndex;
    }

    public static string[] items = Array.Empty<string>();
    public static string[] movelist = Array.Empty<string>();
    public static string[] species = Array.Empty<string>();
    public static string[] types = Array.Empty<string>();

    private static readonly string[] EvoMethods = Enum.GetNames(typeof(EvolutionType));
    private static readonly string[] Levels = Enumerable.Range(0, 100 + 1).Select(z => z.ToString()).ToArray();
    private static readonly string[] Stats = Enumerable.Range(0, 255 + 1).Select(z => z.ToString()).ToArray();
    private static readonly string[] None = { "" };

    private static string[] GetArgs(EvolutionTypeArgumentType type)
    {
        return type switch
        {
            EvolutionTypeArgumentType.NoArg => None,
            EvolutionTypeArgumentType.Level => Levels,
            EvolutionTypeArgumentType.Items => items,
            EvolutionTypeArgumentType.Moves => movelist,
            EvolutionTypeArgumentType.Species => species,
            EvolutionTypeArgumentType.Stat => Stats,
            EvolutionTypeArgumentType.Type => types,
            EvolutionTypeArgumentType.Version => Stats,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }
}
