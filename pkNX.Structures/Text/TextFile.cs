using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace pkNX.Structures;

public class TextFile
{
    public bool SETEMPTYTEXT { get; set; } = true;

    // Text Formatting Config
    private const ushort KEY_BASE = 0x7C89;
    private const ushort KEY_ADVANCE = 0x2983;
    private const ushort KEY_VARIABLE = 0x0010;
    private const ushort KEY_TERMINATOR = 0x0000;
    private const ushort KEY_TEXTRETURN = 0xBE00;
    private const ushort KEY_TEXTCLEAR = 0xBE01;
    private const ushort KEY_TEXTWAIT = 0xBE02;
    private const ushort KEY_TEXTNULL = 0xBDFF;
    private const ushort KEY_TEXTRUBY = 0xFF01;
    private static readonly byte[] emptyTextFile = { 0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00 };

    public TextFile(TextConfig? config = null, bool remapChars = false) : this(emptyTextFile, config, remapChars) { }

    public TextFile(byte[] data, TextConfig? config = null, bool remapChars = false)
    {
        Data = (byte[])data.Clone();

        if (InitialKey != 0)
            throw new Exception("Invalid initial key! Not 0?");
        if (SectionDataOffset + TotalLength != Data.Length || TextSections != 1)
            throw new Exception("Invalid Text File");
        if (SectionLength != TotalLength)
            throw new Exception("Section size and overall size do not match.");

        Config = config ?? TextConfig.Default;
        RemapChars = remapChars;
    }

    public byte[] Data;
    private readonly TextConfig Config;
    private readonly bool RemapChars;

    private ushort TextSections { get => BitConverter.ToUInt16(Data, 0x0); set => BitConverter.GetBytes(value).CopyTo(Data, 0x0); } // Always 0x0001
    private ushort LineCount { get => BitConverter.ToUInt16(Data, 0x2); set => BitConverter.GetBytes(value).CopyTo(Data, 0x2); }
    private uint TotalLength { get => BitConverter.ToUInt32(Data, 0x4); set => BitConverter.GetBytes(value).CopyTo(Data, 0x4); }
    private uint InitialKey => BitConverter.ToUInt32(Data, 0x8); // Always 0x00000000
    private uint SectionDataOffset { get => BitConverter.ToUInt32(Data, 0xC); set => BitConverter.GetBytes(value).CopyTo(Data, 0xC); } // Always 0x0010
    private uint SectionLength { get => BitConverter.ToUInt32(Data, (int)SectionDataOffset); set => BitConverter.GetBytes(value).CopyTo(Data, SectionDataOffset); }

    private TextLine[] LineOffsets
    {
        get
        {
            TextLine[] result = new TextLine[LineCount];
            int sdo = (int)SectionDataOffset;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new TextLine
                {
                    Offset = BitConverter.ToInt32(Data, (i * 8) + sdo + 4) + sdo,
                    Length = BitConverter.ToInt16(Data, (i * 8) + sdo + 8),
                };
            }

            return result;
        }
        set
        {
            int sdo = (int)SectionDataOffset;
            for (int i = 0; i < value.Length; i++)
            {
                BitConverter.GetBytes(value[i].Offset).CopyTo(Data, (i * 8) + sdo + 4);
                BitConverter.GetBytes(value[i].Length).CopyTo(Data, (i * 8) + sdo + 8);
            }
        }
    }

    public byte[] GetEncryptedLine(int index)
    {
        ushort key = GetLineKey(index);
        var line = LineOffsets[index];
        byte[] EncryptedLineData = new byte[line.Length * 2];
        Array.Copy(Data, line.Offset, EncryptedLineData, 0, EncryptedLineData.Length);

        return CryptLineData(EncryptedLineData, key);
    }

    private static ushort GetLineKey(int index)
    {
        ushort key = KEY_BASE;
        for (int i = 0; i < index; i++)
            key += KEY_ADVANCE;
        return key;
    }

    public byte[][] LineData
    {
        get
        {
            ushort key = KEY_BASE;
            var result = new byte[LineCount][];
            var lines = LineOffsets;
            for (int i = 0; i < lines.Length; i++)
            {
                byte[] EncryptedLineData = new byte[lines[i].Length * 2];
                Array.Copy(Data, lines[i].Offset, EncryptedLineData, 0, EncryptedLineData.Length);

                result[i] = CryptLineData(EncryptedLineData, key);
                key += KEY_ADVANCE;
            }
            return result;
        }
        set
        {
            // rebuild LineInfo
            var lines = new TextLine[value.Length];
            int bytesUsed = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new TextLine { Offset = 4 + (8 * value.Length) + bytesUsed, Length = value[i].Length / 2 };
                bytesUsed += value[i].Length;
            }

            // Apply Line Data
            int sdo = (int)SectionDataOffset;
            Array.Resize(ref Data, sdo + 4 + (8 * value.Length) + bytesUsed);
            LineOffsets = lines;
            value.SelectMany(i => i).ToArray().CopyTo(Data, Data.Length - bytesUsed);
            TotalLength = SectionLength = (uint)(Data.Length - sdo);
            LineCount = (ushort)value.Length;
        }
    }

    public string[] Lines
    {
        get => LineData.Select(GetLineString).ToArray();
        set => LineData = ConvertLinesToData(value);
    }

    private byte[][] ConvertLinesToData(string?[] value)
    {
        ushort key = KEY_BASE;
        var lineData = new byte[value.Length][];
        for (int i = 0; i < value.Length; i++)
        {
            string text = value[i]?.Trim() ?? string.Empty;
            if (text.Length == 0 && SETEMPTYTEXT)
                text = $"[~ {i}]";
            byte[] DecryptedLineData = GetLineData(text);
            lineData[i] = CryptLineData(DecryptedLineData, key);
            if (lineData[i].Length % 4 == 2)
                Array.Resize(ref lineData[i], lineData[i].Length + 2);
            key += KEY_ADVANCE;
        }

        return lineData;
    }

    private static byte[] CryptLineData(byte[] data, ushort key)
    {
        byte[] result = (byte[])data.Clone();
        for (int i = 0; i < result.Length; i += 2)
        {
            result[i + 0] ^= (byte)key;
            result[i + 1] ^= (byte)(key >> 8);
            key = (ushort)(key << 3 | key >> 13);
        }
        return result;
    }

    private byte[] GetLineData(string line)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        int i = 0;
        while (i < line.Length)
        {
            ushort val = line[i++];
            val = TryRemapChar(val);

            switch (val)
            {
                case '[':
                    // grab the string
                    int bracket = line.IndexOf(']', i);
                    if (bracket < 0)
                        throw new ArgumentException("Variable text is not capped properly: " + line);
                    string varText = line[i..bracket];
                    var varValues = GetVariableValues(varText);
                    foreach (ushort v in varValues) bw.Write(v);
                    i += 1 + varText.Length;
                    break;
                case '{':
                    int brace = line.IndexOf('}', i);
                    if (brace < 0)
                        throw new ArgumentException("Ruby text is not capped properly: " + line);
                    string rubyText = line[i..brace];
                    var rubyValues = GetRubyValues(rubyText);
                    foreach (ushort v in rubyValues) bw.Write(v);
                    i += 1 + rubyText.Length;
                    break;
                case '\\':
                    var escapeValues = GetEscapeValues(line[i++]);
                    foreach (ushort v in escapeValues)
                        bw.Write(v);
                    break;
                default:
                    bw.Write(val);
                    break;
            }
        }
        bw.Write(KEY_TERMINATOR); // cap the line off
        return ms.ToArray();
    }

    private ushort TryRemapChar(ushort val)
    {
        if (!RemapChars)
            return val;
        return val switch
        {
            0x202F => 0xE07F // nbsp
            ,
            0x2026 => 0xE08D // …
            ,
            0x2642 => 0xE08E // ♂
            ,
            0x2640 => 0xE08F // ♀
            ,
            _ => val,
        };
    }

    private ushort TryUnmapChar(ushort val)
    {
        if (!RemapChars)
            return val;
        return val switch
        {
            0xE07F => 0x202F // nbsp
            ,
            0xE08D => 0x2026 // …
            ,
            0xE08E => 0x2642 // ♂
            ,
            0xE08F => 0x2640 // ♀
            ,
            _ => val,
        };
    }

    private string GetLineString(byte[] data)
    {
        var s = new StringBuilder();
        int i = 0;
        while (i < data.Length)
        {
            ushort val = BitConverter.ToUInt16(data, i);
            if (val == KEY_TERMINATOR)
                break;
            i += 2;

            switch (val)
            {
                case KEY_VARIABLE: s.Append(GetVariableString(Config, data, ref i)); break;
                case '\n': s.Append(@"\n"); break;
                case '\\': s.Append(@"\\"); break;
                case '[': s.Append(@"\["); break;
                case '{': s.Append(@"\{"); break;
                default: s.Append((char)TryUnmapChar(val)); break;
            }
        }
        return s.ToString(); // Shouldn't get hit if the string is properly terminated.
    }

    private string GetVariableString(TextConfig config, byte[] data, ref int i)
    {
        var s = new StringBuilder();
        ushort count = BitConverter.ToUInt16(data, i); i += 2;
        ushort variable = BitConverter.ToUInt16(data, i); i += 2;

        switch (variable)
        {
            case KEY_TEXTRETURN: // "Waitbutton then scroll text \r"
                return "\\r";
            case KEY_TEXTCLEAR: // "Waitbutton then clear text \c"
                return "\\c";
            case KEY_TEXTWAIT: // Dramatic pause for a text line. New!
                ushort time = BitConverter.ToUInt16(data, i); i += 2;
                return $"[WAIT {time}]";
            case KEY_TEXTNULL: // Empty Text line? Includes linenum so maybe for betatest finding used-unused lines?
                ushort line = BitConverter.ToUInt16(data, i); i += 2;
                return $"[~ {line}]";
            case KEY_TEXTRUBY: // Ruby text/furigana for Japanese
                ushort baseLength = BitConverter.ToUInt16(data, i); i += 2;
                ushort rubyLength = BitConverter.ToUInt16(data, i); i += 2;
                string baseText1 = GetLineString(data.AsSpan(i, baseLength * 2).ToArray()); i += baseLength * 2;
                string rubyText = GetLineString(data.AsSpan(i, rubyLength * 2).ToArray()); i += rubyLength * 2;
                string baseText2 = GetLineString(data.AsSpan(i, baseLength * 2).ToArray()); i += baseLength * 2;
                s.Append('{').Append(baseText1).Append('|').Append(rubyText);
                if (baseText1 != baseText2)
                {
                    s.Append('|').Append(baseText2); // basetext1 should duplicate basetext2, so this shouldn't occur
                }
                s.Append('}');
                return s.ToString();
        }

        string varName = config.GetVariableString(variable);

        s.Append("[VAR").Append(' ').Append(varName);
        if (count > 1)
        {
            s.Append('(');
            while (count > 1)
            {
                ushort arg = BitConverter.ToUInt16(data, i); i += 2;
                s.Append(arg.ToString("X4"));
                if (--count == 1)
                    break;
                s.Append(',');
            }
            s.Append(')');
        }
        s.Append(']');
        return s.ToString();
    }

    private static IEnumerable<ushort> GetEscapeValues(char esc)
    {
        var vals = new List<ushort>();
        switch (esc)
        {
            case 'n': vals.Add('\n'); return vals;
            case '\\': vals.Add('\\'); return vals;
            case '[': vals.Add('['); return vals;
            case '{': vals.Add('{'); return vals;
            case 'r': vals.AddRange(new ushort[] { KEY_VARIABLE, 1, KEY_TEXTRETURN }); return vals;
            case 'c': vals.AddRange(new ushort[] { KEY_VARIABLE, 1, KEY_TEXTCLEAR }); return vals;
            default: throw new Exception("Invalid terminated line: \\" + esc);
        }
    }

    private IEnumerable<ushort> GetVariableValues(string variable)
    {
        string[] split = variable.Split(' ');
        if (split.Length < 2)
            throw new ArgumentException("Incorrectly formatted variable text: " + variable);

        var vals = new List<ushort> { KEY_VARIABLE };
        switch (split[0])
        {
            case "~": // Blank Text Line Variable (No text set - debug/quality testing variable?)
                vals.Add(1);
                vals.Add(KEY_TEXTNULL);
                vals.Add(Convert.ToUInt16(split[1]));
                break;
            case "WAIT": // Event pause Variable.
                vals.Add(1);
                vals.Add(KEY_TEXTWAIT);
                vals.Add(Convert.ToUInt16(split[1]));
                break;
            case "VAR": // Text Variable
                vals.AddRange(GetVariableParameters(split[1]));
                break;
            default: throw new Exception("Unknown variable method type: " + variable);
        }
        return vals;
    }

    private IEnumerable<ushort> GetRubyValues(string ruby)
    {
        string[] split = ruby.Split('|');
        if (split.Length < 2)
            throw new ArgumentException("Incorrectly formatted ruby text: " + ruby);

        string baseText1 = split[0];
        string rubyText = split[1];
        string baseText2 = split.Length < 3 ? baseText1 : split[2];
        if (baseText1.Length != baseText2.Length)
            throw new ArgumentException("Incorrectly formatted ruby text: " + ruby);

        var vals = new List<ushort>
        {
            KEY_VARIABLE,
            Convert.ToUInt16(3 + baseText1.Length + rubyText.Length),
            KEY_TEXTRUBY,
            Convert.ToUInt16(baseText1.Length),
            Convert.ToUInt16(rubyText.Length)
        };
        vals.AddRange(baseText1.Select(val => Convert.ToUInt16(TryRemapChar(val))));
        vals.AddRange(rubyText.Select(val => Convert.ToUInt16(TryRemapChar(val))));
        vals.AddRange(baseText2.Select(val => Convert.ToUInt16(TryRemapChar(val))));
        return vals;
    }

    private IEnumerable<ushort> GetVariableParameters(string text)
    {
        var vals = new List<ushort>();
        int bracket = text.IndexOf('(');
        bool noArgs = bracket < 0;
        string variable = noArgs ? text : text[..bracket];
        ushort varVal = Config.GetVariableNumber(variable);

        if (!noArgs)
        {
            string[] args = text.Substring(bracket + 1, text.Length - bracket - 2).Split(',');
            vals.Add((ushort)(1 + args.Length));
            vals.Add(varVal);
            vals.AddRange(args.Select(t => Convert.ToUInt16(t, 16)));
        }
        else
        {
            vals.Add(1);
            vals.Add(varVal);
        }
        return vals;
    }

    // Exposed Methods
    public static string[]? GetStrings(byte[] data, TextConfig? config = null, bool remapChars = false)
    {
        try
        {
            var t = new TextFile(data, config, remapChars);
            return t.Lines;
        }
        catch { return null; }
    }

    public static byte[] GetBytes(string[] lines, TextConfig? config = null, bool remapChars = false)
    {
        return new TextFile(config: config, remapChars: remapChars) { Lines = lines }.Data;
    }
}
