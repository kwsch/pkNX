using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ThrowPermissionSetType8a : ulong
{
	Minigame = 0x2806e5fb21a98b98, //minigame
	Ball = 0x5f89f09249c10944, //ball
	FieldItem = 0xacab9dbb80de90a0, //fielditem
	Calmball = 0xf02d16da37827587, //calmball
	CantThrow = 0xfe62f8dd886f25f5, //cantthrow
	None = 0xCBF29CE484222645, // ""
}
