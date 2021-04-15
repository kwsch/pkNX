namespace pkNX.Structures
{
    public enum Shiny : byte
    {
        /// <summary>
        /// PID is purely random; will always be shiny.
        /// </summary>
        Random = 0,

        /// <summary>
        /// PID is randomly created and forced to be shiny.
        /// </summary>
        Always = 1,

        /// <summary>
        /// PID is randomly created and forced to always be shiny.
        /// </summary>
        Never = 2,
    }
}
