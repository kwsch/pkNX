using System.Linq;

namespace pkNX.Structures
{
    public static partial class Legal
    {
        public static readonly int[] BasicStarters_1 =
        {
            001, 004, 007, 010, 013, 016, 029, 032, 041, 043, 060, 063, 066, 069, 074, 081, 092, 111, 116, 137, 147,
        };

        public static readonly int[] BasicStarters_6 = BasicStarters_1.Concat(new[]
        {
            152, 155, 158, 172, 173, 174, 175, 179, 187, 220, 239, 240, 246, 252, 255, 258, 265, 270, 273, 280, 287,
            293, 298, 304, 328, 355, 363, 371, 374, 387, 390, 393, 396, 403, 406, 440, 443, 495, 498, 501, 506, 519,
            607, 610, 633, 650, 653, 656, 661, 664, 669, 679, 704,
        }).ToArray();

        public static readonly int[] BasicStarters_7 = BasicStarters_6.Concat(new[]
        {
            722, 725, 728, 731, 736, 761, 782, 789,
        }).ToArray();
    }
}