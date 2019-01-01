using System;
using System.Collections.Generic;
using System.Linq;

namespace pkNX.Structures
{
    /// <summary>
    /// Pawn Script (.amx) Script File
    /// </summary>
    /// <remarks>https://github.com/compuphase/pawn</remarks>
    public class Script
    {
        public readonly byte[] Data;
        public readonly AmxHeader Header;
        public readonly int CellSize;

        public Script(byte[] data)
        {
            Data = data;
            Header = data.ToClass<AmxHeader>();
            CellSize = Header.CellSize;

            if (Header.Flags.HasFlagFast(AmxFlags.OVERLAY))
                throw new ArgumentException("Multi-environment script!?");
        }

        public byte[] Write() => Data;
        public bool Debug => Header.Flags.HasFlagFast(AmxFlags.DEBUG);

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
                yield return $"Code Start: 0x{Header.COD:X4}";
                yield return $"Data Start: 0x{Header.Data:X4}";
                yield return $"Total Used Size: 0x{Header.Heap:X4}";
                yield return $"Reserved Size: 0x{Header.StackTop:X4}";
                yield return $"Compressed Len: 0x{CompressedLength:X4}";
                yield return $"Decompressed Len: 0x{DecompressedLength:X4}";
                yield return $"Compression Ratio: {(DecompressedLength - CompressedLength) / (decimal) DecompressedLength:p1}";
            }
        }
    }
}
