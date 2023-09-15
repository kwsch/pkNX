using System;
using System.Windows;
using System.Windows.Controls;

namespace pkNX.WinForms;

public class LabeledControl : ContentControl
{
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledControl), new PropertyMetadata("Label:"));
    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }
}
