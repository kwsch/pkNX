namespace pkNX.Structures;

public enum VariableType : uint
{
    Normal,
    Reference,
    Array,
    ArrayReference,
    Variadic
}

public static class VariableTypeExtensions
{
    private const byte IDENT_VARIABLE = 1;
    private const byte IDENT_REFERENCE = 2;
    private const byte IDENT_ARRAY = 3;
    private const byte IDENT_REFARRAY = 4;
    //private const byte IDENT_FUNCTION = 9;
    private const byte IDENT_VARARGS = 11;

    public static VariableType FromIdent(this byte ident)
    {
        return ident switch
        {
            IDENT_VARIABLE => VariableType.Normal,
            IDENT_REFERENCE => VariableType.Reference,
            IDENT_ARRAY => VariableType.Array,
            IDENT_REFARRAY => VariableType.ArrayReference,
            IDENT_VARARGS => VariableType.Variadic,
            _ => VariableType.Normal
        };
    }
}
