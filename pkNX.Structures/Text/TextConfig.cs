using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace pkNX.Structures;

/// <summary>
/// Version specific text parsing configuration object to interact with text variable codes.
/// </summary>
public class TextConfig(GameVersion game)
{
    internal static readonly TextConfig Default = new(GameVersion.Any);
    private static readonly char[] _trim_hex = ['0', 'x'];
    private readonly TextVariableCode[] Variables = TextVariableCode.GetVariables(game);

    public IEnumerable<string> GetVariableList() => Variables.Select(z => $"{z.Code:X4}={z.Name}");

    private TextVariableCode? GetCode(string name) => Array.Find(Variables, v => v.Name == name);
    private TextVariableCode? GetName(ushort value) => Array.Find(Variables, v => v.Code == value);

    /// <summary>
    /// Gets the machine-friendly variable instruction code to be written to the data.
    /// </summary>
    /// <param name="variable">Variable name</param>
    public ushort GetVariableNumber(string variable)
    {
        var v = GetCode(variable);
        if (v != null)
            return v.Code;
        if (ushort.TryParse(variable.TrimStart(_trim_hex), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
            return result;
        throw new ArgumentException($"Variable parse error: {variable}. Expected a hexadecimal value or standard variable code.");
    }

    /// <summary>
    /// Gets the human-friendly variable instruction name to be written to the output text line.
    /// </summary>
    /// <param name="variable">Variable code</param>
    public string GetVariableString(ushort variable)
    {
        var v = GetName(variable);
        return v?.Name ?? variable.ToString("X4");
    }
}
