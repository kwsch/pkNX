using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pkNX.WinForms;

public class SelectedItemsChangedEventArgs : EventArgs
{
    public IList OldSelection { get; private set; }
    public IList NewSelection { get; private set; }

    public SelectedItemsChangedEventArgs(IList oldSelection, IList newSelection)
    {
        OldSelection = oldSelection;
        NewSelection = newSelection;
    }
}

public delegate void SelectedItemsChangedEventHandler(object sender, SelectedItemsChangedEventArgs e);

public sealed class MultiSelectTreeView : TreeView
{
    #region Fields

    // Used in shift selections
    private TreeViewItem? _lastItemSelected;

    #endregion Fields
    #region Dependency Properties

    public event SelectedItemsChangedEventHandler? SelectedItemsChanged;

    public static readonly DependencyProperty IsItemSelectedProperty =
        DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(false, OnIsItemSelectedChanged));

    private static void OnIsItemSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        UpdateSelectedItems((UIElement)d);
    }

    private static readonly DependencyPropertyKey SelectedItemsPropertyKey =
        DependencyProperty.RegisterReadOnly("SelectedItems", typeof(IList), typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(new List<object>()));
    public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

    public static void SetIsItemSelected(UIElement element, bool value)
    {
        element.SetValue(IsItemSelectedProperty, value);
    }
    public static bool GetIsItemSelected(UIElement element)
    {
        return (bool)element.GetValue(IsItemSelectedProperty);
    }

    private static void UpdateSelectedItems(UIElement element)
    {
        MultiSelectTreeView treeView = element.FindParentOfType<MultiSelectTreeView>();

        var updatedSelection = treeView.Internal_GetSelectedItems();

        var args = new SelectedItemsChangedEventArgs(treeView.SelectedItems, updatedSelection);

        treeView.SetValue(SelectedItemsPropertyKey, updatedSelection);

        treeView.SelectedItemsChanged?.Invoke(treeView, args);
    }

    #endregion Dependency Properties
    #region Properties

    private static bool IsCtrlPressed => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);

    private static bool IsShiftPressed => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

    [Bindable(true)]
    [Category("Appearance")]
    [ReadOnly(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList SelectedItems => (IList)GetValue(SelectedItemsProperty);

    private List<object> Internal_GetSelectedItems()
    {
        var selectedTreeViewItems = GetTreeViewItems(this, true).Where(x => GetIsItemSelected(x) && x.Header != null);
        var selectedModelItems = selectedTreeViewItems.Select(treeViewItem => treeViewItem.Header);

        return selectedModelItems.ToList();
    }

    #endregion Properties
    #region Event Handlers

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseDown(e);

        // If clicking on a tree branch expander...
        if (e.OriginalSource is Shape || e.OriginalSource is Grid || e.OriginalSource is Border)
            return;

        var item = GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);
        if (item != null) SelectedItemChangedInternal(item);
    }

    #endregion Event Handlers
    #region Utility Methods

    private void SelectedItemChangedInternal(TreeViewItem tvItem)
    {
        // Clear all previous selected item states if ctrl is NOT being held down
        if (!IsCtrlPressed)
            ClearSelection();

        // Is this an item range selection?
        if (IsShiftPressed && _lastItemSelected != null)
        {
            var items = GetTreeViewItemRange(_lastItemSelected, tvItem);
            if (items.Count > 0)
            {
                foreach (var treeViewItem in items)
                    SetIsItemSelected(treeViewItem, true);
            }
        }
        // Otherwise, individual selection
        else
        {
            SetIsItemSelected(tvItem, !GetIsItemSelected(tvItem));
            _lastItemSelected = tvItem;
        }
    }
    private static TreeViewItem? GetTreeViewItemClicked(DependencyObject? sender)
    {
        while (sender != null && sender is not TreeViewItem)
            sender = VisualTreeHelper.GetParent(sender);
        return sender as TreeViewItem;
    }
    private static List<TreeViewItem> GetTreeViewItems(ItemsControl parentItem, bool includeCollapsedItems, List<TreeViewItem>? itemList = null)
    {
        itemList ??= [];
        for (var index = 0; index < parentItem.Items.Count; index++)
        {
            var tvItem = (TreeViewItem)parentItem.ItemContainerGenerator.ContainerFromIndex(index);
            if (tvItem == null)
                continue;

            itemList.Add(tvItem);
            if (includeCollapsedItems || tvItem.IsExpanded)
                GetTreeViewItems(tvItem, includeCollapsedItems, itemList);
        }
        return itemList;
    }
    private List<TreeViewItem> GetTreeViewItemRange(TreeViewItem start, TreeViewItem end)
    {
        var items = GetTreeViewItems(this, false);

        var startIndex = items.IndexOf(start);
        var endIndex = items.IndexOf(end);
        var rangeStart = startIndex > endIndex || startIndex == -1 ? endIndex : startIndex;
        var rangeCount = startIndex > endIndex ? startIndex - endIndex + 1 : endIndex - startIndex + 1;

        if (startIndex == -1 && endIndex == -1)
            rangeCount = 0;
        else if (startIndex == -1 || endIndex == -1)
            rangeCount = 1;

        return rangeCount > 0 ? items.GetRange(rangeStart, rangeCount) : [];
    }

    public void ClearSelection()
    {
        var items = GetTreeViewItems(this, true);
        foreach (var treeViewItem in items)
            SetIsItemSelected(treeViewItem, false);
    }

    #endregion Utility Methods
}
