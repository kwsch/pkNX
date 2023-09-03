using FontAwesome.Sharp;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace pkNX.WinForms;

public class IconImage : FontAwesome.Sharp.IconImage
{
    public new static readonly DependencyProperty IconFontProperty = DependencyProperty.Register(nameof(IconFont),
        typeof(IconFont), typeof(IconImage),
        new PropertyMetadata(IconFont.Auto, OnUpdateIconChanged));

    public new static readonly DependencyProperty ForegroundProperty =
        DependencyProperty.RegisterAttached("Foreground", typeof(Brush), typeof(IconImage),
            new FrameworkPropertyMetadata(
                new SolidColorBrush(Color.FromRgb(239, 239, 239)),
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,
                OnUpdateIconChanged));

    public new IconFont IconFont
    {
        get => (IconFont)GetValue(IconFontProperty);
        set => SetValue(IconFontProperty, value);
    }

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static void SetForeground(DependencyObject element, Brush value)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        element.SetValue(ForegroundProperty, value);
    }
    public static Brush GetForeground(DependencyObject element)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element));
        }

        return (Brush)element.GetValue(ForegroundProperty);
    }

    private static void OnUpdateIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not IconImage iconImage)
            return;

        var icon = (IconChar)d.GetValue(IconProperty);
        if (icon == IconChar.None)
            return;

        var imageSource = iconImage.ImageSourceFor(icon);
        iconImage.SetValue(SourceProperty, imageSource);
    }

    protected FontFamily FontFor(IconChar icon)
    {
        return icon.FontFamilyFor(IconFont);
    }

    protected override ImageSource ImageSourceFor(IconChar icon)
    {
        double size = Math.Max(IconHelper.DefaultSize, Math.Max(ActualWidth, ActualHeight));
        return FontFor(icon).ToImageSource(icon, Foreground, size);
    }
}
