using System;
using System.Globalization;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pkNX.WinForms;

public enum NumericTextBoxMode
{
    Integer,
    Double,
}

public partial class NumericTextBox : TextBox
{
    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.NegativeInfinity));

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.PositiveInfinity));

    public static readonly DependencyProperty NumberModeProperty =
        DependencyProperty.Register(nameof(NumberMode), typeof(NumericTextBoxMode), typeof(NumericTextBox), new PropertyMetadata(NumericTextBoxMode.Integer));

    public static readonly DependencyProperty EvaluatedValueProperty =
        DependencyProperty.Register(nameof(EvaluatedValue), typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0));

    // Bind to Value Property
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0, OnValueChanged));

    // Use this to check for value changes
    public event EventHandler EvaluatedValueChanged;

    private readonly Regex doubleChars = RexexChar();

    private readonly Regex intChars = RegexInt();
    [GeneratedRegex(@"[^\d.\-*^%\+/()]+", RegexOptions.Compiled)] private static partial Regex RexexChar();
    [GeneratedRegex(@"[^\d-*^%\+/()]+", RegexOptions.Compiled)] private static partial Regex RegexInt();

    private static readonly string textFormat = "0.##";

    public NumericTextBox()
    {
        SetResourceReference(StyleProperty, typeof(TextBox));
        Text = EvaluatedValue.ToString(textFormat, CultureInfo.InvariantCulture);

        PreviewTextInput += (obj, e) => e.Handled = !IsTextAllowed(e.Text);
        PreviewKeyDown += OnPreviewKeyDown;
        LostFocus += OnLostFocus;
        TextChanged += OnTextChanged;
        DataObject.AddPastingHandler(this, OnPasting);
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            TextBox t = (TextBox)sender;
            t.FocusParent();
        }
    }

    private Regex AllowedChars => NumberMode == NumericTextBoxMode.Double ? doubleChars : intChars;

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public double MinValue
    {
        get => (double)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    public double EvaluatedValue
    {
        get => (double)GetValue(EvaluatedValueProperty);
        private set
        {
            var v = Math.Clamp(value, MinValue, MaxValue);

            if (EvaluatedValue == v)
                return;

            SetValue(EvaluatedValueProperty, v);
            EvaluatedValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set
        {
            var v = Math.Clamp(value, MinValue, MaxValue);

            if (Value == v)
                return;

            SetValue(ValueProperty, v);
            EvaluatedValue = v;

            Text = Value.ToString(textFormat, CultureInfo.InvariantCulture);
        }
    }

    public NumericTextBoxMode NumberMode
    {
        get => (NumericTextBoxMode)GetValue(NumberModeProperty);
        set => SetValue(NumberModeProperty, value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var minValue = (double)d.GetValue(MinValueProperty);
        var maxValue = (double)d.GetValue(MaxValueProperty);
        var v = Math.Clamp((double)e.NewValue, minValue, maxValue);

        d.SetValue(TextProperty, v.ToString(textFormat, CultureInfo.InvariantCulture));
    }

    private bool IsTextAllowed(string text)
    {
        return !AllowedChars.IsMatch(text);
    }

    private void OnPasting(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(typeof(string)))
        {
            e.CancelCommand();
        }

        string text = (string)e.DataObject.GetData(typeof(string));
        if (!IsTextAllowed(text))
        {
            SystemSounds.Beep.Play();
            e.CancelCommand();
        }
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (StringToFormula.TryEval(Text, out double value))
            EvaluatedValue = value;
    }

    private void OnLostFocus(object sender, RoutedEventArgs e)
    {
        if (StringToFormula.TryEval(Text, out double value))
            Value = value;
        else
            Value = EvaluatedValue;
    }
}
