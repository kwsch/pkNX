using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls;

public abstract class EditorBase
{
    protected readonly GameManager ROM;

    public GameVersion Game => ROM.Game;
    public int Language { get => ROM.Language; set => ROM.Language = value; }

    protected EditorBase(GameManager rom) => ROM = rom;
    public string? Location { get; internal set; }

    public void Initialize() => ROM.Initialize();

    private static string GetEditorName(string name)
    {
        var newName = name.Replace('_', ' ').ToCharArray();
        var builder = new StringBuilder();

        // Force first char to upper
        newName[0] = char.ToUpper(newName[0]);

        for (int i = 0; i < newName.Length; ++i)
        {
            char c = newName[i];
            builder.Append(c);

            // Check the next char
            if (i + 1 >= newName.Length)
                continue;
            char nextC = newName[i + 1];

            // If current is space, replace next with upper char
            if (c == ' ')
            {
                newName[i + 1] = char.ToUpper(nextC);
            }
            // If current is lower and next is upper, add a space in between
            else if (char.IsLower(c) && char.IsUpper(nextC))
            {
                builder.Append(' ');
            }
            // If previous is upper, current is upper and next is lower, add a space in between
            else if (i + 2 < newName.Length && char.IsUpper(c) && char.IsUpper(nextC) && char.IsLower(newName[i + 2]))
            {
                builder.Append(' ');
            }
        }
        return builder.ToString();
    }

    public IEnumerable<Button> GetControls(int width, int height)
    {
        var type = GetType();
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            const string prefix = "Edit";
            if (!m.Name.StartsWith(prefix))
                continue;

            var name = m.Name[prefix.Length..];
            var b = new Button
            {
                Width = width,
                Height = height,
                Name = $"B_{name}",
                Text = GetEditorName(name),
            };
            b.Click += (s, e) =>
            {
                try
                {
                    m.Invoke(this, null);
                }
                catch (Exception exception)
                {
                    if (exception.InnerException is { } x)
                        exception = x;
                    Console.WriteLine(exception);
                    WinFormsUtil.Error(exception.Message, exception.StackTrace ?? string.Empty);
                }
            };
            yield return b;
        }
    }

    public void Close() => ROM.SaveAll(true);
    public void Save() => ROM.SaveAll(false);

    private static EditorBase? GetEditor(GameManager ROM) => ROM switch
    {
        GameManagerGG gg => new EditorGG(gg),
        GameManagerSWSH swsh => new EditorSWSH(swsh),
        GameManagerPLA pla => new EditorPLA(pla),
        _ => null,
    };

    public static EditorBase? GetEditor(string loc, int language)
    {
        var gl = GameLocation.GetGame(loc);
        if (gl == null)
            return null;

        var gm = GameManager.GetManager(gl, language);
        var editor = GetEditor(gm);
        if (editor == null)
            return null;

        editor.Location = loc;
        return editor;
    }
}
