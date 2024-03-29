using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySlot : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _slotItemImage;
    [SerializeField] private TMP_Text _slotItemCountText;

    public Item Item { get; set; }

    public int ItemCount{ get; set; }

    public bool IsEmpty => ItemCount == 0;

    private void Awake()
    {
        UpdateSlot();
    }
    public bool CanAddOrSet(Item item)
    {
        if (IsEmpty) return true;
        if (item == Item) return true;

        return false;
    }

    public void Highlight(bool enable)
    {
        if(enable)
            _backgroundImage.color = Color.red;
        else
            _backgroundImage.color = Color.white;
    }
    
    public void UpdateSlot()
    {
        if (ItemCount <= 0)
        {
            Item = null;
        }
        
        if(Item)
        {
            _slotItemCountText.text = $"{ItemCount}\n{Item.Name}";
            _slotItemImage.sprite = Item.Icon;
        }
        else
        {
            _slotItemCountText.text = $"";
            _slotItemImage.sprite = null;
        }
        
    }
}
