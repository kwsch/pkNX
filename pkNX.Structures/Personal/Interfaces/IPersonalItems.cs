using System;

namespace pkNX.Structures;

public interface IPersonalItems
{
    int Item1 { get; set; }
    int Item2 { get; set; }
    int Item3 { get; set; }
}

public static class IPersonalItemExtensions
{
    /// <summary>
    /// Gets the index of the <see cref="itemID"/> within the specification's list of items.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="itemID">Item ID</param>
    /// <returns>Item Index or -1 if not found</returns>
    public static int GetIndexOfItem(this IPersonalItems a, int itemID) => itemID == a.Item1 ? 0 : itemID == a.Item2 ? 1 : itemID == a.Item3 ? 2 : -1;

    public static void GetItems(this IPersonalItems a, Span<int> result)
    {
        result[0] = a.Item1;
        result[1] = a.Item2;
        result[2] = a.Item3;
    }

    public static void SetItems(this IPersonalItems a, Span<int> result)
    {
        a.Item1 = result[0];
        a.Item2 = result[1];
        a.Item3 = result[2];
    }

    /// <summary>
    /// Gets the requested item value with the requested <see cref="index"/>.
    /// </summary>
    public static int GetItemAtIndex(this IPersonalItems a, int index) => index switch
    {
        0 => a.Item1,
        1 => a.Item2,
        2 => a.Item3,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <summary>
    /// Sets the requested item value with the requested <see cref="index"/>.
    /// </summary>
    public static int SetItemAtIndex(this IPersonalItems a, int index, int value) => index switch
    {
        0 => a.Item1 = value,
        1 => a.Item2 = value,
        2 => a.Item3 = value,
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    /// <summary>
    /// Gets the total number of items available.
    /// </summary>
    /// <remarks>Duplicate items still count separately.</remarks>
    public static int GetNumItems(this IPersonalItems _) => 3;

    public static void ImportIPersonalItems(this IPersonalItems self, IPersonalItems other)
    {
        for (int j = 0; j < other.GetNumItems(); ++j)
            self.SetItemAtIndex(j, other.GetItemAtIndex(j));
    }
}
