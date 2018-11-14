namespace pkNX.Structures
{
    public enum Shiny : byte
    {
        /// <summary>
        /// PID is purely random; can be shiny or not shiny.
        /// </summary>
        Random = 0,

        /// <summary>
        /// PID is randomly created and forced to be shiny.
        /// </summary>
        Always = 1,

        /// <summary>
        /// PID is randomly created and forced to be not shiny.
        /// </summary>
        Never = 2,
    }
}