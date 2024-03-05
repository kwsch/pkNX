using pkNX.Containers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static System.Buffers.Binary.BinaryPrimitives;

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
    private static ReadOnlySpan<byte> emptyTextFile => [0x01, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00];

    public TextFile(TextConfig? config = null, bool remapChars = false) : this(emptyTextFile, config, remapChars) { }

    public TextFile(ReadOnlySpan<byte> data, TextConfig? config = null, bool remapChars = false)
    {
        Data = data.ToArray();

        if (InitialKey != 0)
            throw new Exception("Invalid initial key! Not 0?");
        if (SectionDataOffset + TotalLength != Data.Length || TextSections != 1)
            throw new Exception("Invalid Text File");
        if (SectionLength != TotalLength)
            throw new Exception("Section size and overall size do not match.");

        Config = config ?? TextConfig.Default;
        RemapChars = remapChars;
    }

    public TextFile(IEnumerable<string> lines, TextConfig? config = null, bool remapChars = false)
        : this(config, remapChars)
    {
        Lines = lines.ToArray();
    }

    public byte[] Data;
    private readonly TextConfig Config;
    private readonly bool RemapChars;

    private ushort TextSections { get => ReadUInt16LittleEndian(Data.AsSpan(0x00)); set => WriteUInt16LittleEndian(Data.AsSpan(0x00), value); } // Always 0x0001
    private ushort LineCount { get => ReadUInt16LittleEndian(Data.AsSpan(0x02)); set => WriteUInt16LittleEndian(Data.AsSpan(0x02), value); }
    private uint TotalLength { get => ReadUInt32LittleEndian(Data.AsSpan(0x04)); set => WriteUInt32LittleEndian(Data.AsSpan(0x04), value); }
    private uint InitialKey { get => ReadUInt32LittleEndian(Data.AsSpan(0x08)); set => WriteUInt32LittleEndian(Data.AsSpan(0x08), value); } // Always 0x00000000
    private uint SectionDataOffset { get => ReadUInt32LittleEndian(Data.AsSpan(0x0C)); set => WriteUInt32LittleEndian(Data.AsSpan(0x0C), value); } // Always 0x0010
    private uint SectionLength { get => ReadUInt32LittleEndian(Data.AsSpan((int)SectionDataOffset)); set => WriteUInt32LittleEndian(Data.AsSpan((int)SectionDataOffset), value); }

    private TextLine[] LineOffsets
    {
        get
        {
            var result = new TextLine[LineCount];
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
        get
        {
            var sb = new StringBuilder();
            var result = new string[LineCount];
            for (int i = 0; i < result.Length; i++)
            {
                GetLineString(GetEncryptedLine(i), sb);
                result[i] = sb.ToString();
                sb.Clear();
            }
            return result;
        }
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

            var data = GetLineData(Config, RemapChars, text);
            CryptLineDataInPlace(data, key);
            if (data.Length % 4 == 2)
                Array.Resize(ref data, data.Length + 2);

            lineData[i] = data;
            key += KEY_ADVANCE;
        }

        return lineData;
    }

    private static byte[] CryptLineData(byte[] data, ushort key)
    {
        byte[] result = (byte[])data.Clone();
        CryptLineDataInPlace(result, key);
        return result;
    }

    private static void CryptLineDataInPlace(Span<byte> result, ushort key)
    {
        if (!BitConverter.IsLittleEndian)
        {
            for (int i = 0; i < result.Length; i += 2)
            {
                result[i + 0] ^= (byte)key;
                result[i + 1] ^= (byte)(key >> 8);
                key = (ushort)(key << 3 | key >> 13);
            }
            return;
        }

        var data = MemoryMarshal.Cast<byte, ushort>(result);
        foreach (ref var u16 in data)
        {
            u16 ^= key;
            key = (ushort)(key << 3 | key >> 13);
        }
    }

    private static byte[] GetLineData(TextConfig config, bool remap, ReadOnlySpan<char> line)
    {
        using var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);
        int i = 0;
        while (i < line.Length)
        {
            ushort val = line[i++];
            val = TryRemapChar(val, remap);

            switch (val)
            {
                case '[':
                    // grab the string
                    int bracket = line.IndexOf(']', i);
                    if (bracket < 0)
                        throw new ArgumentException("Variable text is not capped properly: " + line.ToString());
                    var varText = line[i..bracket];
                    var varValues = GetVariableValues(config, [], varText);
                    foreach (ushort v in varValues)
                        bw.Write(v);
                    i += 1 + varText.Length;
                    break;
                case '{':
                    int brace = line.IndexOf('}', i);
                    if (brace < 0)
                        throw new ArgumentException("Ruby text is not capped properly: " + line.ToString());
                    var rubyText = line[i..brace];
                    List<ushort> rubyValues = [];
                    GetRubyValues(rubyText.ToString(), remap, rubyValues);
                    foreach (ushort v in rubyValues)
                        bw.Write(v);
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

    private static ushort TryRemapChar(ushort val, bool RemapChars)
    {
        if (!RemapChars)
            return val;
        return val switch
        {
            0x202F => 0xE07F, // nbsp
            0x2026 => 0xE08D, // …
            0x2642 => 0xE08E, // ♂
            0x2640 => 0xE08F, // ♀
            _ => val,
        };
    }

    private ushort TryUnmapChar(ushort val)
    {
        if (!RemapChars)
            return val;
        return val switch
        {
            0xE07F => 0x202F, // nbsp
            0xE08D => 0x2026, // …
            0xE08E => 0x2642, // ♂
            0xE08F => 0x2640, // ♀
            _ => val,
        };
    }

    private void GetLineString(ReadOnlySpan<byte> data, StringBuilder s)
    {
        int i = 0;
        while (i < data.Length)
        {
            ushort val = ReadUInt16LittleEndian(data[i..]);
            if (val == KEY_TERMINATOR)
                break;
            i += 2;

            switch (val)
            {
                case KEY_VARIABLE: AppendVariableString(Config, data, s, ref i); break;
                case '\n': s.Append(@"\n"); break;
                case '\\': s.Append(@"\\"); break;
                case '[': s.Append(@"\["); break;
                case '{': s.Append(@"\{"); break;
                default: s.Append((char)TryUnmapChar(val)); break;
            }
        }
    }

    private void AppendVariableString(TextConfig config, ReadOnlySpan<byte> data, StringBuilder s, ref int i)
    {
        ushort count = ReadUInt16LittleEndian(data[i..]); i += 2;
        ushort variable = ReadUInt16LittleEndian(data[i..]); i += 2;

        switch (variable)
        {
            case KEY_TEXTRETURN: // "Waitbutton then scroll text \r"
                s.Append("\\r");
                return;
            case KEY_TEXTCLEAR: // "Waitbutton then clear text \c"
                s.Append("\\c");
                return;
            case KEY_TEXTWAIT: // Dramatic pause for a text line. New!
                ushort time = ReadUInt16LittleEndian(data[i..]); i += 2;
                s.Append($"[WAIT {time}]");
                return;
            case KEY_TEXTNULL: // nullptr text, Includes linenum
                ushort line = ReadUInt16LittleEndian(data[i..]); i += 2;
                s.Append($"[~ {line}]");
                return;
            case KEY_TEXTRUBY: // Ruby text/furigana for Japanese
                ushort baseLength = ReadUInt16LittleEndian(data[i..]); i += 2;
                ushort rubyLength = ReadUInt16LittleEndian(data[i..]); i += 2;

                var baseSpan1 = data.Slice(i, baseLength * 2);
                i += baseLength * 2;
                var rubySpan = data.Slice(i, rubyLength * 2);
                i += rubyLength * 2;
                var baseSpan2 = data.Slice(i, baseLength * 2);
                i += baseLength * 2;

                s.Append('{');
                GetLineString(baseSpan1, s);
                s.Append('|');
                GetLineString(rubySpan, s);
                if (!baseSpan1.SequenceEqual(baseSpan2))
                {
                    // basetext1 should duplicate basetext2, so this shouldn't occur
                    s.Append('|');
                    GetLineString(baseSpan2, s);
                }
                s.Append('}');
                return;
        }

        string varName = config.GetVariableString(variable);
        s.Append("[VAR").Append(' ').Append(varName);
        if (count > 1)
        {
            s.Append('(');
            while (count > 1)
            {
                ushort arg = ReadUInt16LittleEndian(data[i..]); i += 2;
                s.Append(arg.ToString("X4"));
                if (--count == 1)
                    break;
                s.Append(',');
            }
            s.Append(')');
        }
        s.Append(']');
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
            case 'r': vals.AddRange([KEY_VARIABLE, 1, KEY_TEXTRETURN]); return vals;
            case 'c': vals.AddRange([KEY_VARIABLE, 1, KEY_TEXTCLEAR]); return vals;
            default: throw new Exception($"Invalid terminated line: \\{esc}");
        }
    }

    private static IEnumerable<ushort> GetVariableValues(TextConfig config, List<ushort> vals, ReadOnlySpan<char> variable)
    {
        var spaceIndex = variable.IndexOf(' ');
        if (spaceIndex == -1)
            throw new ArgumentException($"Incorrectly formatted variable text: {variable}");

        var cmd = variable[..spaceIndex];
        var args = variable[(spaceIndex + 1)..];

        vals.Add(KEY_VARIABLE);
        switch (cmd)
        {
            case "~": // Blank Text Line Variable (nullptr text)
                vals.Add(1);
                vals.Add(KEY_TEXTNULL);
                vals.Add(ushort.Parse(args));
                break;
            case "WAIT": // Event pause Variable.
                vals.Add(1);
                vals.Add(KEY_TEXTWAIT);
                vals.Add(ushort.Parse(args));
                break;
            case "VAR": // Text Variable
                GetVariableParameters(config, args, vals);
                break;
            default: throw new Exception($"Unknown variable method type: {variable}");
        }
        return vals;
    }

    private static void GetRubyValues(ReadOnlySpan<char> ruby, bool remap, List<ushort> vals)
    {
        var split1 = ruby.IndexOf('|');
        if (split1 < 0)
            throw new ArgumentException($"Incorrectly formatted ruby text: {ruby}");

        ReadOnlySpan<char> baseText1 = ruby[..split1];
        ruby = ruby[(split1 + 1)..];
        var split2 = ruby.IndexOf('|');
        ReadOnlySpan<char> rubyText, baseText2;
        if (split2 < 0)
        {
            rubyText = ruby;
            baseText2 = baseText1;
        }
        else
        {
            rubyText = ruby[..split2];
            baseText2 = ruby[(split2 + 1)..];
        }
        if (baseText1.Length != baseText2.Length)
            throw new ArgumentException($"Incorrectly formatted ruby text: {ruby}");

        vals.Add(KEY_VARIABLE);
        vals.Add(Convert.ToUInt16(3 + baseText1.Length + rubyText.Length));
        vals.Add(KEY_TEXTRUBY);
        vals.Add(Convert.ToUInt16(baseText1.Length));
        vals.Add(Convert.ToUInt16(rubyText.Length));

        ToU16(baseText1, remap, vals);
        ToU16(rubyText, remap, vals);
        ToU16(baseText2, remap, vals);
        static void ToU16(ReadOnlySpan<char> text, bool remap, List<ushort> vals)
        {
            foreach (var c in text)
                vals.Add(TryRemapChar(c, remap));
        }
    }

    private static void GetVariableParameters(TextConfig config, ReadOnlySpan<char> text, List<ushort> vals)
    {
        int bracket = text.IndexOf('(');
        bool noArgs = bracket < 0;
        var variable = noArgs ? text : text[..bracket];
        ushort varVal = config.GetVariableNumber(variable.ToString());

        if (!noArgs)
        {
            int index = vals.Count;
            vals.Add(1); // change count later
            vals.Add(varVal);
            var args = text[(bracket + 1)..^1];
            // Add the hex args to the list, with a `,` separator. When done, revise the index to the final count.
            int count = 1;
            while (args.Length > 0)
            {
                int comma = args.IndexOf(',');
                if (comma == -1)
                    comma = args.Length;
                if (ushort.TryParse(args[..comma], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
                    vals.Insert(index, result);
                else
                    throw new ArgumentException($"Invalid hex value: {args[..comma]} in text: {text}");
                count++;
                var skip = comma + 1;
                if (skip >= args.Length)
                    break;
                args = args[skip..];
            }
            vals[index] = (ushort)count;
        }
        else
        {
            vals.Add(1);
            vals.Add(varVal);
        }
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

    public static byte[] GetBytes(IEnumerable<string> lines, TextConfig? config = null, bool remapChars = false)
    {
        return new TextFile(lines, config, remapChars).Data;
    }
}
