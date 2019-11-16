using System;
using System.Diagnostics;

namespace pkNX.Structures
{
    /// <summary>
    /// <see cref="PersonalInfo"/> table (array).
    /// </summary>
    /// <remarks>
    /// Serves as the main object that is accessed for stat data in a particular generation/game format.
    /// </remarks>
    public class PersonalTable
    {
        private static byte[][] SplitBytes(byte[] data, int size)
        {
            byte[][] r = new byte[data.Length / size][];
            for (int i = 0; i < data.Length; i += size)
            {
                r[i / size] = new byte[size];
                Array.Copy(data, i, r[i / size], 0, size);
            }
            return r;
        }

        private static Func<byte[], PersonalInfo> GetConstructor(GameVersion format)
        {
            switch (format)
            {
                case GameVersion.BW:
                    return z => new PersonalInfoBW(z);
                case GameVersion.B2W2:
                    return z => new PersonalInfoB2W2(z);
                case GameVersion.XY:
                    return z => new PersonalInfoXY(z);
                case GameVersion.ORAS:
                    return z => new PersonalInfoORAS(z);
                case GameVersion.SM:
                case GameVersion.USUM:
                    return z => new PersonalInfoSM(z);
                case GameVersion.GG:
                    return z => new PersonalInfoGG(z);
                default:
                    return z => new PersonalInfoSWSH(z);
            }
        }

        private static int GetEntrySize(GameVersion format)
        {
            switch (format)
            {
                case GameVersion.BW: return PersonalInfoBW.SIZE;
                case GameVersion.B2W2: return PersonalInfoB2W2.SIZE;
                case GameVersion.XY: return PersonalInfoXY.SIZE;
                case GameVersion.ORAS: return PersonalInfoORAS.SIZE;
                case GameVersion.SM:
                case GameVersion.USUM:
                case GameVersion.GG: return PersonalInfoSM.SIZE;

                case GameVersion.SW:
                case GameVersion.SH:
                case GameVersion.SWSH: return PersonalInfoSWSH.SIZE;

                default: return -1;
            }
        }

        public PersonalTable(byte[] data, GameVersion format)
        {
            var get = GetConstructor(format);
            int size = GetEntrySize(format);
            byte[][] entries = SplitBytes(data, size);
            Table = new PersonalInfo[data.Length / size];
            for (int i = 0; i < Table.Length; i++)
                Table[i] = get(entries[i]);

            MaxSpeciesID = format.GetMaxSpeciesID();
        }

        public readonly PersonalInfo[] Table;

        /// <summary>
        /// Gets an index from the inner <see cref="Table"/> array.
        /// </summary>
        /// <remarks>Has built in length checks; returns empty (0) entry if out of range.</remarks>
        /// <param name="index">Index to retrieve</param>
        /// <returns>Requested index entry</returns>
        public PersonalInfo this[int index]
        {
            get
            {
                if (0 <= index && index < Table.Length)
                    return Table[index];
                return Table[0];
            }
            set
            {
                if (index < 0 || index >= Table.Length)
                    return;
                Table[index] = value;
            }
        }

        /// <summary>
        /// Gets the abilities possible for a given Species ID and AltForm ID.
        /// </summary>
        /// <param name="species">Species ID</param>
        /// <param name="forme">AltForm ID</param>
        /// <returns>Array of possible abilities</returns>
        public int[] GetAbilities(int species, int forme)
        {
            return GetFormeEntry(species, forme).Abilities;
        }

        /// <summary>
        /// Gets the <see cref="PersonalInfo"/> entry index for a given Species ID and AltForm ID.
        /// </summary>
        /// <param name="species">Species ID</param>
        /// <param name="forme">AltForm ID</param>
        /// <returns>Entry index for the input criteria</returns>
        public int GetFormeIndex(int species, int forme)
        {
            if (species > MaxSpeciesID)
            { Debug.WriteLine($"Requested out of bounds {nameof(species)}: {species} (max={MaxSpeciesID})"); species = 0; }
            return this[species].FormeIndex(species, forme);
        }

        /// <summary>
        /// Gets the <see cref="PersonalInfo"/> entry for a given Species ID and AltForm ID.
        /// </summary>
        /// <param name="species">Species ID</param>
        /// <param name="forme">AltForm ID</param>
        /// <returns>Entry for the input criteria</returns>
        public PersonalInfo GetFormeEntry(int species, int forme)
        {
            return this[GetFormeIndex(species, forme)];
        }

        /// <summary>
        /// Count of entries in the table, which includes default species entries and their separate AltForm ID entreis.
        /// </summary>
        public int TableLength => Table.Length;

        /// <summary>
        /// Maximum Species ID for the Table.
        /// </summary>
        public readonly int MaxSpeciesID;

        /// <summary>
        /// Gets form names for every species.
        /// </summary>
        /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
        /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
        /// <returns>Array of species containing an array of form names for that species.</returns>
        public string[][] GetFormList(string[] species, int MaxSpecies)
        {
            string[][] FormList = new string[MaxSpecies + 1][];
            for (int i = 0; i < FormList.Length; i++)
            {
                int FormCount = this[i].FormeCount;
                FormList[i] = new string[FormCount];
                if (FormCount <= 0) continue;
                FormList[i][0] = species[i];
                for (int j = 1; j < FormCount; j++)
                    FormList[i][j] = $"{species[i]} {j}";
            }

            return FormList;
        }

        /// <summary>
        /// Gets an arranged list of Form names and indexes for use with the individual <see cref="PersonalInfo"/> AltForm ID values.
        /// </summary>
        /// <param name="AltForms">Raw string resource (Forms) for the corresponding table.</param>
        /// <param name="species">Raw string resource (Species) for the corresponding table.</param>
        /// <param name="MaxSpecies">Max Species ID (Species ID)</param>
        /// <param name="baseForm">Pointers for base form IDs</param>
        /// <param name="formVal">Pointers for table indexes for each form</param>
        /// <returns>Sanitized list of species names, and outputs indexes for various lookup purposes.</returns>
        public string[] GetPersonalEntryList(string[][] AltForms, string[] species, int MaxSpecies, out int[] baseForm, out int[] formVal)
        {
            string[] result = new string[Table.Length];
            baseForm = new int[result.Length];
            formVal = new int[result.Length];
            for (int i = 0; i <= MaxSpecies; i++)
            {
                result[i] = species[i];
                if (AltForms[i].Length == 0) continue;
                int altformpointer = this[i].FormStatsIndex;
                if (altformpointer <= 0) continue;
                for (int j = 1; j < AltForms[i].Length; j++)
                {
                    int ptr = altformpointer + j - 1;
                    baseForm[ptr] = i;
                    formVal[ptr] = j;
                    result[ptr] = AltForms[i][j];
                }
            }
            return result;
        }

        public int[] GetSpeciesForm(int entry) => GetSpeciesForm(entry, MaxSpeciesID);

        public int[] GetSpeciesForm(int entry, int maxSpecies)
        {
            if (entry < maxSpecies)
                return new[] { entry, 0 };

            for (int i = 0; i < maxSpecies; i++)
            {
                var altformpointer = this[i].FormStatsIndex;
                if (altformpointer <= 0)
                    continue;

                int formes = this[i].FormeCount - 1; // Mons with no alt forms have a FormCount of 1.
                for (int j = 0; j < formes; j++)
                {
                    if (altformpointer + j == entry)
                        return new[] { i, j };
                }
            }

            return new[] { -1, -1 };
        }
    }
}
