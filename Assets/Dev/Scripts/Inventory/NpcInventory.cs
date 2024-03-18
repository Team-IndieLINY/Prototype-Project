using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInventory : Inventory
{
    private List<Item> _items = new List<Item>();

    public override IReadOnlyList<Item> GetItems() => _items;

    protected override bool HasItem(Item item)
        => _items.Contains(item);

    protected override bool CanAddItem(Item item)
    {
        return true;
    }

    protected override void ItemAdded(Item item)
    {
        
    }

    protected override void ItemRemoved(Item item)
    {
    }
}
