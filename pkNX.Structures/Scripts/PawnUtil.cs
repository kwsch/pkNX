using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    public static partial class PawnUtil
    {
        // FireFly's (github.com/FireyFly) concise decompression (ported c->c#):
        // https://github.com/FireyFly/poketools/blob/e74538a5b5e5dab1e78c1cd313c55d158f37534d/src/formats/script.c#L61
        public static uint[] QuickDecompress(byte[] data, int count)
        {
            uint[] code = new uint[count];
            uint i = 0, j = 0, x = 0, f = 0;
            while (i < code.Length)
            {
                int b = data[f++],
                    v = b & 0x7F,
                    c = b & 0x80;
                if (++j == 1)                                                  // sign extension possible
                    x = (uint)((((v >> 6 == 0 ? 1 : 0) - 1) << 6) | v); // only for bit6 being set
                else
                    x = (x << 7) | (byte)v; // shift data into place

                if (c > 0)
                    continue; // more data to read

                code[i++] = x;
                j = 0; // write finalized instruction
            }

            return code;
        }

        // https://github.com/gameswop/mtasa-resources/blob/d557a72fefef57ac34780a76edf16383d3dff0e8/%5Bgamemodes%5D/%5Bamx%5D/amx-deps/src/amx/amx.c#L1119
        // Slightly more readable, but potentially slower
        public static uint[] QuickDecompress2(byte[] data, int count)
        {
            var memsize = count * sizeof(uint);
            var instructions = new uint[count];
            int i = data.Length;

            while (i > 0)
            {
                uint cell = 0;
                var shift = 0;

                do
                {
                    i--;
                    cell |= (uint)(data[i] & 0x7f) << shift;
                    shift += 7;
                }
                while (i > 0 && (data[i - 1] & 0x80) != 0);

                if ((data[i] & 0x40) != 0)
                {
                    while (shift < 8 * sizeof(uint))
                    {
                        cell |= (uint)0xff << shift;
                        shift += 8;
                    }
                }

                memsize -= sizeof(uint);
                instructions[memsize / sizeof(uint)] = cell;
            }

            return instructions;
        }

        // Compression
        /*public static byte[] CompressScript(byte[] data)
		{
			if (data == null || data.Length % 4 != 0) // Bad Input
				return null;
			using (MemoryStream mn = new MemoryStream())
			using (BinaryWriter bw = new BinaryWriter(mn))
			{
				for ( var pos = 0; pos < data.Length; pos += 4 )
				{
					byte[] db = data.Skip(pos).Take(4).ToArray();
					byte[] cb = CompressBytes(db);
					bw.Write(cb);
				}
				return mn.ToArray();
			}
		}*/

        public static byte[] CompressScript(uint[] instructions)
            => instructions.SelectMany(CompressInstruction).ToArray();

        private static byte[] CompressInstruction(uint instruction)
        {
            var bytes = new List<byte>();
            var sign = (instruction & 0x80000000) > 0;

            // Signed (negative) values are handled opposite of unsigned (positive) values.
            // Positive values are "done" when we've shifted the value down to zero, but
            // we don't need to store the highest 1s in a signed value. We handle this by
            // tracking the loop via a NOTed shadow copy of the instruction if it's signed.
            var shadow = sign ? ~instruction : instruction;

            do
            {
                var least7 = instruction & 0b01111111;
                var byteVal = (byte)least7;

                if (bytes.Count > 0)
                {
                    // Continuation bit on all but the lowest byte
                    byteVal |= 0x80;
                }

                bytes.Add(byteVal);

                instruction >>= 7;
                shadow >>= 7;
            }
            while (shadow != 0);

            if (bytes.Count < 5)
            {
                // Ensure "sign bit" (bit just to the right of highest continuation bit) is
                // correct. Add an extra empty continuation byte if we need to. Values can't
                // be longer than 5 bytes, though.

                var signBit = sign ? 0x40 : 0x00;

                if ((bytes.Last() & 0x40) != signBit)
                    bytes.Add(sign ? (byte)0xFF : (byte)0x80);
            }

            // Little endian to big endian
            bytes.Reverse();

            return bytes.ToArray();
        }

        // Interpreting
        public static string[] ParseScript(uint[] cmd, int sanity = -1)
        {
            // sub_148CBC Moon v1.0
            List<string> parse = new();
            const int sanityMode = 0; // todo

            string ErrorNear(int line, string error)
            {
                var start = Math.Max(line - 6, 0);
                var end = Math.Min(line + 6, cmd.Length - 1);
                var toPrint = cmd.Skip(start).Take(end - start);
                var message = $"Error at line {line}:" + Environment.NewLine;

                message += string.Join(" ", toPrint.Select(b => $"{b:X2}")) + Environment.NewLine;

                for (var x = 0; x < line - start; x++)
                    message += "   ";

                message += "^^" + Environment.NewLine;
                message += error;

                return message;
            }

            int i = 0;               // Current Offset of decompressed instructions
            while (i < cmd.Length) // read away
            {
                // Read a Command

                string instrLine;
                var line = i;
                var opcodeval = cmd[i++];
                var opcodesafe = opcodeval & 0xFFFF;

                if (!Enum.IsDefined(typeof(AmxOpCode), opcodesafe))
                    throw new ArgumentException(ErrorNear(line, $"Invalid command ID: {opcodesafe:X4} ({opcodesafe})"));

                var opcode = (AmxOpCode)opcodesafe;

                if (!OpCodeTypes.TryGetValue(opcode, out var optype))
                    throw new ArgumentException(ErrorNear(line, $"Unknown OpCode: {opcodesafe:X4} ({opcodesafe})"));

                switch (optype)
                {
                    default:
                        throw new ArgumentException("Invalid Command Type");

                    case AmxOpCodeType.NoParams:
                        {
                            instrLine = EchoIntCommand(opcode);
                            break;
                        }

                    case AmxOpCodeType.OneParam:
                        {
                            var param = (int)cmd[i++];

                            instrLine = EchoIntCommand(opcode, param);
                            break;
                        }

                    case AmxOpCodeType.TwoParams:
                        {
                            var param1 = (int)cmd[i++];
                            var param2 = (int)cmd[i++];

                            instrLine = EchoIntCommand(opcode, param1, param2);
                            break;
                        }

                    case AmxOpCodeType.ThreeParams:
                        {
                            var param1 = (int)cmd[i++];
                            var param2 = (int)cmd[i++];
                            var param3 = (int)cmd[i++];

                            instrLine = EchoIntCommand(opcode, param1, param2, param3);
                            break;
                        }

                    case AmxOpCodeType.FourParams:
                        {
                            var param1 = (int)cmd[i++];
                            var param2 = (int)cmd[i++];
                            var param3 = (int)cmd[i++];
                            var param4 = (int)cmd[i++];

                            instrLine = EchoIntCommand(opcode, param1, param2, param3, param4);
                            break;
                        }

                    case AmxOpCodeType.FiveParams:
                        {
                            var param1 = (int)cmd[i++];
                            var param2 = (int)cmd[i++];
                            var param3 = (int)cmd[i++];
                            var param4 = (int)cmd[i++];
                            var param5 = (int)cmd[i++];

                            instrLine = EchoIntCommand(opcode, param1, param2, param3, param4, param5);
                            break;
                        }

                    case AmxOpCodeType.Jump:
                        {
                            var jumpOffset = (int)cmd[i++];
                            var jumpDest = (line * 4) + jumpOffset;

                            instrLine = $"{Commands[opcode].PadRight(MaxCommandLength, ' ')} => 0x{jumpDest:X4} ({jumpOffset})";
                            break;
                        }

                    case AmxOpCodeType.Packed:
                        {
                            var param = (short)(opcodeval >> 16);

                            instrLine = EchoIntCommand(opcode, param);
                            break;
                        }

                    case AmxOpCodeType.CaseTable:
                        {
                            //var jOffset = (i * 4) - 4; // this may be the correct jump start point...
                            var count = cmd[i++]; // switch case table
                                                  // sanity check

                            // Populate Switch-Case Tree
                            var tree = new List<string>();

                            // Cases
                            for (int j = 0; j < count; j++)
                            {
                                var jmp = (int)cmd[i++];
                                var toOffset = ((i - 2) * 4) + jmp;
                                var ifValue = (int)cmd[i++];
                                tree.Add($"\t{ifValue} => 0x{toOffset:X4} ({jmp})");
                            }

                            // Default
                            {
                                int jmp = (int)cmd[i++];
                                var toOffset = ((i - 2) * 4) + jmp;
                                tree.Add($"\t* => 0x{toOffset:X4} ({jmp})");
                            }

                            instrLine = Commands[opcode] + Environment.NewLine + string.Join(Environment.NewLine, tree);
                            break;
                        }
                }

                if (opcode is AmxOpCode.RET or AmxOpCode.RETN or AmxOpCode.IRETN)
                {
                    // Newline after return
                    instrLine += Environment.NewLine;
                }

                if (parse.Count == 0 && opcode == AmxOpCode.HALT_P)
                {
                    // Newline after 0x0000 HALT.P
                    instrLine += Environment.NewLine;
                }

                parse.Add($"0x{line * 4:X4}: [{opcodeval & 0x7FF:X2}] {instrLine}");
            }

            if (sanity >= 0 && sanity != sanityMode)
                throw new ArgumentException();

            return parse.ToArray();
        }

        internal static string[] ParseMovement(uint[] cmd) => Util.GetHexLines(cmd);

        internal static string EchoIntCommand(AmxOpCode c, params int[] arr)
        {
            static string FormatParameter(int param)
            {
                if (param is < -100 or > 100)
                    return $"0x{param:X4}";
                return param.ToString();
            }

            string commandLeft = Commands[c].PadRight(MaxCommandLength, ' ');
            string parameters = arr.Length == 0 ? "" : string.Join(", ", arr.Select(FormatParameter));
            return $"{commandLeft} {parameters}";
        }

        private static readonly Func<uint, float> getFloat = val => BitConverter.ToSingle(BitConverter.GetBytes(val), 0);

        internal static string EchoFloatCommand(AmxOpCode c, params uint[] arr)
        {
            string commandLeft = Commands[c].PadRight(MaxCommandLength, ' ');
            string parameters = arr.Length == 1 ? "" : string.Join(", ", arr.Select(getFloat));
            return $"{commandLeft} {parameters}";
        }

        private static readonly Dictionary<AmxOpCode, string> Commands = Enum.GetValues(typeof(AmxOpCode))
                                                                             .Cast<AmxOpCode>()
                                                                             .ToDictionary(v => v, v => v.ToString().Replace('_', '.'));

        private static readonly int MaxCommandLength = Commands.Values.Max(cmd => cmd.Length);
    }
}