#nullable disable
using System.ComponentModel;
using FlatSharp.Attributes;
// ReSharper disable ArrangeTypeModifiers
// ReSharper disable All
#pragma warning disable CS0169
#pragma warning disable RCS1213
#pragma warning disable IDE0051
#pragma warning disable IDE0044
#pragma warning disable RCS1169

namespace pkNX.Structures.FlatBuffers;

class PokeDataDLCGift
{
    DevID DevId; // species
    short FormId;
    int Level;
    SexType Sex;
    TokuseiType Tokusei;
    GemType GemType; // tera type
    RareType RareType; // shiny
    SizeType ScaleType;
    short ScaleValue;
    SizeType WeightType;
    short WaightValue;
    TalentType TalentType; // random | random w/ flawless | specified
    sbyte TalentVnum; // flawless count
    ParamSet TalentValue; // IV spec
    ParamSet EffortValue; // EV spec
    ItemID Item;
    SeikakuType Seikaku; // nature
    SeikakuType SeikakuHosei; // stat nature
    WazaType WazaType; // default | manual
    WazaSet Waza1;
    WazaSet Waza2;
    WazaSet Waza3;
    WazaSet Waza4;
    BallType BallId;
    bool UseNickName;
    ulong NicknameLabel;
    SexType ParentSex;
}
