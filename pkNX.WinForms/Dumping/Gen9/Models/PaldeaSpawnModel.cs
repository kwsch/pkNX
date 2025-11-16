using System;
using pkNX.Containers;
using pkNX.Structures.FlatBuffers.SV;

namespace pkNX.Structures.FlatBuffers;

public record PaldeaSpawnModel
{
    public PaldeaSpawnSet Main { get; init; }
    public PaldeaSpawnSet Atlantis { get; init; }
    public PaldeaSpawnSet Kitakami { get; init; }
    public PaldeaSpawnSet Terarium { get; init; }

    public PaldeaSpawnModel(IFileInternal rom)
    {
        var mlEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(rom.GetPackedFile("world/data/encount/point_data/point_data/encount_data_100000.bin"));
        var alEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(rom.GetPackedFile("world/data/encount/point_data/point_data/encount_data_atlantis.bin"));
        var su1EncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(rom.GetPackedFile("world/data/encount/point_data/point_data/encount_data_su1.bin"));
        var su2EncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(rom.GetPackedFile("world/data/encount/point_data/point_data/encount_data_su2.bin"));
        //var lcEncPoints = FlatBufferConverter.DeserializeFrom<PointDataArray>(ROM.GetPackedFile("world/data/encount/point_data/point_data/encount_data_lc.bin"));
        var pokeDataMain = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(rom.GetPackedFile("world/data/encount/pokedata/pokedata/pokedata_array.bin"));
        var pokeDataSu1 = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(rom.GetPackedFile("world/data/encount/pokedata/pokedata_su1/pokedata_su1_array.bin"));
        var pokeDataSu2 = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(rom.GetPackedFile("world/data/encount/pokedata/pokedata_su2/pokedata_su2_array.bin"));
        //var pokeDataLc = FlatBufferConverter.DeserializeFrom<EncountPokeDataArray>(ROM.GetPackedFile("world/data/encount/pokedata/pokedata_lc/pokedata_lc_array.bfbs"));

        Main = new PaldeaSpawnSet(pokeDataMain, mlEncPoints);
        Atlantis = new PaldeaSpawnSet(pokeDataMain, alEncPoints);
        Kitakami = new PaldeaSpawnSet(pokeDataSu1, su1EncPoints);
        Terarium = new PaldeaSpawnSet(pokeDataSu2, su2EncPoints);
        //var lc = ReformatPoints(lcEncPoints);
    }

    public PaldeaSpawnSet GetSet(PaldeaFieldIndex fieldIndex, PaldeaPointPivot type) => fieldIndex switch
    {
        PaldeaFieldIndex.Paldea => type switch
        {
            PaldeaPointPivot.Overworld => Main,
            PaldeaPointPivot.AreaZero => Atlantis,
            _ => throw new ArgumentException($"Could not handle {type}"),
        },
        PaldeaFieldIndex.Kitakami => Kitakami,
        PaldeaFieldIndex.Terarium => Terarium,
        _ => throw new ArgumentException($"Could not handle {fieldIndex}"),
    };

    public LocationPointDetail[] GetPoints(PaldeaFieldIndex fieldIndex, PaldeaPointPivot type) => GetSet(fieldIndex, type).Points;
    public EncountPokeDataArray GetPokeData(PaldeaFieldIndex fieldIndex) => GetSet(fieldIndex, PaldeaPointPivot.Overworld).Criteria;

    public record PaldeaSpawnSet(EncountPokeDataArray Criteria, LocationPointDetail[] Points)
    {
        public PaldeaSpawnSet(EncountPokeDataArray criteria, PointDataArray points) : this(criteria, ReformatPoints(points))
        {
        }

        // Points can be used by multiple areas as crossover sources. Need to be able to "belong" to multiple areas, and indicate their parent area.
        private static LocationPointDetail[] ReformatPoints(PointDataArray all)
        {
            var arr = all.Table;
            var result = new LocationPointDetail[arr.Count];
            for (int i = 0; i < arr.Count; i++)
                result[i] = new LocationPointDetail(arr[i]);
            return result;
        }
    }
}
