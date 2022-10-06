using FlatSharp.Attributes;

// ReSharper disable UnusedMember.Global
#pragma warning disable RCS1154 // Sort enum members.

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(ulong))]
public enum ThrowEffectTrajectoryType8a : ulong
{
    EffectTrajectory = 0xe65f2a9cfd11ed0e, //ef_traj
    EffectTrajectorySpeed = 0x772110bcb4b8f2ec, //ef_traj_spd
}
