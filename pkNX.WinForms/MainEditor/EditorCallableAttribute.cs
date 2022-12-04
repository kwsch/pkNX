using System;

namespace pkNX.WinForms;

public enum EditorCategory // The order of the enum members defines the order they appear in editor
{
    None,
    Pokemon,
    Gameplay,
    AI,
    Battle,
    Field,
    Shops,
    Items,
    Dialog,
    NPC,
    Player,
    Rides,
    Physics,
    Graphics,
    Audio,
    Misc,
}

[AttributeUsage(AttributeTargets.Method)]
public class EditorCallableAttribute : Attribute
{
    public EditorCategory Category { get; }
    public bool IsAdvanced { get; }
    public string EditorName { get; }

    /// <summary>
    /// Add this attribute to customize the button that will be displayed in the main editor
    /// </summary>
    /// <param name="category">The category of this editor. Add this to hide it under a sub editor button</param>
    /// <param name="advanced">True is the editor should only be displayed when the user turned on advanced view</param>
    /// <param name="editorName">The name that should be displayed on the editor button. Leave empty for automatic conversion from the method name.</param>
    public EditorCallableAttribute(EditorCategory category = EditorCategory.Misc, bool advanced = false, string editorName = "")
    {
        Category = category;
        EditorName = editorName;
        IsAdvanced = advanced;
    }

    public bool HasCustomEditorName() => !string.IsNullOrWhiteSpace(EditorName);

    public bool HasCategory() => Category != EditorCategory.None;
}
