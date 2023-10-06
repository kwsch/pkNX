using System;

namespace pkNX.Structures.FlatBuffers.SV
{
    public partial struct FoodPokeTypeParam
    {
        public int GetBoostFromIndex(int index) => index switch
        {
            00 => Normal,
            01 => Kakutou,
            02 => Hikou,
            03 => Doku,
            04 => Jimen,
            05 => Iwa,
            06 => Mushi,
            07 => Ghost,
            08 => Hagane,
            09 => Honoo,
            10 => Mizu,
            11 => Kusa,
            12 => Denki,
            13 => Esper,
            14 => Koori,
            15 => Dragon,
            16 => Aku,
            17 => Fairy,
            _ => throw new ArgumentOutOfRangeException(nameof(index), index, null)
        };
    }
}
