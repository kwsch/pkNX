using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pkNX.WinForms
{
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector : UserControl
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(PathSelector), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty IsPathReadOnlyProperty = DependencyProperty.Register(nameof(IsPathReadOnly), typeof(bool), typeof(PathSelector), new PropertyMetadata(false));
        public static readonly DependencyProperty FolderSelectDescriptionProperty = DependencyProperty.Register(nameof(FolderSelectDescription), typeof(string), typeof(PathSelector), new PropertyMetadata(null));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public bool IsPathReadOnly
        {
            get { return (bool)GetValue(IsPathReadOnlyProperty); }
            set { SetValue(IsPathReadOnlyProperty, value); }
        }

        public string FolderSelectDescription
        {
            get { return (string)GetValue(FolderSelectDescriptionProperty); }
            set { SetValue(FolderSelectDescriptionProperty, value); }
        }

        public event TextChangedEventHandler? PathChanged;

        public PathSelector()
        {
            InitializeComponent();
        }

        private void B_PathSelect_Click(object sender, RoutedEventArgs e)
        {
            using System.Windows.Forms.FolderBrowserDialog fbd = new();
            fbd.Description = FolderSelectDescription;
            fbd.UseDescriptionForTitle = true;
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Path = fbd.SelectedPath;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PathChanged?.Invoke(sender, e);
        }
    }
}
