using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static System.Buffers.Binary.BinaryPrimitives;

namespace pkNX.Structures;

/// <summary>
/// Collision data for a map, used by Scarlet / Violet.
/// </summary>
public static class HavokCollision
{
    public static AABBTree ParseAABBTree(ReadOnlySpan<byte> trcol)
    {
        ReadOnlySpan<byte> data = null;
        var tst1 = new List<string>();
        var tna1 = new List<HavokTypeObject>();
        var fst1 = new List<string>();
        var item = new List<HavokItem>();

        Debug.Assert(ReadUInt32BigEndian(trcol[4..8]) == 0x54414730); // TAG0

        var tag0 = trcol[8..];

        foreach ((uint tag, int start, int end) in ReadSections(tag0))
        {
            var subSec = tag0[start..end];
            switch (tag)
            {
                case 0x44415441: // DATA
                    data = subSec;
                    break;
                case 0x54595045: // TYPE
                    foreach ((uint tag1, int start1, int end1) in ReadSections(subSec))
                    {
                        var subSec1 = subSec[start1..end1];
                        switch (tag1)
                        {
                            case 0x54535431: // TST1
                                tst1 = ReadStrings(subSec1);
                                break;
                            case 0x544E4131: // TNA1
                                tna1 = ReadTna1(subSec1, tst1);
                                break;
                            case 0x46535431: // FST1
                                fst1 = ReadStrings(subSec1);
                                break;
                            case 0x54424459: // TBDY
                                ReadTbdy(subSec1, tna1, fst1);
                                break;
                        }
                    }
                    break;
                case 0x494E4458: // INDX
                    foreach ((uint tag1, int start1, int end1) in ReadSections(subSec))
                    {
                        var subSec1 = subSec[start1..end1];
                        switch (tag1) {
                            case 0x4954454D: // ITEM
                                item = ReadItems(subSec1, tna1);
                                break;
                        }
                    }
                    break;
            }
        }

        // Hardcoded traversal. TODO: General-case deserialization?
        Debug.Assert(item.Count > 1 && item[1].Type.Name == "hkRootLevelContainer");
        var hkRootLevelContainer = item[1];
        Debug.Assert(hkRootLevelContainer.Type.Name == "hkRootLevelContainer");
        Debug.Assert(hkRootLevelContainer.Type.FormatType == FormatType.Record);
        var ofs = hkRootLevelContainer.Type.AlignUp(hkRootLevelContainer.Offset);
        Debug.Assert(hkRootLevelContainer.Type.Fields.Count == 1);

        var namedVariantsField = hkRootLevelContainer.Type.Fields[0];
        Debug.Assert(namedVariantsField.Name == "namedVariants");
        ofs = namedVariantsField.Type.AlignUp(ofs + namedVariantsField.Offset);
        Debug.Assert(namedVariantsField.Type.FormatType == FormatType.Array);

        var namedVariantsItem = item[ReadInt32LittleEndian(data[(int)ofs..])];
        ofs = namedVariantsItem.Offset;
        Debug.Assert(namedVariantsItem.Type.Name == "hkRootLevelContainer::NamedVariant");
        Debug.Assert(namedVariantsItem.Count == 2);

        // There are two named variants, hknpCompressedMeshShape and hknpMaterialPalette.
        Debug.Assert(namedVariantsItem.Type.Fields.Count == 3);
        Debug.Assert(namedVariantsItem.Type.Fields[0].Name == "name");
        Debug.Assert(namedVariantsItem.Type.Fields[1].Name == "className");
        Debug.Assert(namedVariantsItem.Type.Fields[2].Name == "variant");

        var variantField = namedVariantsItem.Type.Fields[2];
        ofs = variantField.Type.AlignUp(ofs + variantField.Offset);
        Debug.Assert(variantField.Type.FormatType == FormatType.Pointer);

        var meshItem = item[ReadInt32LittleEndian(data[(int)ofs..])];
        ofs = meshItem.Offset;
        Debug.Assert(meshItem.Type.Name == "hknpCompressedMeshShape");
        Debug.Assert(meshItem.Type.Fields.Count == 5);
        Debug.Assert(meshItem.Type.Fields[0].Name == "data");

        var dataField = meshItem.Type.Fields[0];
        ofs = dataField.Type.AlignUp(ofs + dataField.Offset);
        Debug.Assert(dataField.Type.FormatType == FormatType.Pointer);

        var dataItem = item[ReadInt32LittleEndian(data[(int)ofs..])];
        ofs = dataItem.Offset;
        Debug.Assert(dataItem.Type.Name == "hknpCompressedMeshShapeData");
        Debug.Assert(dataItem.Type.Fields.Count == 4);
        Debug.Assert(dataItem.Type.Fields[1].Name == "simdTree");

        var simdTreeField = dataItem.Type.Fields[1];
        ofs = simdTreeField.Type.AlignUp(ofs + simdTreeField.Offset);
        Debug.Assert(simdTreeField.Type.Name == "hkcdSimdTree");
        Debug.Assert(simdTreeField.Type.FormatType == FormatType.Record);
        Debug.Assert(simdTreeField.Type.Fields.Count == 2);
        Debug.Assert(simdTreeField.Type.Fields[0].Name == "nodes");

        var nodesField = simdTreeField.Type.Fields[0];
        ofs = nodesField.Type.AlignUp(ofs + nodesField.Offset);
        Debug.Assert(nodesField.Type.FormatType == FormatType.Array);

        var nodesItem = item[ReadInt32LittleEndian(data[(int)ofs..])];
        ofs = nodesItem.Offset;
        Debug.Assert(nodesItem.Type.Name == "hkcdSimdTree::Node");
        Debug.Assert(nodesItem.Type.FormatType == FormatType.Record);
        Debug.Assert(nodesItem.Type.Parent.Name == "hkcdFourAabb");
        Debug.Assert(nodesItem.Type.Parent.FormatType == FormatType.Record);
        Debug.Assert(nodesItem.Type.Parent.Fields.Count == 6);
        Debug.Assert(nodesItem.Type.Parent.Fields[0].Name == "lx");
        Debug.Assert(nodesItem.Type.Parent.Fields[1].Name == "hx");
        Debug.Assert(nodesItem.Type.Parent.Fields[2].Name == "ly");
        Debug.Assert(nodesItem.Type.Parent.Fields[3].Name == "hy");
        Debug.Assert(nodesItem.Type.Parent.Fields[4].Name == "lz");
        Debug.Assert(nodesItem.Type.Parent.Fields[5].Name == "hz");
        foreach (var vecField in nodesItem.Type.Parent.Fields)
        {
            Debug.Assert(vecField.Type.Name == "hkVector4");
            Debug.Assert(vecField.Type.Parent.Name == "hkVector4f");
            Debug.Assert(vecField.Type.Parent.FormatType == FormatType.Array);
            Debug.Assert((vecField.Type.Parent.Format & 0x20) != 0); // Inline Array
            Debug.Assert(vecField.Type.Parent.SubType.Name == "float");
        }
        Debug.Assert(nodesItem.Type.Fields.Count == 2);
        Debug.Assert(nodesItem.Type.Fields[0].Name == "data");
        Debug.Assert(nodesItem.Type.Fields[0].Type.FormatType == FormatType.Array);
        Debug.Assert((nodesItem.Type.Fields[0].Type.Format & 0x20) != 0); // Inline Array
        Debug.Assert(nodesItem.Type.Fields[0].Type.SubType.Name == "hkUint32");
        Debug.Assert(nodesItem.Type.Fields[1].Name == "isLeaf");
        Debug.Assert(nodesItem.Type.Fields[1].Type.FormatType == FormatType.Bool);
        var nodes = new List<hkcdSimdTreeNode>((int)nodesItem.Count);
        for (var i = 0; i < nodesItem.Count; ++i)
        {
            var type = nodesItem.Type;
            var nofs = (int)ofs + ((int)type.Size * i);
            var slice = data[nofs..];

            var parentFields = type.Parent.Fields;
            nodes.Add(new hkcdSimdTreeNode
            {
                LoX = ReadVector4(slice[(int)parentFields[0].Offset..]),
                HiX = ReadVector4(slice[(int)parentFields[1].Offset..]),
                LoY = ReadVector4(slice[(int)parentFields[2].Offset..]),
                HiY = ReadVector4(slice[(int)parentFields[3].Offset..]),
                LoZ = ReadVector4(slice[(int)parentFields[4].Offset..]),
                HiZ = ReadVector4(slice[(int)parentFields[5].Offset..]),
                Data = ReadNodeData(slice[(int)type.Fields[0].Offset..]),
                IsLeaf = slice[(int)type.Fields[1].Offset] != 0,
            });
        }

        return new(nodes);
    }

    private static float[] ReadVector4(ReadOnlySpan<byte> buf) => new[]
    {
        ReadSingleLittleEndian(buf),
        ReadSingleLittleEndian(buf[4..]),
        ReadSingleLittleEndian(buf[8..]),
        ReadSingleLittleEndian(buf[12..]),
    };

    private static uint[] ReadNodeData(ReadOnlySpan<byte> buf) => new[]
    {
        ReadUInt32LittleEndian(buf),
        ReadUInt32LittleEndian(buf[4..]),
        ReadUInt32LittleEndian(buf[8..]),
        ReadUInt32LittleEndian(buf[12..]),
    };

    private static List<string> ReadStrings(ReadOnlySpan<byte> buffer)
    {
        var strings = new List<string>();
        var ofs = 0;
        while (ofs < buffer.Length)
        {
            if (buffer[ofs] == 0xFF)
                break;

            var endOfs = ofs + 1;
            while (buffer[endOfs] != 0)
                endOfs++;

            strings.Add(System.Text.Encoding.ASCII.GetString(buffer[ofs..endOfs]));

            ofs = endOfs + 1;
        }
        return strings;
    }

    private static List<HavokTypeObject> ReadTna1(ReadOnlySpan<byte> buffer, List<string> tst1)
    {
        var (ofs, count) = ReadVar32(buffer);
        var tna1 = new List<HavokTypeObject>((int)count + 1);
        for (var i = 0; i < count + 1; ++i)
            tna1.Add(new HavokTypeObject { Name = "Invalid" }); // Types are 1-indexed.

        for (var i = 0; i < count; ++i)
        {
            var (tiCnt, ti) = ReadVar32(buffer[ofs..]);
            ofs += tiCnt;
            var (pCnt, numParams) = ReadVar32(buffer[ofs..]);
            ofs += pCnt;

            var tna = tna1[i + 1];
            tna.Name = tst1[(int)ti];
            var list = tna.Template = new List<HavokTemplateParam>((int)numParams);

            for (var n = 0; n < numParams; ++n)
            {
                var (pnCnt, pni) = ReadVar32(buffer[ofs..]);
                ofs += pnCnt;

                var templateName = tst1[(int)pni];

                var (pvCnt, pVal) = ReadVar32(buffer[ofs..]);
                ofs += pvCnt;

                if (templateName.StartsWith('t'))
                    list.Add(new HavokTemplateParam { Name = templateName, TypeValue = tna1[(int)pVal] });
                else
                    list.Add(new HavokTemplateParam { Name = templateName, IntValue = (int)pVal });
            }
        }

        return tna1;
    }

    private static void ReadTbdy(ReadOnlySpan<byte> buffer, List<HavokTypeObject> tna1, List<string> fst1)
    {
        var ofs = 0;
        while (ofs < buffer.Length)
        {
            var (cnt, id) = ReadVar32(buffer[ofs..]);
            ofs += cnt;

            // If empty type id, continue
            if (id == 0)
                continue;

            var tna = tna1[(int)id];

            (cnt, var pid) = ReadVar32(buffer[ofs..]);
            ofs += cnt;

            tna.Parent = tna1[(int)pid];

            (cnt, tna.Option) = ReadVar32(buffer[ofs..]);
            ofs += cnt;

            if (tna.IsOptionFormat)
            {
                (cnt, tna.Format) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
            }
            if (tna.IsOptionSubtype)
            {
                if (tna.Format == 0)
                    throw new ArgumentException("Invalid type with Subtype but not Format");

                (cnt, var sid) = ReadVar32(buffer[ofs..]);
                ofs += cnt;

                tna.SubType = tna1[(int)sid];
            }
            if (tna.IsOptionVersion)
            {
                (cnt, tna.Version) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
            }
            if (tna.IsOptionSizeAlign)
            {
                (cnt, tna.Size) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
                (cnt, tna.Align) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
            }
            if (tna.IsOptionFlags)
            {
                (cnt, var flags) = ReadVar32(buffer[ofs..]);
                ofs += cnt;

                tna.Flags = (ushort)flags;
            }
            if (tna.IsOptionFields)
            {
                (cnt, var fieldPair) = ReadVar32(buffer[ofs..]);
                ofs += cnt;

                var numFields = fieldPair & 0xFFFF;
                tna.Fields = new List<HavokField>((int)numFields);
                for (var i = 0; i < numFields; ++i)
                {
                    (cnt, var fnIdx) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;
                    (cnt, var fFlags) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;
                    (cnt, var fOffset) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;
                    (cnt, var ftIdx) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;

                    tna.Fields.Add(new HavokField
                    {
                        Name = fst1[(int)fnIdx],
                        Flags = fFlags,
                        Offset = fOffset,
                        Type = tna1[(int)ftIdx],
                    });
                }
            }
            else
            {
                tna.Fields = new List<HavokField>();
            }
            if (tna.IsOptionInterfaces)
            {
                (cnt, var intCount) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
                tna.Interfaces = new List<HavokInterface>((int)intCount);

                for (var i = 0; i < (int)intCount; ++i)
                {
                    (cnt, var tIdx) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;
                    (cnt, var nIdx) = ReadVar32(buffer[ofs..]);
                    ofs += cnt;

                    tna.Interfaces.Add(new HavokInterface
                    {
                        Type = tna1[(int)tIdx],
                        Name = fst1[(int)nIdx],
                    });
                }
            }
            else
            {
                tna.Interfaces = new List<HavokInterface>();
            }
            if (tna.IsOptionAttribute)
            {
                (cnt, tna.Attribute) = ReadVar32(buffer[ofs..]);
                ofs += cnt;
            }
        }
    }

    private static List<HavokItem> ReadItems(ReadOnlySpan<byte> buffer, List<HavokTypeObject> tna1)
    {
        var items = new List<HavokItem>();

        var ofs = 0;
        while (ofs < buffer.Length)
        {
            var typeFlags = ReadUInt32LittleEndian(buffer[ofs..]);
            var offset = ReadUInt32LittleEndian(buffer[(ofs + 4)..]);
            var count = ReadUInt32LittleEndian(buffer[(ofs + 8)..]);

            items.Add(new HavokItem
            {
                Type = tna1[(int)(typeFlags & 0xFFFFFF)],
                Flags = typeFlags >> 24,
                Offset = offset,
                Count = count,
            });

            ofs += 12;
        }
        return items;
    }

    private static IEnumerable<(uint, int, int)> ReadSections(ReadOnlySpan<byte> buffer)
    {
        var secs = new List<(uint, int, int)>();
        var secOfs = 0;
        while (secOfs < buffer.Length)
        {
            var secSizeAndFlags = ReadUInt32BigEndian(buffer[secOfs..]);
            var secSize = (int)(secSizeAndFlags & 0x3FFFFFFF);
            var secTag = ReadUInt32BigEndian(buffer[(secOfs + 4)..]);

            secs.Add((secTag, secOfs + 8, secOfs + secSize));

            secOfs += secSize;
        }
        return secs;
    }

    private static (int, uint) ReadVar32(ReadOnlySpan<byte> buffer)
    {
        static ulong Mask(ulong val, int start, int end) => val & (((1ul << (end - start + 1)) - 1) << start);
        static ulong Extract(ulong val, int start, int end) => Mask(val, start, end) >> start;
        static ulong ReverseExtract(ulong val, int start, int end) => Extract(val, 63 - end, 63 - start);

        var val = 0ul;
        for (var i = 0; i < 8 && i < buffer.Length; i++)
        {
            val <<= 8;
            val |= buffer[i];
        }

        var msb = ReverseExtract(val, 0, 7);
        var mode = msb >> 3;

        return mode switch
        {
            <= 15 => (1, (uint)msb),
            <= 23 => (2, (uint)ReverseExtract(val, 2, 15)),
            <= 27 => (3, (uint)ReverseExtract(val, 3, 23)),
               28 => (4, (uint)ReverseExtract(val, 5, 31)),
               29 => (5, (uint)ReverseExtract(val, 5, 39)),
               30 => (8, (uint)ReverseExtract(val, 5, 63)),
                _ => throw new ArgumentException($"VarInt, mode={mode}"),
        };
    }

    public class AABBTree : IContainsV3f
    {
        public List<hkcdSimdTreeNode> Nodes { get; }
        public int NumBoundingBoxes { get; }
        public Rectangle3D[] BoundingBoxRectangles { get; }

        public AABBTree(List<hkcdSimdTreeNode> nodes)
        {
            Nodes = nodes;
            foreach (var node in Nodes)
            {
                if (!node.IsLeaf)
                    continue;

                for (var i = 0; i < hkcdSimdTreeNode.NodeCount; i++)
                {
                    if (node.IsBound(i))
                        NumBoundingBoxes++;
                }
            }

            BoundingBoxRectangles = new Rectangle3D[NumBoundingBoxes];
            var n = 0;
            foreach (var node in Nodes)
            {
                if (!node.IsLeaf)
                    continue;

                for (var i = 0; i < hkcdSimdTreeNode.NodeCount; i++)
                {
                    if (!node.IsBound(i))
                        continue;
                    var lo = new Vector3(node.LoX[i], node.LoY[i], node.LoZ[i]);
                    var hi = new Vector3(node.HiX[i], node.HiY[i], node.HiZ[i]);
                    BoundingBoxRectangles[n++] = new Rectangle3D(lo, hi);
                }
            }
        }

        // Official logic for area-containment checks for y intersection between y+1 and y-10000.0
        public bool ContainsPoint(float x, float y, float z) => ContainsPointInNode(1, x, y - 10000, y + 1, z);
        public bool ContainsPoint(float x, float y, float z, float toleranceX, float toleranceY, float toleranceZ)
            => ContainsPointInNode(1, x, y - 10000, y + 1, z, toleranceX, toleranceY, toleranceZ);

        private bool ContainsPointInNode(int nodeIndex, float x, float ly, float hy, float z, float tx = 0f, float ty = 0f, float tz = 0f)
        {
            if (nodeIndex == 0)
                return false;

            var node = Nodes[nodeIndex];
            for (var i = 0; i < hkcdSimdTreeNode.NodeCount; i++)
            {
                if (node.LoX[i] > node.HiX[i] || !(node.LoX[i] - tx <= x) || !(node.HiX[i] + tx >= x))
                    continue;
                if (node.LoZ[i] > node.HiZ[i] || !(node.LoZ[i] - tz <= z) || !(node.HiZ[i] + tz >= z))
                    continue;
                if (node.LoY[i] > node.HiY[i] || !(node.LoY[i] - ty <= hy) || !(node.HiY[i] + ty >= ly))
                    continue;
                if (node.IsLeaf)
                    return true;
                if (ContainsPointInNode((int)node.Data[i], x, ly, hy, z, tx, ty, tz))
                    return true;
            }
            return false;
        }

        public bool ContainedBy(IContainsV3f other) => other.ContainsPoint(BoundingBoxRectangles[0].X, BoundingBoxRectangles[0].Y, BoundingBoxRectangles[0].Z);
    }

    public struct Rectangle3D
    {
        /// <summary> X-coordinate of one corner </summary>
        public float X { get; set; }
        /// <summary> Y-coordinate of one corner </summary>
        public float Y { get; set; }
        /// <summary> Z-coordinate of one corner </summary>
        public float Z { get; set; }

        /// <summary> Width of the rectangle (X) </summary>
        public float Width { get; set; }
        /// <summary> Height of the rectangle (Y) </summary>
        public float Height { get; set; }
        /// <summary> Depth of the rectangle (Z) </summary>
        public float Depth { get; set; }

        public Rectangle3D(float x, float y, float z, float width, float height, float depth)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            Height = height;
            Depth = depth;
        }

        public Rectangle3D(Vector3 lo, Vector3 hi)
        {
            X = lo.X;
            Y = lo.Y;
            Z = lo.Z;
            Width = hi.X - lo.X;
            Height = hi.Y - lo.Y;
            Depth = hi.Z - lo.Z;
        }
    }

#nullable disable
    /// <summary>
    /// Represents 4 nodes in a 3D spatial tree structure used for spatial logic, able to work with SIMD (Single Instruction, Multiple Data) operations
    /// </summary>
    public class hkcdSimdTreeNode
    {
        public required float[] LoX { get; set; }
        public required float[] LoY { get; set; }
        public required float[] LoZ { get; set; }

        public required float[] HiX {get; set; }
        public required float[] HiY {get; set; }
        public required float[] HiZ {get; set; }

        public required uint[] Data { get; set; }
        public required bool IsLeaf { get; set; }
        public bool IsBound(int i) => LoX[i] <= HiX[i] && LoY[i] <= HiY[i] && LoZ[i] <= HiZ[i];
        public const int NodeCount = 4;
    }

    private class HavokTypeObject
    {
        public string Name;
        public List<HavokTemplateParam> Template;
        public HavokTypeObject Parent;
        public uint Option;
        public uint Format;
        public HavokTypeObject SubType;
        public uint Version;
        public uint Size;
        public uint Align;
        public ushort Flags;
        public List<HavokField> Fields;
        public List<HavokInterface> Interfaces;
        public uint Attribute;

        public bool IsOptionFormat => (Option & (1u << 0)) != 0;
        public bool IsOptionSubtype => (Option & (1u << 1)) != 0;
        public bool IsOptionVersion => (Option & (1u << 2)) != 0;
        public bool IsOptionSizeAlign => (Option & (1u << 3)) != 0;
        public bool IsOptionFlags => (Option & (1u << 4)) != 0;
        public bool IsOptionFields => (Option & (1u << 5)) != 0;
        public bool IsOptionInterfaces => (Option & (1u << 6)) != 0;
        public bool IsOptionAttribute => (Option & (1u << 7)) != 0;

        public FormatType FormatType => (FormatType)(Format & 31);

        public uint AlignUp(uint x) {
            if (IsOptionSizeAlign && BitOperations.IsPow2(Align))
                x = (x + Align - 1) & ~(Align - 1);
            return x;
        }
    }

    private enum FormatType
    {
        Void = 0,
        Opaque = 1,
        Bool = 2,
        String = 3,
        Int = 4,
        Float = 5,
        Pointer = 6,
        Record = 7,
        Array = 8,
    }

    private struct HavokTemplateParam
    {
        public string Name;
        public int IntValue;
        public HavokTypeObject TypeValue;

        public bool IsType => Name.StartsWith('t');
    }

    private struct HavokField
    {
        public string Name;
        public uint Flags;
        public uint Offset;
        public HavokTypeObject Type;
    }

    private struct HavokInterface
    {
        public HavokTypeObject Type;
        public string Name;
    }

    private struct HavokItem
    {
        public HavokTypeObject Type;
        public uint Flags;
        public uint Offset;
        public uint Count;
    }
}

public interface IContainsV3f
{
    bool ContainsPoint(float x, float y, float z);
    bool ContainsPoint(float x, float y, float z, float toleranceX, float toleranceY, float toleranceZ);

    /// <summary>
    /// Checks if an outer collision volume contains this (inner) volume
    /// </summary>
    /// <param name="outer">Outer collision volume</param>
    /// <returns>True if the inner volume is contained within this volume</returns>
    bool ContainedBy(IContainsV3f outer);
}
