using System.ComponentModel;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers.Arceus;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ThrowableParamTable
{
    public void AddEntry(int i)
    {
        Table = Table.Append(new ThrowableParam
        {
            Label01 = string.Empty,
            Label02 = string.Empty,
        }).ToList();
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ThrowableParam;
