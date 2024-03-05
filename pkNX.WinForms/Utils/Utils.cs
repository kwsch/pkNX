using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace pkNX.WinForms;

// Source: https://stackoverflow.com/questions/21750824/how-to-convert-a-string-to-a-mathematical-expression-programmatically
public static class StringToFormula
{
    private static readonly string[] operators = ["-", "+", "/", "%", "*", "^"];
    private static readonly Func<double, double, double>[] operations = [
            (a1, a2) => a1 - a2,
            (a1, a2) => a1 + a2,
            (a1, a2) => a1 / a2,
            (a1, a2) => a1 % a2,
            (a1, a2) => a1 * a2,
            Math.Pow,
    ];

    public static bool TryEval(string expression, out double value)
    {
        try
        {
            value = Eval(expression);
            return true;
        }
        catch
        {
            value = 0.0;
            return false;
        }
    }

    public static double Eval(string expression)
    {
        List<string> tokens = GetTokens(expression);
        var operandStack = new Stack<double>();
        var operatorStack = new Stack<string>();
        int tokenIndex = 0;

        while (tokenIndex < tokens.Count)
        {
            string token = tokens[tokenIndex];
            switch (token)
            {
                case "(":
                {
                    string subExpr = GetSubExpression(tokens, ref tokenIndex);
                    operandStack.Push(Eval(subExpr));
                    continue;
                }
                case ")":
                    throw new ArgumentException("Mis-matched parentheses in expression");
            }

            //If this is an operator  
            if (Array.IndexOf(operators, token) >= 0)
            {
                while (operatorStack.Count > 0 && Array.IndexOf(operators, token) < Array.IndexOf(operators, operatorStack.Peek()))
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(operations[Array.IndexOf(operators, op)](arg1, arg2));
                }
                operatorStack.Push(token);
            }
            else
            {
                operandStack.Push(double.Parse(token, CultureInfo.InvariantCulture));
            }
            tokenIndex++;
        }

        while (operatorStack.Count > 0)
        {
            string op = operatorStack.Pop();
            double arg2 = operandStack.Pop();
            double arg1 = operandStack.Pop();
            operandStack.Push(operations[Array.IndexOf(operators, op)](arg1, arg2));
        }
        return operandStack.Pop();
    }

    private static string GetSubExpression(IReadOnlyList<string> tokens, ref int index)
    {
        var subExpr = new StringBuilder();
        int parenlevels = 1;
        index++;
        while (index < tokens.Count && parenlevels > 0)
        {
            string token = tokens[index];
            if (tokens[index] == "(")
                parenlevels++;
            else if (tokens[index] == ")")
                parenlevels--;

            if (parenlevels > 0)
                subExpr.Append(token);

            index++;
        }

        if (parenlevels > 0)
            throw new ArgumentException("Mis-matched parentheses in expression");
        return subExpr.ToString();
    }

    private static List<string> GetTokens(string expression)
    {
        const string operators = "()^*/%+-";
        List<string> tokens = [];
        StringBuilder sb = new();

        foreach (char c in expression.Replace(" ", string.Empty))
        {
            if (operators.Contains(c))
            {
                if (sb.Length > 0)
                {
                    tokens.Add(sb.ToString());
                    sb.Length = 0;
                }
                tokens.Add(c.ToString());
            }
            else
            {
                sb.Append(c);
            }
        }

        if (sb.Length > 0)
        {
            tokens.Add(sb.ToString());
        }
        return tokens;
    }
}
public static class Utils
{
    [System.Runtime.InteropServices.DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    public static ImageSource? ImageSourceFromBitmap(System.Drawing.Bitmap bmp)
    {
        var handle = bmp.GetHbitmap();
        try
        {
            ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(handle);
            return newSource;
        }
        catch (Exception)
        {
            DeleteObject(handle);
            return null;
        }
    }

    public static System.Drawing.Bitmap ResizeBitmap(System.Drawing.Image sourceBmp, int width, int height)
    {
        var result = new System.Drawing.Bitmap(width, height);
        using System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result);
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        g.DrawImage(sourceBmp, 0, 0, width, height);
        return result;
    }

    public static IconChar FromFileExtension(string fileExtension)
    {
        fileExtension = fileExtension.Replace(".", "").ToLowerInvariant(); // Make sure it doesn't contain a dot

        return fileExtension switch // Only add icons for supported extensions
        {
            "obj" or "fbx" => IconChar.Shapes,
            "vox" => IconChar.Th,
            "txt" or "json" => IconChar.FileAlt,
            "zip" => IconChar.FileArchive,
            "wav" => IconChar.FileAudio,
            "jpg" or "jpeg" or "png" or "tga" or "bmp" or "psd" or "gif" or "hdr" or "pic" => IconChar.FileImage,
            "cs" or "cpp" or "hpp" or "h" => IconChar.FileCode,
            "fx" or "ps" or "vs" or "hs" or "ds" or "gs" or "comp" => IconChar.FileCode,// Shaders
            _ => IconChar.File,
        };
    }

    public static bool HasChildren(this DependencyObject? depObj)
    {
        if (depObj == null) return false;

        return VisualTreeHelper.GetChildrenCount(depObj) > 0;
    }

    public static IEnumerable<DependencyObject?> GetChildren(this DependencyObject? depObj)
    {
        if (depObj == null) yield return null;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj!); i++)
        {
            yield return VisualTreeHelper.GetChild(depObj!, i);
        }
    }

    public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject obj)
    {
        foreach (var child in obj.GetChildren())
        {
            yield return child!;
            foreach (var descendant in child!.GetDescendants())
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<T> GetDescendants<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        foreach (var child in obj.GetChildren())
        {
            if (child is T x)
                yield return x;

            foreach (var descendant in child!.GetDescendants())
            {
                if (descendant is T y)
                    yield return y;
            }
        }
    }

    public static T GetDescendant<T>(this DependencyObject obj)
        where T : DependencyObject
    {
        return GetDescendants<T>(obj).First();
    }

    public static T FindParentOfType<T>(this DependencyObject element)
        where T : DependencyObject
    {
        return Enumerable.FirstOrDefault(Enumerable.OfType<T>(element.GetParents()));
    }

    public static IEnumerable<DependencyObject> GetParents(this DependencyObject element)
    {
        while ((element = element.GetParent()) != null)
            yield return element;
    }

    private static DependencyObject GetParent(this DependencyObject element)
    {
        DependencyObject? parent = VisualTreeHelper.GetParent(element);
        if (parent != null)
            return parent;

        if (element is FrameworkElement frameworkElement)
            parent = frameworkElement.Parent;
        return parent;
    }

    public static void OverrideDefault(DependencyProperty property, Type forType, object newDefault)
    {
        var metaData = property.GetMetadata(forType);

        PropertyMetadata newMetaData;
        if (metaData is FrameworkPropertyMetadata frameworkMeta)
        {
            newMetaData = new FrameworkPropertyMetadata(newDefault, TranslateFlags(frameworkMeta), metaData.PropertyChangedCallback, metaData.CoerceValueCallback, frameworkMeta.IsAnimationProhibited, frameworkMeta.DefaultUpdateSourceTrigger);
        }
        else if (metaData is UIPropertyMetadata uiMeta)
        {
            newMetaData = new UIPropertyMetadata(newDefault, metaData.PropertyChangedCallback, metaData.CoerceValueCallback, uiMeta.IsAnimationProhibited);
        }
        else
        {
            newMetaData = new PropertyMetadata(newDefault, metaData.PropertyChangedCallback, metaData.CoerceValueCallback);
        }

        property.OverrideMetadata(forType, newMetaData);
    }

    public static FrameworkPropertyMetadataOptions TranslateFlags(FrameworkPropertyMetadata metaData)
    {
        FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.None;

        if (metaData.AffectsMeasure)
            flags |= FrameworkPropertyMetadataOptions.AffectsMeasure;

        if (metaData.AffectsArrange)
            flags |= FrameworkPropertyMetadataOptions.AffectsArrange;
        if (metaData.AffectsParentMeasure)
            flags |= FrameworkPropertyMetadataOptions.AffectsParentMeasure;
        if (metaData.AffectsParentArrange)
            flags |= FrameworkPropertyMetadataOptions.AffectsParentArrange;
        if (metaData.AffectsRender)
            flags |= FrameworkPropertyMetadataOptions.AffectsRender;
        if (metaData.OverridesInheritanceBehavior)
            flags |= FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior;
        if (metaData.IsNotDataBindable)
            flags |= FrameworkPropertyMetadataOptions.NotDataBindable;
        if (metaData.BindsTwoWayByDefault)
            flags |= FrameworkPropertyMetadataOptions.BindsTwoWayByDefault;
        if (metaData.Journal)
            flags |= FrameworkPropertyMetadataOptions.Journal;
        if (metaData.SubPropertiesDoNotAffectRender)
            flags |= FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender;

        var prop = ReflectionUtils.GetProperty(metaData, "IsInherited");
        if ((bool)prop.GetValue(metaData))
            flags |= FrameworkPropertyMetadataOptions.Inherits;

        return flags;
    }

    public static void FocusParent(this FrameworkElement element)
    {
        FrameworkElement parent = (FrameworkElement)element.Parent;
        while (parent is IInputElement { Focusable: false })
        {
            parent = (FrameworkElement)parent.Parent;
        }

        DependencyObject scope = FocusManager.GetFocusScope(element);
        FocusManager.SetFocusedElement(scope, parent);
    }

    private static readonly string[] SizeSuffixes =
               ["bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
    public static string FormatByteString(long value, int decimalPlaces = 1)
    {
        if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException(nameof(decimalPlaces)); }
        switch (value)
        {
            case < 0:
                return "-" + FormatByteString(-value, decimalPlaces);
            case 0:
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
        }

        // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
        int mag = (int)Math.Log(value, 1024);

        // 1L << (mag * 10) == 2 ^ (10 * mag) 
        // [i.e. the number of bytes in the unit corresponding to mag]
        decimal adjustedSize = (decimal)value / (1L << (mag * 10));

        // make adjustment when the value is large enough that
        // it would round up to 1000 or more
        if (Math.Round(adjustedSize, decimalPlaces) < 1000)
            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);

        mag++;
        adjustedSize /= 1024;

        return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
    }
}

public static class FileUtils
{
    public static void RemoveReadOnly(string filePath, bool force = false)
    {
        // Remove read only property
        if (!File.Exists(filePath))
            return;

        if (!force && MessageBox.Show(
                "The file is marked read-only, would you like to remove this flag?",
                "Remove Read-Only flag", MessageBoxButton.YesNo, MessageBoxImage.Warning) !=
            MessageBoxResult.Yes)
        {
            return;
        }

        FileAttributes attributes = File.GetAttributes(filePath);
        attributes &= ~FileAttributes.ReadOnly;
        File.SetAttributes(filePath, attributes);
    }

    public static void CopyFile(string sourceFilePath, string destinationFilePath, bool forceOverwrite = false)
    {
        if (!File.Exists(destinationFilePath))
        {
            File.Copy(sourceFilePath, destinationFilePath);
        }
        else
        {
            bool overwrite = forceOverwrite;

            if (!overwrite)
            {
                var source = new FileInfo(sourceFilePath);

                const string caption = "Overwrite file";
                var msg = $"A file with the name \"{source.Name}\" already exists in the folder: \"{source.DirectoryName}\"\nWould you like to overwrite the file?";

                overwrite = MessageBox.Show(msg, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
            }

            if (overwrite)
            {
                RemoveReadOnly(destinationFilePath);
                File.Copy(sourceFilePath, destinationFilePath, true);
            }
        }
    }

    public static void CopyFileToDirectory(string sourceFilePath, string destinationDirPath)
    {
        if (sourceFilePath == destinationDirPath)
            return;

        var source = new FileInfo(sourceFilePath);
        if (!source.Exists)
            throw new DirectoryNotFoundException($"Source file does not exist: {sourceFilePath}");

        if (!Directory.Exists(destinationDirPath))
            throw new DirectoryNotFoundException($"Destination directory does not exist: {destinationDirPath}");

        CopyFile(sourceFilePath, destinationDirPath + "/" + source.Name);
    }

    public static void CopyDirectory(string sourceDirPath, string destinationDirPath)
    {
        DirectoryInfo source = new DirectoryInfo(sourceDirPath);
        if (!source.Exists)
            throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDirPath}");

        if (!Directory.Exists(destinationDirPath))
            throw new DirectoryNotFoundException($"Destination directory does not exist: {destinationDirPath}");

        string newDirPath = destinationDirPath + "/" + source.Name;

        // If the new directory doesn't exist, create it.
        if (!Directory.Exists(newDirPath))
            Directory.CreateDirectory(newDirPath);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = source.GetFiles();
        foreach (FileInfo file in files)
            CopyFileToDirectory(file.FullName, newDirPath); // TODO: Overwrite? Yes for all files option

        // Copy contents of sub-directories
        DirectoryInfo[] dirs = source.GetDirectories();
        foreach (DirectoryInfo subdir in dirs)
            CopyDirectory(subdir.FullName, newDirPath);
    }
}

public static class ColorUtils
{
    public static readonly double RGBToDouble = 0.0039215686274509803921568627451;

    public struct RGB(double r, double g, double b)
    {
        public double R { get; set; } = r;
        public double G { get; set; } = g;
        public double B { get; set; } = b;

        public static RGB FromColor(Color color)
        {
            return new RGB { R = color.R * RGBToDouble, G = color.G * RGBToDouble, B = color.B * RGBToDouble };
        }

        public Color ToColor()
        {
            return Color.FromRgb((byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }

        public Color ToColor(byte alpha)
        {
            return Color.FromArgb(alpha, (byte)(R * 255), (byte)(G * 255), (byte)(B * 255));
        }
    };

    public struct HSV(double h, double s, double v)
    {
        public double H { get; set; } = h;
        public double S { get; set; } = s;
        public double V { get; set; } = v;
    };

    public static RGB HSVtoRGB(HSV hsv)
    {
        int i;
        double f, p, q, t;

        if (hsv.S <= 0)
        {
            // achromatic (grey)
            return new RGB(hsv.V, hsv.V, hsv.V);
        }

        if (hsv.H >= 360.0)
            hsv.H = 0.0;

        hsv.H /= 60.0;
        i = (int)Math.Floor(hsv.H);
        f = hsv.H - i;
        p = hsv.V * (1.0 - hsv.S);
        q = hsv.V * (1.0 - (hsv.S * f));
        t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

        return i switch
        {
            0 => new RGB(hsv.V, t, p),
            1 => new RGB(q, hsv.V, p),
            2 => new RGB(p, hsv.V, t),
            3 => new RGB(p, q, hsv.V),
            4 => new RGB(t, p, hsv.V),
            _ => new RGB(hsv.V, p, q),
        };
    }

    public static HSV RGBToHSV(RGB rgb)
    {
        double min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));
        double max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));

        double delta = max - min;

        var hsv = new HSV
        {
            V = max,
        };

        if (max == 0)
        {
            hsv.S = 0;
            hsv.H = -1;
            return hsv;
        }

        hsv.S = delta / max;

        if (rgb.R == max)
            hsv.H = (rgb.G - rgb.B) / delta;       // between yellow & magenta
        else if (rgb.G == max)
            hsv.H = 2 + ((rgb.B - rgb.R) / delta);   // between cyan & yellow
        else
            hsv.H = 4 + ((rgb.R - rgb.G) / delta);   // between magenta & cyan

        hsv.H *= 60;               // degrees
        if (hsv.H < 0)
            hsv.H += 360;

        return hsv;
    }
}
