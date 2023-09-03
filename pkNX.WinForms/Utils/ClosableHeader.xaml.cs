using System.Windows;

namespace pkNX.WinForms;

/// <summary>
/// Interaction logic for ClosableHeader.xaml
/// </summary>
public partial class ClosableHeader
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(string), typeof(ClosableHeader), new PropertyMetadata("Title"));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public ClosableHeader()
    {
        InitializeComponent();
    }

    public ClosableHeader(string header)
    {
        Header = header;

        InitializeComponent();
    }
}
