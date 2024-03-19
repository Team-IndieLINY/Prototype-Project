using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


public class PlayerInventoryCursor
{
    private List<PlayerInventorySlot> _slots;
    private int _cursor;
    public int CursorIndex => _cursor;

    [CanBeNull]
    public Item GetItem()
    {
        Debug.Assert(_slots != null);
        Debug.Assert(_slots.Count > _cursor && _cursor >= 0);
            
        return _slots[_cursor].Item;
    }

    public bool TryGetItem(out Item item)
    {
        item = GetItem();

        return item;
    }

    public void ShiftCursor(int dir)
    {
        _cursor += dir;
        if (_slots.Count <= _cursor)
        {
            _cursor = 0;
        }
        if (_cursor < 0)
        {
            _cursor = _slots.Count - 1;
        }
    }

    public void MoveCursor(int index)
    {
        if (_slots.Count <= index || index < 0) return;
        _cursor = index;
    }

    public PlayerInventoryCursor(List<PlayerInventorySlot> list)
    {
        _slots = list;
        _cursor = 0;
    }
}

public class PlayerInventory : Inventory
{
    
    private List<PlayerInventorySlot> _slots;
    public PlayerInventoryCursor Cursor { get; private set; }

    private void Awake()
    {
        _slots = GetComponentsInChildren<PlayerInventorySlot>().ToList();
        Cursor = new PlayerInventoryCursor(_slots);
    }

    private void Update()
    {
        var scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        _slots[Cursor.CursorIndex].Highlight(false);
        if (scrollAxis > 0f)
        {
            Cursor.ShiftCursor(-1);
        }
        if (scrollAxis < 0f)
        {
            Cursor.ShiftCursor(1);
        }
        _slots[Cursor.CursorIndex].Highlight(true);

        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                _slots[Cursor.CursorIndex].Highlight(false);
                Cursor.MoveCursor(i == 0 ? 10 : i - 1);
                _slots[Cursor.CursorIndex].Highlight(true);
            }
        }
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

    protected override void ItemAdded(Item item)
    {
        var slot = GetSlotAny(item);
        Debug.Assert(slot);

        slot.Item = item;
        slot.ItemCount += 1;
        slot.UpdateSlot();
    }

    protected override void ItemRemoved(Item item)
    {
        var slot = GetSlotFromItem(item);

        if (slot)
        {
            slot.ItemCount -= 1;
            slot.UpdateSlot();
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
