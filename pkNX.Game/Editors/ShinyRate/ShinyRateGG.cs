using System;
using pkNX.Structures;

namespace pkNX.Game
{
    public sealed class ShinyRateGG : ShinyRateInfo
    {
        /*
            Shiny Rate Patch -- fix the loop counter regardless of input param value.
            ARM64 disassembly @ sub_71007399D8
            LDRB	W8, [X19, #0x25] // load loop max
            ADD	W20, W20, #1 // increment counter
            CMP	W20, W8 // compare counter to max
            BL	-44 // branch if less (loop)
            B	[....] // branch (no loop), PID is done!

            Patch creation: accept input for reroll count=1-4091
            Get u32: ((count & 0xFFF) << 10) | 0b111000100_000000000000_1010011111
            Convert to bytes, this is our new "CMP W20, XXX" instruction

            Replace these bytes:
            68 96 40 39 94 06 00 11 9F 02 08 6B
            With these bytes:
            68 96 40 39 94 06 00 11 [u32 bytes]

            ***
            Always Shiny Patch -- nop the bl
            write 1F 20 03 D5 after the above 12 byte sequence

            ***
            Revert above patch -- restore the bl
            write AB FE FF 54 after the above 12 byte sequence
        */

        private readonly int CodeOffset;
        private static readonly byte[] Pattern = { 0x68, 0x96, 0x40, 0x39, 0x94, 0x06, 0x00, 0x11 };
        private static readonly byte[] Default = { 0x9F, 0x02, 0x08, 0x6B }; // cmp W20, W8
        private static readonly byte[] Always12 = { 0x1F, 0x20, 0x03, 0xD5 }; // nop
        private static readonly byte[] Revert12 = { 0xAB, 0xFE, 0xFF, 0x54 }; // bl

        public ShinyRateGG(byte[] data) : base(data) => CodeOffset = CodePattern.IndexOfBytes(data, Pattern, 0x500_000);

        public override bool IsEditable => CodeOffset > 0;

        public override bool IsDefault => !IsAlways && IsPresent(Data, Default, CodeOffset + 8);
        public override bool IsFixed => !IsAlways && !IsDefault;
        public override bool IsAlways => IsPresent(Data, Always12, CodeOffset + 12);

        public override bool AllowAlways => false;

        public override int GetFixedRate() // "CMP W20, {val}" instruction
        {
            if (!IsFixed)
                return -1;
            var instr = BitConverter.ToUInt32(Data, CodeOffset + 8);
            return (int)((instr >> 10) & 0xFFF);
        }

        public override void SetDefault()
        {
            Default.CopyTo(Data, CodeOffset + 8);
            Revert12.CopyTo(Data, CodeOffset + 12);
        }

        public override void SetFixedRate(int rerollCount)
        {
            var instr = GetFixedInstruction(rerollCount);
            instr.CopyTo(Data, CodeOffset + 8);
            Revert12.CopyTo(Data, CodeOffset + 12);
        }

        public override void SetAlwaysShiny()
        {
            Default.CopyTo(Data, CodeOffset + 8);
            Always12.CopyTo(Data, CodeOffset + 12);
        }

        public byte[] GetFixedInstruction(int count)
        {
            if (count <= 0)
                count = 1;
            else if (count >= 4092)
                count = 4091;
            var val = ((count & 0xFFF) << 10) | 0b111000100_000000000000_1010011111;
            return BitConverter.GetBytes((uint)val);
        }
    }
}