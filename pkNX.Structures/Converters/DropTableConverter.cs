using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

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