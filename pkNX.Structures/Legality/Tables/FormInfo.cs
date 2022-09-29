using System;
using System.Collections.Generic;
using static pkNX.Structures.Species;

namespace pkNX.Structures;

/// <summary>
/// Contains logic for Alternate Form information.
/// </summary>
public static class FormInfo
{
    public static bool HasBattleOnlyForm(ushort species)
    {
        return BattleOnly.Contains(species);
    }

    /// <summary>
    /// Checks if the form cannot exist outside of a Battle.
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="format">Current generation format</param>
    /// <returns>True if it can only exist in a battle, false if it can exist outside of battle.</returns>
    public static bool IsBattleOnlyForm(ushort species, byte form, int format)
    {
        if (!HasBattleOnlyForm(species))
            return false;

        // Some species have battle only forms as well as out-of-battle forms (other than base form).
        switch (species)
        {
            case (int)Slowbro when form == 2 && format >= 8: // this one is OK, Galarian Slowbro (not a Mega)
            case (int)Darmanitan when form == 2 && format >= 8: // this one is OK, Galarian non-Zen
            case (int)Zygarde when form < 4: // Zygarde Complete
            case (int)Mimikyu when form == 2: // Totem disguise Mimikyu
            case (int)Necrozma when form < 3: // Only mark Ultra Necrozma as Battle Only
                return false;
            case (int)Minior:
                return form < 7; // Minior Shields-Down

            default:
                return form != 0;
        }
    }

    public static byte RemoveBattleOnlyFormsFromCount(ushort species, byte formCount, int format)
    {
        if (!HasBattleOnlyForm(species))
            return formCount;

        return Math.Min(formCount, GetOutOfBattleFormCount_Impl(species));
    }

    public static byte RemoveTotemFormsFromCount(ushort species, byte formCount, int format)
    {
        if (!HasTotemForm(species))
            return formCount;

        return Math.Min(formCount, GetOutOfBattleFormCount_Impl(species));
    }

    public static byte GetOutOfBattleFormCount(ushort species, byte formCount, int format)
    {
        if (!HasTotemForm(species) && !HasBattleOnlyForm(species))
            return formCount;

        return GetOutOfBattleFormCount_Impl(species);
    }

    /// <summary>
    /// Warning! Untested! Keep this private untill it's properly tested.
    /// Get the amount of forms that can be used out of battle and carried over to newer generations
    /// </summary>
    /// <remarks>This removes mega forms, totem forms, dynamax forms, etc.</remarks>
    /// <param name="species">The species to get the count for</param>
    /// <returns></returns>
    private static byte GetOutOfBattleFormCount_Impl(ushort species)
    {
        return species switch
        {
            (int)Rattata => 2,// Standard Form, Alolan Form
            (int)Raticate => 2,// Standard Form, Alolan Form
            (int)Pikachu => 10, // Pikachu with cap
            (int)Raichu => 2, // Standard Form, Alolan Form
            (int)Sandshrew => 2, // Standard Form, Alolan Form
            (int)Sandslash => 2, // Standard Form, Alolan Form
            (int)Vulpix => 2, // Standard Form, Alolan Form
            (int)Ninetales => 2, // Standard Form, Alolan Form
            (int)Diglett => 2, // Standard Form, Alolan Form
            (int)Dugtrio => 2, // Standard Form, Alolan Form
            (int)Meowth => 3, // Standard Form, Alolan Form, Galarian Form
            (int)Persian => 2, // Standard Form, Alolan Form
            (int)Growlithe => 2, // Standard Form, Hisuian Form
            (int)Arcanine => 2, // Standard Form, Hisuian Form
            (int)Geodude => 2, // Standard Form, Hisuian Form
            (int)Graveler => 2, // Standard Form, Hisuian Form
            (int)Golem => 2, // Standard Form, Hisuian Form
            (int)Ponyta => 2, // Standard Form, Galarian Form
            (int)Rapidash => 2, // Standard Form, Galarian Form
            (int)Slowpoke => 2, // Standard Form, Galarian Form
            (int)Slowbro => 2, // Standard Form, Galarian Form
            (int)Farfetchd => 2, // Standard Form, Galarian Form
            (int)Grimer => 2, // Standard Form, Alolan Form
            (int)Muk => 2, // Standard Form, Alolan Form
            (int)Voltorb => 2, // Standard Form, Hisuian Form
            (int)Electrode => 2, // Standard Form, Hisuian Form
            (int)Exeggutor => 2, // Standard Form, Alolan Form
            (int)Marowak => 2, // Standard Form, Alolan Form
            (int)Weezing => 2, // Standard Form, Galarian Form
            (int)MrMime => 2, // Standard Form, Galarian Form
            (int)Articuno => 2, // Standard Form, Galarian Form
            (int)Zapdos => 2, // Standard Form, Galarian Form
            (int)Moltres => 2, // Standard Form, Galarian Form

            (int)Typhlosion => 2, // Standard Form, Hisuian Form
            (int)Slowking => 2, // Standard Form, Galarian Form
            (int)Unown => 28, // Unknown forms have no personal data
            (int)Sneasel => 2, // Standard Form, Hisuian Form
            (int)Corsola => 2, // Standard Form, Galarian Form

            (int)Zigzagoon => 2, // Standard Form, Galarian Form
            (int)Linoone => 2, // Standard Form, Galarian Form
            //Spinda?
            (int)Castform => 4, // Normal Form, Sunny Form, Rainy Form, Snowy Form

            (int)Burmy => 3, // Plant Cloak, Sandy Cloak, Trash Cloak
            (int)Wormadam => 3, // Plant Cloak, Sandy Cloak, Trash Cloak
            (int)Shellos => 2, // West Sea, East Sea
            (int)Gastrodon => 2, // West Sea, East Sea
            (int)Rotom => 6, // Standard Form, Heat Rotom, Wash Rotom, Frost Rotom, Fan Rotom, Mow Rotom
            (int)Dialga => 2, // Standard Form, Origin Form
            (int)Palkia => 2, // Standard Form, Origin Form
            (int)Giratina => 2, // Standard Form, Origin Form
            (int)Shaymin => 2, // Land Form, Sky Form

            (int)Samurott => 2, // Standard Form, Hisuian Form
            (int)Lilligant => 2, // Standard Form, Hisuian Form
            (int)Basculin => 3, // Red-Striped Form, Blue-Striped Form, White-Striped Form
            (int)Darumaka => 2,// Standard Form, Galarian Form
            (int)Darmanitan => 2,// Standard Form, Galarian non-Zen Form
            (int)Yamask => 2,// Standard Form, Galarian Form
            (int)Zorua => 2,// Standard Form, Hisuian Form
            (int)Zoroark => 2,// Standard Form, Hisuian Form
            (int)Deerling => 4,
            (int)Sawsbuck => 4,
            (int)Braviary => 2,// Standard Form, Hisuian Form
            (int)Tornadus => 2, // Incarnate Form, Therian Form
            (int)Thundurus => 2, // Incarnate Form, Therian Form
            (int)Landorus => 2, // Incarnate Form, Therian Form

            (int)Kyurem => 3, // Normal Form, Black Form, White Form
            (int)Keldeo => 2, // Ordinary Form, Resolute Form
            (int)Meloetta => 2, // Aria Form, Pirouette Form
            (int)Genesect => 5, // Normal, Electric, Fire, Ice, Water
            (int)Flabébé => 5, // Red Flower, Yellow Flower, Orange Flower, Blue Flower, White Flower
            (int)Floette => 6, // Red Flower, Yellow Flower, Orange Flower, Blue Flower, White Flower
            (int)Florges => 5, // Red Flower, Yellow Flower, Orange Flower, Blue Flower, White Flower

            //(int)Aegislash => 2, // Sword Form, Shield Form
            (int)Sliggoo => 2,// Standard Form, Hisuian Form
            (int)Goodra => 2,// Standard Form, Hisuian Form
            (int)Avalugg => 2,// Standard Form, Hisuian Form
            (int)Zygarde => 4,// (0,1,2,3) can be out-of-battle,  Zygarde Complete (4) is a battle form

            (int)Decidueye => 2,// Standard Form, Hisuian Form
            (int)Oricorio => 4, // Baile Style, Pom-Pom Style, Pa'u Style, Sensu Style
            (int)Lycanroc => 3, // Midday Form, Midnight Form, Dusk Form

            //(int)Wishiwashi => 2, // Solo Form, School Form
            (int)Silvally => 18, // Form for each type
            (int)Minior => 14, // (0-7) Meteor Forms, 7-14 are Core Forms
            (int)Mimikyu => 2, // Standard Form (0) && Totem disguise Mimikyu (2)
            (int)Necrozma => 3, // Standard Form, Dusk Mane Necrozma, Dawn Wings Necrozma

            (int)Toxtricity => 2, // Amped Form, Low Key Form
            (int)Urshifu => 2, // Single Strike Style, Rapid Strike Style

            (int)Calyrex => 3, // Standard Form, Ice Rider, Shadow Rider

            (int)Enamorus => 2, // Incarnate Form, Therian Form

            _ => 1,
        };
    }

    /// <summary>
    /// Reverts the Battle Form to the form it would have outside of Battle.
    /// </summary>
    /// <remarks>Only call this if you've already checked that <see cref="IsBattleOnlyForm"/> returns true.</remarks>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="format">Current generation format</param>
    /// <returns>Suggested alt form value.</returns>
    public static byte GetOutOfBattleForm(ushort species, byte form, int format) => species switch
    {
        (int)Darmanitan => (byte)(form & 2),
        (int)Zygarde when format > 6 => 3,
        (int)Minior => (byte)(form + 7),
        _ => 0,
    };

    /// <summary>
    /// Checks if the <see cref="form"/> is a fused form, which indicates it cannot be traded away.
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="format">Current generation format</param>
    /// <returns>True if it is a fused species-form, false if it is not fused.</returns>
    public static bool IsFusedForm(ushort species, byte form, int format) => species switch
    {
        (int)Kyurem when form != 0 && format >= 5 => true,
        (int)Necrozma when form != 0 && format >= 7 => true,
        (int)Calyrex when form != 0 && format >= 8 => true,
        _ => false,
    };

    /// <summary>Checks if the form may be different than the original encounter detail.</summary>
    /// <param name="species">Original species</param>
    /// <param name="oldForm">Original form</param>
    /// <param name="newForm">Current form</param>
    /// <param name="format">Current format</param>
    public static bool IsFormChangeable(ushort species, byte oldForm, byte newForm, int format)
    {
        if (FormChange.Contains(species))
            return true;

        // Zygarde Form Changing
        // Gen6: Introduced; no form changing.
        // Gen7: Form changing introduced; can only change to Form 2/3 (Power Construct), never to 0/1 (Aura Break). A form-1 can be boosted to form-0.
        // Gen8: Form changing improved; can pick any Form & Ability combination.
        if (species == (int)Zygarde)
        {
            return format switch
            {
                6 => false,
                7 => newForm >= 2 || (oldForm == 1 && newForm == 0),
                _ => true,
            };
        }
        return false;
    }

    /// <summary>
    /// Species that can change between their forms, regardless of origin.
    /// </summary>
    /// <remarks>Excludes Zygarde as it has special conditions. Check separately.</remarks>
    private static readonly HashSet<ushort> FormChange = new()
    {
        // Sometimes considered for wild encounters
        (int)Burmy,
        (int)Rotom,
        (int)Furfrou,
        (int)Oricorio,

        (int)Deoxys,
        (int)Dialga,
        (int)Palkia,
        (int)Giratina,
        (int)Shaymin,
        (int)Arceus,
        (int)Tornadus,
        (int)Thundurus,
        (int)Landorus,
        (int)Kyurem,
        (int)Keldeo,
        (int)Genesect,
        (int)Hoopa,
        (int)Silvally,
        (int)Necrozma,
        (int)Calyrex,
        (int)Enamorus,
    };

    /// <summary>
    /// Species that have an alternate form that cannot exist outside of battle.
    /// </summary>
    private static readonly HashSet<ushort> BattleForms = new()
    {
        (int)Castform,
        (int)Cherrim,
        (int)Darmanitan,
        (int)Meloetta,
        (int)Aegislash,
        (int)Xerneas,
        (int)Zygarde,

        (int)Wishiwashi,
        (int)Minior,
        (int)Mimikyu,

        (int)Cramorant,
        (int)Morpeko,
        (int)Eiscue,

        (int)Zacian,
        (int)Zamazenta,
        (int)Eternatus,
    };

    /// <summary>
    /// Species that have a mega form that cannot exist outside of battle.
    /// </summary>
    /// <remarks>Using a held item to change form during battle, via an in-battle transformation feature.</remarks>
    private static readonly HashSet<ushort> BattleMegas = new()
    {
        // XY
        (int)Venusaur, (int)Charizard, (int)Blastoise,
        (int)Alakazam, (int)Gengar, (int)Kangaskhan, (int)Pinsir,
        (int)Gyarados, (int)Aerodactyl, (int)Mewtwo,

        (int)Ampharos, (int)Scizor, (int)Heracross, (int)Houndoom, (int)Tyranitar,

        (int)Blaziken, (int)Gardevoir, (int)Mawile, (int)Aggron, (int)Medicham,
        (int)Manectric, (int)Banette, (int)Absol, (int)Latios, (int)Latias,

        (int)Garchomp, (int)Lucario, (int)Abomasnow,

        // AO
        (int)Beedrill, (int)Pidgeot, (int)Slowbro,

        (int)Steelix,

        (int)Sceptile, (int)Swampert, (int)Sableye, (int)Sharpedo, (int)Camerupt,
        (int)Altaria, (int)Glalie, (int)Salamence, (int)Metagross, (int)Rayquaza,

        (int)Lopunny, (int)Gallade,
        (int)Audino, (int)Diancie,

        // USUM
        (int)Necrozma, // Ultra Necrozma
    };

    /// <summary>
    /// Species that have a primal form that cannot exist outside of battle.
    /// </summary>
    private static readonly HashSet<ushort> BattlePrimals = new() { (int)Kyogre, (int)Groudon };

    private static readonly HashSet<ushort> BattleOnly = GetBattleFormSet();

    private static HashSet<ushort> GetBattleFormSet()
    {
        var hs = new HashSet<ushort>(BattleForms);
        hs.UnionWith(BattleMegas);
        hs.UnionWith(BattlePrimals);
        return hs;
    }

    /// <summary>
    /// Checks if the <see cref="form"/> for the <see cref="species"/> is a Totem form.
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="format">Current generation format</param>
    public static bool IsTotemForm(ushort species, byte form, int format) => format == 7 && IsTotemForm(species, form);

    /// <summary>
    /// Checks if the <see cref="form"/> for the <see cref="species"/> is a Totem form.
    /// </summary>
    /// <remarks>Use <see cref="IsTotemForm(ushort,byte,int)"/> if you aren't 100% sure the format is 7.</remarks>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    public static bool IsTotemForm(ushort species, byte form)
    {
        if (form == 0)
            return false;
        if (!Legal.Totem_USUM.Contains(species))
            return false;
        if (species == (int)Mimikyu)
            return form is 2 or 3;
        if (Legal.Totem_Alolan.Contains(species))
            return form == 2;
        return form == 1;
    }

    public static bool HasTotemForm(ushort species)
    {
        return Legal.Totem_USUM.Contains(species);
    }

    /// <summary>
    /// Gets the base <see cref="form"/> for the <see cref="species"/> when the Totem form is reverted (on transfer).
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    public static byte GetTotemBaseForm(ushort species, byte form)
    {
        if (species == (int)Mimikyu)
            return 0;
        return --form;
    }

    public static bool IsLordForm(ushort species, byte form, int generation)
    {
        if (generation != 8)
            return false;
        return IsLordForm(species, form);
    }

    private static bool IsLordForm(ushort species, byte form) => form != 0 && species switch
    {
        (int)Arcanine when form == 2 => true,
        (int)Electrode when form == 2 => true,
        (int)Lilligant when form == 2 => true,
        (int)Avalugg when form == 2 => true,
        (int)Kleavor when form == 1 => true,
        _ => false,
    };

    private const int Vivillon3DSMaxWildFormID = 17; // 0-17 valid form indexes

    /// <summary>
    /// Checks if the <see cref="form"/> exists for the <see cref="species"/> without having an associated <see cref="PersonalInfo"/> index.
    /// </summary>
    /// <param name="species">Entity species</param>
    /// <param name="form">Entity form</param>
    /// <param name="format">Current generation format</param>
    /// <seealso cref="HasFormValuesNotIndicatedByPersonal"/>
    public static bool IsValidOutOfBoundsForm(ushort species, byte form, int format) => (Species)species switch
    {
        Unown => form < (format == 2 ? 26 : 28), // A-Z : A-Z?!
        Mothim => form < 3, // Burmy base form is kept

        Scatterbug => form <= Vivillon3DSMaxWildFormID, // Vivillon Pre-evolutions
        Spewpa => form <= Vivillon3DSMaxWildFormID, // Vivillon Pre-evolutions

        _ => false,
    };

    /// <summary>
    /// Checks if the <see cref="PKM"/> data should have a drop-down selection visible for the <see cref="PKM.Form"/> value.
    /// </summary>
    /// <param name="pi">Game specific personal info</param>
    /// <param name="species"><see cref="Species"/> ID</param>
    /// <param name="format"><see cref="PKM.Form"/> ID</param>
    /// <returns>True if has forms that can be provided by <see cref="FormConverter.GetFormList"/>, otherwise false for none.</returns>
    public static bool HasFormSelection(IPersonalInfo pi, ushort species, int format)
    {
        if (format <= 3 && species != (int)Unown)
            return false;

        if (HasFormValuesNotIndicatedByPersonal.Contains(species))
            return true;

        int count = pi.FormCount;
        return count > 1;
    }

    /// <summary>
    /// <seealso cref="IsValidOutOfBoundsForm"/>
    /// </summary>
    private static readonly HashSet<ushort> HasFormValuesNotIndicatedByPersonal = new()
    {
        (int)Unown,
        (int)Mothim, // (Burmy form is not cleared on evolution)
        (int)Scatterbug, (int)Spewpa, // Vivillon pre-evos
    };
}
