// This code is probably not in the right place and 
//   hashes could probably be inferred automatically 
//   as described in https://github.com/kwsch/pkNX/issues/383


using System.Collections.Generic;

namespace pkNX.Structures;

public static class ItemHash8{
    public static readonly Dictionary<int,ulong> IDtoHash = new Dictionary<int, ulong>(){
        {1, 0xB2FFCB9037D24A9E},    //Master Ball
        {2, 0xED1E0E40FCF7F73C},    //Ultra Ball
        {3, 0x2A49DDAF3E13BB35},    //Great Ball
        {4, 0xF9CF8682BDF2F64C},    //Poké Ball
        {5, 0xB2095947BB95478C},    //Safari Ball
        {6, 0xEAD616831BB38414},    //Net Ball
        {7, 0xE4EED5466E83E157},    //Dive Ball
        {8, 0x6DF34298D30DE03C},    //Nest Ball
        {9, 0x6A908E8D544BDB5E},    //Repeat Ball
        {10, 0x65F6FF1F16ADEA11},    //Timer Ball
        {11, 0x67F4D1E0450E209F},    //Luxury Ball
        {12, 0x74BD4E12C97DDE87},    //Premier Ball
        {13, 0x991AA6EC829B2D8C},    //Dusk Ball
        {14, 0x853C176407BEA0D3},    //Heal Ball
        {15, 0x45959E68EB650E40},    //Quick Ball
        {16, 0xBEF4605699980279},    //Cherish Ball
        {17, 0x77C84B6E84AAEA9F},    //Potion
        {18, 0x2ED2CC760DB17D02},    //Antidote
        {19, 0x90D8C428659B026A},    //Burn Heal
        {20, 0xFE42D82C4F263CA3},    //Ice Heal
        {21, 0xC18CB09D822E2603},    //Awakening
        {22, 0xDA99496119ED9C74},    //Paralyze Heal
        {23, 0x20F956D89A3B1633},    //Full Restore
        {24, 0xB251E8B11D09A5BA},    //Max Potion
        {25, 0xA4F2C7D669DCA988},    //Hyper Potion
        {26, 0x5ED8099C61D86C95},    //Super Potion
        {27, 0x45377237A67158DF},    //Full Heal
        {28, 0x02DF11F6634E4C55},    //Revive
        {29, 0x6117E6ABEA9EDEF8},    //Max Revive
        {30, 0x1A677465F90CF43D},    //Fresh Water
        {31, 0xA7E30636E672C2F0},    //Soda Pop
        {32, 0xDC40F1124266407A},    //Lemonade
        {33, 0x8A2FDA05C3443F52},    //Moomoo Milk
        {34, 0x732068E27B2460C9},    //Energy Powder
        {35, 0x436AD024490118B6},    //Energy Root
        {36, 0xEC798E17D3C263F5},    //Heal Powder
        {37, 0x3B0BEBA67B5EAD5F},    //Revival Herb
        {38, 0xA944675C44E43548},    //Ether
        {39, 0x287714E489B4DB96},    //Max Ether
        {40, 0x8369DEC90FC057F9},    //Elixir
        {41, 0xD3A9514F0B43BDDE},    //Max Elixir
        {42, 0x32BC80826AA0893D},    //Lava Cookie
        {43, 0xF1CD4B446CD24FBD},    //Berry Juice
        {45, 0x6536B052B0DD20A0},    //HP Up
        {46, 0x906C430B1B84ECE0},    //Protein
        {47, 0x8FE96B42832BF3CA},    //Iron
        {48, 0x1CC3F4D3372E57B6},    //Carbos
        {49, 0xE22366C78D995F57},    //Calcium
        {50, 0x0C05A768BC75B196},    //Rare Candy
        {51, 0xC7350ECEAE1FE826},    //PP Up
        {52, 0x2CCB196A52EF63FC},    //Zinc
        {53, 0x4839B2E087F8D637},    //PP Max
        {54, 0xB46124D4EFA08CBA},    //Old Gateau
        {55, 0xA0B59641101574F0},    //Guard Spec.
        {56, 0x97968CB6EFE5FE02},    //Dire Hit
        {57, 0xC641C7FBF9F6632D},    //X Attack
        {58, 0x6A36B41CC84DEB33},    //X Defense
        {59, 0xF78687BF724A5F1B},    //X Speed
        {60, 0x61E7A7D3647633B9},    //X Accuracy
        {61, 0x5A009CE4EE879B9C},    //X Sp. Atk
        {62, 0x19CC0DA57E1F5730},    //X Sp. Def
        {63, 0x02082CFA1ED05200},    //Poké Doll
        {76, 0xA28BCA53E5AFC5E7},    //Super Repel
        {77, 0xD817CD0FF3DE34DB},    //Max Repel
        {78, 0x91910011B5905B1E},    //Escape Rope
        {79, 0x2D9DF249F3FC58EA},    //Repel
        {80, 0x40328DB129B8FB3C},    //Sun Stone
        {81, 0x5F8C05AFDEDC295E},    //Moon Stone
        {82, 0x389F26326DDEE7F0},    //Fire Stone
        {83, 0x7A1EEBF4503AF251},    //Thunder Stone
        {84, 0xABDA0CCA9C524592},    //Water Stone
        {85, 0x8B1017157156AFBC},    //Leaf Stone
        {86, 0xAB67274A9EE011BB},    //Tiny Mushroom
        {87, 0xFB4B51BAC68D3549},    //Big Mushroom
        {88, 0xF665C670ECE3D62D},    //Pearl
        {89, 0x5DFADE322489E612},    //Big Pearl
        {90, 0xB7E350EF3FDF86C8},    //Stardust
        {91, 0xAFACB8281CCE7520},    //Star Piece
        {92, 0x13446DBD2A9B1C41},    //Nugget
        {94, 0x81F9917264A79D56},    //Honey
        {106, 0xD7C049750DD8652D},    //Rare Bone
        {107, 0x86A321B146DFE8CD},    //Shiny Stone
        {108, 0x6C1AFC847393D7AB},    //Dusk Stone
        {109, 0xF9DDD15DEA351AD5},    //Dawn Stone
        {110, 0x575117DB01777855},    //Oval Stone
        {112, 0x0F2FE6B1EB04F534},    //Griseous Orb
        {116, 0xA12054E1DC6C6AFA},    //Douse Drive
        {117, 0xA091B74AD8712F45},    //Shock Drive
        {118, 0xA7B92398A539A2FC},    //Burn Drive
        {119, 0xE6E390C318FE758E},    //Chill Drive
        {134, 0x7B09F9F0B3009C4F},    //Sweet Heart
        {135, 0x341C30CB8758F5B3},    //Adamant Orb
        {136, 0xDF501DA83F35354B},    //Lustrous Orb
        {149, 0xF7484CBDF8F1506C},    //Cheri Berry
        {150, 0x6CC64681F251AFB0},    //Chesto Berry
        {151, 0x9D27496CD2BA570E},    //Pecha Berry
        {152, 0x8A84655107A78264},    //Rawst Berry
        {153, 0xBE07B055619CDC06},    //Aspear Berry
        {154, 0xC5E0A17981C47F66},    //Leppa Berry
        {155, 0x1C2A78BA392E3FE0},    //Oran Berry
        {156, 0xFD3846567A2485A3},    //Persim Berry
        {157, 0x240B99F40359D329},    //Lum Berry
        {158, 0x5807A02F998EE432},    //Sitrus Berry
        {159, 0x31520D1A59F141AA},    //Figy Berry
        {160, 0x78DB73A8822FEAF0},    //Wiki Berry
        {161, 0x71CA19E21C077856},    //Mago Berry
        {162, 0xB0256B087D304E34},    //Aguav Berry
        {163, 0xEC62FC9EC6E3BC44},    //Iapapa Berry
        {169, 0xE77E98F85BD3EB50},    //Pomeg Berry
        {170, 0x2A8FF28ECD951420},    //Kelpsy Berry
        {171, 0x3011BD13F97DAE81},    //Qualot Berry
        {172, 0x987AC8E4205E5AA7},    //Hondew Berry
        {173, 0x010752BEFA53243E},    //Grepa Berry
        {174, 0x6033345ABECD73DF},    //Tamato Berry
        {184, 0x21DBD370E974A858},    //Occa Berry
        {185, 0xEAC7ACC19813C8C6},    //Passho Berry
        {186, 0xC2D5B23850C7EB57},    //Wacan Berry
        {187, 0x48C7F238DCBCAFF8},    //Rindo Berry
        {188, 0x69C6101488A416B1},    //Yache Berry
        {189, 0xE4DF53D97884BC71},    //Chople Berry
        {190, 0x77D0E6546628EF11},    //Kebia Berry
        {191, 0x3289F9D9B68EEB5B},    //Shuca Berry
        {192, 0x0B5EFF40C02F8866},    //Coba Berry
        {193, 0x5AA93DAE8DC7D70C},    //Payapa Berry
        {194, 0x34C5F84CD6753C4B},    //Tanga Berry
        {195, 0x63897EBE3D3762C5},    //Charti Berry
        {196, 0xF6F4A779A69E26CF},    //Kasib Berry
        {197, 0xCA5D27FA2ADB7160},    //Haban Berry
        {198, 0xE7DDAB279100234B},    //Colbur Berry
        {199, 0xC5D75E35DFDE83A7},    //Babiri Berry
        {200, 0x431E10D76325875E},    //Chilan Berry
        {201, 0xF45A12B0E0E3C277},    //Liechi Berry
        {202, 0xE45F6B1BCC496A78},    //Ganlon Berry
        {203, 0x739E7BB6CBCDAB19},    //Salac Berry
        {204, 0x9ACD2595C1A62408},    //Petaya Berry
        {205, 0xAA285326B339BEE0},    //Apicot Berry
        {206, 0xCA081AB122792F36},    //Lansat Berry
        {207, 0xCE262E65D5C3852A},    //Starf Berry
        {208, 0xB32FCF7D2B457AFC},    //Enigma Berry
        {209, 0x8E0E92CE27E51009},    //Micle Berry
        {210, 0xC5021A06D7A13C8A},    //Custap Berry
        {211, 0x5C368D627FD49765},    //Jaboca Berry
        {212, 0xC6052484D55C457C},    //Rowap Berry
        {213, 0x97015C2A4C15F415},    //Bright Powder
        {214, 0xD4017C3F3464D936},    //White Herb
        {215, 0xC3803D9D65DCD229},    //Macho Brace
        {217, 0x8ABC11AA5D23620C},    //Quick Claw
        {218, 0x9CFED48E5AAAC502},    //Soothe Bell
        {219, 0xC60EBF553F5425B6},    //Mental Herb
        {220, 0x5C58902B05EE580D},    //Choice Band
        {221, 0x7A70C3DEB0BE40EB},    //King’s Rock
        {222, 0xCF13594CEDE25057},    //Silver Powder
        {223, 0xE7912E72A6D58C16},    //Amulet Coin
        {224, 0x8110767AD7FF5137},    //Cleanse Tag
        {225, 0xA55C7D67319C11E6},    //Soul Dew
        {228, 0x86C421489C90C385},    //Smoke Ball
        {229, 0x0749DE54B1B3A59F},    //Everstone
        {230, 0xCC2AA34CBCEDAD90},    //Focus Band
        {231, 0xCF7F463AD4A13FE9},    //Lucky Egg
        {232, 0x9123C4886233FEF1},    //Scope Lens
        {233, 0xA8C32DE324D2590B},    //Metal Coat
        {234, 0x75BF03D34DF39F54},    //Leftovers
        {235, 0x4E1D184E5BDF05C7},    //Dragon Scale
        {236, 0xA6218470BDF5637B},    //Light Ball
        {237, 0x6116654CF52E59E2},    //Soft Sand
        {238, 0xECEFFEDD54845F0E},    //Hard Stone
        {239, 0x1BC6015FD88E113E},    //Miracle Seed
        {240, 0x6484343F7F949D94},    //Black Glasses
        {241, 0xAF53F1D5253CEA44},    //Black Belt
        {242, 0x1491E2447C4F03F1},    //Magnet
        {243, 0x649FDD1B452397AA},    //Mystic Water
        {244, 0x21EC55656DF7111C},    //Sharp Beak
        {245, 0x16023C2D95C7D842},    //Poison Barb
        {246, 0x9E427EAAE66A5644},    //Never-Melt Ice
        {247, 0x0C0546940023BD06},    //Spell Tag
        {248, 0x1748C470BCCAAC6C},    //Twisted Spoon
        {249, 0x96E6C1FE349A5EA6},    //Charcoal
        {250, 0x07D96D8AF69604D6},    //Dragon Fang
        {251, 0x3DD8A99BAAB16863},    //Silk Scarf
        {252, 0x76D3664DC4EEECEA},    //Upgrade
        {253, 0x1813A4A66AC551D1},    //Shell Bell
        {254, 0x871FB358F53446A6},    //Sea Incense
        {255, 0x9BDC29EF85748EF7},    //Lax Incense
        {257, 0x4253A350261DDFA3},    //Metal Powder
        {258, 0xC7647B42D4A230A0},    //Thick Club
        {259, 0xD64896B82F1DC06F},    //Leek
        {265, 0x8487B6095E9CED4C},    //Wide Lens
        {266, 0xC1BCC57465803E8C},    //Muscle Band
        {267, 0xDDA3B6EC3A55C722},    //Wise Glasses
        {268, 0x8FEBC0AF818483F1},    //Expert Belt
        {269, 0xAE546152ED473052},    //Light Clay
        {270, 0x81CE1D0B1083DA08},    //Life Orb
        {271, 0x6ABC4E98D224CABD},    //Power Herb
        {272, 0x213E983B3FEF30F2},    //Toxic Orb
        {273, 0xB040792974BAF831},    //Flame Orb
        {274, 0xFB1F7912BB908EF2},    //Quick Powder
        {275, 0xF11AF045A90F84FD},    //Focus Sash
        {276, 0x7AB305933A2353E3},    //Zoom Lens
        {277, 0x1BC6F00E61EA863B},    //Metronome
        {278, 0xB7C6A04FB62DB9CF},    //Iron Ball
        {279, 0x6BF70C799345550B},    //Lagging Tail
        {280, 0x0530073420F21D29},    //Destiny Knot
        {281, 0x02550949599D9E7E},    //Black Sludge
        {282, 0x760902529D28071B},    //Icy Rock
        {283, 0x0C70E5D28D8FFCDE},    //Smooth Rock
        {284, 0x4D00C8BF7FB5DA07},    //Heat Rock
        {285, 0x41F47533DE9FA013},    //Damp Rock
        {286, 0xC99ABBA6E884A2F0},    //Grip Claw
        {287, 0x5AD9BE5A83BF246D},    //Choice Scarf
        {288, 0x1F950E3F375A291E},    //Sticky Barb
        {289, 0xCF90A871108641B7},    //Power Bracer
        {290, 0x4D3149CA727E99EE},    //Power Belt
        {291, 0x98F16E2FAA3E29C1},    //Power Lens
        {292, 0x7E64A2D056A47975},    //Power Band
        {293, 0x8BAE99D80D003571},    //Power Anklet
        {294, 0x30B2C9C369634099},    //Power Weight
        {295, 0x61B10922F6AD2C68},    //Shed Shell
        {296, 0x6D36E0313CBE2C74},    //Big Root
        {297, 0xE74F254444349190},    //Choice Specs
        {298, 0x69FF8364BDEF0BE6},    //Flame Plate
        {299, 0x3E25D433DB87D9E6},    //Splash Plate
        {300, 0xA49EFD7E87B3C268},    //Zap Plate
        {301, 0x25FB1CA9B6A72A96},    //Meadow Plate
        {302, 0x177A31BE5B7A8F19},    //Icicle Plate
        {303, 0xEB2C7250AE3E992D},    //Fist Plate
        {304, 0x513601C0609999F5},    //Toxic Plate
        {305, 0x5D71CB1C244C7715},    //Earth Plate
        {306, 0x1AF162A1FF017E43},    //Sky Plate
        {307, 0x42937C973AB77015},    //Mind Plate
        {308, 0xA4B30222F5CB066C},    //Insect Plate
        {309, 0xAE69BB344EBEED15},    //Stone Plate
        {310, 0xF7D438D2F6657635},    //Spooky Plate
        {311, 0x458177B731CC1099},    //Draco Plate
        {312, 0x851C73F7BFCB30EE},    //Dread Plate
        {313, 0x1901896B0640F818},    //Iron Plate
        {314, 0xB386E91D373682BD},    //Odd Incense
        {315, 0xCBAE51B4B1D747BD},    //Rock Incense
        {316, 0x394D19DEED17CAB0},    //Full Incense
        {317, 0xEA08466FE932FFE8},    //Wave Incense
        {318, 0xF4F563CDB01446E7},    //Rose Incense
        {319, 0xE8A8AFE0DCFF41B6},    //Luck Incense
        {320, 0x5B1DB44AF735CB94},    //Pure Incense
        {321, 0x83A97B7ECEA45C02},    //Protector
        {322, 0xD2E2C14A2A7297F9},    //Electirizer
        {323, 0x5A842E72F6F75275},    //Magmarizer
        {324, 0xF0C3A3637ACE825F},    //Dubious Disc
        {325, 0xBD4DAF5676868045},    //Reaper Cloth
        {326, 0xD763C83D73EABF13},    //Razor Claw
        {328, 0x8299B75A6EEA54F9},    //TM01
        {329, 0x8299B45A6EEA4FE0},    //TM02
        {330, 0x8299B55A6EEA5193},    //TM03
        {331, 0x8299BA5A6EEA5A12},    //TM04
        {332, 0x8299BB5A6EEA5BC5},    //TM05
        {333, 0x8299B85A6EEA56AC},    //TM06
        {334, 0x8299B95A6EEA585F},    //TM07
        {335, 0x8299BE5A6EEA60DE},    //TM08
        {336, 0x8299BF5A6EEA6291},    //TM09
        {337, 0x829D3C5A6EED6CCF},    //TM10
        {338, 0x829D3B5A6EED6B1C},    //TM11
        {339, 0x829D3E5A6EED7035},    //TM12
        {340, 0x829D3D5A6EED6E82},    //TM13
        {341, 0x829D385A6EED6603},    //TM14
        {342, 0x829D375A6EED6450},    //TM15
        {343, 0x829D3A5A6EED6969},    //TM16
        {344, 0x829D395A6EED67B6},    //TM17
        {345, 0x829D445A6EED7A67},    //TM18
        {346, 0x829D435A6EED78B4},    //TM19
        {347, 0x8292CA5A6EE45694},    //TM20
        {348, 0x8292CB5A6EE45847},    //TM21
        {349, 0x8292CC5A6EE459FA},    //TM22
        {350, 0x8292CD5A6EE45BAD},    //TM23
        {351, 0x8292C65A6EE44FC8},    //TM24
        {352, 0x8292C75A6EE4517B},    //TM25
        {353, 0x8292C85A6EE4532E},    //TM26
        {354, 0x8292C95A6EE454E1},    //TM27
        {355, 0x8292C25A6EE448FC},    //TM28
        {356, 0x8292C35A6EE44AAF},    //TM29
        {357, 0x8296505A6EE7701D},    //TM30
        {358, 0x82964F5A6EE76E6A},    //TM31
        {359, 0x82964E5A6EE76CB7},    //TM32
        {360, 0x82964D5A6EE76B04},    //TM33
        {361, 0x82964C5A6EE76951},    //TM34
        {362, 0x82964B5A6EE7679E},    //TM35
        {363, 0x82964A5A6EE765EB},    //TM36
        {364, 0x8296495A6EE76438},    //TM37
        {365, 0x8296485A6EE76285},    //TM38
        {366, 0x8296475A6EE760D2},    //TM39
        {367, 0x828C3E5A6EDEFD02},    //TM40
        {368, 0x828C3F5A6EDEFEB5},    //TM41
        {369, 0x828C3C5A6EDEF99C},    //TM42
        {370, 0x828C3D5A6EDEFB4F},    //TM43
        {371, 0x828C3A5A6EDEF636},    //TM44
        {372, 0x828C3B5A6EDEF7E9},    //TM45
        {373, 0x828C385A6EDEF2D0},    //TM46
        {374, 0x828C395A6EDEF483},    //TM47
        {375, 0x828C465A6EDF0A9A},    //TM48
        {376, 0x828C475A6EDF0C4D},    //TM49
        {377, 0x828F445A6EE13D0B},    //TM50
        {378, 0x828F435A6EE13B58},    //TM51
        {379, 0x828F465A6EE14071},    //TM52
        {380, 0x828F455A6EE13EBE},    //TM53
        {381, 0x828F485A6EE143D7},    //TM54
        {382, 0x828F475A6EE14224},    //TM55
        {383, 0x828F4A5A6EE1473D},    //TM56
        {384, 0x828F495A6EE1458A},    //TM57
        {385, 0x828F3C5A6EE12F73},    //TM58
        {386, 0x828F3B5A6EE12DC0},    //TM59
        {387, 0x8285325A6ED8C9F0},    //TM60
        {388, 0x8285335A6ED8CBA3},    //TM61
        {389, 0x8285345A6ED8CD56},    //TM62
        {390, 0x8285355A6ED8CF09},    //TM63
        {391, 0x8285365A6ED8D0BC},    //TM64
        {392, 0x8285375A6ED8D26F},    //TM65
        {393, 0x8285385A6ED8D422},    //TM66
        {394, 0x8285395A6ED8D5D5},    //TM67
        {395, 0x82853A5A6ED8D788},    //TM68
        {396, 0x82853B5A6ED8D93B},    //TM69
        {397, 0x8288B85A6EDBE379},    //TM70
        {398, 0x8288B75A6EDBE1C6},    //TM71
        {399, 0x8288B65A6EDBE013},    //TM72
        {400, 0x8288B55A6EDBDE60},    //TM73
        {401, 0x8288BC5A6EDBEA45},    //TM74
        {402, 0x8288BB5A6EDBE892},    //TM75
        {403, 0x8288BA5A6EDBE6DF},    //TM76
        {404, 0x8288B95A6EDBE52C},    //TM77
        {405, 0x8288C05A6EDBF111},    //TM78
        {406, 0x8288BF5A6EDBEF5E},    //TM79
        {407, 0x82B4C65A6F01362E},    //TM80
        {408, 0x82B4C75A6F0137E1},    //TM81
        {409, 0x82B4C45A6F0132C8},    //TM82
        {410, 0x82B4C55A6F01347B},    //TM83
        {411, 0x82B4CA5A6F013CFA},    //TM84
        {412, 0x82B4CB5A6F013EAD},    //TM85
        {413, 0x82B4C85A6F013994},    //TM86
        {414, 0x82B4C95A6F013B47},    //TM87
        {415, 0x82B4BE5A6F012896},    //TM88
        {416, 0x82B4BF5A6F012A49},    //TM89
        {417, 0x82B84C5A6F044FB7},    //TM90
        {418, 0x82B84B5A6F044E04},    //TM91
        {419, 0x82B84E5A6F04531D},    //TM92
        {485, 0xFA40E519E7CCCA64},    //Red Apricorn
        {486, 0x820493C2E1A64813},    //Blue Apricorn
        {487, 0x883A4B1806754A7B},    //Yellow Apricorn
        {488, 0x5EB386C4D8477752},    //Green Apricorn
        {489, 0xADA33002C9DFBE73},    //Pink Apricorn
        {490, 0xCDF9FCE3E4D490BE},    //White Apricorn
        {491, 0xAFDE4855C383721A},    //Black Apricorn
        {492, 0x36E104FC4B145ECB},    //Fast Ball
        {493, 0xC0C8EDB562A57D57},    //Level Ball
        {494, 0xE87B13F3FE9ADB1D},    //Lure Ball
        {495, 0x78E86A2AADA42D85},    //Heavy Ball
        {496, 0x1894ADC72B09B50C},    //Love Ball
        {497, 0xA55D3FAFBAB5C3CD},    //Friend Ball
        {498, 0xDB096CD6E9D172D3},    //Moon Ball
        {499, 0x3ADBC9334B3B6333},    //Sport Ball
        {500, 0xE5EC42F4A5183938},    //Park Ball
        {504, 0xF2900C9885502CFA},    //Rage Candy Bar
        {537, 0xB33D0DB55731FE2E},    //Prism Scale
        {538, 0x91CAC3ABD09A55C0},    //Eviolite
        {539, 0x86FFAC20AB24F95D},    //Float Stone
        {540, 0xF32B898E981B568A},    //Rocky Helmet
        {541, 0x4F31616740489B83},    //Air Balloon
        {542, 0x279C10B3D5836877},    //Red Card
        {543, 0x5A6CCADAB5F5334A},    //Ring Target
        {544, 0x296FFDCD54D8A1B2},    //Binding Band
        {545, 0x99CAF66347154C1B},    //Absorb Bulb
        {546, 0xC7704234E3D8DC2C},    //Cell Battery
        {547, 0xAC4287B989EA05DB},    //Eject Button
        {564, 0xEBD9603DBE9D2BC0},    //Normal Gem
        {565, 0x6233DC21BE8E1136},    //Health Feather
        {566, 0x09AD5F8196A1473E},    //Muscle Feather
        {567, 0xA712F65501C9E3F9},    //Resist Feather
        {568, 0x576EA3A2E9E7BEE7},    //Genius Feather
        {569, 0x4F99DB651B5CBA51},    //Clever Feather
        {570, 0x8F59E15BE5EEC54D},    //Swift Feather
        {571, 0x6C229DAA2359876A},    //Pretty Feather
        {576, 0xE1834155E65FE0B5},    //Dream Ball
        {580, 0x28BB42FE27C02760},    //Balm Mushroom
        {581, 0xA3ED789A018DE1D9},    //Big Nugget
        {582, 0xC8A509D7DDF091A3},    //Pearl String
        {583, 0x1B96C77BCFB19A13},    //Comet Shard
        {591, 0x24336E91FCC24FDD},    //Casteliacone
        {618, 0x82B84D5A6F04516A},    //TM93
        {619, 0x82B8485A6F0448EB},    //TM94
        {620, 0x82B8475A6F044738},    //TM95
        {628, 0x7009A2AA1EEA28E3},    //DNA Splicers
        {629, 0x888ADD7CE716EA3F},    //DNA Splicers (2)
        {631, 0xA0B2C3EFCAD2864F},    //Oval Charm
        {632, 0x394B58084D574509},    //Shiny Charm
        {638, 0xBDD3B1C6331B2E53},    //Reveal Glass
        {639, 0x82088AC1A8820FDD},    //Weakness Policy
        {640, 0x1903B748EBB942BC},    //Assault Vest
        {644, 0x4F8D0509FBD0D88E},    //Pixie Plate
        {645, 0x346ABBA954840A1F},    //Ability Capsule
        {646, 0xD89F9564D30E815C},    //Whipped Dream
        {647, 0xB59E71109F970D74},    //Sachet
        {648, 0xA8EE79A0626DE5F7},    //Luminous Moss
        {649, 0x9227F533E288DFC8},    //Snowball
        {650, 0x8F9E5B2A600A044A},    //Safety Goggles
        {686, 0x8CA644FA7AF5A92D},    //Roseli Berry
        {687, 0xFD429E44243A2B76},    //Kee Berry
        {688, 0x08E01121DFC5148F},    //Maranga Berry
        {690, 0x82B84A5A6F044C51},    //TM96
        {691, 0x82B8495A6F044A9E},    //TM97
        {692, 0x82B8445A6F04421F},    //TM98
        {693, 0x82B8435A6F04406C},    //TM99
        {703, 0xE52896802DE992D6},    //Adventure Guide
        {708, 0x58683422C3110D39},    //Lumiose Galette
        {709, 0xCCA90D53EE77D1A3},    //Shalour Sable
        {795, 0x6986B223F665B7F2},    //Bottle Cap
        {796, 0xBA470182D3A411D6},    //Gold Bottle Cap
        {846, 0x3E851FF78A35E0CF},    //Adrenaline Orb
        {847, 0x611A3F7911B92A9B},    //Zygarde Cube
        {849, 0x7426327FCC7D8BEF},    //Ice Stone
        {851, 0x5559FEBA87B46F24},    //Beast Ball
        {852, 0xE2A158ED3D8E66E8},    //Big Malasada
        {879, 0xB20910B4242D8D5D},    //Terrain Extender
        {880, 0x491B572848FFB75B},    //Protective Pads
        {881, 0x757CB5845DB6A907},    //Electric Seed
        {882, 0x83C03E0264E8E32E},    //Psychic Seed
        {883, 0xE88AB61F3C306F2E},    //Misty Seed
        {884, 0xD16D939D9D207A88},    //Grassy Seed
        {903, 0x12A1FE4077049186},    //Pewter Crunchies
        {904, 0x508F6D964CD9D793},    //Fighting Memory
        {905, 0x164DE1B742018871},    //Flying Memory
        {906, 0x4D1E27215D9FA219},    //Poison Memory
        {907, 0x06603905CD4C3617},    //Ground Memory
        {908, 0x67B209702C738886},    //Rock Memory
        {909, 0xEB8512D8EA6274D5},    //Bug Memory
        {910, 0xE09EBFF252D5DF98},    //Ghost Memory
        {911, 0xDF594A6BB6E20755},    //Steel Memory
        {912, 0x5779773CE9BEA249},    //Fire Memory
        {913, 0x0D16CDA4BD38833D},    //Water Memory
        {914, 0xC0AE0929A7EE5C69},    //Grass Memory
        {915, 0x84E4F258F49B080A},    //Electric Memory
        {916, 0xFF0D8EBDED15C4D0},    //Psychic Memory
        {917, 0x64AB64EA92C6438E},    //Ice Memory
        {918, 0x9045B75A5AF194D2},    //Dragon Memory
        {919, 0xB4F819AB4CC660F4},    //Dark Memory
        {920, 0xB837D7B8A14AAFD0},    //Fairy Memory
        {943, 0x24F714AB3F6CA1B2},    //N-Solarizer
        {944, 0x4B5DC52922EA9CA5},    //N-Lunarizer
        {945, 0x5A87E483BFD4C2B2},    //N-Solarizer (2)
        {946, 0x13A42061DAB9254D},    //N-Lunarizer (2)
        {1074, 0x1E6F3F165A6C68AD},    //Endorsement
        {1075, 0xCB0A6D4359AB1D19},    //Pokémon Box Link (2)
        {1076, 0xF427A84D48EEA1BA},    //Wishing Star
        {1077, 0x0D86E7AB10C9E0A4},    //Dynamax Band
        {1080, 0x8AA28E727A2C3D51},    //Fishing Rod (SW/SH)
        {1081, 0x5A685F9D3368AFA5},    //Rotom Bike (1)
        {1084, 0x0FDD09BFF445DA5B},    //Sausages
        {1085, 0xE5AD8441B3058765},    //Bob’s Food Tin
        {1086, 0x59CF036C53CA769F},    //Bach’s Food Tin
        {1087, 0x5656A9944D7BADB8},    //Tin of Beans
        {1088, 0xAAD56999F088265F},    //Bread
        {1089, 0x61BC3F850CFCF003},    //Pasta
        {1090, 0xF074CE0CD56C9F4A},    //Mixed Mushrooms
        {1091, 0xC82DF3DB8A2D691A},    //Smoke-Poke Tail
        {1092, 0x1732BC55572427A8},    //Large Leek
        {1093, 0x57D3F1C13C66A74C},    //Fancy Apple
        {1094, 0x62D0C11D10D0C585},    //Brittle Bones
        {1095, 0xA9E3335A70F2A770},    //Pack of Potatoes
        {1096, 0xA34AA32C84657BA8},    //Pungent Root
        {1097, 0x079B009A63B9B764},    //Salad Mix
        {1098, 0xB436013F42FAE90E},    //Fried Food
        {1099, 0x9682D7AE84A63581},    //Boiled Egg
        {1100, 0x20E7FB32D1FAAD74},    //Camping Gear
        {1103, 0xFF38E4EABE713FF5},    //Rusted Sword
        {1104, 0x082F905FCEFDE31B},    //Rusted Shield
        {1105, 0x5D3B423A5148476C},    //Fossilized Bird
        {1106, 0xC2673322DB4BC615},    //Fossilized Fish
        {1107, 0x1EFF746A139D48AB},    //Fossilized Drake
        {1108, 0xE2E52DD52937289C},    //Fossilized Dino
        {1109, 0x7D3C06B30D83D133},    //Strawberry Sweet
        {1110, 0x22465CE9290427E8},    //Love Sweet
        {1111, 0x8638C82BC9431A89},    //Berry Sweet
        {1112, 0x22BD5673497F4AEE},    //Clover Sweet
        {1113, 0x5420D426AAF67E69},    //Flower Sweet
        {1114, 0x35F2282B8E1F44B4},    //Star Sweet
        {1115, 0x5A638584D1FB1AD6},    //Ribbon Sweet
        {1116, 0x80E734F3EAC68DA9},    //Sweet Apple
        {1117, 0xFA773F25DFA37D40},    //Tart Apple
        {1118, 0xF06BF4A920227BCE},    //Throat Spray
        {1119, 0x39F76928C051E31D},    //Eject Pack
        {1120, 0x8EECF398B6B3858B},    //Heavy-Duty Boots
        {1121, 0x4D29D7AF1959F7EF},    //Blunder Policy
        {1122, 0xFC8CB805DE54D1CF},    //Room Service
        {1123, 0x227A6417F2EC1C12},    //Utility Umbrella
        {1124, 0xA6799AA1261B6158},    //Exp. Candy XS
        {1125, 0xA6799DA1261B6671},    //Exp. Candy S
        {1126, 0xA6799CA1261B64BE},    //Exp. Candy M
        {1127, 0xA6799FA1261B69D7},    //Exp. Candy L
        {1128, 0xA6799EA1261B6824},    //Exp. Candy XL
        {1129, 0xC4A7383B179ACF08},    //Dynamax Candy
        {1130, 0x4226B758AA88B8D0},    //TR00
        {1131, 0x4226B858AA88BA83},    //TR01
        {1132, 0x4226B958AA88BC36},    //TR02
        {1133, 0x4226BA58AA88BDE9},    //TR03
        {1134, 0x4226BB58AA88BF9C},    //TR04
        {1135, 0x4226BC58AA88C14F},    //TR05
        {1136, 0x4226BD58AA88C302},    //TR06
        {1137, 0x4226BE58AA88C4B5},    //TR07
        {1138, 0x4226BF58AA88C668},    //TR08
        {1139, 0x4226C058AA88C81B},    //TR09
        {1140, 0x4229BD58AA8AF8D9},    //TR10
        {1141, 0x4229BC58AA8AF726},    //TR11
        {1142, 0x4229BB58AA8AF573},    //TR12
        {1143, 0x4229BA58AA8AF3C0},    //TR13
        {1144, 0x4229C158AA8AFFA5},    //TR14
        {1145, 0x4229C058AA8AFDF2},    //TR15
        {1146, 0x4229BF58AA8AFC3F},    //TR16
        {1147, 0x4229BE58AA8AFA8C},    //TR17
        {1148, 0x4229C558AA8B0671},    //TR18
        {1149, 0x4229C458AA8B04BE},    //TR19
        {1150, 0x422D4358AA8E1262},    //TR20
        {1151, 0x422D4458AA8E1415},    //TR21
        {1152, 0x422D4158AA8E0EFC},    //TR22
        {1153, 0x422D4258AA8E10AF},    //TR23
        {1154, 0x422D3F58AA8E0B96},    //TR24
        {1155, 0x422D4058AA8E0D49},    //TR25
        {1156, 0x422D3D58AA8E0830},    //TR26
        {1157, 0x422D3E58AA8E09E3},    //TR27
        {1158, 0x422D4B58AA8E1FFA},    //TR28
        {1159, 0x422D4C58AA8E21AD},    //TR29
        {1160, 0x4230C958AA912BEB},    //TR30
        {1161, 0x4230C858AA912A38},    //TR31
        {1162, 0x4230CB58AA912F51},    //TR32
        {1163, 0x4230CA58AA912D9E},    //TR33
        {1164, 0x4230CD58AA9132B7},    //TR34
        {1165, 0x4230CC58AA913104},    //TR35
        {1166, 0x4230CF58AA91361D},    //TR36
        {1167, 0x4230CE58AA91346A},    //TR37
        {1168, 0x4230C158AA911E53},    //TR38
        {1169, 0x4230C058AA911CA0},    //TR39
        {1170, 0x42344F58AA944574},    //TR40
        {1171, 0x42345058AA944727},    //TR41
        {1172, 0x42345158AA9448DA},    //TR42
        {1173, 0x42345258AA944A8D},    //TR43
        {1174, 0x42344B58AA943EA8},    //TR44
        {1175, 0x42344C58AA94405B},    //TR45
        {1176, 0x42344D58AA94420E},    //TR46
        {1177, 0x42344E58AA9443C1},    //TR47
        {1178, 0x42344758AA9437DC},    //TR48
        {1179, 0x42344858AA94398F},    //TR49
        {1180, 0x42375558AA96857D},    //TR50
        {1181, 0x42375458AA9683CA},    //TR51
        {1182, 0x42375358AA968217},    //TR52
        {1183, 0x42375258AA968064},    //TR53
        {1184, 0x42375158AA967EB1},    //TR54
        {1185, 0x42375058AA967CFE},    //TR55
        {1186, 0x42374F58AA967B4B},    //TR56
        {1187, 0x42374E58AA967998},    //TR57
        {1188, 0x42374D58AA9677E5},    //TR58
        {1189, 0x42374C58AA967632},    //TR59
        {1190, 0x423ABB58AA9968A6},    //TR60
        {1191, 0x423ABC58AA996A59},    //TR61
        {1192, 0x423AB958AA996540},    //TR62
        {1193, 0x423ABA58AA9966F3},    //TR63
        {1194, 0x423ABF58AA996F72},    //TR64
        {1195, 0x423AC058AA997125},    //TR65
        {1196, 0x423ABD58AA996C0C},    //TR66
        {1197, 0x423ABE58AA996DBF},    //TR67
        {1198, 0x423AC358AA99763E},    //TR68
        {1199, 0x423AC458AA9977F1},    //TR69
        {1200, 0x423E4158AA9C822F},    //TR70
        {1201, 0x423E4058AA9C807C},    //TR71
        {1202, 0x423E4358AA9C8595},    //TR72
        {1203, 0x423E4258AA9C83E2},    //TR73
        {1204, 0x423E3D58AA9C7B63},    //TR74
        {1205, 0x423E3C58AA9C79B0},    //TR75
        {1206, 0x423E3F58AA9C7EC9},    //TR76
        {1207, 0x423E3E58AA9C7D16},    //TR77
        {1208, 0x423E4958AA9C8FC7},    //TR78
        {1209, 0x423E4858AA9C8E14},    //TR79
        {1210, 0x4241C758AA9F9BB8},    //TR80
        {1211, 0x4241C858AA9F9D6B},    //TR81
        {1212, 0x4241C958AA9F9F1E},    //TR82
        {1213, 0x4241CA58AA9FA0D1},    //TR83
        {1214, 0x4241CB58AA9FA284},    //TR84
        {1215, 0x4241CC58AA9FA437},    //TR85
        {1216, 0x4241CD58AA9FA5EA},    //TR86
        {1217, 0x4241CE58AA9FA79D},    //TR87
        {1218, 0x4241BF58AA9F8E20},    //TR88
        {1219, 0x4241C058AA9F8FD3},    //TR89
        {1220, 0x42454D58AAA2B541},    //TR90
        {1221, 0x42454C58AAA2B38E},    //TR91
        {1222, 0x42454B58AAA2B1DB},    //TR92
        {1223, 0x42454A58AAA2B028},    //TR93
        {1224, 0x42455158AAA2BC0D},    //TR94
        {1225, 0x42455058AAA2BA5A},    //TR95
        {1226, 0x42454F58AAA2B8A7},    //TR96
        {1227, 0x42454E58AAA2B6F4},    //TR97
        {1228, 0x42454558AAA2A7A9},    //TR98
        {1229, 0x42454458AAA2A5F6},    //TR99
        {1230, 0x8299B65A6EEA5346},    //TM00
        {1231, 0x0D13C93BD8DCCDF9},    //Lonely Mint
        {1232, 0x1A57D9E807480324},    //Adamant Mint
        {1233, 0x2092478DF6322B6E},    //Naughty Mint
        {1234, 0xE75C13CE760845E6},    //Brave Mint
        {1235, 0xE084CC69C1C95A8E},    //Bold Mint
        {1236, 0x26085B4BA06ED201},    //Impish Mint
        {1237, 0x7A583D71927B1BF3},    //Lax Mint
        {1238, 0x0601B424987513FF},    //Relaxed Mint
        {1239, 0xED6F0AC2C2377F5E},    //Modest Mint
        {1240, 0x93372A77418FDD45},    //Mild Mint
        {1241, 0x0CAE0D6D1B491E57},    //Rash Mint
        {1242, 0xC03D5019F90C88DD},    //Quiet Mint
        {1243, 0xB7E780E8B06E2A76},    //Calm Mint
        {1244, 0x3DEAFB3849FB760C},    //Gentle Mint
        {1245, 0xABCC44EC6B73CA1D},    //Careful Mint
        {1246, 0xF4CBF84FE225E458},    //Sassy Mint
        {1247, 0x24A75F1945A4BCCE},    //Timid Mint
        {1248, 0x739BA8A8BF5E6DA7},    //Hasty Mint
        {1249, 0x7EED9C64B111DD2F},    //Jolly Mint
        {1250, 0xF6850582EE4DB390},    //Naive Mint
        {1251, 0x27F3EBB3C7D3F927},    //Serious Mint
        {1252, 0x6DF0628A3E904257},    //Wishing Piece
        {1253, 0xF081D2232E80E3F5},    //Cracked Pot
        {1254, 0x64DC5998A3A8FA5E},    //Chipped Pot
        {1255, 0xA43AF70F387527AE},    //Hi-tech Earbuds
        {1256, 0x1C02A2319236E06A},    //Fruit Bunch
        {1257, 0xB920364B5EA775FA},    //Moomoo Cheese
        {1258, 0xE1E3A9DE9CCB79C1},    //Spice Mix
        {1259, 0xB126947E3C449224},    //Fresh Cream
        {1260, 0x3707752283DB9EA2},    //Packaged Curry
        {1261, 0x42BF1A83922609FD},    //Coconut Milk
        {1262, 0xE338CFDD79020881},    //Instant Noodles
        {1263, 0x70F2A96B0E14814A},    //Precooked Burger
        {1264, 0x68D0D0E99C23ADA4},    //Gigantamix
        {1266, 0xE82FBE0232A4D9C6},    //Rotom Bike (2)
        {1267, 0x7339946C4DC53920},    //Catching Charm
        {1269, 0x43D4C62EE72FBFC4},    //Old Letter
        {1270, 0x8787A04ED06E117D},    //Band Autograph
        {1271, 0x4CA0AE2E434B9721},    //Sonia’s Book
        {1278, 0x6A73EED362C2A0EC},    //Rotom Catalog
        {1279, 0x528CC905C5115D84},    //★And458
        {1280, 0x528CCC05C511629D},    //★And15
        {1281, 0x528CCB05C51160EA},    //★And337
        {1282, 0x528CC605C511586B},    //★And603
        {1283, 0x528CC505C51156B8},    //★And390
        {1284, 0x528CC805C5115BD1},    //★Sgr6879
        {1285, 0x528CC705C5115A1E},    //★Sgr6859
        {1286, 0x528CC205C511519F},    //★Sgr6913
        {1287, 0x528CC105C5114FEC},    //★Sgr7348
        {1288, 0x52894405C50E45AE},    //★Sgr7121
        {1289, 0x52894505C50E4761},    //★Sgr6746
        {1290, 0x52894205C50E4248},    //★Sgr7194
        {1291, 0x52894305C50E43FB},    //★Sgr7337
        {1292, 0x52894805C50E4C7A},    //★Sgr7343
        {1293, 0x52894905C50E4E2D},    //★Sgr6812
        {1294, 0x52894605C50E4914},    //★Sgr7116
        {1295, 0x52894705C50E4AC7},    //★Sgr7264
        {1296, 0x52893C05C50E3816},    //★Sgr7597
        {1297, 0x52893D05C50E39C9},    //★Del7882
        {1298, 0x5285BE05C50B2C25},    //★Del7906
        {1299, 0x5285BD05C50B2A72},    //★Del7852
        {1300, 0x5285BC05C50B28BF},    //★Psc596
        {1301, 0x5285BB05C50B270C},    //★Psc361
        {1302, 0x5285BA05C50B2559},    //★Psc510
        {1303, 0x5285B905C50B23A6},    //★Psc437
        {1304, 0x5285B805C50B21F3},    //★Psc8773
        {1305, 0x5285B705C50B2040},    //★Lep1865
        {1306, 0x5285C605C50B39BD},    //★Lep1829
        {1307, 0x5285C505C50B380A},    //★Boo5340
        {1308, 0x5282B805C508EC1C},    //★Boo5506
        {1309, 0x5282B905C508EDCF},    //★Boo5435
        {1310, 0x5282BA05C508EF82},    //★Boo5602
        {1311, 0x5282BB05C508F135},    //★Boo5733
        {1312, 0x5282B405C508E550},    //★Boo5235
        {1313, 0x5282B505C508E703},    //★Boo5351
        {1314, 0x5282B605C508E8B6},    //★Hya3748
        {1315, 0x5282B705C508EA69},    //★Hya3903
        {1316, 0x5282C005C508F9B4},    //★Hya3418
        {1317, 0x5282C105C508FB67},    //★Hya3482
        {1318, 0x527F3205C505D293},    //★Hya3845
        {1319, 0x527F3105C505D0E0},    //★Eri1084
        {1320, 0x527F3405C505D5F9},    //★Eri472
        {1321, 0x527F3305C505D446},    //★Eri1666
        {1322, 0x527F3605C505D95F},    //★Eri897
        {1323, 0x527F3505C505D7AC},    //★Eri1231
        {1324, 0x527F3805C505DCC5},    //★Eri874
        {1325, 0x527F3705C505DB12},    //★Eri1298
        {1326, 0x527F3A05C505E02B},    //★Eri1325
        {1327, 0x527F3905C505DE78},    //★Eri984
        {1328, 0x527BCC05C502EF6A},    //★Eri1464
        {1329, 0x527BCD05C502F11D},    //★Eri1393
        {1330, 0x527BCA05C502EC04},    //★Eri850
        {1331, 0x527BCB05C502EDB7},    //★Tau1409
        {1332, 0x527BC805C502E89E},    //★Tau1457
        {1333, 0x527BC905C502EA51},    //★Tau1165
        {1334, 0x527BC605C502E538},    //★Tau1791
        {1335, 0x527BC705C502E6EB},    //★Tau1910
        {1336, 0x527BC405C502E1D2},    //★Tau1346
        {1337, 0x527BC505C502E385},    //★Tau1373
        {1338, 0x52784605C4FFD5E1},    //★Tau1412
        {1339, 0x52784505C4FFD42E},    //★CMa2491
        {1340, 0x52784405C4FFD27B},    //★CMa2693
        {1341, 0x52784305C4FFD0C8},    //★CMa2294
        {1342, 0x52784A05C4FFDCAD},    //★CMa2827
        {1343, 0x52784905C4FFDAFA},    //★CMa2282
        {1344, 0x52784805C4FFD947},    //★CMa2618
        {1345, 0x52784705C4FFD794},    //★CMa2657
        {1346, 0x52783E05C4FFC849},    //★CMa2646
        {1347, 0x52783D05C4FFC696},    //★UMa4905
        {1348, 0x5274C005C4FCBC58},    //★UMa4301
        {1349, 0x5274C105C4FCBE0B},    //★UMa5191
        {1350, 0x5274C205C4FCBFBE},    //★UMa5054
        {1351, 0x5274C305C4FCC171},    //★UMa4295
        {1352, 0x5274C405C4FCC324},    //★UMa4660
        {1353, 0x5274C505C4FCC4D7},    //★UMa4554
        {1354, 0x5274C605C4FCC68A},    //★UMa4069
        {1355, 0x5274C705C4FCC83D},    //★UMa3569
        {1356, 0x5274B805C4FCAEC0},    //★UMa3323
        {1357, 0x5274B905C4FCB073},    //★UMa4033
        {1358, 0x5271BA05C4FA7C4F},    //★UMa4377
        {1359, 0x5271B905C4FA7A9C},    //★UMa4375
        {1360, 0x5271BC05C4FA7FB5},    //★UMa4518
        {1361, 0x5271BB05C4FA7E02},    //★UMa3594
        {1362, 0x5271B605C4FA7583},    //★Vir5056
        {1363, 0x5271B505C4FA73D0},    //★Vir4825
        {1364, 0x5271B805C4FA78E9},    //★Vir4932
        {1365, 0x5271B705C4FA7736},    //★Vir4540
        {1366, 0x5271C205C4FA89E7},    //★Vir4689
        {1367, 0x5271C105C4FA8834},    //★Vir5338
        {1368, 0x526E3405C4F762C6},    //★Vir4910
        {1369, 0x526E3505C4F76479},    //★Vir5315
        {1370, 0x526E3205C4F75F60},    //★Vir5359
        {1371, 0x526E3305C4F76113},    //★Vir5409
        {1372, 0x526E3805C4F76992},    //★Vir5107
        {1373, 0x526E3905C4F76B45},    //★Ari617
        {1374, 0x526E3605C4F7662C},    //★Ari553
        {1375, 0x526E3705C4F767DF},    //★Ari546
        {1376, 0x526E3C05C4F7705E},    //★Ari951
        {1377, 0x526E3D05C4F77211},    //★Ori1713
        {1378, 0x49768F05BFCCFB6C},    //★Ori2061
        {1379, 0x49769005BFCCFD1F},    //★Ori1790
        {1380, 0x49769105BFCCFED2},    //★Ori1903
        {1381, 0x49769205BFCD0085},    //★Ori1948
        {1382, 0x49768B05BFCCF4A0},    //★Ori2004
        {1383, 0x49768C05BFCCF653},    //★Ori1852
        {1384, 0x49768D05BFCCF806},    //★Ori1879
        {1385, 0x49768E05BFCCF9B9},    //★Ori1899
        {1386, 0x49769705BFCD0904},    //★Ori1543
        {1387, 0x49769805BFCD0AB7},    //★Cas21
        {1388, 0x497A1505BFD014F5},    //★Cas168
        {1389, 0x497A1405BFD01342},    //★Cas403
        {1390, 0x497A1305BFD0118F},    //★Cas153
        {1391, 0x497A1205BFD00FDC},    //★Cas542
        {1392, 0x497A1105BFD00E29},    //★Cas219
        {1393, 0x497A1005BFD00C76},    //★Cas265
        {1394, 0x497A0F05BFD00AC3},    //★Cnc3572
        {1395, 0x497A0E05BFD00910},    //★Cnc3208
        {1396, 0x497A1D05BFD0228D},    //★Cnc3461
        {1397, 0x497A1C05BFD020DA},    //★Cnc3449
        {1398, 0x497D1B05BFD254FE},    //★Cnc3429
        {1399, 0x497D1C05BFD256B1},    //★Cnc3627
        {1400, 0x497D1905BFD25198},    //★Cnc3268
        {1401, 0x497D1A05BFD2534B},    //★Cnc3249
        {1402, 0x497D1F05BFD25BCA},    //★Com4968
        {1403, 0x497D2005BFD25D7D},    //★Crv4757
        {1404, 0x497D1D05BFD25864},    //★Crv4623
        {1405, 0x497D1E05BFD25A17},    //★Crv4662
        {1406, 0x497D1305BFD24766},    //★Crv4786
        {1407, 0x497D1405BFD24919},    //★Aur1708
        {1408, 0x4980A105BFD56E87},    //★Aur2088
        {1409, 0x4980A005BFD56CD4},    //★Aur1605
        {1410, 0x4980A305BFD571ED},    //★Aur2095
        {1411, 0x4980A205BFD5703A},    //★Aur1577
        {1412, 0x49809D05BFD567BB},    //★Aur1641
        {1413, 0x49809C05BFD56608},    //★Aur1612
        {1414, 0x49809F05BFD56B21},    //★Pav7790
        {1415, 0x49809E05BFD5696E},    //★Cet911
        {1416, 0x49809905BFD560EF},    //★Cet681
        {1417, 0x49809805BFD55F3C},    //★Cet188
        {1418, 0x49691705BFC1A528},    //★Cet539
        {1419, 0x49691805BFC1A6DB},    //★Cet804
        {1420, 0x49691905BFC1A88E},    //★Cep8974
        {1421, 0x49691A05BFC1AA41},    //★Cep8162
        {1422, 0x49691B05BFC1ABF4},    //★Cep8238
        {1423, 0x49691C05BFC1ADA7},    //★Cep8417
        {1424, 0x49691D05BFC1AF5A},    //★Cen5267
        {1425, 0x49691E05BFC1B10D},    //★Cen5288
        {1426, 0x49690F05BFC19790},    //★Cen551
        {1427, 0x49691005BFC19943},    //★Cen5459
        {1428, 0x496C1D05BFC3E531},    //★Cen5460
        {1429, 0x496C1C05BFC3E37E},    //★CMi2943
        {1430, 0x496C1B05BFC3E1CB},    //★CMi2845
        {1431, 0x496C1A05BFC3E018},    //★Equ8131
        {1432, 0x496C2105BFC3EBFD},    //★Vul7405
        {1433, 0x496C2005BFC3EA4A},    //★UMi424
        {1434, 0x496C1F05BFC3E897},    //★UMi5563
        {1435, 0x496C1E05BFC3E6E4},    //★UMi5735
        {1436, 0x496C1505BFC3D799},    //★UMi6789
        {1437, 0x496C1405BFC3D5E6},    //★Crt4287
        {1438, 0x496FA305BFC6FEBA},    //★Lyr7001
        {1439, 0x496FA405BFC7006D},    //★Lyr7178
        {1440, 0x496FA105BFC6FB54},    //★Lyr7106
        {1441, 0x496FA205BFC6FD07},    //★Lyr7298
        {1442, 0x496F9F05BFC6F7EE},    //★Ara6585
        {1443, 0x496FA005BFC6F9A1},    //★Sco6134
        {1444, 0x496F9D05BFC6F488},    //★Sco6527
        {1445, 0x496F9E05BFC6F63B},    //★Sco6553
        {1446, 0x496F9B05BFC6F122},    //★Sco5953
        {1447, 0x496F9C05BFC6F2D5},    //★Sco5984
        {1448, 0x49730905BFC9E1E3},    //★Sco6508
        {1449, 0x49730805BFC9E030},    //★Sco6084
        {1450, 0x49730B05BFC9E549},    //★Sco5944
        {1451, 0x49730A05BFC9E396},    //★Sco6630
        {1452, 0x49730D05BFC9E8AF},    //★Sco6027
        {1453, 0x49730C05BFC9E6FC},    //★Sco6247
        {1454, 0x49730F05BFC9EC15},    //★Sco6252
        {1455, 0x49730E05BFC9EA62},    //★Sco5928
        {1456, 0x49731105BFC9EF7B},    //★Sco6241
        {1457, 0x49731005BFC9EDC8},    //★Sco6165
        {1458, 0x495B1F05BFB57564},    //★Tri544
        {1459, 0x495B2005BFB57717},    //★Leo3982
        {1460, 0x495B2105BFB578CA},    //★Leo4534
        {1461, 0x495B2205BFB57A7D},    //★Leo4357
        {1462, 0x495B1B05BFB56E98},    //★Leo4057
        {1463, 0x495B1C05BFB5704B},    //★Leo4359
        {1464, 0x495B1D05BFB571FE},    //★Leo4031
        {1465, 0x495B1E05BFB573B1},    //★Leo3852
        {1466, 0x495B1705BFB567CC},    //★Leo3905
        {1467, 0x495B1805BFB5697F},    //★Leo3773
        {1468, 0x495EA505BFB88EED},    //★Gru8425
        {1469, 0x495EA405BFB88D3A},    //★Gru8636
        {1470, 0x495EA305BFB88B87},    //★Gru8353
        {1471, 0x495EA205BFB889D4},    //★Lib5685
        {1472, 0x495EA105BFB88821},    //★Lib5531
        {1473, 0x495EA005BFB8866E},    //★Lib5787
        {1474, 0x495E9F05BFB884BB},    //★Lib5603
        {1475, 0x495E9E05BFB88308},    //★Pup3165
        {1476, 0x495E9D05BFB88155},    //★Pup3185
        {1477, 0x495E9C05BFB87FA2},    //★Pup3045
        {1478, 0x644CA005CF3DF80D},    //★Cyg7924
        {1479, 0x644C9F05CF3DF65A},    //★Cyg7417
        {1480, 0x644C9E05CF3DF4A7},    //★Cyg7796
        {1481, 0x644C9D05CF3DF2F4},    //★Cyg8301
        {1482, 0x644C9C05CF3DF141},    //★Cyg7949
        {1483, 0x644C9B05CF3DEF8E},    //★Cyg7528
        {1484, 0x644C9A05CF3DEDDB},    //★Oct7228
        {1485, 0x644C9905CF3DEC28},    //★Col1956
        {1486, 0x644C9805CF3DEA75},    //★Col2040
        {1487, 0x644C9705CF3DE8C2},    //★Col2177
        {1488, 0x64491A05CF3ADE84},    //★Gem2990
        {1489, 0x64491B05CF3AE037},    //★Gem2891
        {1490, 0x64491C05CF3AE1EA},    //★Gem2421
        {1491, 0x64491D05CF3AE39D},    //★Gem2473
        {1492, 0x64491605CF3AD7B8},    //★Gem2216
        {1493, 0x64491705CF3AD96B},    //★Gem2777
        {1494, 0x64491805CF3ADB1E},    //★Gem2650
        {1495, 0x64491905CF3ADCD1},    //★Gem2286
        {1496, 0x64491205CF3AD0EC},    //★Gem2484
        {1497, 0x64491305CF3AD29F},    //★Gem2930
        {1498, 0x64530C05CF431B3F},    //★Peg8775
        {1499, 0x64530B05CF43198C},    //★Peg8781
        {1500, 0x64530E05CF431EA5},    //★Peg39
        {1501, 0x64530D05CF431CF2},    //★Peg8308
        {1502, 0x64530805CF431473},    //★Peg8650
        {1503, 0x64530705CF4312C0},    //★Peg8634
        {1504, 0x64530A05CF4317D9},    //★Peg8684
        {1505, 0x64530905CF431626},    //★Peg8450
        {1506, 0x64531405CF4328D7},    //★Peg8880
        {1507, 0x64531305CF432724},    //★Peg8905
        {1508, 0x64500605CF40DB36},    //★Oph6556
        {1509, 0x64500705CF40DCE9},    //★Oph6378
        {1510, 0x64500405CF40D7D0},    //★Oph6603
        {1511, 0x64500505CF40D983},    //★Oph6149
        {1512, 0x64500A05CF40E202},    //★Oph6056
        {1513, 0x64500B05CF40E3B5},    //★Oph6075
        {1514, 0x64500805CF40DE9C},    //★Ser5854
        {1515, 0x64500905CF40E04F},    //★Ser7141
        {1516, 0x64500E05CF40E8CE},    //★Ser5879
        {1517, 0x64500F05CF40EA81},    //★Her6406
        {1518, 0x643F0805CF326B69},    //★Her6148
        {1519, 0x643F0705CF3269B6},    //★Her6410
        {1520, 0x643F0605CF326803},    //★Her6526
        {1521, 0x643F0505CF326650},    //★Her6117
        {1522, 0x643F0C05CF327235},    //★Her6008
        {1523, 0x643F0B05CF327082},    //★Per936
        {1524, 0x643F0A05CF326ECF},    //★Per1017
        {1525, 0x643F0905CF326D1C},    //★Per1131
        {1526, 0x643F1005CF327901},    //★Per1228
        {1527, 0x643F0F05CF32774E},    //★Per834
        {1528, 0x643B8205CF2F51E0},    //★Per941
        {1529, 0x643B8305CF2F5393},    //★Phe99
        {1530, 0x643B8405CF2F5546},    //★Phe338
        {1531, 0x643B8505CF2F56F9},    //★Vel3634
        {1532, 0x643B8605CF2F58AC},    //★Vel3485
        {1533, 0x643B8705CF2F5A5F},    //★Vel3734
        {1534, 0x643B8805CF2F5C12},    //★Aqr8232
        {1535, 0x643B8905CF2F5DC5},    //★Aqr8414
        {1536, 0x643B8A05CF2F5F78},    //★Aqr8709
        {1537, 0x643B8B05CF2F612B},    //★Aqr8518
        {1538, 0x64459405CF37C4FB},    //★Aqr7950
        {1539, 0x64459305CF37C348},    //★Aqr8499
        {1540, 0x64459605CF37C861},    //★Aqr8610
        {1541, 0x64459505CF37C6AE},    //★Aqr8264
        {1542, 0x64459805CF37CBC7},    //★Cru4853
        {1543, 0x64459705CF37CA14},    //★Cru4730
        {1544, 0x64459A05CF37CF2D},    //★Cru4763
        {1545, 0x64459905CF37CD7A},    //★Cru4700
        {1546, 0x64458C05CF37B763},    //★Cru4656
        {1547, 0x64458B05CF37B5B0},    //★PsA8728
        {1548, 0x64420E05CF34AB72},    //★TrA6217
        {1549, 0x64420F05CF34AD25},    //★Cap7776
        {1550, 0x64420C05CF34A80C},    //★Cap7754
        {1551, 0x64420D05CF34A9BF},    //★Cap8278
        {1552, 0x64420A05CF34A4A6},    //★Cap8322
        {1553, 0x64420B05CF34A659},    //★Cap7773
        {1554, 0x64420805CF34A140},    //★Sge7479
        {1555, 0x64420905CF34A2F3},    //★Car2326
        {1556, 0x64421605CF34B90A},    //★Car3685
        {1557, 0x64421705CF34BABD},    //★Car3307
        {1558, 0x64679005CF54A495},    //★Car3699
        {1559, 0x64678F05CF54A2E2},    //★Dra5744
        {1560, 0x64678E05CF54A12F},    //★Dra5291
        {1561, 0x64678D05CF549F7C},    //★Dra6705
        {1562, 0x64678C05CF549DC9},    //★Dra6536
        {1563, 0x64678B05CF549C16},    //★Dra7310
        {1564, 0x64678A05CF549A63},    //★Dra6688
        {1565, 0x64678905CF5498B0},    //★Dra4434
        {1566, 0x64679805CF54B22D},    //★Dra6370
        {1567, 0x64679705CF54B07A},    //★Dra7462
        {1568, 0x64640A05CF518B0C},    //★Dra6396
        {1569, 0x64640B05CF518CBF},    //★Dra6132
        {1570, 0x64640C05CF518E72},    //★Dra6636
        {1571, 0x64640D05CF519025},    //★CVn4915
        {1572, 0x64640605CF518440},    //★CVn4785
        {1573, 0x64640705CF5185F3},    //★CVn4846
        {1574, 0x64640805CF5187A6},    //★Aql7595
        {1575, 0x64640905CF518959},    //★Aql7557
        {1576, 0x64641205CF5198A4},    //★Aql7525
        {1577, 0x64641305CF519A57},    //★Aql7602
        {1578, 0x5B366505C9F99442},    //★Aql7235
        {1579, 0x3F998C790290B0E2},    //Max Honey
        {1580, 0x3796FEC405E69A7E},    //Max Mushrooms
        {1581, 0xF7E614282C1D4AC1},    //Galarica Twig
        {1582, 0xBAE2A03193D0817C},    //Galarica Cuff
        {1583, 0x8137FF85A79CA290},    //Style Card
        {1584, 0x8C6B4030724A9EE0},    //Armor Pass
        {1585, 0xAABFDC7155F6DC3E},    //Rotom Bike (3)
        {1586, 0xEDE580B158C4AF5A},    //Rotom Bike (4)
        {1587, 0xDF9B18C58E6F0EAE},    //Exp. Charm
        {1588, 0x037AB9848F426E37},    //Armorite Ore
        {1589, 0x990FC8A1D406275D},    //Mark Charm
        {1590, 0x810EDF44AD9F2AD7},    //Reins of Unity (1)
        {1591, 0xDE667A0323705AAB},    //Reins of Unity (2)
        {1592, 0x1C3DBB55BA05DC84},    //Galarica Wreath
        {1593, 0x5CD706FB669CDB84},    //Legendary Clue 1
        {1594, 0x5CD709FB669CE09D},    //Legendary Clue 2
        {1595, 0x5CD708FB669CDEEA},    //Legendary Clue 3
        {1596, 0xD355CFAADA929868},    //Legendary Clue?
        {1597, 0x3B30E7EA90C28B3F},    //Crown Pass
        {1598, 0x2517847006D43D4D},    //Wooden Crown
        {1599, 0x55EF2DF7F7A17F0F},    //Radiant Petal
        {1600, 0x9D50CB22F3E54325},    //White Mane Hair
        {1601, 0xA7F40665118EE8B9},    //Black Mane Hair
        {1602, 0x919C07B41D9D7ECA},    //Iceroot Carrot
        {1603, 0x61A5067644DCF72D},    //Shaderoot Carrot
        {1604, 0x5863F61E4B7865F7},    //Dynite Ore
        {1605, 0xE4ADE1A19BC426E8},    //Carrot Seeds
        {1606, 0x65BAB6FC45D8E0B9},    //Ability Patch
        {1607, 0xD002DE38AF69DDC0}    //Reins of Unity (3)
    };
}