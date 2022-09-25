using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace pkNX.Structures
{
    public class ResourcesUtil
    {
        private static readonly Assembly thisAssembly = typeof(Util).GetTypeInfo().Assembly;
        private static readonly Dictionary<string, string> resourceNameMap = BuildLookup(thisAssembly.GetManifestResourceNames());

        /// <summary>
        /// Personal Table used in <see cref="GameVersion.SWSH"/>.
        /// </summary>
        public static readonly PersonalTable8SWSH SWSH = new(GetTableBinary("swsh"));

        /// <summary>
        /// Personal Table used in <see cref="GameVersion.USUM"/>.
        /// </summary>
        public static readonly PersonalTable7SM USUM = new(GetTableBinary("usum"), Legal.MaxSpeciesID_7_USUM);

        /// <summary>
        /// Evolution Table used in <see cref="GameVersion.SWSH"/>.
        /// </summary>
        public static readonly IReadOnlyList<EvolutionMethod[]> SWSH_Evolutions = EvolutionSet8.GetArray(GetReader("ss"));

        /// <summary>
        /// Evolution Table used in <see cref="GameVersion.USUM"/>.
        /// </summary>
        public static readonly IReadOnlyList<EvolutionMethod[]> USUM_Evolutions = EvolutionSet7.GetArray(GetReader("uu"));

        private static ReadOnlySpan<byte> GetTableBinary(string game) => GetBinaryResource($"personal_{game}");
        private static ReadOnlySpan<byte> GetEvolutionBinary(string game) => GetBinaryResource($"evos_{game}.pkl");
        private static BinLinkerAccessor GetReader(string resource) => BinLinkerAccessor.Get(GetEvolutionBinary(resource), resource);

        private static string GetFileName(string resName)
        {
            var period = resName.LastIndexOf('.', resName.Length - 5);
            var start = period + 1;
            System.Diagnostics.Debug.Assert(start != 0);

            // text file fetch excludes ".txt" (mixed case...); other extensions are used (all lowercase).
            return resName.EndsWith(".txt", StringComparison.Ordinal) ? resName[start..^4].ToLowerInvariant() : resName[start..];
        }

        private static Dictionary<string, string> BuildLookup(IReadOnlyCollection<string> manifestNames)
        {
            var result = new Dictionary<string, string>(manifestNames.Count);
            foreach (var resName in manifestNames)
            {
                var fileName = GetFileName(resName);
                result.Add(fileName, resName);
            }
            return result;
        }

        public static byte[] GetBinaryResource(string name)
        {
            if (!resourceNameMap.TryGetValue(name, out var resName))
                return Array.Empty<byte>();

            using var resource = thisAssembly.GetManifestResourceStream(resName);
            if (resource is null)
                return Array.Empty<byte>();

            var buffer = new byte[resource.Length];
            _ = resource.Read(buffer, 0, (int)resource.Length);
            return buffer;
        }
    }
}
