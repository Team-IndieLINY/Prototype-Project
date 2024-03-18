using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerInventory : Inventory
{
    private List<PlayerInventorySlot> _slots;

    private void Awake()
    {
        _slots = GetComponentsInChildren<PlayerInventorySlot>().ToList();
    }

    public override IReadOnlyList<Item> GetItems() 
        => _slots
            .Where(x => x.IsEmpty == false)
            .Select(x => x.Item)
            .ToList();

    protected override bool HasItem(Item item)
        => _slots.Select(x => x.Item).Contains(item);

    protected override bool CanAddItem(Item item)
    {
        var slot = GetSlotAny(item);

        return slot == true;
    }

    protected override void OnOnItemAdded(Item item)
    {
        var slot = GetSlotAny(item);
        Debug.Assert(slot);

        slot.Item = item;
        slot.ItemCount += 1;
        slot.UpdateSlot();
    }

    protected override void OnOnItemRemoved(Item item)
    {
        var slot = GetSlotFromItem(item);

        if (slot)
        {
            slot.ItemCount -= 1;
        }

    }

    [CanBeNull]
    private PlayerInventorySlot GetSlotAny(Item item)
    {
        var arr = _slots
            .Where(x => x.CanAddOrSet(item))
            .ToArray();

        if (arr.Length == 0) return null;

        return arr[0];
    }

    [CanBeNull]
    private PlayerInventorySlot GetSlotFromItem(Item item)
    {
        return _slots.Find(x => x.Item == item);
    }
}
