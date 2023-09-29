using System.ComponentModel;
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedType.Global
#nullable disable

namespace pkNX.Structures.FlatBuffers.SV;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class DeliveryOutbreakPokeDataArray { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class DeliveryOutbreakPokeData
{
    public bool IsAreaLimited(out uint areaBits)
    {
        areaBits = 0;
        if (Area is null)
            return false;
        if (Area.Area01) areaBits |= 1u << 1;
        if (Area.Area02) areaBits |= 1u << 2;
        if (Area.Area03) areaBits |= 1u << 3;
        if (Area.Area04) areaBits |= 1u << 4;
        if (Area.Area05) areaBits |= 1u << 5;
        if (Area.Area06) areaBits |= 1u << 6;
        if (Area.Area07) areaBits |= 1u << 7;
        if (Area.Area08) areaBits |= 1u << 8;
        if (Area.Area09) areaBits |= 1u << 9;
        if (Area.Area10) areaBits |= 1u << 10;
        if (Area.Area11) areaBits |= 1u << 11;
        if (Area.Area12) areaBits |= 1u << 12;
        if (Area.Area13) areaBits |= 1u << 13;
        if (Area.Area14) areaBits |= 1u << 14;
        if (Area.Area15) areaBits |= 1u << 15;
        if (Area.Area16) areaBits |= 1u << 16;
        if (Area.Area17) areaBits |= 1u << 17;
        if (Area.Area18) areaBits |= 1u << 18;
        if (Area.Area19) areaBits |= 1u << 19;
        if (Area.Area20) areaBits |= 1u << 20;
        if (Area.Area21) areaBits |= 1u << 21;
        if (Area.Area22) areaBits |= 1u << 22;
        if (Area.Area23) areaBits |= 1u << 23;
        return areaBits != 0;
    }

    private const ulong LocationNone = 0xCBF29CE484222645u;
    public bool IsSpecificArea => LocationName != LocationNone;
    public bool IsAreaConstrained => IsAreaLimited(out _);

    public bool IsEnableCompatible(OutbreakEnableTable other)
    {
        if (Enable is not { } en)
            return true;
        if (en.IsUnrestricted)
            return true;
        if (other.IsUnrestricted)
            return true;
        if (other.Air1 == en.Air1)
            return true;
        if (other.Air2 == en.Air2)
            return true;
        if (other.Land == en.Land)
            return true;
        if (other.UpWater == en.UpWater)
            return true;
        if (other.UnderWater == en.Underwater)
            return true;
        return false;
    }

    public bool IsCompatibleArea(ulong areaName) => LocationName != LocationNone && LocationName == areaName;
    public bool IsCompatibleArea(byte area) => !IsAreaLimited(out var bits) || (bits & (1u << area)) != 0;
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class AreaNo { }

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class OutbreakEnableTable
{
    public bool IsUnrestricted => !Air1 && !Air2 && !Land && !UpWater && !UnderWater;
}

public partial struct EnableTable
{
    public bool IsUnrestricted => !Air1 && !Air2 && !Land && !UpWater && !Underwater;
}
