using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using pkNX.Containers;
using pkNX.Game;
using pkNX.Randomization;
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;
using static pkNX.Structures.Species;
using Util = pkNX.Randomization.Util;

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

        PG_RandSettings.SelectedObject = EditUtil.Settings.Species;

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

    private void B_Randomize_Click(object sender, EventArgs e)
    {
        SaveArea();
        RandomizeArea(Area, (SpeciesSettings)PG_RandSettings.SelectedObject);
        LoadArea();
        System.Media.SystemSounds.Asterisk.Play();
    }

    private void RandomizeArea(ResidentArea8a area, SpeciesSettings settings)
    {
        var pt = ROM.Data.PersonalData;
        var rand = new SpeciesRandomizer(ROM.Info, pt);

        var hasForm = new HashSet<int>();
        var banned = new HashSet<int>();
        foreach (var pi in pt.Table.Cast<PersonalInfoLA>())
        {
            if (pi.IsPresentInGame)
            {
                banned.Remove(pi.Species);
                hasForm.Add(pi.Species);
            }
            else if (!hasForm.Contains(pi.Species))
            {
                banned.Add(pi.Species);
            }
        }

        settings.Legends = false; // Legendary encounter slot conditions require you to not have captured the Legendary in order to encounter them; ban altogether.
        rand.Initialize(settings, banned.ToArray());

        var formRand = pt.Table
            .Cast<PersonalInfoLA>()
            .Where(z => z.IsPresentInGame && !(Legal.BattleExclusiveForms.Contains(z.Species) || Legal.BattleFusions.Contains(z.Species)))
            .GroupBy(z => z.Species)
            .ToDictionary(z => z.Key, z => z.ToList());

        var encounters = area.Encounters;
        foreach (var table in encounters)
        {
            foreach (var enc in table.Table)
            {
                if (enc.ShinyLock is not ShinyType8a.Random)
                    continue;

                // to progress the story in Cobalt Coastlands, you are required to show Iscan a Dusclops; ensure one can be captured
                if (enc.Species is (int)Dusclops && table.TableID is 7663383561364099763)
                    continue;

                var spec = rand.GetRandomSpecies(enc.Species);
                enc.Species = spec;
                enc.Form = GetRandomForm(spec);
                enc.ClearMoves();
            }
        }
        int GetRandomForm(int spec)
        {
            if (!formRand.TryGetValue(spec, out var entries))
                return 0;
            var count = entries.Count;

            return (Species)spec switch
            {
                Growlithe or Arcanine or Voltorb or Electrode or Typhlosion or Qwilfish or Samurott or Lilligant or Zorua or Zoroark or Braviary or Sliggoo or Goodra or Avalugg or Decidueye => 1,
                Basculin => 2,
                Kleavor => 0,
                _ => Util.Random.Next(0, count),
            };
        }
    }

    private void LoadArea()
    {
        Debug.WriteLine($"Loading Area {AreaIndex}");
        Edit_Encounters.LoadTable(Area.Encounters, Area.Settings.Encounters);
        Edit_RegularSpawners.LoadTable(Area.Spawners, Area.Settings.Spawners);
        Edit_WormholeSpawners.LoadTable(Area.Wormholes, Area.Settings.WormholeSpawners);
        Edit_LandmarkSpawns.LoadTable(Area.LandItems, Area.Settings.LandmarkItemSpawns);
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
