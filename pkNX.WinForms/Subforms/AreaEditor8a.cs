using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Structures.FlatBuffers;

namespace pkNX.WinForms.Subforms;

public partial class AreaEditor8a : Form
{
    private readonly GameManagerPLA ROM;
    private readonly GFPack Resident;
    private readonly AreaSettingsTable8a Settings;

    private readonly string[] AreaNames;

    private ResidentArea8a Area;
    private int AreaIndex;
    private readonly bool Loading;

    public AreaEditor8a(GameManagerPLA rom)
    {
        ROM = rom;

        Resident = (GFPack)ROM.GetFile(GameFile.Resident);
        var bin_settings = Resident.GetDataFullPath("bin/field/resident/AreaSettings.bin");
        Settings = FlatBufferConverter.DeserializeFrom<AreaSettingsTable8a>(bin_settings);

        AreaNames = Settings.Table.Select(z => z.Name).ToArray();

        const string startingArea = "ha_area01";
        (AreaIndex, Area) = LoadAreaByName(startingArea);

        InitializeComponent();

        Loading = true;
        CB_Area.Items.AddRange(AreaNames);
        CB_Area.SelectedIndex = AreaIndex;
        LoadArea();
        Loading = false;
    }

    private (int index, ResidentArea8a area) LoadAreaByName(string name)
    {
        var index = Array.IndexOf(AreaNames, name);
        var area = new ResidentArea8a(Resident, Settings.Find(name));
        area.LoadInfo();
        return (index, area);
    }

    private void CB_Map_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        SaveArea();
        (AreaIndex, Area) = LoadAreaByName(AreaNames[CB_Area.SelectedIndex]);
        LoadArea();
    }

    private void LoadArea()
    {
        Debug.WriteLine($"Loading Area {AreaIndex}");
        Edit_Encounters.LoadTable(Area.Encounters, Area.Settings.Encounters);
    }

    private void SaveArea()
    {
        Debug.WriteLine($"Saving Area {AreaIndex}");
        Area.SaveInfo();
    }

    private void B_Save_Click(object sender, EventArgs e)
    {
        Save = true;
        Close();
    }

    private bool Save;

    private void AreaEditor8a_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (Save)
            SaveArea();
        else
            Resident.CancelEdits();
    }
}
