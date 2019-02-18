namespace pkNX.Structures
{
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
        private const byte IDENT_FUNCTION = 9;
        private const byte IDENT_VARARGS = 11;

        public static VariableType FromIdent(this byte ident)
        {
            switch (ident)
            {
                case IDENT_VARIABLE:
                    return VariableType.Normal;
                case IDENT_REFERENCE:
                    return VariableType.Reference;
                case IDENT_ARRAY:
                    return VariableType.Array;
                case IDENT_REFARRAY:
                    return VariableType.ArrayReference;
                case IDENT_VARARGS:
                    return VariableType.Variadic;
                default:
                    return VariableType.Normal;
            }
        }
    }
}