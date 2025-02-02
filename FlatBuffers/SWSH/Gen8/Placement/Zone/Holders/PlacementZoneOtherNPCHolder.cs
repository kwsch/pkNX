using System.ComponentModel;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace pkNX.Structures.FlatBuffers.SWSH;

// more NPCs? Trainers?
[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZoneOtherNPCHolder
{
    public static readonly Dictionary<ulong, string> Models = new()
    {
        // { 0xFAB9E0BC5EB53C61, "???" }, // CROSS_SHADOW_CHR_0
        { 0x6E0EF08728A00183, "Allister" },
        { 0x6F31044210C526CA, "Artist" },
        { 0x17C135EB3A312C5A, "Avery" },
        { 0xF5A2F864A1EA03C8, "Backpacker" },
        { 0xC4BD80C146AF5526, "Ball Guy" },
        { 0x4D01D66AA548F0DC, "Bea" },
        { 0xF34C5E3BBF2CA12D, "Beauty" },
        { 0x3832B1FD10EA624C, "Bede" },
        { 0x405B1AFD1564C777, "Bede (Gym Leader)" },
        { 0x1E6D78B53BDAC273, "Black Belt" },
        { 0x75616746BD4D626C, "Cabbie" },
        { 0x5EC6DF381EF1AC4F, "Cafe Master" },
        { 0x4B75CAF6B6D9821E, "Cameraman" },
        { 0x46DD9B2B85198218, "Camping King" },
        { 0xAD1F1CBFF500E7EA, "Cara Liss" },
        { 0xCA7C32F9B2364791, "Chef" },
        { 0xEEF53504D3C99ABC, "Child (F)" },
        { 0x515246E1C4850AE9, "Child (M)" },
        { 0x2533395D8C92F2BC, "Clerk (F)" },
        { 0x9EF56DE5A7896E29, "Corviknight Taxi" },
        { 0xE27DD41A334B8864, "Dancer" },
        { 0x6DC9C4941B9DEA36, "Doctor (F)" },
        { 0xA222B32AE2D6DC90, "Doctor (M)" },
        { 0xA86E35CD887C7089, "Dojo Student (F)" },
        { 0xD3C3FECDA1097C14, "Dojo Student (M)" },
        { 0x0DD0DD66398138EA, "Fisher" },
        { 0xCC2FDFB1297BC7EC, "Gentleman" },
        { 0x519298AB56E81403, "Gordie" },
        { 0xA4E43DAB847A65AA, "Gym Challenger Cher" },
        { 0x3A67EB8C01B8787A, "Gym Challenger Corvin" },
        { 0x97381826E816D73F, "Gym Challenger Deneb" },
        { 0xFF82741CC62487EE, "Gym Challenger Dunne" },
        { 0xA8AA3D3DD9090944, "Gym Challenger Icla" },
        { 0xC694A45D0FA7419E, "Gym Challenger Izar" },
        { 0x9F4C7F98FBFE29A8, "Gym Challenger Kent" },
        { 0xB0A5D9D703DE8309, "Gym Challenger Phoebus" },
        { 0x7A61EE8827C20C2B, "Gym Challenger Pia" },
        { 0xC0102468F510883B, "Gym Challenger Polaire" },
        { 0xDB7E13D3072008FE, "Gym Challenger Terry" },
        { 0x93092A2DE60A30E1, "Gym Challenger Theemin" },
        { 0xEDAF14829B6758FA, "Gym Challenger Vega" },
        { 0x4B590544A749C33A, "Gym Challenger Wei" },
        { 0x8DDA7FF298142A66, "Gym Challenger Yue" },
        { 0xAD0CC990973D924C, "Gym Guide" },
        { 0x68E102B9B9C3FCD4, "Gym Trainer (Dark, F)" },
        { 0xE9AAC0E370028C31, "Gym Trainer (Dark, M)" },
        { 0x612098186DB5276E, "Gym Trainer (Dragon, F)" },
        { 0x8DDC3E562E04AEF7, "Gym Trainer (Dragon, M)" },
        { 0xBF61B6E222A4BDCA, "Gym Trainer (Fairy)" },
        { 0x1A6DC73645DA8730, "Gym Trainer (Fighting, F)" },
        { 0xD188F915AA965CBD, "Gym Trainer (Fighting, M)" },
        { 0x03CF766AAE0365E5, "Gym Trainer (Fire, F)" },
        { 0x2E48383E83C58108, "Gym Trainer (Fire, M)" },
        { 0xFD1B0B03A0284FEC, "Gym Trainer (Ghost, F)" },
        { 0x5F779CE090E2E699, "Gym Trainer (Ghost, M)" },
        { 0x7DB4774C9DC12CFF, "Gym Trainer (Grass, F)" },
        { 0xDB41A4BFA1C3F886, "Gym Trainer (Grass, M)" },
        { 0xE779C955C9EADEFE, "Gym Trainer (Ice, F)" },
        { 0x16E06B686F5DD107, "Gym Trainer (Ice, M)" },
        { 0x4785DFF8E04C7A6C, "Gym Trainer (Rock, F)" },
        { 0xA9E271D5D1071119, "Gym Trainer (Rock, M)" },
        { 0xE028414753629816, "Gym Trainer (Water)" },
        { 0x9521A8F7769EA2D1, "Hiker" },
        { 0x8A674F502E959F7C, "Honey" },
        { 0x2E8D2F2916BE7C7B, "Hop" },
        { 0x23D846291019B890, "Hop (Gym Outfit)" },
        { 0x2E53B82FF113D3D0, "Hop's Mother" },
        { 0x6778893672622C8E, "Hop's Poké Ball" },
        { 0x50F70038EB311D7F, "Hyde" },
        { 0x92F76E5098674167, "Jack" },
        { 0x9DAEC40F62597D12, "Kabu" },
        { 0xA03DC074D6206787, "Klara" },
        { 0xCBFEDC059EF3A979, "Lass" },
        { 0xC0BA5B7E2930D108, "League Staff (F)" },
        { 0x6E154F07B75792C8, "League Staff (F)" },
        { 0x964199AA536EB5E5, "League Staff (M)" },
        { 0x42C608079ECFCFD1, "League Staff (M)" },
        { 0xC157261277011D12, "Leon" },
        { 0xC8A60F127AC2B7FD, "Leon (Battle Tower)" },
        { 0x5492E0DC2DA3C026, "Macro Cosmos Employee (F)" },
        { 0x07C16BDC02736E97, "Macro Cosmos Employee (M)" },
        { 0xF44E0641A6E8EB9B, "Madame" },
        { 0x3D5AC2E3C57B98D1, "Marnie" },
        { 0x2C308CE3BBCE33CF, "Marnie (Gym Leader)" },
        { 0x337F59E3BF8F9F26, "Marnie (Gym Outfit)" },
        { 0x672FECC2B55DD125, "Mayor" },
        { 0xCBCF516D11FCA7CF, "Melony" },
        { 0x5B94E3278355861F, "Middle-Aged Man" },
        { 0xC988D3A9DF5B4916, "Middle-Aged Man (Bald)" },
        { 0xB921909A87577826, "Middle-Aged Woman" },
        { 0x45294E1089750AD0, "Milo" },
        { 0xE5C03B4E75AF7A06, "Model" },
        { 0x996C123D20B92166, "Mother" },
        { 0x4A601EA0934A667E, "Muscular Man" },
        { 0xE2081EDAED7AF5AB, "Musician" },
        { 0x09631C357ACCAF23, "Mustard" },
        { 0xFEADD33574274818, "Mustard (No Jacket)" },
        { 0x0711270D2F2871C3, "Nessa" },
        { 0xD9244B1D4E2EA67D, "Office Worker (F)" },
        { 0xA0727E7FCB024401, "Office Worker (M)" },
        { 0x65270056A7A9437D, "Old Man" },
        { 0xB172CE7745D249F0, "Old Woman" },
        { 0x70C43DC41CB92D6B, "Oleana" },
        { 0x929145895D09467D, "Opal" },
        { 0x43C7B6AD72ECEEE9, "Opal" },
        { 0x52711CEE970D947F, "Peonia" },
        { 0x15F141EE74D3611E, "Peony" },
        { 0x11C9205A1E955ADB, "Piers" },
        { 0x5A5B1E27FA6E0894, "Poké Kid (F)" },
        { 0xDB25DC51B0AE4AF1, "Poké Kid (M)" },
        { 0x87904B3A7D4E62E9, "Poké Mart Clerk" },
        { 0xB3A105D9796A7804, "Pokémon Breeder (F)" },
        { 0x89284405A3A85CE1, "Pokémon Breeder (M)" },
        { 0xA33132DF03F2EF24, "Pokémon Center Lady" },
        { 0x9D6F9780F555F4CB, "Police Officer" },
        { 0xA2C1EAC01CF8F115, "Postman" },
        { 0xED9863A48E9B02B1, "Preschooler (F)" },
        { 0x6CCEA57AD85C7354, "Preschooler (M)" },
        { 0xFF195704BFB8A00D, "Professor Magnolia" },
        { 0xF53DEE04B9CCA662, "Professor Magnolia" },
        { 0xE8C7CCFBFB33F29F, "Raihan" },
        { 0x46760CC68612DC3D, "Rail Staff" },
        { 0xC89044F25A853988, "Reporter" },
        { 0x8647F49FC658C046, "Rose" },
        { 0x8E70DD9FCAD3FEF1, "Rose (Casual)" },
        { 0xCDAA7C271FA2DB1F, "Rotomi" },
        { 0xAF66C64C8606E02A, "Rusted Sword" },
        { 0xAF66C94C8606E543, "Rusted Shield" },
        { 0x8D72924BAD918727, "Schoolboy" },
        { 0x0A466C0B77D1159E, "Schoolgirl" },
        { 0x69C39C997BCD88B4, "Shielbert" },
        { 0x98382B1AA21F4CD7, "Sonia" },
        { 0x91C2421A9F15A2AC, "Sonia (Lab Coat)" },
        { 0x072E0468EA46DC96, "Sordward" },
        { 0x4EAAD88E8CF613A9, "Swimmer (F)" },
        { 0x09589FAEEDCED86F, "Swimmer (M)" },
        { 0x7CC9EA6A85A2496F, "Team Yell Grunt (F)" },
        { 0x2C699C0B1880FFF6, "Team Yell Grunt (M)" },
        { 0x040C69585A07DB5A, "Team Yell Grunt (M)" },
        { 0xB7FE5C9AB8D8AC9A, "Villager (F)" },
        { 0x6C06C79A8E61C86B, "Villager (M)" },
        { 0x3897DF33903FCED3, "Worker (F)" },
        { 0xBFD8EDB294F9341A, "Worker (M)" },
        { 0x70F62EB0C1F197D6, "Young Man" },
        { 0x1369813DBDEFA5CF, "Young Woman" },
        { 0x0AADBADDC4027B3F, "Youngster" },
    };

    public override string ToString()
    {
        var ident = Field00.Field00.Identifier;
        var hashModel = Field00.Field00.HashModel;
        var name = Models.TryGetValue(hashModel, out var model) ? model : hashModel.ToString("X16");
        return $"{name}: {ident.HashObjectName:X16} v{ModelVariant} @ {ident.Location3f}";
    }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F16_ArrayEntry;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F16;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F16_A;

[TypeConverter(typeof(ExpandableObjectConverter))]
public partial class PlacementZone_F16_IntFloat;
