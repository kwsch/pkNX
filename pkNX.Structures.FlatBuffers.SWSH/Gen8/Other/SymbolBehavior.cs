using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

// symbol_encount_mons_param.bin
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SymbolBehaveRoot { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SymbolBehave
{
    public Species Species => (Species)SpeciesID;
}
