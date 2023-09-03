using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace pkNX.WinForms;

public static class FormsIconHelper
{
    private static int UniCode<TEnum>(this TEnum icon)
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        return icon.ToInt32(CultureInfo.InvariantCulture);
    }

    private static readonly string[] FontTitles =
    {
        "Font Awesome 6 Free Regular", // fa-regular-400.ttf
        "Font Awesome 6 Free Solid", // fa-solid-900.ttf
        "Font Awesome 6 Brands Regular" // fa-brands-400.ttf
    };

    private static readonly Typeface[] Typefaces = typeof(IconHelper).Assembly.LoadTypefaces("fonts", FontTitles);

    internal static FontFamily FontFor(IconChar iconChar)
    {
        if (IconHelper.Orphans.Contains(iconChar)) return null;
        var typeFace = Typefaces.Find(iconChar.UniCode(), out _, out _);
        return typeFace?.FontFamily;
    }

    internal static FontFamily FontFamilyFor(this IconChar iconChar, IconFont iconFont)
    {
        if (iconFont == IconFont.Auto) return FontFor(iconChar);
        var key = (int)iconFont;
        if (FontForStyle.TryGetValue(key, out var fontFamily)) return fontFamily;

        int id = Array.FindIndex(FontTitles, f =>
            f.IndexOf(iconFont.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0);

        fontFamily = Typefaces[id].FontFamily;
        FontForStyle.Add(key, fontFamily);
        return fontFamily;
    }
    private static readonly Dictionary<int, FontFamily> FontForStyle = new Dictionary<int, FontFamily>();

}
