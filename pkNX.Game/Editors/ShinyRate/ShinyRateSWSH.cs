using System;
using System.Diagnostics;
using pkNX.Structures;

namespace pkNX.Game
{
    public sealed class ShinyRateSWSH : ShinyRateInfo
    {
        /*
            Shiny Rate Patch -- fix the loop counter regardless of input param value.
            ARM64 disassembly @ sub_710076FAD0

            LDRB W8, [X19,#0x35] // load loop max
            ADD W21, W21, #1 // increment counter
            CMP W21, W8 // compare counter to max
            B.CC loc_710076FBA4 // branch if less (loop)
            B loc_710076FD40 // branch (no loop), PID is done!

            Patch creation: accept input for reroll count=1-4091
            Get u32: ((count & 0xFFF) << 10) | 0b111000100_000000000000_10101_11111
            Convert to bytes, this is our new "CMP W20, XXX" instruction

            Replace these bytes:
            68 D6 40 39 B5 06 00 11 BF 02 08 6B
            With these bytes:
            68 D6 40 39 94 06 00 11 [u32 bytes]

            ***
            Always Shiny Patch -- nop the bl
            write 1F 20 03 D5 after the above 12 byte sequence

            ***
            Revert above patch -- restore the bl
            write E3 FD FF 54 after the above 12 byte sequence for the first RNG branch
            write C3 FE FF 54 after the above 12 byte sequence for the second RNG branch
        */

        private readonly int CodeOffset1; // xorshift rotate RNG
        private readonly int CodeOffset2; // another RNG
        private static readonly byte[] Pattern = { 0x68, 0xD6, 0x40, 0x39, 0xB5, 0x06, 0x00, 0x11 };
        private static readonly byte[] Default = { 0xBF, 0x02, 0x08, 0x6B }; // cmp W20, W8
        private static readonly byte[] Always12 = { 0x1F, 0x20, 0x03, 0xD5 }; // nop

        private static readonly byte[] Revert12_1 = { 0xE3, 0xFD, 0xFF, 0x54 }; // bl
        private static readonly byte[] Revert12_2 = { 0xC3, 0xFE, 0xFF, 0x54 }; // bl

        public ShinyRateSWSH(byte[] data, int offset = 0x700_000) : base(data)
        {
            CodeOffset1 = CodePattern.IndexOfBytes(data, Pattern, offset);
            CodeOffset2 = CodePattern.IndexOfBytes(data, Pattern, CodeOffset1 + 8); // after CodeOffset1

            Debug.Assert(CodeOffset2 - 0x30 == CodeOffset1);
        }

        public override bool IsEditable => CodeOffset1 > 0;

        public override bool IsDefault => !IsAlways && IsPresent(Data, Default, CodeOffset1 + Pattern.Length);
        public override bool IsFixed => !IsAlways && !IsDefault;
        public override bool IsAlways => IsPresent(Data, Always12, Pattern.Length + 4);

        public override bool AllowAlways => false;

        public override int GetFixedRate() // "CMP W20, {val}" instruction
        {
            if (!IsFixed)
                return -1;
            var instr = BitConverter.ToUInt32(Data, CodeOffset1 + Pattern.Length);
            return (int)((instr >> 10) & 0xFFF);
        }

        public override void SetDefault()
        {
            Default.CopyTo(Data, CodeOffset1 + Pattern.Length);
            Revert12_1.CopyTo(Data, CodeOffset1 + Pattern.Length + 4);

            Default.CopyTo(Data, CodeOffset2 + Pattern.Length);
            Revert12_2.CopyTo(Data, CodeOffset2 + Pattern.Length + 4);
        }

        public override void SetFixedRate(int rerollCount)
        {
            var instr = GetFixedInstruction(rerollCount);
            instr.CopyTo(Data, CodeOffset1 + Pattern.Length);
            Revert12_1.CopyTo(Data, CodeOffset1 + Pattern.Length + 4);

            instr.CopyTo(Data, CodeOffset2 + Pattern.Length);
            Revert12_2.CopyTo(Data, CodeOffset2 + Pattern.Length + 4);
        }

        public override void SetAlwaysShiny()
        {
            Default.CopyTo(Data, CodeOffset1 + Pattern.Length);
            Always12.CopyTo(Data, CodeOffset1 + Pattern.Length + 4);

            Default.CopyTo(Data, CodeOffset2 + Pattern.Length);
            Always12.CopyTo(Data, CodeOffset2 + Pattern.Length + 4);
        }

        public static byte[] GetFixedInstruction(int count)
        {
            if (count <= 0)
                count = 1;
            else if (count >= 4092)
                count = 4091;
            var val = ((count & 0xFFF) << 10) | 0b111000100_000000000000_10101_11111;
            return BitConverter.GetBytes((uint)val);
        }
    }
}
