using System;

namespace pkNX.Structures
{
    /// <summary>
    /// Feature flags for the <see cref="AmxHeader"/>
    /// </summary>
    /// <remarks>
    /// Flag version listed here is for CUR_FILE_VERSION = 10, circa 2008
    /// </remarks>
    [Flags]
    public enum AmxFlags : ushort
    {
        NONE,

        /// <summary> All function calls use overlays </summary>
        OVERLAY = 0x01,

        /// <summary> Symbolic info is available </summary>
        DEBUG = 0x02,

        /// <summary> Compact encoding </summary>
        COMPACT = 0x04,

        /// <summary> Script uses the sleep instruction (possible re-entry or power-down mode) </summary>
        SLEEP = 0x08,

        /// <summary> No array bounds checking; no BREAK opcodes </summary>
        NOCHECKS = 0x10,

        /// <summary> Data section is explicitly initialized </summary>
        DSEG_INIT = 0x20,

        /// <summary> Script new (optimized) version of SYSREQ opcode </summary>
        SYSREQN = 0x800,

        /// <summary> All native functions are registered </summary>
        NTVREG = 0x1000,

        /// <summary> Abstract machine is JIT compiled </summary>
        JITC = 0x2000,

        /// <summary> Busy verifying P-code </summary>
        VERIFY = 0x4000,

        /// <summary> AMX has been initialized </summary>
        INIT = 0x8000
    }

    public static class AmxFlagsExtensions
    {
        public static bool HasFlagFast(this AmxFlags value, AmxFlags flag)
        {
            return (value & flag) != 0;
        }
    }
}