using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pkNX.WinForms;

public static class ReflectionUtils
{
    public static IEnumerable<FieldInfo> GetAllFields(object target, Func<FieldInfo, bool> predicate)
    {
        List<Type> types =
        [
            target.GetType(),
        ];

        while (types[^1].BaseType is { } x)
            types.Add(x);

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<FieldInfo> fieldInfos = types[i]
                .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (var fieldInfo in fieldInfos)
                yield return fieldInfo;
        }
    }

    public static IEnumerable<PropertyInfo> GetAllProperties(object target, Func<PropertyInfo, bool> predicate)
    {
        List<Type> types =
        [
            target.GetType(),
        ];

        while (types[^1].BaseType is { } x)
            types.Add(x);

        for (int i = types.Count - 1; i >= 0; i--)
        {
            IEnumerable<PropertyInfo> propertyInfos = types[i]
                .GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(predicate);

            foreach (var propertyInfo in propertyInfos)
                yield return propertyInfo;
        }
    }

    public static IEnumerable<MethodInfo> GetAllMethods(object target, Func<MethodInfo, bool> predicate)
    {
        var methodInfos = target.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(predicate);

        return methodInfos;
    }

    public static FieldInfo? GetField(object? target, string fieldName)
    {
        if (target == null)
            return null;
        return GetAllFields(target, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
    }

    public static PropertyInfo? GetProperty(object target, string propertyName)
    {
        return GetAllProperties(target, p => p.Name.Equals(propertyName, StringComparison.InvariantCulture)).FirstOrDefault();
    }

    public static MethodInfo? GetMethod(object target, string methodName)
    {
        return GetAllMethods(target, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
    }
}
