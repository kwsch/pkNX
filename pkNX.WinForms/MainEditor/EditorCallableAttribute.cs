using System;
using FontAwesome.Sharp;

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

public static class EditorCategoryExt
{
    public static IconChar GetIcon(this EditorCategory category) => category switch
    {
        EditorCategory.None => IconChar.None,
        EditorCategory.Pokemon => IconChar.Dragon,
        EditorCategory.Gameplay => IconChar.Gamepad,
        EditorCategory.AI => IconChar.Robot,
        EditorCategory.Battle => IconChar.Burst,
        EditorCategory.Field => IconChar.Volcano,
        EditorCategory.Shops => IconChar.ShoppingCart,
        EditorCategory.Items => IconChar.FlaskVial,
        EditorCategory.Dialog => IconChar.Comments,
        EditorCategory.NPC => IconChar.UserNinja,
        EditorCategory.Player => IconChar.User,
        EditorCategory.Rides => IconChar.Horse,
        EditorCategory.Physics => IconChar.Ruler,
        EditorCategory.Graphics => IconChar.Display,
        EditorCategory.Audio => IconChar.VolumeUp,
        EditorCategory.Misc => IconChar.Toolbox,
        _ => throw new ArgumentOutOfRangeException(nameof(category), category, null),
    };
}

[AttributeUsage(AttributeTargets.Method)]
public class EditorCallableAttribute : Attribute
{
    public EditorCategory Category { get; }
    public IconChar Icon { get; }
    public bool IsAdvanced { get; }
    public string EditorName { get; }

    /// <summary>
    /// Add this attribute to customize the button that will be displayed in the main editor
    /// </summary>
    /// <param name="category">The category of this editor. Add this to hide it under a sub editor button</param>
    /// <param name="icon">The icon to display for this editor. Add this to display an icon next to the name</param>
    /// <param name="advanced">True is the editor should only be displayed when the user turned on advanced view</param>
    /// <param name="editorName">The name that should be displayed on the editor button. Leave empty for automatic conversion from the method name.</param>
    public EditorCallableAttribute(EditorCategory category = EditorCategory.Misc, IconChar icon = IconChar.None, bool advanced = false, string editorName = "")
    {
        Category = category;
        Icon = icon;
        EditorName = editorName;
        IsAdvanced = advanced;
    }

    public bool HasCustomEditorName() => !string.IsNullOrWhiteSpace(EditorName);

    public bool HasCategory() => Category != EditorCategory.None;
    public bool HasIcon() => Icon != IconChar.None;
}
