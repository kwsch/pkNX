using System;
using System.ComponentModel;

namespace pkNX.Structures;

public class DropTableConverter : TypeConverter
{
    public static ulong[] DropTableHashes = Array.Empty<ulong>();

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(DropTableHashes);
    }
}
