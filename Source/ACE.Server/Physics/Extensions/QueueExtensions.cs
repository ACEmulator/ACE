using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extensions for <see cref="System.Collections.Generic.Queue{T}"/>.
/// </summary>
public static class QueueExtensions
{
    /// <summary>
    /// Removes the last item from the <see cref="System.Collections.Generic.Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object in <see cref="System.Collections.Generic.Queue{T}"/>.</typeparam>
    /// <param name="q">Instance of <see cref="System.Collections.Generic.Queue{T}"/> to remove item from.</param>
    /// <returns>Item removed from the <see cref="System.Collections.Generic.Queue{T}"/>.</returns>
    public static T DequeueLast<T>(this Queue<T> q)
    {
        for (var i = 1; i < q.Count; i++)
            q.Enqueue(q.Dequeue());

        return q.Dequeue();
    }

    /// <summary>
    /// Removes the last <see cref="quantity"/> item(s) from the <see cref="System.Collections.Generic.Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object in <see cref="System.Collections.Generic.Queue{T}"/>.</typeparam>
    /// <param name="q">Instance of <see cref="System.Collections.Generic.Queue{T}"/> to remove item from.</param>
    /// <param name="quantity">Number of items to pop off the end of the <see cref="System.Collections.Generic.Queue{T}"/>.</param>
    /// <returns>Item removed from the <see cref="System.Collections.Generic.Queue{T}"/>.</returns>
    public static IEnumerable<T> DequeueLast<T>(this Queue<T> q, int quantity)
    {
        for (var i = quantity; i < q.Count; i++)
            q.Enqueue(q.Dequeue());

        var poppedItems = new List<T>(quantity);
        for (int i = 0; i < quantity; i++)
            poppedItems.Add(q.Dequeue());

        return poppedItems;
    }

    /// <summary>
    /// Adds an item(<see cref="T"/>) to the start of the <see cref="System.Collections.Generic.Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object in <see cref="System.Collections.Generic.Queue{T}"/>.</typeparam>
    /// <param name="q">Instance of <see cref="System.Collections.Generic.Queue{T}"/> to remove item from.</param>
    public static void EnqueueFirst<T>(this Queue<T> q, T item)
    {
        q.Enqueue(item);
        for (var i = 1; i < q.Count; i++)
            q.Enqueue(q.Dequeue());
    }

    /// <summary>
    /// Adds items(<see cref="T"/>) to the start of the <see cref="System.Collections.Generic.Queue{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of object in <see cref="System.Collections.Generic.Queue{T}"/>.</typeparam>
    /// <param name="q">Instance of <see cref="System.Collections.Generic.Queue{T}"/> to remove item from.</param>
    /// <param name="items">List of items(<see cref="T"/>) to add to the <see cref="System.Collections.Generic.Queue{T}"/>.</param>
    public static void EnqueueFirst<T>(this Queue<T> q, IEnumerable<T> items)
    {
        if (items == null || !items.Any()) return;

        foreach (var item in items)
            q.Enqueue(item);

        for (var i = items.Count(); i < q.Count; i++)
            q.Enqueue(q.Dequeue());
    }
}
