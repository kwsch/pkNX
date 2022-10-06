using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ThrowItemCategoryType8a : ulong
{
	ball = 0x5f89f09249c10944, //ball
	bait = 0x5f9b0e9249cfaf71, //bait
	pippidoll = 0xdbff893542f03b18, //pippidoll
	None = 0xCBF29CE484222645, // ""
}
