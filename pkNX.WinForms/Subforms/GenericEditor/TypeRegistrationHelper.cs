using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace pkNX.WinForms;

public static class TypeRegistrationHelper
{
    // Keep track of types we’ve already processed.
    private static readonly HashSet<Type> RegisteredTypes = [];
    private static readonly Lock LockObj = new();

    /// <summary>
    /// Scans the type (and its nested types) for any properties of type IList&lt;&gt;
    /// and registers a custom type descriptor provider for types that have such properties.
    /// </summary>
    public static void RegisterIListConvertersRecursively(Type type)
    {
        lock (LockObj)
        {
            if (!RegisteredTypes.Add(type))
                return;
        }

        // Check each public instance property.
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propType = prop.PropertyType;

            // Is it a generic type and exactly an IList<>
            if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                // For this type, add our custom provider that overrides the IList property converter.
                // (The provider itself will decide which property to wrap based on its name or other criteria.)
                TypeDescriptor.AddProvider(new DynamicListTypeDescriptionProvider(type), type);

                // Also, register for the element type in case it in turn contains IList<> properties.
                var elementType = propType.GetGenericArguments()[0];
                if (elementType.IsClass && elementType != typeof(string))
                    RegisterIListConvertersRecursively(elementType);
            }
            else if (propType.IsClass && propType != typeof(string))
            {
                // Also check non-list properties recursively.
                RegisterIListConvertersRecursively(propType);
            }
        }
    }
}
public class DynamicListTypeDescriptionProvider(Type type)
    : TypeDescriptionProvider(TypeDescriptor.GetProvider(type))
{
    private readonly TypeDescriptionProvider _baseProvider = TypeDescriptor.GetProvider(type);

    public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object? instance)
    {
        var baseDescriptor = _baseProvider.GetTypeDescriptor(objectType, instance);
        return new DynamicListTypeDescriptor(baseDescriptor);
    }
}

public class DynamicListTypeDescriptor(ICustomTypeDescriptor? parent)
    : CustomTypeDescriptor(parent)
{
    public override PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
    {
        var originalProps = base.GetProperties(attributes);
        List<PropertyDescriptor> newProps = [];

        foreach (PropertyDescriptor pd in originalProps)
        {
            // Check if this is our target property.
            // For example, check by name or by type.
            var type = pd.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                // Wrap the property descriptor with one that supplies a custom type converter.
                newProps.Add(new DynamicListPropertyDescriptor(pd));
            }
            else
            {
                newProps.Add(pd);
            }
        }

        return new PropertyDescriptorCollection(newProps.ToArray(), true);
    }
}

public class DynamicListPropertyDescriptor(PropertyDescriptor baseDescriptor)
    : PropertyDescriptor(baseDescriptor)
{
    public override bool CanResetValue(object component) => baseDescriptor.CanResetValue(component);
    public override Type ComponentType => baseDescriptor.ComponentType;
    public override object? GetValue(object? component) => baseDescriptor.GetValue(component);
    public override bool IsReadOnly => baseDescriptor.IsReadOnly;
    public override Type PropertyType => baseDescriptor.PropertyType;
    public override void ResetValue(object component) => baseDescriptor.ResetValue(component);
    public override void SetValue(object? component, object? value) => baseDescriptor.SetValue(component, value);
    public override bool ShouldSerializeValue(object component) => baseDescriptor.ShouldSerializeValue(component);

    public override TypeConverter Converter
    {
        get
        {
            // Get the element type (T) from IList<T>
            var elementType = PropertyType.GetGenericArguments().FirstOrDefault();
            if (elementType == null)
                return baseDescriptor.Converter;

            // Create an instance of our custom converter, e.g. ListTypeConverter<T>.
            var converterType = typeof(ListTypeConverter<>).MakeGenericType(elementType);
            if (Activator.CreateInstance(converterType) is TypeConverter converter)
                return converter;
            return baseDescriptor.Converter;
        }
    }
}

public class ListTypeConverter<T> : ExpandableObjectConverter
{
    public override bool GetPropertiesSupported(ITypeDescriptorContext? context)
    {
        // We want the properties (the list items) to be shown in the grid.
        return true;
    }

    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
    {
        if (value is not IList<T?> list)
            return base.GetProperties(context, value, attributes);

        var props = new PropertyDescriptor[list.Count];
        for (int i = 0; i < list.Count; i++)
            props[i] = new ListItemPropertyDescriptor<T>(list, i);

        return new PropertyDescriptorCollection(props);
    }
}

public class ListItemPropertyDescriptor<T>(IList<T?> list, int index)
    : PropertyDescriptor($"[{index}]", null)
{
    public override object? GetValue(object? component) => list[index];
    public override void SetValue(object? component, object? value) => list[index] = (T?)value;
    public override bool IsReadOnly => false;
    public override Type ComponentType => list.GetType();
    public override bool CanResetValue(object component) => false;
    public override Type PropertyType => typeof(T);
    public override void ResetValue(object component) { }
    public override bool ShouldSerializeValue(object component) => true;
}
