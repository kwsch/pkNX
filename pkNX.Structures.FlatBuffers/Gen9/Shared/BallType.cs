using FlatSharp.Attributes;
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers;

[FlatBufferEnum(typeof(int))]
public enum BallType
{
    NONE = 0,
    MASUTAABOORU = 1,
    HAIPAABOORU = 2,
    SUUPAABOORU = 3,
    MONSUTAABOORU = 4,
    SAFARIBOORU = 5,
    NETTOBOORU = 6,
    DAIBUBOORU = 7,
    NESUTOBOORU = 8,
    RIPIITOBOORU = 9,
    TAIMAABOORU = 10,
    GOOZYASUBOORU = 11,
    PUREMIABOORU = 12,
    DAAKUBOORU = 13,
    HIIRUBOORU = 14,
    KUIKKUBOORU = 15,
    PURESYASUBOORU = 16,
    SUPIIDOBOORU = 17,
    REBERUBOORU = 18,
    RUAABOORU = 19,
    HEBIIBOORU = 20,
    RABURABUBOORU = 21,
    HURENDOBOORU = 22,
    MUUNBOORU = 23,
    KONPEBOORU = 24,
    DORIIMUBOORU = 25,
    URUTORABOORU = 26,
}
