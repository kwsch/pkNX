using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum AjitoDifficultEnum
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
}
