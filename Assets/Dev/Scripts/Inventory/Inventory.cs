using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Inventory : MonoBehaviour
{
    private VisualElement m_Root;
    private VisualElement m_InventoryContainer;
    private VisualElement m_InventoryGrid;
    private static Label m_ItemDetailHeader;
    private static Label m_ItemDetailBody;
    private bool m_IsInventoryReady;
    public List<StoredItem> StoredItems = new List<StoredItem>();
    public Dimensions InventoryDimensions;
    public static Dimensions SlotDimension { get; private set; }
    
    public void OpenInventory()
    {
        m_InventoryContainer.style.visibility = Visibility.Visible;

        foreach (var item in StoredItems)
        {
            item.RootVisual.style.visibility = Visibility.Visible;
        }
    }

    public void CloseInventory()
    {
        foreach (var item in StoredItems)
        {
            item.RootVisual.style.visibility = Visibility.Hidden;
        }
        
        m_InventoryContainer.style.visibility = Visibility.Hidden;
    }
    
    public void AddItemToInventory(ItemDefinition item)
    {
        StoredItem storedItem = new StoredItem();
        storedItem.Details = item;

        ItemVisual itemVisual = new ItemVisual(item);
        ConfigureInventoryItem(storedItem, itemVisual);
        
        AddItemToInventoryGrid(itemVisual);
        
        //꽉 찾을 때 처리해야함
        
        StoredItems.Add(storedItem);
    }
    
    public void RemoveItemToInventory(ItemDefinition item)
    {
        StoredItem removedStoredItem = null;
        
        foreach (var storedItem in StoredItems)
        {
            if (storedItem.Details.Equals(item))
            {
                removedStoredItem = storedItem;
                RemoveItemFromInventoryGrid(storedItem.RootVisual);
                break;
            }
        }

        if (removedStoredItem != null)
        {
            StoredItems.Remove(removedStoredItem);
        }
    }
    
    private void AddItemToInventoryGrid(VisualElement item) => m_InventoryGrid.Add(item);
    private void RemoveItemFromInventoryGrid(VisualElement item) => m_InventoryGrid.Remove(item);
    private static void ConfigureInventoryItem(StoredItem item, ItemVisual visual)
    {
        item.RootVisual = visual;
        visual.style.visibility = Visibility.Visible;
    }
}
