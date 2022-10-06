using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ThrowEffectHitType8a : ulong
{
	EffectHit = 0xe81b7c1ecbec8a8, //ef_hit
	EffectYukiHit = 0xec2bf01c23011916, //ef_yukihit
	EffectStickyHit = 0xdf7f2703d7359ada, //ef_sticky_hit
	EffectKemuriHit = 0xea16eea823e31e31, //ef_kemurihit
	EffectBibiriHit = 0x87168142f28b42f7, //ef_bibirihit
	EffectPippiHit = 0x474876e8e47215c8, //ef_pippihit
	EffectUnk1Hit = 0x262b03b924121730,
	EffectUnk2Hit = 0x7f5d5908e7deb625,
	EffectUnk3Hit = 0x9fa86e9cc66822a,
	EffectUnk4Hit = 0x69d10d3d31d81522, 
}
