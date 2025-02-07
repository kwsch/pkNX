using System;
using System.Windows.Forms;
using PKHeX.Core;
using pkNX.Structures;
using PKHeX.Drawing.PokeSprite;

namespace pkNX.WinForms;

public partial class MegaEvoEntry : UserControl
{
    public static string[] items = [];

    private static readonly string[] EvoMethods = Enum.GetNames<MegaEvolutionMethod>();

    public MegaEvoEntry()
    {
        InitializeComponent();

        CB_Method.Items.AddRange(EvoMethods);
        CB_Arg.Items.AddRange(items);

        NUD_Form.ValueChanged += (_, __) => ChangeSpecies((int)NUD_Form.Value);
        CB_Method.SelectedIndexChanged += (s, e) =>
        {
            PB_Preview.Visible = PB_Base.Visible = L_Into.Visible = CB_Method.SelectedIndex > 0;
            CB_Arg.Visible = CB_Method.SelectedIndex == (int)MegaEvolutionMethod.Item;
        };
    }

    public int Species { private get; set; }
    private MegaEvolutionSet? current;

    private void ChangeSpecies(int form)
    {
        PB_Base.Image = SpriteUtil.GetSprite((ushort)Species, 0, 0, 0, 0, false, PKHeX.Core.Shiny.Never, EntityContext.Gen7);
        PB_Preview.Image = SpriteUtil.GetSprite((ushort)Species, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never, EntityContext.Gen7);
        PB_Preview.Visible = PB_Base.Visible = L_Into.Visible = CB_Method.SelectedIndex > 0;
    }

    public void LoadEvolution(MegaEvolutionSet s, int species)
    {
        Species = species;
        current = s;

        CB_Method.SelectedIndex = s.Method;
        NUD_Form.Value = s.ToForm;
    }

    public void SaveEvolution()
    {
        if (current == null)
            return;

        if (CB_Method.SelectedIndex <= 0)
        {
            current.ToForm = 0;
            current.Method = 0;
            current.Argument = 0;
            return;
        }
        current.Method = CB_Method.SelectedIndex;
        current.Argument = CB_Arg.SelectedIndex;
        current.ToForm = (int) NUD_Form.Value;
    }
}
