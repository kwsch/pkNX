namespace pkNX.Structures.FlatBuffers.ZA;

public partial class PokeDataBattle
{
    public void SerializePKHeX(BinaryWriter bw, sbyte captureLv)
    {
        // flag BallId if not none
        if (BallId != BallID.BALL_NULL)
            throw new ArgumentOutOfRangeException(nameof(BallId), BallId, $"No {nameof(BallId)} allowed!");

        ushort species = SpeciesConverterZA.GetNational9((ushort)DevId);
        byte form = species switch
        {
            //(ushort)Species.Vivillon or (ushort)Species.Spewpa or (ushort)Species.Scatterbug => 30,
            (ushort)Species.Minior when FormId < 7 => (byte)(FormId + 7),
            _ => (byte)FormId,
        };

        bw.Write(species);
        bw.Write(form);
        bw.Write((byte)Sex);
        bw.Write((byte)Tokusei);
        bw.Write((byte)RareType);
        bw.Write((byte)captureLv);
    }
}
