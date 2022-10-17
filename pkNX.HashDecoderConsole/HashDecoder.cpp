#include "pch.h"

#include "HashDecoder.h"
#include "Timer.h"
#include <unordered_set>

using namespace pkNXHashDecoder;

std::string FnvHashDecoder::BuildString(const size_t length) const
{
    // Create string from chars
    std::vector<char> chars(length);
    for (int j = 0; j < length; ++j)
        chars[j] = AllowedChars[charIndex[j]];

    return { chars.begin(), chars.end() };
}

std::unordered_map<uint64_t, std::string> FnvHashDecoder::StartDecoding(const int length, const int startChar, const std::string_view prefix)
{
    std::unordered_map<uint64_t, std::string> foundKeys;

    std::unordered_set<uint64_t> HashesToLookFor{
        0xC75DDBE25402B11C,
        0x4202065E4900E5E6,
        0x90C6018B1C83C999,
        0xA5EC1270693E5554,
        0xAA24EE0FC9930161,
        0x738D45B3FCCD8683,
        0x7874900CA52EAAC4,
        0xC5B7CDC56F890702,
        0xC672CDBC18D185EE,
        0x295E0F613E5B2E99,
        0xA03A90A554738B04,
        0xFEC35D67DB8FB3F7,
        0x74B4B2C7F9D82197,
        0x329D6D72A5EE893E,
        0x3B24B21F94D759CC,
        0x0E8AF24E711420B5,
        0x3C3D26BA3C4D928B,
        0xBD9D0E1275E19DC3,
        0x780CF5544A1BA14E,
        0x3994B8E5146DF87A,
        0x3BFE00C60AAA4629,
        0xE2FD1EBB1CFC547D,
        0x8F5F9697B53FA69A,
        0x310A3E4E64072A45,
        0xE165FD88E9D8F4E8,
        0xDD8D4287CD69B2B1,
        0x0ADC40FE7471C28F,
        0xAE26E44B48B9C1A0,
        0x39DA6B4E0445AF7C,
        0x2FAF26C61D6B8371,
        0xAF552936A4E91A20,
        0x893A2307830B3947,
        0xD259B1492CBB69EE,
        0x0C7783C2EAD1F11F,
        0x4066F0333032E760,
        0x69824E27FD3241CD,
        0xF44EA11B1C09F9E6,
        0x9AE6B7DA086DF529,
        0xE7A76BB22F55DCCD,
        0x9AEBB2EBE8A0E277,
        0xFE46ADD5A3C9E59D,
        0xA9240BDFF13535FF,
        0xF0CC2D7DBA68D605,
        0xF76AB6EFC7B2EF31,
        0xB06192701C4CD0BF,
        0xA253E743C3DFFCFA,
        0xAA8FCBBCAF4CC477,
        0x1C971DC264EE265C,
        0x4ADFF188FA2CC2BF,
        0xCC8C967D0984A782,
        0x5DFED77E3EF484E6,
        0xD11CA01879E2560E,
        0xF439879C2A32AC27,
        0x71A6B1803B68A0DF,
        0x1CCD92966FC276B2,
        0x4D7DD48708B9B178,
        0xEF917720395BFB4B,
        0x3D953062B5D32036,
        0x3FAAA2AB2B669F0F,
        0x6684C6B1DE87286A,
        0xF7D3DEDBA87A5A7B,
        0x56BFDBA5BF372818,
        0x37E6296E94F84FC2,
        0x15BD8A0A55DF6F9B,
        0xC2B88644957DF334,
        0xCE684213D609E4D0,
        0x89D4AB3B91E4193E,
        0xB0E870D70AF6C856,
        0x4FB5FDD592E5C19C,
        0x884CAE4F264E7C53,
        0x2F3677ACAD0B264F,
        0xFFCFC4B53B6CEB73,
        0x55E136321BD4820B,
        0x69C60410B32CFA7C,
        0x56EBF2B961D8C2F0,
        0xBD638443042BA775,
        0x04EFF829AE5A9CAE,
        0xB924FA5E88CBF499,
        0x983CBC0A01DBCE18,
        0x0789E7AD3B62AE98,
        0x11A4B28C3399FCAE,
        0xB23DD104F7E3E7B4,
        0xAC41D5A69B91BDAB,
        0x20E1A4325B9B726C,
        0xBF886D72A5EB4BFD,
        0x9602FE3839CA4144,
        0x7EB49AC6E76098FF,
        0x73DE6E93E4244BA6,
        0x0B6DBBC6F0E94019,
        0xF9B45DF47EC0E120,
        0x313CF262DB0926BB,
        0xCEBA0A3E967DCE42,
        0xB6F99FCA4E14B215,
        0x088F9B28A225E77C,
        0x0614F4AB758CA4EC,
        0x0C0EABF9C83A6645,
        0x276F6051571BDC6E,
        0xAA3B9A7B0AC22E07,
        0x1A82687E9C21FC88,
        0x2011F8FC2EAF5BC1,
        0xE7B2049B04BC0D8A,
        0x4C385FB04D7D4E83,
        0xECF48E67086948B4,
        0xC27F8EE33D371BAD,
        0x35B1F3EAFB73E6BD,
        0x0649E4802BBCA904,
        0x4759AB9110BC0EBF,
        0xF93D4C6B30D60166,
        0x1C75AA69FDB3BAD9,
        0x2AA9B9D0303F75E0,
        0xF32E56B416E23C7B,
        0x522BF06C1BD0F102,
        0xC4DB525F8E431D41,
        0x18223B561661AFAA,
        0x27198363BE073D43,
        0xC3AA40619506B41E,
        0x5C57D1B11EF558EA,
        0xAE6050F3E9BEADF7,
        0x4DC996846E32E10F,
        0x64EA03C28BB0A824,
        0x3A685B964260E751,
        0xD566A415A1D96AC6,
        0xE0C3B017F309DB7F,
        0xFE62D6A9E1C90FA0,
        0x6EB817B2E5CE41A1,
        0xC513F3A8B387CE60,
        0x00FFEC5427C03217,
        0x263AAFAD40F6F534,
        0x557EA5E216D1EB04,
        0x5B10EEB3C42F6EF3,
        0xA4B557C69DC53B4C,
        0x171318BDE5AFB37F,
        0xF5117E70DB5C0CDD,
        0xB70B74B682AEDDC0,
        0x5A0C4392036A5A42,
        0x707D85F01E28F5F9,
        0x6E67F96382437D3E,
        0x754C295FB5F44D9B,
        0xC5F20F76E7A93719,
        0xA763E53AE978AB27,
        0xB6CBA708417AC6DC,
        0xF7D10E1E3D07DF2F,
        0xF4441F03DDB7443F,
        0x61424A6B6B33EF06,
        0x1335E7CF78B73FD4,
        0xA0553AA5DAAD1093,
        0x21CFEF4845D0131B,
        0x5DFEF42CC1CFDDDA,
        0xD3DA7E5EE9860CFC,
        0xBF2D97F2A6E9C72D,
        0x91C4588D028D31BC,
        0x733DF163257691E4,
        0xC16DA720E3FF43B1,
        0x7A99A59187C4FEB5,
        0xF75D819C39B27EC6,
        0x7F59A6B1A057AAEC,
        0xAE3C6A3413ACD75E,
        0x0E3F241026905B7F,
        0x57BA84BA67C80340,
        0x9E3D34369C67DB99,
        0xC416B116EAA6D876,
        0x5933EFC31901ADBF,
        0x9D068E51A9F2B451,
        0x66A9CB7F10A10C9B,
        0xE4860525BC865A83,
        0xB1BA5D4013E6B112,
        0x0737E620799C467D,
        0x1C0192447952EEF2,
        0x89D04AD6EC6542B5,
        0xDD7ECD313002CC84,
        0xE35A7D27BA3D0026,
        0x9491991D82A95AB9,
        0x7E3796C48490E2F4,
        0xA9BAEC93EB49A97B,
        0x5F4117769E95A4E2,
        0x27600C74B64ECBF1,
        0x8C77F0DA56F7DE7C,
        0xA1BB2DD9130A46F2,
        0xDD75C52F7283E9BB,
        0xC8F7D5BF4173CE93,
        0xE9E6C6B33718E60E,
        0xBF27AC1629E90381,
        0xF0F994239E2F346F,
        0x2D773D3EE1C014F6,
        0xF0F994239E2F346F,
        0x96C22933A5E58E3B,
        0x168A00F24E6405B7,
        0xFA5643FB989B3B1E,
        0x984D50F262AFCEAF,
        0x22F84F7815153DD6,
        0x7F6D9F484D4C0EA6,
        0x0C13D8D28EC907E5,
        0x9ECE3FC55775155F,
        0x93ABEE50A1E49467,
        0x7370532B23D76DAA,
        0x47AE1BFCD6B42B84,
        0x4AE56FF6E6334961,
        0xB509DB56AEE937C2,
        0xF4E140FB3B645905,
        0xA94E411B4AED0770,
        0x8F6AE63389246593,
        0x7974384D9B4C2FB6,
        0x14019D9941038DFD,
        0x6ABFCC37361003B4,
        0x4CDAC9407BF83AF7,
        0x4BB7814C7C192D19,
        0xF1127750E1886890,
        0x25964C22FD8497F7,
        0xAE27E66F62B5A72D,
        0xE597BC60EC178830,
        0x5AD9AD5A8D788501,
        0xDD355F94F2313486,
        0x6BF855FAE859F756,
        0x220FCB19BE6F4AD1,
        0x219A868593E0F143,
        0x8E22980132312750,
        0x2C54942E5AC186C5,
        0x4F33189725E4177E,
        0x3E9C477265BDCCFB,
        0x2CFB32D06BB0ABCE,
        0x36A980CFC8258668,
        0x3A220AFBFED99063,
        0x3D972A66844FA833,
        0xC917F1220D646DBE,
        0x3F6DE4B7DD492468,
        0x20D99D21DD492357,
        0xC460FB2533D4748D,
        0xCD7032496A7C365A,
        0x5C60998F55A4C354,
        0x313B485EBC2171CD,
        0x78ACE351AD05FBD4,
        0x9328F61FB61F13E5,
        0xD055895E765EEC2F,
        0x4199B6BDAAAD449E,
        0x032B6F5BC52B5EFD,
        0xF3D0F56A34A59780,
        0x0FD922D98261F028,
        0x19EE4EA3ADF0B902,
        0x584AF50A97D6E1D6,
        0xB03300749E45BB04,
        0xFB9DAD7C0A306304,
        0xB8497707B9AFFFC3,
        0xE7676C20D594CB1D,
        0x28C7077067B3CBC1,
        0x6F92EA1DC51CABCF,
        0x11BA4D9585AACE22,
        0xF894F0AAA6DF1348,
        0x0E7DBE6B422BF617,
        0x625850C05A022E48,
        0x2B17D38BA6A71C75,
        0x8FD0138AED004409,
        0x5FFAD59344D84D98,
        0xA0047EEB132ED1D6,
        0x906BBDF18FDA41D7,
        0x17D0C6D90344C370,
        0x079843AF139A4302,
        0x4A086453137B138D,
        0x1955D84E92121D4D,
        0x372A069678F1F45C,
        0x3E4BDFC71176F055,
        0x8F2F65B0686BDBC0,
        0x317C95E85C275D22,
        0x74DAFA4E821C0EDB,
        0xE614F33C0065709B,
        0xC181CFE9A52A6D56,
        0x945188E7CDB5341A,
        0x3A5895D1101F9037,
        0x30721017FA97EC02,
        0xA01FDF8F36D4C849,
        0x94D65F44E4DB603A,
        0x9E23D7B27A3550BA,
        0xC1C9454686BF4F77,
        0x980E30C4962AF568,
        0x2620F9FA5756901A,
        0xA329B2071F86D54D,
        0x98B23602AC81C150,
        0xA74E932CFE83EAE7,
        0xCEEB29248D1FA839,
        0x8D15EF082F42FC05,
        0x469278A74D72F144,
        0x2C2A4B83CF084B3B,
        0x34DDE3ADD16B4F75,
        0xB568913D0AC707CE,
        0xFA9958E9109B302F,
        0xA8ED6A8D845C7B48,
        0x42C7F372045E2459,
        0x42A006A7E230D188,
        0xC68BD7FDEAACED3A,
        0x9C12AB8FB6B90E68,
        0x5877A05E2FBA2709,
        0x386D73EBE6A7B8F4,
        0x306E81A939CB3993,
        0xC08438286952740C,
        0x5E5D71F3E0CBE9DC,
        0xDC018AA5878C8807,
        0x6DE0698BBCB2CA2C,
        0x7B36F7A58164D977,
        0x9E8D599701378583,
        0x6862791F06B78BDE,
        0xFBBC6B6C015DC941,
        0xB95C351BE7E9F834,
        0xD6EA55B5B5D9744F,
        0xC56571B25632522A,
        0x97CC9929EDDBD5F8,
        0x8C9946681252830B,
        0xA8EF99AAF7F5F7C5,
        0x106E7182195C260F,
        0xA055D9B805436A0C,
        0x206088562697898E,
        0xBFA4299E98FB33D9,
        0x23026AD6C36A3435,
        0x2E7B41BCE75826F6,
        0x97E7ED85C87A3DB6,
        0x4E0596A7E5646341,
        0x4535E33F5E564902,
        0x983A46CEAB2742D7,
        0x8E7C1BF15AA87E41,
        0x586CE849A27477A0,
        0xB7ECCDAE6F5707D2,
        0x38B4B39EC0768C67,
        0x7600DD72ED3B4920,
        0xBB368794840D45A5,
        0xFF87AC2753F1C9C6,
        0x4CA11DC3DAF364CB,
        0x0615848DE67F0FB4,
        0x6FEF6D44ECB59C69,
        0x7E7D8A31F8F21EEA,
        0x2F0E2FB68A052C5F,
        0xDC6E25257282CB55,
        0x40D7A8B92AF09D90,
        0x4B8A666460A38C97,
        0x25FABC2DDC931F82,
        0xB312234D6E8F40D9,
        0x7725EA3FCD8EFBE4,
        0x0FE7C88F0C511E3B,
        0x666B55C4B5063836,
        0xAF4985CD1D218AFC,
        0x4A4A7B20881294E4,
        0x84FE2D0C81A88918,
        0xF74BB7703FA523A3,
        0x0EBA8CE66319FEFA,
        0x865F5B29F7D3F6AF,
        0x4C497F2A0096EEFB,
        0x630448CC6C50E58A,
        0xC41B98740AB889EB,
        0xD1DDBC2BA3D9DE20,
        0xDABA16B3B282D08F,
        0xB006977E13C42A32,
        0x4984C3C7710B2AFC,
        0x9ECC7B26EA9E99A0,
        0x761DBBDEC4BAFC5E,
        0xC445D3C3B5C7F567,
        0x73CD7670DE50BF92,
        0x50042EF6FFC50F49,
        0x302B5F4800F44C2C,
        0xC5EA76327F79BF9B,
        0xF39C3111234AA504,
        0x4504FED3686326BA,
        0x109714EC91FC123D,
        0x92AE783A3DD4F467,
        0x26BD2AA2A097117C,
        0x86369F877D6A715A,
        0x00177EC637E0F547,
        0xA7370AC81DEE9DA3,
        0x016B2219F2C34655,
        0x62F9C9295910928D,
        0x5E18A362DB785C35,
        0x1BBFAF0587DD34D2,
        0x1F2BC2563CDEBE0A,
        0x5CB5954C8863A223,
        0x1A09CC57C175EF9B,
        0xE4AC9550E5F1457C,
        0x31B695D92696C535,
        0x5ABD56E662A3AF06,
        0x91202B07BCD8488F,
        0x906E97236B575388,
        0xDBE39C496C3FE76F,
        0x3A491A32B94D2E10,
        0x30215DFE4B009037,
        0x0414E6EA64920553,
        0x0A09CFACC06039F6,
        0x2BC88AEF207952D8,
        0xC198D435BB6CE1B3,
        0x5757780F91E52E20,
        0xABE2F65B74FBFCF0,
        0xDA85A46835474EC3,
        0xE84B34AB4E79AA10,
        0xF5C64BF800CA5BC9,
        0xC70206D6B252D40D,
        0x119437A2C563161A,
        0xB496C9BC67BD78CE,
        0xAF53D8D0F230E305,
        0x6916B336961C2FA0,
        0x709A1C8060864479,
        0x41ACC4325A7D85DE,
        0x119E0E76033FCD57,
        0x581F53B307DAB7BF,
        0xB480F619B5F3AE80,
        0xC5E1F48622CDF5FB,
        0x470C3CBEC51541FD,
        0x138F48FE9DB8EDF2,
        0x6BF4C2BBB3238803,
        0x8E3A34C6BD7D3D0D,

        0xE0F6613235D0AEEE,
        0xE0F9DF3235D3BADF,
        0xE0F6623235D0B0A1,
    };

    const size_t startIndex = prefix.length();

    // Reset all chars to starting point
    for (int i = 0; i < prefix.length(); ++i)
    {
        const auto& ch = prefix[i];
        charIndex[i] = AllowedChars.find_first_of(ch);
    }

    charIndex[startIndex] = (uint32_t)startChar;

    // Build the hash of all chars except the last one
    hashBases[0] = kOffsetBasis_64;
    for (int i = 0; i < length - 1; ++i)
    {
        hashBases[i] ^= AllowedChars[charIndex[i]];
        hashBases[i] *= kFnvPrime_64;

        // Move the hash to the next index as basis
        hashBases[i + 1] = hashBases[i];
    }

    //LoopTimer timer{};
    while (charIndex[startIndex] == startChar)
    {
        // Loop through all AllowedChars
        const uint64_t hashBase = hashBases[length - 1];
        for (int i = 0; i < AllowedCharsCount; ++i)
        {
            uint64_t hash = hashBase;
            hash ^= AllowedChars[i];
            hash *= kFnvPrime_64;

            if (HashesToLookFor.find(hash) != HashesToLookFor.end())
            {
                charIndex[length - 1] = i;
                foundKeys.emplace(hash, BuildString(length));
            }
        }

        // Already found all keys
        if (!foundKeys.empty())
            return foundKeys;

        bool carry = false;
        int c = length - 2;
        for (; c >= 0; c--)
        {
            ++charIndex[c];
            carry = charIndex[c] >= AllowedCharsCount;

            if (!carry)
                break;

            charIndex[c] = 0;
        }

        // Reached the end
        if (carry && c == 0)
            break;

        // Rebuild the hash basis
        hashBases[c] = hashBases[c - 1];
        for (int i = c; i < length - 1; ++i)
        {
            hashBases[i] ^= AllowedChars[charIndex[i]];
            hashBases[i] *= kFnvPrime_64;

            // Move the hash to the next index as basis
            hashBases[i + 1] = hashBases[i];

            // Automatically test for shorter length strings
            if (HashesToLookFor.find(hashBases[i]) != HashesToLookFor.end())
            {
                foundKeys.emplace(hashBases[i], BuildString(i + 1));
            }
        }

        /*if (c < length - 4)
            timer.Loop();

        if (c < length - 6)
            timer.Print();*/
    }

    return foundKeys;
}
