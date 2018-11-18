namespace pkNX.Structures
{
    public interface IPokeData
    {
        int Species { get; set; }
        int Level { get; set; }
        int Nature { get; set; }
        int Form { get; set; }
        int HeldItem { get; set; }
        int Gender { get; set; }
        int Ability { get; set; }

        int IV_HP { get; set; }
        int IV_ATK { get; set; }
        int IV_DEF { get; set; }
        int IV_SPA { get; set; }
        int IV_SPD { get; set; }
        int IV_SPE { get; set; }

        int EV_HP { get; set; }
        int EV_ATK { get; set; }
        int EV_DEF { get; set; }
        int EV_SPA { get; set; }
        int EV_SPD { get; set; }
        int EV_SPE { get; set; }
    }
}