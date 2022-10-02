using System;
using pkNX.Structures;

namespace pkNX.Game;

public sealed class ShinyRateSWSH : ShinyRateInfo
{
    /*
        Shiny Rate Patch -- fix the overworld loop counter regardless of shiny rate factors.
        ARM64 disassembly @ FUN_7100d311f0 (Shield), FUN_7100d311c0 (Sword)

        orr        w24,w24,w0      // set flag for shiny if found
        cmp        w25,w23         // check loop counter
        b.cs       LAB_7100d314c8  // break if maximum loop reached


        Patch creation: accept input for reroll count=0-4095
        Get u32: ((count & 0xFFF) << 10) | 0b0111000100_000000000000_11001_11111
        Convert to bytes, this is our new "CMP W25, XXX" instruction

        Replace these bytes:
        18 03 00 2a 3f 03 17 6b 62 00 00 54
        With these bytes:
        18 03 00 2a [u32 bytes] 62 00 00 54

        ***
        Always Shiny Patch -- nop the b.cs
        write 1f 20 03 d5 to the last 4 bytes of the above sequence

        ***
        Revert above patch -- restore the b.cs
        write 62 00 00 54 after the above 12 byte sequence.
    */

    private readonly int FunctionOffset; // loop counter and break
    private static readonly byte[] FunctionPrelude = { 0xff, 0x03, 0x06, 0xd1, 0xfc, 0x6f, 0x12, 0xa9, 0xfa, 0x67, 0x13, 0xa9, 0xf8, 0x5f, 0x14, 0xa9, 0xf6, 0x57, 0x15, 0xa9, 0xf4, 0x4f, 0x16, 0xa9, 0xfd, 0x7b, 0x17, 0xa9, 0xfd, 0xc3, 0x05, 0x91, 0xfa, 0xc6, 0x00, 0xf0 };

    private const int RerollCountCheckOffset = 0x2C8;
    private static readonly byte[] RerollCountCheckDefault = { 0x3f, 0x03, 0x17, 0x6b};

    private const int RerollCountBreakOffset = 0x2CC;
    private static readonly byte[] RerollCountBreakDefault = { 0x62, 0x00, 0x00, 0x54 }; // b.cs $pc + 12
    private static readonly byte[] RerollCountBreakNop = { 0x1F, 0x20, 0x03, 0xD5 }; // nop

    public ShinyRateSWSH(byte[] data, int offset = 0x700_000) : base(data)
    {
        FunctionOffset = CodePattern.IndexOfBytes(data, FunctionPrelude, offset);
    }

    public override bool IsEditable => FunctionOffset > 0;

    public override bool IsDefault => !IsAlways && !IsAlways;
    public override bool IsFixed => !IsPresent(Data, RerollCountCheckDefault, FunctionOffset + RerollCountCheckOffset);
    public override bool IsAlways => IsPresent(Data, RerollCountBreakNop, FunctionOffset + RerollCountBreakOffset);

    public override bool AllowAlways => true;

    public override int GetFixedRate() // "CMP W25, {val}" instruction
    {
        if (!IsFixed)
            return -1;
        var instr = BitConverter.ToUInt32(Data, FunctionOffset + RerollCountCheckOffset);
        return (int)((instr >> 10) & 0xFFF);
    }

    public override void SetDefault()
    {
        RerollCountCheckDefault.CopyTo(Data, FunctionOffset + RerollCountCheckOffset);
        RerollCountBreakDefault.CopyTo(Data, FunctionOffset + RerollCountBreakOffset);
    }

    public override void SetFixedRate(int rerollCount)
    {
        SetDefault();
        var instr = GetFixedInstruction(rerollCount);
        instr.CopyTo(Data, FunctionOffset + RerollCountCheckOffset);
    }

    public override void SetAlwaysShiny()
    {
        SetDefault();
        RerollCountBreakNop.CopyTo(Data, FunctionOffset + RerollCountBreakOffset);
    }

    public static byte[] GetFixedInstruction(int count)
    {
        count = Math.Clamp(count, 1, 4091);
        var val = ((count & 0xFFF) << 10) | 0b0111000100_000000000000_11001_11111;
        return BitConverter.GetBytes((uint)val);
    }
}
