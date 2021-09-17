using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pkNX.Structures.FlatBuffers
{
    /// <summary>
    /// Note: does not support creating schemas for fixed size inline struct arrays.
    /// </summary>
    public class FlatSchemaDump
    {
        public readonly List<string> GeneratedSchemas = new();
        private readonly List<string> GeneratedClasses = new();

        public FlatSchemaDump(object obj) => Recurse(obj.GetType());

        public string GetSingleFileSchema(Type type) =>
$@"namespace {type.Namespace};
{string.Join(Environment.NewLine + Environment.NewLine, GeneratedSchemas)}
root_type {type.Name};";

        private void Recurse(Type t)
        {
            var type = GetType(t);
            if (type.IsValueType || type == typeof(string))
                return;
            var name = type.Name;
            if (GeneratedClasses.Contains(name))
                return;

            var props = type.GetTypeInfo().DeclaredProperties.ToArray();
            var lines = props.Select(GetPropLine);

            var schema =
@$"table {name} {{
  {string.Join(Environment.NewLine + "  ", lines)}
}}";

            GeneratedClasses.Add(name);
            GeneratedSchemas.Add(schema);

            foreach (var p in props)
                Recurse(p.PropertyType);
        }

        private static Type GetType(Type t)
        {
            if (t.IsArray)
                t = t.GetElementType() ?? throw new NullReferenceException("Array type should not be null.");
            if (t.Namespace == "Generated")
                return t.BaseType ?? throw new NullReferenceException("Base type should not be null.");
            return t;
        }

        private static string GetPropLine(PropertyInfo p)
        {
            var name = p.Name;
            var type = p.PropertyType;
            bool array = type.IsArray;

            var realType = GetType(type);
            if (!Aliases.TryGetValue(realType, out var tn))
                tn = realType.Name;
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
}
