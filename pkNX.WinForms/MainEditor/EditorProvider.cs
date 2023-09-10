using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Controls;
using pkNX.Game;
using pkNX.Structures;

namespace pkNX.WinForms.Controls;

public abstract class EditorBase
{
    protected abstract GameManager ROM { get; }

    public GameVersion Game => ROM.Game;
    public int Language { get => ROM.Language; set => ROM.Language = value; }
    public string? Location { get; private set; }

    private const string prefix = "Edit";
    private readonly MethodInfo[] editorMethods;
    private readonly EditorCallableAttribute[] editorAttributes;

    protected EditorBase()
    {
        // Collect all methods that are marked as editors
        // The method name needs to start with `Edit` or an EditorCallableAttribute should be added
        var editors = GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => new { Method = x, Callable = x.GetCustomAttribute<EditorCallableAttribute>() })
            .Where(x => x.Callable != null || x.Method.Name.StartsWith(prefix))
            .ToList();

        editorMethods = editors.Select(x => x.Method).ToArray();
        editorAttributes = editors.Select(x => x.Callable ?? new EditorCallableAttribute(EditorCategory.None)).ToArray();
    }

    public void Initialize()
    {
        GamePath.Initialize(ROM.Game, ROM.Language);
        ROM.Initialize();
        UIStaticSources.SetupForGame(ROM);
    }

    public int CountControlsForCategory(EditorCategory category) => editorAttributes.Count(a => a.Category == category);

    public IEnumerable<EditorButtonData> GetControls(EditorCategory category = EditorCategory.None, bool displayAdvanced = false)
    {
        for (int i = 0; i < editorMethods.Length; ++i)
        {
            var m = editorMethods[i];
            var callable = editorAttributes[i];

            // Ignore all editors that are not of the requested category
            if (callable.Category != category)
                continue;

            // Ignore all advanced editors when the user doesn't have this view enabled
            if (callable.IsAdvanced && !displayAdvanced)
                continue;

            var name = m.Name.Replace(prefix, ""); // Might or might not contain prefix
            var b = new EditorButtonData
            {
                Title = callable.HasCustomEditorName() ? callable.EditorName : WinFormsUtil.GetSpacedCapitalized(name),
                Icon = callable.HasIcon() ? callable.Icon : null,
                OnClick = (_, _) =>
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
                }
            };

            yield return b;
        }
    }

    public void Close()
    {
        ROM.SaveAll(true);
        if (ROM is IDisposable d)
            d.Dispose();
    }

    public void Save() => ROM.SaveAll(false);

    private static EditorBase? GetEditor(GameManager ROM) => ROM switch
    {
        GameManagerGG gg => new EditorGG(gg),
        GameManagerSWSH swsh => new EditorSWSH(swsh),
        GameManagerPLA pla => new EditorPLA(pla),
        GameManagerSV sv => new EditorSV(sv),
        _ => null,
    };

    public static (EditorBase?, GameLoadResult) GetEditor(string loc, int language, GameVersion gameOverride)
    {
        var (gl, result) = GameLocation.GetGame(loc, gameOverride);
        if (gl == null)
            return (null, result);

        var gm = GameManager.GetManager(gl, language);
        var editor = GetEditor(gm);
        if (editor == null)
            return (null, GameLoadResult.UnsupportedGame);

        editor.Location = loc;
        return (editor, GameLoadResult.Success);
    }
}
