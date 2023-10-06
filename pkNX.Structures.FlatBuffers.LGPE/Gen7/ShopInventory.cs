using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.LGPE;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class ShopInventory { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class SingleShop
{
    public override string ToString() => $"{Hash:X16} - {Inventories}";

    public static readonly Dictionary<ulong, string> LGPE = new()
    {
        { 14688060730225415067, "Celadon Department Store [TMs]" },
        { 04251032178319698087, "Celadon Department Store [Evolution Stones]" },
    };
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class MultiShop
{
    public override string ToString() => $"{Hash:X16} - {string.Join(", ", Inventories.Select(z => z.ToString()))}";

    public static readonly Dictionary<ulong, string> LGPE = new()
    {
        { 0x66CA73B2966BB871, "Poké Mart Inventories [0-8 Badges]" },
    };
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class Inventory
{
    public override string ToString() => $"{string.Join(",", Items.Select(z => z.ToString()))}";
}
