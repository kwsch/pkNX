using System;
using System.Media;
using System.Windows;

namespace pkNX.WinForms;

public static class WinFormsUtil
{
    /// <summary>
    /// Displays a dialog showing the details of an error.
    /// </summary>
    /// <param name="lines">User-friendly message about the error.</param>
    /// <returns>The <see cref="MessageBoxResult"/> associated with the dialog.</returns>
    internal static MessageBoxResult Error(params string[] lines)
    {
        SystemSounds.Hand.Play();
        string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
        return MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    internal static MessageBoxResult Alert(params string[] lines)
    {
        SystemSounds.Asterisk.Play();
        string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
        return MessageBox.Show(msg, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    internal static MessageBoxResult Warn(MessageBoxButton btn, params string[] lines)
    {
        SystemSounds.Hand.Play();
        string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
        return MessageBox.Show(msg, "Warning", btn, MessageBoxImage.Warning);
    }

    internal static MessageBoxResult Prompt(MessageBoxButton btn, params string[] lines)
    {
        SystemSounds.Question.Play();
        string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
        return MessageBox.Show(msg, "Prompt", btn, MessageBoxImage.Asterisk);
    }

    /// <summary>
    /// Gets the selected value of the input <see cref="cb"/>. If no value is selected, will return 0.
    /// </summary>
    /// <param name="cb">ComboBox to retrieve value for.</param>
    internal static int GetIndex(System.Windows.Forms.ComboBox cb) => (int)(cb.SelectedValue ?? 0);

    /// <summary>
    /// Manual implementation of setting Title Case, replacing underscores and upper-casing spaced words.
    /// </summary>
    /// <param name="value">String to convert</param>
    /// <returns>Title Case string</returns>
    internal static string GetSpacedCapitalized(ReadOnlySpan<char> value)
    {
        Span<char> tmp = stackalloc char[value.Length * 2];
        int ctr = 0;
        bool lastSpace = true; // force first capitalized
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (c == '_')
            {
                // If current is space, replace next with upper char.
                tmp[ctr++] = ' ';
                lastSpace = true;
            }
            else if (lastSpace)
            {
                // If previous was space, replace current with upper char.
                tmp[ctr++] = char.ToUpper(c);
                lastSpace = false;
            }
            else
            {
                // If current is upper and next is lower, add a space before.
                // If current is lower and next is upper, add a space after.
                if (i + 1 < value.Length) // has next
                {
                    var n = value[i + 1];
                    if (n != '_')
                    {
                        var u0 = char.IsUpper(c);
                        var u1 = char.IsUpper(n);
                        if (u0 != u1)
                        {
                            tmp[ctr++] = u0 ? ' ' : c;
                            tmp[ctr++] = u1 ? ' ' : c;
                            c = n; // fall through write next
                            i++;
                        }
                    }
                }
                tmp[ctr++] = c;
            }
        }
        return new string(tmp[..ctr]);
    }
}
