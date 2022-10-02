namespace pkNX.Structures;
#pragma warning disable CA1027 // Mark enums with FlagsAttribute
public enum AmxError
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
{
    AMX_ERR_NONE,
    /* reserve the first 15 error codes for exit codes of the abstract machine */
    EXIT,         /* forced exit */
    ASSERT,       /* assertion failed */
    STACKERR,     /* stack/heap collision */
    BOUNDS,       /* index out of bounds */
    MEMACCESS,    /* invalid memory access */
    INVINSTR,     /* invalid instruction */
    STACKLOW,     /* stack underflow */
    HEAPLOW,      /* heap underflow */
    CALLBACK,     /* no callback, or invalid callback */
    NATIVE,       /* native function failed */
    DIVIDE,       /* divide by zero */
    SLEEP,        /* go into sleepmode - code can be restarted */
    INVSTATE,     /* no implementation for this state, no fall-back */

    MEMORY = 16,  /* out of memory */
    FORMAT,       /* invalid file format */
    VERSION,      /* file is for a newer version of the AMX */
    NOTFOUND,     /* function not found */
    INDEX,        /* invalid index parameter (bad entry point) */
    DEBUG,        /* debugger cannot run */
    INIT,         /* AMX not initialized (or doubly initialized) */
    USERDATA,     /* unable to set user data field (table full) */
    INIT_JIT,     /* cannot initialize the JIT */
    PARAMS,       /* parameter error */
    DOMAIN,       /* domain error, expression result does not fit in range */
    GENERAL,      /* general error (unknown or unspecific error) */
    OVERLAY,      /* overlays are unsupported (JIT) or uninitialized */
}
