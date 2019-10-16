using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                return Magic switch
                {
                    MAGIC_16 => 16,
                    MAGIC_32 => 32,
                    MAGIC_64 => 64,
                    _ => throw new ArgumentException("Invalid Magic identifier.")
                };
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

    /// <summary>
    /// Debug data at the end of an amx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class AmxDebugHeader
    {
        public const ushort MAGIC_DEBUG = 0xf1ef;

        public int Size;
        public ushort Magic;
        public byte FileVersion;
        public byte AMXVersion;
        public AmxFlags Flags;
        public short Files;
        public short Lines;
        public short Symbols;
        public short Tags;
        public short Automatons;
        public short States;

        public const int SIZE = 36;
    }

    public class TableRecord
    {
        public TableRecord(string name, uint address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }
        public uint Address { get; }
    }

    public class Tag
    {
        public Tag(string name, uint tagID)
        {
            TagID = tagID;
            Name = name;
        }

        public uint TagID { get; }
        public string Name { get; }
    }

    public class Dimension
    {
        public Dimension(int tagID, Tag tag, int size)
        {
            TagID = tagID;
            Tag = tag;
            Size = size;
        }

        public int TagID { get; }
        public Tag Tag { get; }
        public int Size { get; }
    }

    public class Argument
    {
        public Argument(VariableType type, string name, int tagID, Tag tag, Dimension[] dims)
        {
            Type = type;
            Name = name;
            TagID = tagID;
            Tag = tag;
            Dimensions = dims;
        }

        public int TagID { get; }
        public VariableType Type { get; }
        public string Name { get; }
        public Tag Tag { get; }
        public Dimension[] Dimensions { get; }
    }

    public enum Register : uint
    {
        Pri,
        Alt
    }

    public enum Scope : uint
    {
        Global,
        Local,
        Static
    }

    public class Variable
    {
        public Variable(int addr, int tagID, Tag tag, uint codeStart,
            uint codeEnd, VariableType type, Scope scope,
            string name, Dimension[] dims = null)
        {
            Address = addr;
            TagID = (uint)tagID;
            Tag = tag;
            CodeStart = codeStart;
            CodeEnd = codeEnd;
            Type = type;
            Scope = scope;
            Name = name;
            Dims = dims;
        }

        public int Address { get; }
        public uint CodeStart { get; }
        public uint CodeEnd { get; }
        public string Name { get; }
        public VariableType Type { get; }
        public Scope Scope { get; }
        public Tag Tag { get; set; }
        public uint TagID { get; }
        public Dimension[] Dims { get; }
    }

    public class Signature
    {
        public Signature(string name) => Name = name;

        public Tag ReturnType { get; set; }
        public uint TagID { get; protected set; }
        public string Name { get; }

        public Argument[] Args { get; protected set; }
    }

    public class Native : Signature
    {
        public Native(string name, int index) : base(name) => Index = index;

        public int Index { get; }

        public void SetDebugInfo(int tagID, Tag tag, Argument[] args)
        {
            TagID = (uint)tagID;
            ReturnType = tag;
            Args = args;
        }
    }

    public class Function : Signature
    {
        public Function(uint addr, uint codeStart, uint codeEnd, string name, Tag tag)
            : base(name)
        {
            Address = addr;
            CodeStart = codeStart;
            CodeEnd = codeEnd;
            ReturnType = tag;
        }

        public Function(uint addr, uint codeStart, uint codeEnd, string name, uint tagID)
            : base(name)
        {
            Address = addr;
            CodeStart = codeStart;
            CodeEnd = codeEnd;
            TagID = tagID;
        }

        public void SetArguments(List<Argument> from) => Args = from.ToArray();

        public uint Address { get; }
        public uint CodeStart { get; }
        public uint CodeEnd { get; }

        public bool Within(uint pc) => pc >= CodeStart && pc < CodeEnd;
    }
}
