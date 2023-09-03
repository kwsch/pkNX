using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace pkNX.WinForms;

public class BoolToBrushConverter : BoolToValueConverter<Brush>
{
}

public class BoolToImageSourceConverter : BoolToValueConverter<ImageSource>
{
}

public class BoolToStoryboardConverter : BoolToValueConverter<Storyboard>
{
}

public class BoolToFloatConverter : BoolToValueConverter<float>
{
}

public class BoolToDoubleConverter : BoolToValueConverter<double>
{
}

public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
{
    public BoolToVisibilityConverter()
    {
        TrueValue = Visibility.Visible;
        FalseValue = Visibility.Collapsed;
    }
}

public class IntToBoolEqualsConverter : EqualsConverter<bool, int>
{
}

/// <summary>
/// Use as the base class for BoolToXXX style converters
/// </summary>
/// <typeparam name="T"></typeparam>    
public abstract class BoolToValueConverter<T> : MarkupExtension, IValueConverter
{
    public T FalseValue { get; set; }
    public T TrueValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToBoolean(value) ? TrueValue : FalseValue;
    }

    // Override if necessary
    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(TrueValue);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

/// <summary>
/// Returns true if not null
/// </summary>
public class NotNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null;
    }

    // Override if necessary
    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Use as the base class for XXXToBool style converters
/// </summary>
/// <typeparam name="T"></typeparam>    
/// <typeparam name="Compare"></typeparam>
public abstract class EqualsConverter<T, Compare> : FrameworkElement, IValueConverter
{
    public T FalseValue { get; set; }
    public T TrueValue { get; set; }

    //public Compare ComparisonValue { get; set; }

    public static readonly DependencyProperty ComparisonValueProperty = DependencyProperty.Register(
        "ComparisonValue", typeof(Compare), typeof(EqualsConverter<T, Compare>),
        new PropertyMetadata(default(Compare)));

    public Compare ComparisonValue
    {
        get { return (Compare)GetValue(ComparisonValueProperty); }
        set { SetValue(ComparisonValueProperty, value); }
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return EqualityComparer<Compare>.Default.Equals((Compare)value, ComparisonValue) ? TrueValue : FalseValue;
    }

    // Override if necessary
    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ComparisonValue;
    }
}

public class EnumValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;

        if (!Enum.TryParse(value.GetType(), parameter.ToString(), out object result))
            throw new ArgumentException();

        return value.Equals(result);
    }

    public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return null;
        var rtnValue = parameter.ToString();
        try
        {
            object returnEnum = Enum.Parse(targetType, rtnValue);
            return returnEnum;
        }
        catch
        {
            return null;
        }
    }
}

public class MultiValueEqualsConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values?.All(o => o?.Equals(values[0]) == true) == true || values?.All(o => o == null) == true;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class PercentageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
