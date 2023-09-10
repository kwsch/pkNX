using pkNX.Game;
using pkNX.Structures;
using System;

namespace pkNX.WinForms
{
    public static class UIStaticSources
    {
        public static readonly string[] EvolutionMethods = Enum.GetNames(typeof(EvolutionType));
        public static readonly string[] EggGroups = Enum.GetNames(typeof(EggGroup));
        public static readonly string[] PokeColors = Enum.GetNames(typeof(PokeColor));
        public static readonly string[] EXPGroups = Enum.GetNames(typeof(EXPGroup));
        public static string[] SpeciesList = Array.Empty<string>();
        public static string[] FormsList = Array.Empty<string>();
        public static string[] SpeciesClassificationsList = Array.Empty<string>();
        public static string[] ItemsList = Array.Empty<string>();
        public static string[] AbilitiesList = Array.Empty<string>();
        public static string[] MovesList = Array.Empty<string>();
        public static string[] TypesList = Array.Empty<string>();

        public static void SetupForGame(GameManager ROM)
        {
            SpeciesList = ROM.GetStrings(TextName.SpeciesNames);
            FormsList = ROM.GetStrings(TextName.Forms);
            ItemsList = ROM.GetStrings(TextName.ItemNames);
            MovesList = ROM.GetStrings(TextName.MoveNames);
            MovesList = EditorUtil.SanitizeMoveList(MovesList);
            TypesList = ROM.GetStrings(TextName.TypeNames);
            AbilitiesList = ROM.GetStrings(TextName.AbilityNames);
            SpeciesClassificationsList = ROM.GetStrings(TextName.SpeciesClassifications);
        }
    }
}
