using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls;

public abstract class EditorBase
{
    protected abstract GameManager ROM { get; }

    public GameVersion Game => ROM.Game;
    public int Language { get => ROM.Language; set => ROM.Language = value; }
    public string? Location { get; private set; }

    public void Initialize() => ROM.Initialize();

    public IEnumerable<Button> GetControls(int width, int height)
    {
        var type = GetType();
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        foreach (var m in methods)
        {
            const string prefix = "Edit";
            if (!m.Name.StartsWith(prefix))
                continue;

            var name = m.Name.AsSpan(prefix.Length);
            var b = new Button
            {
                Width = width,
                Height = height,
                Name = $"B_{name}",
                Text = WinFormsUtil.GetSpacedCapitalized(name),
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

    public static EditorBase? GetEditor(string loc, int language, GameVersion gameOverride)
    {
        var gl = GameLocation.GetGame(loc, gameOverride);
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
