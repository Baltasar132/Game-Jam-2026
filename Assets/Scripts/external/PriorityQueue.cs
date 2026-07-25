using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly List<(TElement Element, TPriority Priority)> _nodes = new List<(TElement, TPriority)>();

    public int Count => _nodes.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        _nodes.Add((element, priority));
        HeapifyUp(_nodes.Count - 1);
    }

    public TElement Dequeue()
    {
        if (_nodes.Count == 0)
            throw new InvalidOperationException("La cola está vacía.");

        TElement rootElement = _nodes[0].Element;
        int lastIndex = _nodes.Count - 1;
        _nodes[0] = _nodes[lastIndex];
        _nodes.RemoveAt(lastIndex);

        if (_nodes.Count > 0)
            HeapifyDown(0);

        return rootElement;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (_nodes[index].Priority.CompareTo(_nodes[parentIndex].Priority) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = _nodes.Count - 1;
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild <= lastIndex && _nodes[leftChild].Priority.CompareTo(_nodes[smallest].Priority) < 0)
                smallest = leftChild;

            if (rightChild <= lastIndex && _nodes[rightChild].Priority.CompareTo(_nodes[smallest].Priority) < 0)
                smallest = rightChild;

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        var temp = _nodes[i];
        _nodes[i] = _nodes[j];
        _nodes[j] = temp;
    }
}