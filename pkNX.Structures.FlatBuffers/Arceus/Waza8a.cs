using System.ComponentModel;
using FlatSharp.Attributes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Waza8a : IMove
{
    public byte[] Write() => FlatBufferConverter.SerializeFrom(this);

    // Type mismatch; FlatBuffer must use the correct struct type for each field
    // We need to alias these and hide them from any PropertyGrid, so mark Browsable(false).

    [FlatBufferItem(00)] public int MoveID { get; set; } // int
    [FlatBufferItem(01)] public bool CanUseMove { get; set; }
    [FlatBufferItem(02), Browsable(false)] public byte FType      { get; set; } // byte
    [FlatBufferItem(03), Browsable(false)] public byte FQuality   { get; set; } // byte
    [FlatBufferItem(04), Browsable(false)] public byte FCategory  { get; set; } // byte
    [FlatBufferItem(05), Browsable(false)] public byte FPower     { get; set; } // byte
    [FlatBufferItem(06), Browsable(false)] public byte FAccuracy  { get; set; } // byte
    [FlatBufferItem(07), Browsable(false)] public byte FPP        { get; set; } // byte
    [FlatBufferItem(08), Browsable(false)] public sbyte FPriority { get; set; } // byte
    [FlatBufferItem(09), Browsable(false)] public byte FHitMin    { get; set; } // byte
    [FlatBufferItem(10), Browsable(false)] public byte FHitMax    { get; set; } // byte
    [FlatBufferItem(11), Browsable(false)] public short FInflict  { get; set; } // ushort
    [FlatBufferItem(12), Browsable(false)] public byte FInflictPercent  { get; set; } // byte
    [FlatBufferItem(13), Browsable(false)] public byte FRawInflictCount { get; set; } // byte
    [FlatBufferItem(14), Browsable(false)] public byte FTurnMin   { get; set; } // byte
    [FlatBufferItem(15), Browsable(false)] public byte FTurnMax   { get; set; } // byte
    [FlatBufferItem(16), Browsable(false)] public byte FCritStage { get; set; } // byte
    [FlatBufferItem(17), Browsable(false)] public byte FFlinch    { get; set; } // byte
    [FlatBufferItem(18), Browsable(false)] public ushort FEffectSequence { get; set; } // ushort
    [FlatBufferItem(19), Browsable(false)] public byte FRecoil       { get; set; } // byte
    [FlatBufferItem(20), Browsable(false)] public byte FRawHealing   { get; set; } // byte
    [FlatBufferItem(21), Browsable(false)] public byte FRawTarget    { get; set; } // byte
    [FlatBufferItem(22), Browsable(false)] public byte FStat1        { get; set; } // byte
    [FlatBufferItem(23), Browsable(false)] public byte FStat2        { get; set; } // byte
    [FlatBufferItem(24), Browsable(false)] public byte FStat3        { get; set; } // byte
    [FlatBufferItem(25), Browsable(false)] public sbyte FStat1Stage  { get; set; } // byte
    [FlatBufferItem(26), Browsable(false)] public sbyte FStat2Stage  { get; set; } // byte
    [FlatBufferItem(27), Browsable(false)] public sbyte FStat3Stage  { get; set; } // byte
    [FlatBufferItem(28), Browsable(false)] public byte FStat1Percent { get; set; } // byte
    [FlatBufferItem(29), Browsable(false)] public byte FStat2Percent { get; set; } // byte
    [FlatBufferItem(30), Browsable(false)] public byte FStat3Percent { get; set; } // byte
    [FlatBufferItem(31)] public byte FStat1Duration                  { get; set; } // byte
    [FlatBufferItem(32)] public byte FStat2Duration                  { get; set; } // byte
    [FlatBufferItem(33)] public byte FStat3Duration                  { get; set; } // byte
    [FlatBufferItem(34)] public byte GigantamaxPower                 { get; set; } // byte
    [FlatBufferItem(35)] public bool Flag_MakesContact     { get; set; }
    [FlatBufferItem(36)] public bool Flag_Charge           { get; set; }
    [FlatBufferItem(37)] public bool Flag_Recharge         { get; set; }
    [FlatBufferItem(38)] public bool Flag_Protect          { get; set; }
    [FlatBufferItem(39)] public bool Flag_Reflectable      { get; set; }
    [FlatBufferItem(40)] public bool Flag_Snatch           { get; set; }
    [FlatBufferItem(41)] public bool Flag_Mirror           { get; set; }
    [FlatBufferItem(42)] public bool Flag_Punch            { get; set; }
    [FlatBufferItem(43)] public bool Flag_Sound            { get; set; }
    [FlatBufferItem(44)] public bool Flag_Gravity          { get; set; }
    [FlatBufferItem(45)] public bool Flag_Defrost          { get; set; }
    [FlatBufferItem(46)] public bool Flag_DistanceTriple   { get; set; }
    [FlatBufferItem(47)] public bool Flag_Heal             { get; set; }
    [FlatBufferItem(48)] public bool Flag_IgnoreSubstitute { get; set; }
    [FlatBufferItem(49)] public bool Flag_FailSkyBattle    { get; set; }
    [FlatBufferItem(50)] public bool Flag_AnimateAlly      { get; set; }
    [FlatBufferItem(51)] public bool Flag_Dance            { get; set; }
    [FlatBufferItem(52)] public bool Flag_Metronome        { get; set; }
    // New additions!
    [FlatBufferItem(53)] public byte SplinterModifier           { get; set; } // byte
    [FlatBufferItem(54)] public byte Status_Fixated             { get; set; } // byte
    [FlatBufferItem(55)] public byte Status_Obscured            { get; set; } // byte
    [FlatBufferItem(56)] public byte Status_ObscuredDuration    { get; set; } // byte
    [FlatBufferItem(57)] public byte Status_Primed              { get; set; } // byte
    [FlatBufferItem(58)] public byte Status_PrimedDuration      { get; set; } // byte
    [FlatBufferItem(59)] public byte Status_PrimedPercent       { get; set; } // byte
    [FlatBufferItem(60)] public byte Status_StanceSwap          { get; set; } // byte
    [FlatBufferItem(61)] public byte Status_StanceSwapDuration  { get; set; } // byte
    [FlatBufferItem(62)] public byte Status_FutureCrit          { get; set; } // byte
    [FlatBufferItem(63)] public byte Status_FutureCritDuration  { get; set; } // byte
    [FlatBufferItem(64)] public byte Status_FutureCritStage     { get; set; } // byte
    [FlatBufferItem(65)] public bool Flag_CureDrowsy            { get; set; } // byte
    [FlatBufferItem(66)] public bool Flag_CureFrostbite         { get; set; } // byte
    [FlatBufferItem(67)] public int  DamagePercentStatused      { get; set; } // int
    [FlatBufferItem(68)] public byte CanStyle                   { get; set; } // byte
    [FlatBufferItem(69)] public int  ActSpeedModUser            { get; set; } // int
    [FlatBufferItem(70)] public int  ActSpeedModUserAgile       { get; set; } // int
    [FlatBufferItem(71)] public int  ActSpeedModUserStrong      { get; set; } // int
    [FlatBufferItem(72)] public int  ActSpeedModTarget          { get; set; } // int
    [FlatBufferItem(73)] public int  ActSpeedModTargetAgile     { get; set; } // int
    [FlatBufferItem(74)] public byte AgilePower               { get; set; } // byte
    [FlatBufferItem(75)] public byte AgileRawHealing          { get; set; } // byte
    [FlatBufferItem(76)] public byte AgileTurnMax             { get; set; } // byte
    [FlatBufferItem(77)] public byte AgileEffectDuration      { get; set; } // byte
    [FlatBufferItem(78)] public byte AgileStatChangeDuration  { get; set; } // byte
    [FlatBufferItem(79)] public byte StrongPower              { get; set; } // byte
    [FlatBufferItem(80)] public byte StrongAccuracy           { get; set; } // byte
    [FlatBufferItem(81)] public byte StrongCritStage          { get; set; } // byte
    [FlatBufferItem(82)] public byte StrongInflictPercent     { get; set; } // byte
    [FlatBufferItem(83)] public byte StrongStat1Percent       { get; set; } // byte
    [FlatBufferItem(84)] public byte StrongTurnMax            { get; set; } // byte
    [FlatBufferItem(85)] public byte StrongEffectDuration     { get; set; } // byte
    [FlatBufferItem(86)] public byte StrongStatChangeDuration { get; set; } // byte
    [FlatBufferItem(87)] public int  StrongRecoil             { get; set; } // int
    [FlatBufferItem(88)] public byte StrongRawHealing         { get; set; } // byte
    [FlatBufferItem(89)] public byte StrongSplinterModifier   { get; set; } // byte

    public int Type           { get => FType; set => FType = (byte)value; }
    public int Quality        { get => FQuality        ; set => FQuality        = (byte)value; }
    public int Category       { get => FCategory       ; set => FCategory       = (byte)value; }
    public int Power          { get => FPower          ; set => FPower          = (byte)value; }
    public int Accuracy       { get => FAccuracy       ; set => FAccuracy       = (byte)value; }
    public int PP             { get => FPP             ; set => FPP             = (byte)value; }
    public int Priority       { get => FPriority       ; set => FPriority       = (sbyte)value; }
    public int HitMin         { get => FHitMin         ; set => FHitMin         = (byte)value; }
    public int HitMax         { get => FHitMax         ; set => FHitMax         = (byte)value; }
    public int Inflict        { get => FInflict        ; set => FInflict        = (short)value; }
    public int InflictPercent { get => FInflictPercent ; set => FInflictPercent = (byte)value; }
    public int TurnMin        { get => FTurnMin        ; set => FTurnMin        = (byte)value; }
    public int TurnMax        { get => FTurnMax        ; set => FTurnMax        = (byte)value; }
    public int CritStage      { get => FCritStage      ; set => FCritStage      = (byte)value; }
    public int Flinch         { get => FFlinch         ; set => FFlinch         = (byte)value; }
    public int EffectSequence { get => FEffectSequence ; set => FEffectSequence = (ushort)value; }
    public int Recoil         { get => FRecoil         ; set => FRecoil         = (byte)value; }
    public int Stat1          { get => FStat1          ; set => FStat1          = (byte)value; }
    public int Stat2          { get => FStat2          ; set => FStat2          = (byte)value; }
    public int Stat3          { get => FStat3          ; set => FStat3          = (byte)value; }
    public int Stat1Stage     { get => FStat1Stage     ; set => FStat1Stage     = (sbyte)value; }
    public int Stat2Stage     { get => FStat2Stage     ; set => FStat2Stage     = (sbyte)value; }
    public int Stat3Stage     { get => FStat3Stage     ; set => FStat3Stage     = (sbyte)value; }
    public int Stat1Percent   { get => FStat1Percent   ; set => FStat1Percent   = (byte)value; }
    public int Stat2Percent   { get => FStat2Percent   ; set => FStat2Percent   = (byte)value; }
    public int Stat3Percent   { get => FStat3Percent   ; set => FStat3Percent   = (byte)value; }

    public MoveInflictDuration InflictCount
    {
        get => (MoveInflictDuration)FRawInflictCount;
        set => FRawInflictCount = (byte)value;
    }

    public Heal Healing
    {
        get => (Heal)FRawHealing;
        set => FRawHealing = (byte)value;
    }

    public MoveTarget Target
    {
        get => (MoveTarget)FRawTarget;
        set => FRawTarget = (byte)value;
    }
}
