using System;
using System.ComponentModel;
using System.IO;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Learnset;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LearnsetEntry;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class LearnsetMeta
{
    public byte[] WriteLearnsetAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        foreach (var entry in Arceus)
        {
            br.Write(entry.Move);
            br.Write(entry.Level);
        }
        br.Write(-1);
        return ms.ToArray();
    }

    public byte[] WriteMasteryAsLearn6()
    {
        using var ms = new MemoryStream();
        using var br = new BinaryWriter(ms);
        foreach (var entry in Arceus)
        {
            br.Write(entry.Move);
            br.Write(entry.LevelMaster);
        }
        br.Write(-1);
        return ms.ToArray();
    }
}
