using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public abstract IReadOnlyList<Item> GetItems();
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemRemoved;
    
    public bool AddItem(Item item)
    {
        Debug.Assert(item == true);
        
        if (CanAddItem(item) == false) return false;
        
        OnOnItemAdded(item);
        OnItemAdded?.Invoke(item);

        return true;
    }

    public void RemoveItem(Item item)
    {
        Debug.Assert(item == true);
        
        OnOnItemRemoved(item);
        OnItemRemoved?.Invoke(item);
    }

    protected abstract bool HasItem(Item item);
    
    protected abstract bool CanAddItem(Item item);

    protected abstract void OnOnItemAdded(Item item);

    protected abstract void OnOnItemRemoved(Item item);
}
