using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

// Source https://stackoverflow.com/questions/1427471/observablecollection-not-noticing-when-item-in-it-changes-even-with-inotifyprop/32013610

namespace pkNX.WinForms;

public sealed class NotifyObservableCollection<T> : ObservableCollection<T>
    where T : INotifyPropertyChanged
{
    public NotifyObservableCollection()
    {
        CollectionChanged += FullObservableCollectionCollectionChanged;
    }

    public NotifyObservableCollection(IEnumerable<T> pItems) : this()
    {
        foreach (var item in pItems)
        {
            Add(item);
        }
    }

    private void FullObservableCollectionCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (object item in e.NewItems)
            {
                ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            }
        }
        if (e.OldItems != null)
        {
            foreach (object item in e.OldItems)
            {
                ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }

        OnPropertyChanged(new PropertyChangedEventArgs("Items"));
    }

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        NotifyCollectionChangedEventArgs args = new(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
        OnCollectionChanged(args);
    }

    /// <summary> 
    /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
    /// </summary> 
    public void AddRange(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        foreach (var i in collection) Items.Add(i);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary> 
    /// Removes the first occurrence of each item in the specified collection from ObservableCollection(Of T). 
    /// </summary> 
    public void RemoveRange(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        foreach (var i in collection) Items.Remove(i);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary> 
    /// Clears the current collection and replaces it with the specified item. 
    /// </summary> 
    public void Replace(T item)
    {
        ReplaceRange(new[] { item });
    }

    /// <summary> 
    /// Clears the current collection and replaces it with the specified collection. 
    /// </summary> 
    public void ReplaceRange(IEnumerable<T> collection)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        Items.Clear();
        foreach (var i in collection) Items.Add(i);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
