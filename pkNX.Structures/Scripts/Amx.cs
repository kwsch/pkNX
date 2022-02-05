using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace pkNX.Structures
{
    /// <summary>
    /// Pawn Script (.amx) Script File
    /// </summary>
    /// <remarks>https://github.com/compuphase/pawn</remarks>
    public class Amx
    {
        private const int MAX_NAME_LENGTH = 31;

        public readonly byte[] Data;
        public readonly AmxHeader Header;
        public readonly int CellSize;

        public Amx(byte[] data)
        {
            Data = data;
            Header = data.ToClass<AmxHeader>();
            CellSize = Header.CellSize;

            if (Header.Flags.HasFlagFast(AmxFlags.DEBUG))
                return;

            if (Header.Flags.HasFlagFast(AmxFlags.OVERLAY))
                throw new ArgumentException("Multi-environment script!?");

            Unpack();

            Trace.Assert(Header != null);
            Trace.Assert(Header.Magic != 0);
            Trace.Assert(Header.Natives <= Header.Libraries);
        }

        public byte[] Write() => Data;
        public bool IsDebug => Header.Flags.HasFlagFast(AmxFlags.DEBUG);

        // Generated Attributes
        public int CodeLength => Header.Data - Header.COD;
        public int CompressedLength => Header.Size - Header.COD;
        public byte[] CompressedBytes => Data.Skip(Header.COD).ToArray();
        public int DecompressedLength => Header.Heap - Header.COD;
        public uint[] DecompressedInstructions => PawnUtil.QuickDecompress(CompressedBytes, DecompressedLength / sizeof(uint));

        public uint[] ScriptCommands => DecompressedInstructions.Take(CodeLength / sizeof(uint)).ToArray(); // Code
        public uint[] DataPayload => DecompressedInstructions.Skip(CodeLength / sizeof(uint)).ToArray(); // Data
        public string[] ParseScript => PawnUtil.ParseScript(ScriptCommands);
        public string[] DataChunk => PawnUtil.ParseMovement(DataPayload);

        public string Info => string.Join(Environment.NewLine, SummaryLines);

        public IEnumerable<string> SummaryLines
        {
            get
            {
                yield return $"Code Start:        0x{Header.COD:X4}";
                yield return $"Data Start:        0x{Header.Data:X4}";
                yield return $"Total Used Size:   0x{Header.Heap:X4}";
                yield return $"Reserved Size:     0x{Header.StackTop:X4}";
                yield return $"Compressed Len:    0x{CompressedLength:X4}";
                yield return $"Decompressed Len:  0x{DecompressedLength:X4}";
                yield return $"Entry Point:       0x{Header.CurrentInstructionPointer:X4}";
                yield return $"Compression Ratio: {(DecompressedLength - CompressedLength) / (decimal)DecompressedLength:p1}";
            }
        }

        public Function LookupFunction(uint pc) => Array.Find(Functions, f => f.Within(pc));
        public TableRecord LookupPublic(string name) => Array.Find(Publics, t => t.Name == name);
        public TableRecord LookupPublic(uint addr) => Array.Find(Publics, t => t.Address == addr);

        public Function[] Functions { get; protected set; }
        public TableRecord[] Publics { get; protected set; }
        public TableRecord[] Natives { get; protected set; }
        public TableRecord[] Libraries { get; protected set; }
        public TableRecord[] PublicVars { get; protected set; }
        public Variable[] Globals { get; protected set; }

        public static string ReadName(byte[] data, int offset)
        {
            var end = Array.FindIndex(data, offset, z => z == 0);
            if (end < 0)
                end = offset + MAX_NAME_LENGTH;
            if (end >= data.Length)
                return null;
            return System.Text.Encoding.UTF8.GetString(data, offset, end - offset);
        }

        public void Unpack()
        {
            if (Header.Publics > 0)
                ReadPublics();
            if (Header.Natives > 0)
                ReadNatives();
            if (Header.Libraries > 0)
                ReadLibraries();
            if (Header.PublicVars > 0)
                ReadPublicVars();

            if (IsDebug)
            {
                // todo
            }
        }

        public class Cell
        {
        }

        protected void ReadPublics()
        {
            var count = (Header.Natives - Header.Publics) / Header.DefinitionSize;

            Publics = ReadTable(Header.Publics, count);
        }

        protected void ReadNatives()
        {
            var count = (Header.Libraries - Header.Natives) / Header.DefinitionSize;

            Natives = ReadTable(Header.Natives, count);
        }

        protected void ReadLibraries()
        {
            var count = (Header.PublicVars - Header.Libraries) / Header.DefinitionSize;

            Libraries = ReadTable(Header.Libraries, count);
        }

        protected void ReadPublicVars()
        {
            var count = (Header.Tags - Header.PublicVars) / Header.DefinitionSize;

            PublicVars = ReadTable(Header.PublicVars, count);
        }

        protected TableRecord[] ReadTable(int offset, int count)
        {
            using var stream = new MemoryStream(Data, offset, count * Header.DefinitionSize);
            using var reader = new BinaryReader(stream);
            var dest = new TableRecord[count];

            for (int i = 0; i < dest.Length; i++)
            {
                var address = reader.ReadUInt32();
                var nameoffset = reader.ReadUInt32();
                var name = default(string);

                if (nameoffset < Data.Length)
                    name = ReadName(Data, (int)nameoffset);

                name ??= "Unknown";
                dest[i] = new TableRecord(name, address);
            }

            return dest;
        }

        private int sysreq_flg;

        public void ParseOp(AmxOpCode op, ref int cip, ref Cell tgt)
        {
            static void GETPARAM_P(Cell v, AmxOpCode o) { } // (v = ((Cell) (o) >> (int) (CellSize * 4)));}
            switch (op)
            {
                case AmxOpCode.CONST:
                case AmxOpCode.CONST_S:
                    cip += CellSize * 2;
                    break;

                /* Packed Instructions */
                case AmxOpCode.CONST_P_PRI:
                case AmxOpCode.CONST_P_ALT:
                case AmxOpCode.ADDR_P_PRI:
                case AmxOpCode.ADDR_P_ALT:
                case AmxOpCode.STRB_P_I:
                case AmxOpCode.LIDX_P_B:
                case AmxOpCode.IDXADDR_P_B:
                case AmxOpCode.ALIGN_P_PRI:
                case AmxOpCode.PUSH_P_C:
                case AmxOpCode.PUSH_P:
                case AmxOpCode.PUSH_P_S:
                case AmxOpCode.STACK_P:
                case AmxOpCode.HEAP_P:
                case AmxOpCode.SHL_P_C_PRI:
                case AmxOpCode.SHL_P_C_ALT:
                case AmxOpCode.ADD_P_C:
                case AmxOpCode.SMUL_P_C:
                case AmxOpCode.ZERO_P:
                case AmxOpCode.ZERO_P_S:
                case AmxOpCode.EQ_P_C_PRI:
                case AmxOpCode.EQ_P_C_ALT:
                case AmxOpCode.MOVS_P:
                case AmxOpCode.CMPS_P:
                case AmxOpCode.FILL_P:
                case AmxOpCode.HALT_P:
                case AmxOpCode.BOUNDS_P:
                case AmxOpCode.PUSH_P_ADR:
                    break;

                /* Packed Instructions referencing pointers */
                case AmxOpCode.LOAD_P_PRI:
                case AmxOpCode.LOAD_P_ALT:
                case AmxOpCode.INC_P:
                case AmxOpCode.DEC_P:
                    GETPARAM_P(tgt, op);
                    break;

                /* Packed Instructions referencing stack */
                case AmxOpCode.LOAD_P_S_PRI:
                case AmxOpCode.LOAD_P_S_ALT:
                case AmxOpCode.LREF_P_S_PRI:
                case AmxOpCode.LREF_P_S_ALT:
                case AmxOpCode.INC_P_S:
                case AmxOpCode.DEC_P_S:
                    GETPARAM_P(tgt, op); /* verify address */
                    break;

                /* Single-Value Instructions */
                case AmxOpCode.LODB_I:
                case AmxOpCode.CONST_PRI:
                case AmxOpCode.CONST_ALT:
                case AmxOpCode.ADDR_PRI:
                case AmxOpCode.ADDR_ALT:
                case AmxOpCode.STRB_I:
                case AmxOpCode.LIDX_B:
                case AmxOpCode.IDXADDR_B:
                case AmxOpCode.ALIGN_PRI:
                case AmxOpCode.LCTRL:
                case AmxOpCode.SCTRL:
                case AmxOpCode.PICK:
                case AmxOpCode.PUSH_C:
                case AmxOpCode.PUSH:
                case AmxOpCode.PUSH_S:
                case AmxOpCode.STACK:
                case AmxOpCode.HEAP:
                case AmxOpCode.SHL_C_PRI:
                case AmxOpCode.SHL_C_ALT:
                case AmxOpCode.ADD_C:
                case AmxOpCode.SMUL_C:
                case AmxOpCode.ZERO:
                case AmxOpCode.ZERO_S:
                case AmxOpCode.EQ_C_PRI:
                case AmxOpCode.EQ_C_ALT:
                case AmxOpCode.MOVS:
                case AmxOpCode.CMPS:
                case AmxOpCode.FILL:
                case AmxOpCode.HALT:
                case AmxOpCode.BOUNDS:
                case AmxOpCode.PUSH_ADR:
                    cip += CellSize;
                    break;

                case AmxOpCode.LOAD_PRI:
                case AmxOpCode.LOAD_ALT:
                case AmxOpCode.INC:
                case AmxOpCode.DEC:
                    //VerifyAddress(0, );
                    cip += CellSize;
                    break;

                case AmxOpCode.LOAD_S_PRI:
                case AmxOpCode.LOAD_S_ALT:
                case AmxOpCode.LREF_S_PRI:
                case AmxOpCode.LREF_S_ALT:
                case AmxOpCode.INC_S:
                case AmxOpCode.DEC_S:
                    cip += CellSize;
                    break;

                /* Parameterless Instructions */
                case AmxOpCode.LOAD_I:
                case AmxOpCode.STOR_I:
                case AmxOpCode.LIDX:
                case AmxOpCode.IDXADDR:
                case AmxOpCode.XCHG:
                case AmxOpCode.PUSH_PRI:
                case AmxOpCode.PUSH_ALT:
                case AmxOpCode.PPRI:
                case AmxOpCode.PALT:
                case AmxOpCode.PROC:
                case AmxOpCode.RET:
                case AmxOpCode.RETN:
                case AmxOpCode.SHL:
                case AmxOpCode.SHR:
                case AmxOpCode.SSHR:
                case AmxOpCode.SMUL:
                case AmxOpCode.SDIV:
                case AmxOpCode.ADD:
                case AmxOpCode.SUB:
                case AmxOpCode.AND:
                case AmxOpCode.OR:
                case AmxOpCode.XOR:
                case AmxOpCode.NOT:
                case AmxOpCode.NEG:
                case AmxOpCode.INVERT:
                case AmxOpCode.ZERO_PRI:
                case AmxOpCode.ZERO_ALT:
                case AmxOpCode.EQ:
                case AmxOpCode.NEQ:
                case AmxOpCode.SLESS:
                case AmxOpCode.SLEQ:
                case AmxOpCode.SGRTR:
                case AmxOpCode.SGEQ:
                case AmxOpCode.INC_PRI:
                case AmxOpCode.INC_ALT:
                case AmxOpCode.INC_I:
                case AmxOpCode.DEC_PRI:
                case AmxOpCode.DEC_ALT:
                case AmxOpCode.DEC_I:
                case AmxOpCode.SWAP_PRI:
                case AmxOpCode.SWAP_ALT:
                case AmxOpCode.NOP:
                case AmxOpCode.BREAK:
                    break;

                /* Jump w/ Relocation */
                case AmxOpCode.CALL:
                case AmxOpCode.JUMP:
                case AmxOpCode.JZER:
                case AmxOpCode.JNZ:
                case AmxOpCode.JEQ:
                case AmxOpCode.JNEQ:
                case AmxOpCode.JSLESS:
                case AmxOpCode.JSLEQ:
                case AmxOpCode.JSGRTR:
                case AmxOpCode.JSGEQ:
                case AmxOpCode.SWITCH:
                    /* if this file is an older version (absolute references instead of the
					 * current use of position-independent code), convert the parameter
					 * to position-independent code first
					 */
                    cip += CellSize;
                    break;

                /* overlay opcodes (overlays must be enabled) */
                case AmxOpCode.ISWITCH:
                    Debug.Assert(Header.FileVersion >= 10);
                    /* drop through */
                    goto case AmxOpCode.ICALL;
                case AmxOpCode.ICALL:
                    cip += CellSize;
                    /* drop through */
                    goto case AmxOpCode.IRETN;
                case AmxOpCode.IRETN:
                    Debug.Assert(Header.Overlays != 0 && Header.Overlays != Header.NameTable);
                    //return AmxError.OVERLAY;       /* no overlay callback */
                    break;
                case AmxOpCode.ICASETBL:
                    {
                        // Cell num;
                        //DBGPARAM(num);    /* number of records follows the opcode */
                        //cip += (2 * num + 1) * CellSize;
                        //if (Header.Overlays == 0)
                        // return AmxError.OVERLAY;       /* no overlay callback */
                        break;
                    } /* case */

                case AmxOpCode.SYSREQ_C:
                    cip += CellSize;
                    sysreq_flg |= 0x01; /* mark SYSREQ found */
                    break;
                case AmxOpCode.SYSREQ_N:
                    cip += CellSize * 2;
                    sysreq_flg |= 0x02; /* mark SYSREQ.N found */
                    break;

                case AmxOpCode.CASETBL:
                    {
                        DBGPARAM(out _);
                        //cip += (2 * num + 1) * CellSize;
                        break;
                    }

                default:
                    Header.Flags &= ~AmxFlags.VERIFY;
                    //return AmxError.INVINSTR;
                    break;
            }

            static void DBGPARAM(out Cell v) => v = null; // v = (Cell)(amx->code + (int)cip), cip += CellSize)
        }
    }
}
