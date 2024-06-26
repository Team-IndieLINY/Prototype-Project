using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private VisualElement m_Root;
    private VisualElement m_InventoryContainer;
    private VisualElement m_InventoryGrid;
    private static Label m_ItemDetailHeader;
    private static Label m_ItemDetailBody;
    private bool m_IsInventoryReady;
    public List<StoredItem> StoredItems = new List<StoredItem>();
    public Dimensions InventoryDimensions;
    public static Dimensions SlotDimension { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Configure();
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        LoadInventory();
    }
    
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

    public async void AddItemToInventory(ItemDefinition item)
    {
        if (item == null)
        {
            return;
        }
        
        StoredItem storedItem = new StoredItem();
        storedItem.Details = item;

        ItemVisual itemVisual = new ItemVisual(item);
        
        AddItemToInventoryGrid(itemVisual);
        
        bool inventoryHasSpace = await GetPositionForItem(itemVisual);
        if (!inventoryHasSpace)
        {
            Debug.Log("No space - Cannot pick up the item");
            RemoveItemFromInventoryGrid(itemVisual);
        }
        
        ConfigureInventoryItem(storedItem, itemVisual);
        
        itemVisual.style.visibility = Visibility.Visible;
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
    
    private async void Configure()
    {
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_InventoryContainer = m_Root.Q<VisualElement>("Container");
        m_InventoryGrid = m_Root.Q<VisualElement>("Grid");
        // VisualElement itemDetails = m_Root.Q<VisualElement>("ItemDetails");
        // m_ItemDetailHeader = itemDetails.Q<Label>("Header");
        // m_ItemDetailBody = itemDetails.Q<Label>("Body");
        await UniTask.WaitForEndOfFrame();
        ConfigureSlotDimensions();
        m_IsInventoryReady = true;
    }
    private void ConfigureSlotDimensions()
    {
        VisualElement firstSlot = m_InventoryGrid.Children().First();
        SlotDimension = new Dimensions
        {
            Width = Mathf.RoundToInt(firstSlot.worldBound.width),
            Height = Mathf.RoundToInt(firstSlot.worldBound.height)
        };
    }
    
    private async Task<bool> GetPositionForItem(VisualElement newItem)
    {
        for (int y = 0; y < InventoryDimensions.Height; y++)
        {
            for (int x = 0; x < InventoryDimensions.Width; x++)
            {
                //try position
                SetItemPosition(newItem, new Vector2(SlotDimension.Width * x, 
                    SlotDimension.Height * y));
                await UniTask.WaitForEndOfFrame();
                StoredItem overlappingItem = StoredItems.FirstOrDefault(s => 
                    s.RootVisual != null && 
                    s.RootVisual.layout.Overlaps(newItem.layout));
                //Nothing is here! Place the item.
                if (overlappingItem == null)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private static void SetItemPosition(VisualElement element, Vector2 vector)
    {
        element.style.left = vector.x;
        element.style.top = vector.y;
    }
    
    private async void LoadInventory()
    {
        await UniTask.WaitUntil(() => m_IsInventoryReady);
        foreach (StoredItem loadedItem in StoredItems)
        {
            ItemVisual inventoryItemVisual = new ItemVisual(loadedItem.Details);
            
            AddItemToInventoryGrid(inventoryItemVisual);
            bool inventoryHasSpace = await GetPositionForItem(inventoryItemVisual);
            if (!inventoryHasSpace)
            {
                Debug.Log("No space - Cannot pick up the item");
                RemoveItemFromInventoryGrid(inventoryItemVisual);
                continue;
            }
            ConfigureInventoryItem(loadedItem, inventoryItemVisual);
        }
    }

    private void AddItemToInventoryGrid(VisualElement item) => m_InventoryGrid.Add(item);
    private void RemoveItemFromInventoryGrid(VisualElement item) => m_InventoryGrid.Remove(item);
    private static void ConfigureInventoryItem(StoredItem item, ItemVisual visual)
    {
        item.RootVisual = visual;
        visual.style.visibility = Visibility.Hidden;
    }
}