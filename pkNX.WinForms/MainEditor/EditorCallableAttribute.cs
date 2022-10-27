using System;

namespace pkNX.WinForms;

public enum EditorCategory
{
    None,
    Pokemon,
    AI,
    Battle,
    Field,
    Shops,
    Items,
    Dialog,
    NPC,
    Player,
    Rides,
    Graphics,
    Audio,
    Misc,
}

[AttributeUsage(AttributeTargets.Method)]
public class EditorCallableAttribute : Attribute
{
    public EditorCategory Category { get; }
    public string EditorName { get; }

    /// <summary>
    /// Add this attribute to customize the button that will be displayed in the main editor
    /// </summary>
    /// <param name="category">The category of this editor. Add this to hide it under a sub editor button</param>
    /// <param name="editorName">The name that should be displayed on the editor button. Leave empty for automatic conversion from the method name.</param>
    public EditorCallableAttribute(EditorCategory category = EditorCategory.Misc, string editorName = "")
    {
        Category = category;
        EditorName = editorName;
    }

    public bool HasCustomEditorName() => !string.IsNullOrWhiteSpace(EditorName);

    public bool HasCategory() => Category != EditorCategory.None;
}
