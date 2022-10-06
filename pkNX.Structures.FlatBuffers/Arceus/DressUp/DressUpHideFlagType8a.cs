using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum DressUpHideFlagType8a : ulong
{
	HEADWEAR_D = 4173530696190938758, //None of these hashes match the given name
	HEADWEAR_B = 4173532895214195180, //Will update them as soon as they're found
	HEADWEAR_C = 4173533994725823391,
	HEADWEAR_A = 4173536193749079813,
	FOOT_2 = 9746162982005607639,
	FOOT_0 = 9746164081517235850,
	FOOT_1 = 9746165181028864061,
	None = 14695981039346656837,
}
