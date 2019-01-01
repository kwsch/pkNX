using System;
using System.Runtime.InteropServices;

namespace pkNX.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public class AmxHeader
    {
        // Cell Size magic verification
        public const ushort MAGIC_32 = 0xf1e0;
        public const ushort MAGIC_64 = 0xf1e1;
        public const ushort MAGIC_16 = 0xf1e2;

        /// <summary> Size of the "file" </summary>
        public int Size; // 0x00

        /// <summary> Signature </summary>
        public ushort Magic; // 0x04

        /// <summary> File format version </summary>
        public byte FileVersion; // 0x06

        /// <summary> Required version of the AMX </summary>
        public byte AMXVersion; // 0x07

        /// <summary> Feature flags </summary>
        public AmxFlags Flags; // 0x08

        /// <summary> Size of a definition record </summary>
        public short DefinitionSize; // 0x0A

        /// <summary> Initial value of COD - code block </summary>
        public int COD; // 0x0C

        /// <summary> initial value of DAT - data block </summary>
        public int Data; // 0x10

        /// <summary> Initial value of HEA - start of the heap </summary>
        public int Heap; // 0x14

        /// <summary> Initial value of STP - stack top </summary>
        public int StackTop; // 0x18

        /// <summary> Initial value of CIP - the instruction pointer </summary>
        public int CurrentInstructionPointer; // 0x20

        /// <summary> Offset to the "public functions" table </summary>
        public int Publics; // 0x24

        /// <summary> Offset to the "native functions" table </summary>
        public int Natives; // 0x28

        /// <summary> Offset to the table of libraries </summary>
        public int Libraries; // 0x2C

        /// <summary> Offset to the "public variables" table </summary>
        public int PublicVars; // 0x30

        /// <summary> Offset to the "public tagnames" table </summary>
        public int Tags; // 0x34

        /// <summary> Offset to the name table </summary>
        public int NameTable; // 0x38

        /// <summary> Offset to the overlay table </summary>
        public int Overlays; // 0x3C

        public int CellSize
        {
            get
            {
                switch (Magic)
                {
                    case MAGIC_16: return 16;
                    case MAGIC_32: return 32;
                    case MAGIC_64: return 64;
                    default:
                        throw new ArgumentException("Invalid Magic identifier.");
                }
            }
        }
    }

/* File format version
 *   0 original version
 *   1 opcodes JUMP.pri, SWITCH and CASETBL
 *   2 compressed files
 *   3 public variables
 *   4 opcodes SWAP.pri/alt and PUSHADDR
 *   5 tagnames table
 *   6 reformatted header
 *   7 name table, opcodes SYMTAG & SYSREQ.D
 *   8 opcode BREAK, renewed debug interface
 *   9 macro opcodes
 *  10 position-independent code, overlays, packed instructions
 *  11 relocating instructions for the native interface, reorganized instruction set
 */
}
