// Source: https://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=72544

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace pkNX.WinForms;

public partial class EditableTextBlock : TextBlock
{
    public event EventHandler? EnterEditMode;
    public event TextChangedEventHandler? TextChanged;
    public event EventHandler? ExitEditMode;

    public static readonly DependencyProperty IsInEditModeProperty =
        DependencyProperty.Register(nameof(IsInEditMode), typeof(bool), typeof(EditableTextBlock), new UIPropertyMetadata(false, IsInEditModeUpdate));

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(EditableTextBlock), new UIPropertyMetadata(0));

    private EditableTextBlockAdorner? _adorner;

    public Regex AllowedChars = MyRegex();

    [GeneratedRegex("[^\\/:*?\"<>|]+", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    public bool IsInEditMode
    {
        get => (bool)GetValue(IsInEditModeProperty);
        set => SetValue(IsInEditModeProperty, value);
    }

    /// <summary>
    /// Gets or sets the length of the max.
    /// </summary>
    /// <value>The length of the max.</value>
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public EditableTextBlock()
    {
        SetResourceReference(StyleProperty, typeof(TextBlock));
    }

    /// <summary>
    /// Determines whether [is in edit mode update] [the specified obj].
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
    private static void IsInEditModeUpdate(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
        if (obj is not EditableTextBlock textBlock)
            return;

        // Get the adorner layer of the uielement (here TextBlock)
        var layer = AdornerLayer.GetAdornerLayer(textBlock);

        // If the IsInEditMode set to true means the user has enabled the edit mode then
        // add the adorner to the adorner layer of the TextBlock.
        if (textBlock.IsInEditMode)
        {
            if (textBlock._adorner == null)
            {
                textBlock._adorner = new EditableTextBlockAdorner(textBlock);

                // Events wired to exit edit mode when the user presses Enter key or leaves the control.
                textBlock._adorner.TextBoxKeyUp += (_, tE) =>
                {
                    if (tE.Key == Key.Enter)
                    {
                        textBlock.IsInEditMode = false;
                        Keyboard.ClearFocus();
                    }
                };

                textBlock._adorner.TextBoxTextChanged += (xy, xe) => textBlock.TextChanged?.Invoke(xy, xe);
                textBlock._adorner.TextBoxLostFocus += (_, _) => textBlock.IsInEditMode = false;
            }

            layer?.Add(textBlock._adorner);
            textBlock._adorner.StartEdit();
            textBlock.OnEnterEditMode();
        }
        else if (layer != null)
        {
            // Remove the adorner from the adorner layer.
            var adorners = layer.GetAdorners(textBlock);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    if (adorner is EditableTextBlockAdorner)
                    {
                        layer.Remove(adorner);
                    }
                }
            }

            //Update the textblock's text binding.
            var expression = textBlock.GetBindingExpression(TextProperty);
            expression?.UpdateTarget();

            textBlock.FocusParent();
            textBlock.OnExitEditMode();
        }
    }

    private void OnEnterEditMode()
    {
        EnterEditMode?.Invoke(this, EventArgs.Empty);
    }

    private void OnExitEditMode()
    {
        ExitEditMode?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Invoked when an unhandled <see cref="Control.MouseDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
    /// </summary>
    /// <param name="e">The <see cref="MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (e.ClickCount >= 2)
        {
            IsInEditMode = true;
            e.Handled = true;
        }
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.F2)
        {
            IsInEditMode = true;
            e.Handled = true;
        }
    }
}

/// <summary>
/// Adorner class which shows textbox over the text block when the Edit mode is on.
/// </summary>
public class EditableTextBlockAdorner : Adorner
{
    private readonly VisualCollection _collection;

    private readonly TextBox _textBox;

    private readonly TextBlock _textBlock;

    public EditableTextBlockAdorner(EditableTextBlock adornedElement)
        : base(adornedElement)
    {
        _collection = new VisualCollection(this);
        _textBox = new TextBox();
        _textBlock = adornedElement;
        var binding = new Binding("Text") { Source = adornedElement };
        _textBox.SetBinding(TextBox.TextProperty, binding);
        _textBox.AcceptsReturn = true;
        _textBox.MaxLength = adornedElement.MaxLength;
        _textBox.PreviewTextInput += (obj, e) => e.Handled = !adornedElement.AllowedChars.IsMatch(e.Text);
        _textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
        _textBox.Width = adornedElement.Width;
        _textBox.Height = adornedElement.Height;
        _textBox.FontFamily = adornedElement.FontFamily;
        _textBox.FontSize = adornedElement.FontSize;
        _textBox.Padding = new Thickness(0, -1, -2, 0);
        _collection.Add(_textBox);
    }

    private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            var expression = _textBox.GetBindingExpression(TextBox.TextProperty);
            expression?.UpdateSource();

            e.Handled = true;
        }
    }

    protected override Visual GetVisualChild(int index)
    {
        return _collection[index];
    }

    protected override int VisualChildrenCount => _collection.Count;

    protected override Size ArrangeOverride(Size finalSize)
    {
        _textBox.Arrange(new Rect(-3, 0, _textBlock.ActualWidth + 6, _textBlock.ActualHeight));
        return finalSize;
    }

    private void OnMouseDownOutsideElement(object sender, MouseButtonEventArgs e)
    {
        Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);

        ReleaseMouseCapture();
        _textBox.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
    }

    internal void StartEdit()
    {
        Mouse.Capture(this, CaptureMode.SubTree);
        _textBox.Focus();
        _textBox.SelectAll();

        Mouse.AddPreviewMouseDownOutsideCapturedElementHandler(this, OnMouseDownOutsideElement);
    }

    public event RoutedEventHandler TextBoxLostFocus
    {
        add => _textBox.LostFocus += value;
        remove => _textBox.LostFocus -= value;
    }

    public event KeyEventHandler TextBoxKeyUp
    {
        add => _textBox.KeyUp += value;
        remove => _textBox.KeyUp -= value;
    }

    public event TextChangedEventHandler TextBoxTextChanged
    {
        add => _textBox.TextChanged += value;
        remove => _textBox.TextChanged -= value;
    }
}
