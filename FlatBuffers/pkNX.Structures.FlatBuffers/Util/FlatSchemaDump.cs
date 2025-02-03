using System.Reflection;
using FlatSharp.Attributes;

namespace pkNX.Structures.FlatBuffers;

/// <summary>
/// Note: does not support creating schemas for fixed size inline struct arrays.
/// </summary>
public class FlatSchemaDump
{
    public readonly List<string> GeneratedSchemas = [];
    private readonly List<string> GeneratedClasses = [];

    public FlatSchemaDump(object obj) => Recurse(obj.GetType());

    public string GetSingleFileSchema(Type type) =>
        $"""
         namespace {type.Namespace};
         {string.Join(Environment.NewLine + Environment.NewLine, GeneratedSchemas)}
         root_type {GetName(type)};
         """;

    private void Recurse(Type t)
    {
        var type = GetType(t);
        if (type is { IsValueType: true, IsEnum: false } || type == typeof(string))
            return;
        var name = GetName(type);
        if (GeneratedClasses.Contains(name))
            return;

        if (type.IsEnum)
            AddEnum(type, name);
        else if (type.IsGenericType)
            AddGeneric(type, name);
        else
            AddTable(type, name);
    }

    private void AddTable(Type type, string name)
    {
        var props = type.GetTypeInfo().DeclaredProperties.ToArray();
        var lines = props.Select(GetPropLine);

        var defType = type.GetTypeInfo().GetCustomAttribute<FlatBufferStructAttribute>() != null ? "struct" : "table";
        var schema =
            $$"""
              {{defType}} {{name}} {
                {{string.Join(Environment.NewLine + "  ", lines)}}
              }
              """;

        GeneratedClasses.Add(name);
        GeneratedSchemas.Add(schema);

        foreach (var p in props)
            Recurse(p.PropertyType);
    }

    private void AddGeneric(Type type, string name)
    {
        // Create a union schema first, then execute the inner types.
        var types = type.GenericTypeArguments;
        var names = types.Select(z => GetName(GetType(z)));
        var schema = $"union {name} {{ {string.Join(", ", names)} }}";
        GeneratedClasses.Add(name);
        GeneratedSchemas.Add(schema);

        foreach (var t in types)
            Recurse(t);
    }

    private void AddEnum(Type type, string name)
    {
        var underlying = type.GetEnumUnderlyingType();
        var underlyingName = Aliases[underlying];
        var kvps = GetEnumMembers(type);
        var schema = $$"""
                       enum {{GetName(type)}} : {{underlyingName}} {
                         {{string.Join(Environment.NewLine + "  ", kvps)}}
                       }
                       """;
        GeneratedClasses.Add(name);
        GeneratedSchemas.Add(schema);
    }

    private static IEnumerable<string> GetEnumMembers(Type type)
    {
        var names = type.GetEnumNames();
        var values = type.GetEnumValues(); // not index-able, for shame. need to iterate
        int ctr = 0;
        foreach (var v in values)
        {
            var name = names[ctr++];
            var value = Convert.ChangeType(v, Type.GetTypeCode(type));
            yield return $"{name} = {value},";
        }
    }

    private static Type GetType(Type t)
    {
        if (t.IsArray)
            t = t.GetElementType() ?? throw new NullReferenceException("Array type should not be null.");
        if (t.Namespace == "Generated")
            return t.BaseType ?? throw new NullReferenceException("Base type should not be null.");
        return t;
    }

    private static string GetName(Type t)
    {
        var name = t.Name;
        if (!name.Contains('`'))
            return name;
        // generated class names get normalized
        if (!t.IsGenericType)
            return t.Name;

        var types = t.GenericTypeArguments;
        var names = types.Select(z => GetName(GetType(z)));
        var typeConcat = string.Concat(names);
        return t.Name.Replace("`", "") + typeConcat;
    }

    private static string GetPropLine(PropertyInfo p)
    {
        var name = p.Name;
        var type = p.PropertyType;
        bool array = type.IsArray;

        var realType = GetType(type);
        if (!Aliases.TryGetValue(realType, out var tn))
            tn = GetName(realType);
        if (array)
            tn = $"[{tn}]";
        return $"{name}:{tn};";
    }

    private static readonly Dictionary<Type, string> Aliases = new()
    {
        { typeof(byte), "ubyte" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(bool), "bool" },
        { typeof(string), "string" },
    };
}
