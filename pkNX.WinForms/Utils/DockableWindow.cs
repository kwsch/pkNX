using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace pkNX.WinForms;

public class WindowItem : TabItem
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title", typeof(string), typeof(WindowItem),
        new PropertyMetadata("Test", OnTitlePropertyChanged));

    private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var title = (string)d.GetValue(TitleProperty);

        if (title == null)
            return;

        d.SetValue(HeaderProperty, new ClosableHeader(title));
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public WindowItem()
    {
        Header = new ClosableHeader();
    }
}


public partial class DockableWindow : UserControl
{
    [System.ComponentModel.Bindable(true)]
    public ObservableCollection<WindowItem> Windows { get; set; } = new ObservableCollection<WindowItem>();


    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        TabControl tabControl = new TabControl
        {
            ItemsSource = Windows
        };

        Content = tabControl;
    }
}
