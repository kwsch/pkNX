using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(uint))]
public enum FieldNpcMotionType : uint
{
    Wait = 0,
    OneShot = 1,
    Loop = 2,
}
