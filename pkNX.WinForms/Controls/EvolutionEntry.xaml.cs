using PKHeX.Drawing.PokeSprite;
using pkNX.Game;
using pkNX.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace pkNX.WinForms;

/// <summary>
/// Interaction logic for EvolutionEntry.xaml
/// </summary>
public partial class EvolutionEntry : INotifyPropertyChanged
{
    public static string[] MethodArgumentList = Array.Empty<string>();

    public static readonly DependencyProperty MethodProperty = DependencyProperty.Register(nameof(Method), typeof(int), typeof(EvolutionEntry), new PropertyMetadata(0, OnMethodChanged));
    public static readonly DependencyProperty ArgumentProperty = DependencyProperty.Register(nameof(Argument), typeof(int), typeof(EvolutionEntry), new PropertyMetadata(0));
    public static readonly DependencyProperty SpeciesProperty = DependencyProperty.Register(nameof(Species), typeof(int), typeof(EvolutionEntry), new PropertyMetadata(0, OnSpeciesChanged));
    public static readonly DependencyProperty FormProperty = DependencyProperty.Register(nameof(Form), typeof(int), typeof(EvolutionEntry), new PropertyMetadata(0, OnSpeciesChanged));
    public static readonly DependencyProperty LevelProperty = DependencyProperty.Register(nameof(Level), typeof(ushort), typeof(EvolutionEntry), new PropertyMetadata((ushort)0));

    public int Method
    {
        get => (int)GetValue(MethodProperty);
        set => SetValue(MethodProperty, value);
    }
    public int Argument
    {
        get => (int)GetValue(ArgumentProperty);
        set => SetValue(ArgumentProperty, value);
    }
    public int Species
    {
        get => (int)GetValue(SpeciesProperty);
        set => SetValue(SpeciesProperty, value);
    }
    public int Form
    {
        get => (int)GetValue(FormProperty);
        set => SetValue(FormProperty, value);
    }
    public ushort Level
    {
        get => (ushort)GetValue(LevelProperty);
        set => SetValue(LevelProperty, value);
    }

    public EvolutionType EvolutionType => (EvolutionType)Method;
    public EvolutionTypeArgumentType EvolutionArgumentType => EvolutionType.GetArgType();
    public bool IsMethodValid => EvolutionType != EvolutionType.None;
    public bool HasEvolutionArgument => EvolutionArgumentType >= EvolutionTypeArgumentType.Items;

    public EvolutionEntry()
    {
        InitializeComponent();
    }

    private static void OnSpeciesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (EvolutionEntry)d;
        self.ChangeSpecies(self.Species, self.Form);
    }

    private void ChangeSpecies(int spec, int form)
    {
        Bitmap rawImg = (Bitmap)SpriteUtil.GetSprite((ushort)spec, (byte)form, 0, 0, 0, false, PKHeX.Core.Shiny.Never);
        IMG_MonPreview.Source = Utils.ImageSourceFromBitmap(rawImg);
    }

    private static void OnMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (EvolutionEntry)d;

        self.OnPropertyChanged(nameof(EvolutionType));
        self.OnPropertyChanged(nameof(IsMethodValid));
        self.OnPropertyChanged(nameof(EvolutionArgumentType));
        self.OnPropertyChanged(nameof(HasEvolutionArgument));

        if (!self.HasEvolutionArgument)
            return;

        self.SetArgumentLabel();
        MethodArgumentList = GetArgs(self.EvolutionArgumentType);
        self.Argument = 0;
    }

    private void SetArgumentLabel()
    {
        L_Argument.Content = EvolutionArgumentType switch
        {
            EvolutionTypeArgumentType.NoArg => "None",
            EvolutionTypeArgumentType.Items => "Item",
            EvolutionTypeArgumentType.Moves => "Move",
            EvolutionTypeArgumentType.Species => "Species",
            EvolutionTypeArgumentType.Type => "Type",
            EvolutionTypeArgumentType.Level => "Level",
            EvolutionTypeArgumentType.Stat => "Stat",
            EvolutionTypeArgumentType.Version => "Version",
            _ => throw new ArgumentOutOfRangeException(nameof(EvolutionArgumentType), EvolutionArgumentType, null),
        };
    }

    private static string[] GetArgs(EvolutionTypeArgumentType type)
    {
        return type switch
        {
            EvolutionTypeArgumentType.NoArg => new[] { "None" },
            EvolutionTypeArgumentType.Items => UIStaticSources.ItemsList,
            EvolutionTypeArgumentType.Moves => UIStaticSources.MovesList,
            EvolutionTypeArgumentType.Species => UIStaticSources.SpeciesList,
            EvolutionTypeArgumentType.Type => UIStaticSources.TypesList,
            EvolutionTypeArgumentType.Level => Enumerable.Range(0, 100 + 1).Select(z => z.ToString()).ToArray(),
            EvolutionTypeArgumentType.Stat => Enumerable.Range(0, 255 + 1).Select(z => z.ToString()).ToArray(),
            EvolutionTypeArgumentType.Version => Enumerable.Range(0, 255 + 1).Select(z => z.ToString()).ToArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
