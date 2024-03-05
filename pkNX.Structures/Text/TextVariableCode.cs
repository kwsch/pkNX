using System;

namespace pkNX.Structures;

public sealed class TextVariableCode
{
    public readonly string Name;
    public readonly int Code;

    private TextVariableCode(int code, string name)
    {
        Code = code;
        Name = name;
    }

    public static TextVariableCode[] GetVariables(GameVersion game)
    {
        if (game == GameVersion.Any)
            return [];

        if (GameVersion.GG.Contains(game))
            return GG;
        if (GameVersion.XY.Contains(game))
            return XY;
        if (GameVersion.ORAS.Contains(game) || GameVersion.ORASDEMO == game)
            return AO;
        if (GameVersion.SM.Contains(game) || GameVersion.USUM.Contains(game))
            return SM;

        return [];
    }

    private static readonly TextVariableCode[] XY =
    [
        new(0xFF00, "COLOR"),
        new(0x0100, "TRNAME"),
        new(0x0101, "PKNAME"),
        new(0x0102, "PKNICK"),
        new(0x0103, "TYPE"),
        new(0x0105, "LOCATION"),
        new(0x0106, "ABILITY"),
        new(0x0107, "MOVE"),
        new(0x0108, "ITEM1"),
        new(0x0109, "ITEM2"),
        new(0x010A, "sTRBAG"),
        new(0x010B, "BOX"),
        new(0x010D, "EVSTAT"),
        new(0x0110, "OPOWER"),
        new(0x0127, "RIBBON"),
        new(0x0134, "MIINAME"),
        new(0x013E, "WEATHER"),
        new(0x0189, "TRNICK"),
        new(0x018A, "1stchrTR"),
        new(0x018B, "SHOUTOUT"),
        new(0x018E, "BERRY"),
        new(0x018F, "REMFEEL"),
        new(0x0190, "REMQUAL"),
        new(0x0191, "WEBSITE"),
        new(0x019C, "CHOICECOS"),
        new(0x01A1, "GSYNCID"),
        new(0x0192, "PRVIDSAY"),
        new(0x0193, "BTLTEST"),
        new(0x0195, "GENLOC"),
        new(0x0199, "CHOICEFOOD"),
        new(0x019A, "HOTELITEM"),
        new(0x019B, "TAXISTOP"),
        new(0x019F, "MAISTITLE"),
        new(0x1000, "ITEMPLUR0"),
        new(0x1001, "ITEMPLUR1"),
        new(0x1100, "GENDBR"),
        new(0x1101, "NUMBRNCH"),
        new(0x1302, "iCOLOR2"),
        new(0x1303, "iCOLOR3"),
        new(0x0200, "NUM1"),
        new(0x0201, "NUM2"),
        new(0x0202, "NUM3"),
        new(0x0203, "NUM4"),
        new(0x0204, "NUM5"),
        new(0x0205, "NUM6"),
        new(0x0206, "NUM7"),
        new(0x0207, "NUM8"),
        new(0x0208, "NUM9"),
    ];

    private static readonly TextVariableCode[] AO =
    [
        new(0xFF00, "COLOR"),
        new(0x0100, "TRNAME"),
        new(0x0101, "PKNAME"),
        new(0x0102, "PKNICK"),
        new(0x0103, "TYPE"),
        new(0x0105, "LOCATION"),
        new(0x0106, "ABILITY"),
        new(0x0107, "MOVE"),
        new(0x0108, "ITEM1"),
        new(0x0109, "ITEM2"),
        new(0x010A, "sTRBAG"),
        new(0x010B, "BOX"),
        new(0x010D, "EVSTAT"),
        new(0x0110, "OPOWER"),
        new(0x0127, "RIBBON"),
        new(0x0134, "MIINAME"),
        new(0x013E, "WEATHER"),
        new(0x0189, "TRNICK"),
        new(0x018A, "1stchrTR"),
        new(0x018B, "SHOUTOUT"),
        new(0x018E, "BERRY"),
        new(0x018F, "REMFEEL"),
        new(0x0190, "REMQUAL"),
        new(0x0191, "WEBSITE"),
        new(0x019C, "CHOICECOS"),
        new(0x01A1, "GSYNCID"),
        new(0x0192, "PRVIDSAY"),
        new(0x0193, "BTLTEST"),
        new(0x0195, "GENLOC"),
        new(0x0199, "CHOICEFOOD"),
        new(0x019A, "HOTELITEM"),
        new(0x019B, "TAXISTOP"),
        new(0x019F, "MAISTITLE"),
        new(0x1000, "ITEMPLUR0"),
        new(0x1001, "ITEMPLUR1"),
        new(0x1100, "GENDBR"),
        new(0x1101, "NUMBRNCH"),
        new(0x1302, "iCOLOR2"),
        new(0x1303, "iCOLOR3"),
        new(0x0200, "NUM1"),
        new(0x0201, "NUM2"),
        new(0x0202, "NUM3"),
        new(0x0203, "NUM4"),
        new(0x0204, "NUM5"),
        new(0x0205, "NUM6"),
        new(0x0206, "NUM7"),
        new(0x0207, "NUM8"),
        new(0x0208, "NUM9"),
    ];

    private static readonly TextVariableCode[] SM =
    [
        new(0xFF00, "COLOR"),
        new(0x0100, "TRNAME"),
        new(0x0101, "PKNAME"),
        new(0x0102, "PKNICK"),
        new(0x0103, "TYPE"),
        new(0x0105, "LOCATION"),
        new(0x0106, "ABILITY"),
        new(0x0107, "MOVE"),
        new(0x0108, "ITEM1"),
        new(0x0109, "ITEM2"),
        new(0x010A, "sTRBAG"),
        new(0x010B, "BOX"),
        new(0x010D, "EVSTAT"),
        new(0x0110, "OPOWER"),
        new(0x0127, "RIBBON"),
        new(0x0134, "MIINAME"),
        new(0x013E, "WEATHER"),
        new(0x0189, "TRNICK"),
        new(0x018A, "1stchrTR"),
        new(0x018B, "SHOUTOUT"),
        new(0x018E, "BERRY"),
        new(0x018F, "REMFEEL"),
        new(0x0190, "REMQUAL"),
        new(0x0191, "WEBSITE"),
        new(0x019C, "CHOICECOS"),
        new(0x01A1, "GSYNCID"),
        new(0x0192, "PRVIDSAY"),
        new(0x0193, "BTLTEST"),
        new(0x0195, "GENLOC"),
        new(0x0199, "CHOICEFOOD"),
        new(0x019A, "HOTELITEM"),
        new(0x019B, "TAXISTOP"),
        new(0x019F, "MAISTITLE"),
        new(0x1000, "ITEMPLUR0"),
        new(0x1001, "ITEMPLUR1"),
        new(0x1100, "GENDBR"),
        new(0x1101, "NUMBRNCH"),
        new(0x1302, "iCOLOR2"),
        new(0x1303, "iCOLOR3"),
        new(0x0200, "NUM1"),
        new(0x0201, "NUM2"),
        new(0x0202, "NUM3"),
        new(0x0203, "NUM4"),
        new(0x0204, "NUM5"),
        new(0x0205, "NUM6"),
        new(0x0206, "NUM7"),
        new(0x0207, "NUM8"),
        new(0x0208, "NUM9"),
    ];

    private static readonly TextVariableCode[] GG =
    [
        new(0xFF00, "COLOR"),
        new(0x0100, "TRNAME"),
        new(0x0101, "PKNAME"),
        new(0x0102, "PKNICK"),
        new(0x0103, "TYPE"),
        new(0x0105, "LOCATION"),
        new(0x0106, "ABILITY"),
        new(0x0107, "MOVE"),
        new(0x0108, "ITEM1"),
        new(0x0109, "ITEM2"),
        new(0x010A, "sTRBAG"),
        new(0x010B, "BOX"),
        new(0x010D, "EVSTAT"),
        new(0x0110, "OPOWER"),
        new(0x0127, "RIBBON"),
        new(0x0134, "MIINAME"),
        new(0x013E, "WEATHER"),
        new(0x0189, "TRNICK"),
        new(0x018A, "1stchrTR"),
        new(0x018B, "SHOUTOUT"),
        new(0x018E, "BERRY"),
        new(0x018F, "REMFEEL"),
        new(0x0190, "REMQUAL"),
        new(0x0191, "WEBSITE"),
        new(0x019C, "CHOICECOS"),
        new(0x01A1, "GSYNCID"),
        new(0x0192, "PRVIDSAY"),
        new(0x0193, "BTLTEST"),
        new(0x0195, "GENLOC"),
        new(0x0199, "CHOICEFOOD"),
        new(0x019A, "HOTELITEM"),
        new(0x019B, "TAXISTOP"),
        new(0x019F, "MAISTITLE"),
        new(0x1000, "ITEMPLUR0"),
        new(0x1001, "ITEMPLUR1"),
        new(0x1100, "GENDBR"),
        new(0x1101, "NUMBRNCH"),
        new(0x1302, "iCOLOR2"),
        new(0x1303, "iCOLOR3"),
        new(0x0200, "NUM1"),
        new(0x0201, "NUM2"),
        new(0x0202, "NUM3"),
        new(0x0203, "NUM4"),
        new(0x0204, "NUM5"),
        new(0x0205, "NUM6"),
        new(0x0206, "NUM7"),
        new(0x0207, "NUM8"),
        new(0x0208, "NUM9"),
    ];
}
