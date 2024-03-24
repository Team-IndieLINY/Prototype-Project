using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PannelItemSlotModel : IInventoryModel
{
    private Item[,] _array;

    public PannelItemSlotModel(Vector2Int size)
    {
        _array = new Item[size.x, size.y];
    }

    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < _array.GetLength(1) &&
               position.y >= 0 && position.y < _array.GetLength(0)
            ;
    }

    [CanBeNull]
    public Item GetItem(Vector2Int position)
    {
        if (IsValidPosition(position) == false) return null;

        return _array[position.x, position.y];
    }
    

    public bool TryGetItem(Vector2Int position, out Item item)
    {
        item = GetItem(position);

        return item;
    }
    
    public bool SetItem(Vector2Int position, Item item)
    {
        if (IsValidPosition(position) == false) return false;

        _array[position.x, position.y] = item;
        return true;
    }
}
