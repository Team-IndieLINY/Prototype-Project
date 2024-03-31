using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ItemBoxInventory : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _inventoryContainer;
    private VisualElement _inventoryGrid;
    private Button _takeButton;
    private Button _takeAllButton;
    private static Label _itemDetailHeader;
    private static Label _itemDetailBody;
    private bool _isInventoryReady;

    [FormerlySerializedAs("ItemBoxItems")]
    public List<StoredItem> _itemBoxItems = new List<StoredItem>();
    
    [FormerlySerializedAs("StoredItems")]
    private List<StoredItem> _storedItems = new List<StoredItem>();
    [FormerlySerializedAs("InventoryDimensions")]
    public Dimensions inventoryDimensions;
    public static Dimensions SlotDimension { get; private set; }
    
    private void Awake()
    {
        Configure();
    }
    
    public void OpenInventory()
    {
        Test1(); //아이템 박스에서 아이템 로딩하는 함수
        _inventoryContainer.style.visibility = Visibility.Visible;

        foreach (var item in _storedItems)
        {
            item.RootVisual.style.visibility = Visibility.Visible;
        }
    }

    public void CloseInventory()
    {
        foreach (var item in _storedItems)
        {
            item.RootVisual.style.visibility = Visibility.Hidden;
        }
        
        _inventoryContainer.style.visibility = Visibility.Hidden;
        
        ResetInventory();
    }

    public void AddItemToInventory(ItemDefinition item)
    {
        StoredItem storedItem = new StoredItem();
        storedItem.Details = item;

        ItemVisual itemVisual = new ItemVisual(item);
        ConfigureInventoryItem(storedItem, itemVisual);
        
        AddItemToInventoryGrid(itemVisual);
        
        //꽉 찾을 때 처리해야함
        
        _storedItems.Add(storedItem);
    }
    
    public void RemoveItemToInventory(ItemDefinition item)
    {
        StoredItem removedStoredItem = null;
        
        foreach (var storedItem in _storedItems)
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
            _storedItems.Remove(removedStoredItem);
        }
    }
    
    private async void Configure()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _inventoryContainer = _root.Q<VisualElement>("Container");
        _inventoryGrid = _root.Q<VisualElement>("Grid");
        _takeButton = _root.Q<Button>("TakeButton");
        _takeAllButton = _root.Q<Button>("TakeAllButton");
        
        _takeButton.RegisterCallback<ClickEvent>(OnClickTakeButton);
        
        // VisualElement itemDetails = m_Root.Q<VisualElement>("ItemDetails");
        // m_ItemDetailHeader = itemDetails.Q<Label>("Header");
        // m_ItemDetailBody = itemDetails.Q<Label>("Body");
        await UniTask.WaitForEndOfFrame();
        ConfigureSlotDimensions();
        _isInventoryReady = true;
    }
    private void ConfigureSlotDimensions()
    {
        VisualElement firstSlot = _inventoryGrid.Children().First();
        SlotDimension = new Dimensions
        {
            Width = Mathf.RoundToInt(firstSlot.worldBound.width),
            Height = Mathf.RoundToInt(firstSlot.worldBound.height)
        };
    }
    
    private async Task<bool> GetPositionForItem(VisualElement newItem)
    {
        for (int y = 0; y < inventoryDimensions.Height; y++)
        {
            for (int x = 0; x < inventoryDimensions.Width; x++)
            {
                //try position
                SetItemPosition(newItem, new Vector2(SlotDimension.Width * x, 
                    SlotDimension.Height * y));
                await UniTask.WaitForEndOfFrame();
                StoredItem overlappingItem = _storedItems.FirstOrDefault(s => 
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

    private void Test1()
    {
        LoadInventory(_itemBoxItems);
    }
    
    private async void LoadInventory(List<StoredItem> itemBoxStoredItems)
    {
        await UniTask.WaitUntil(() => _isInventoryReady);
        foreach (StoredItem loadedItem in itemBoxStoredItems)
        {
            _storedItems.Add(loadedItem);
            
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
    
    private void ResetInventory()
    {
        foreach (var storedItem in _storedItems)
        {
            _inventoryGrid.Remove(storedItem.RootVisual);
        }
        // _storedItems.Clear();
    }

    private void AddItemToInventoryGrid(VisualElement item) => _inventoryGrid.Add(item);
    private void RemoveItemFromInventoryGrid(VisualElement item) => _inventoryGrid.Remove(item);
    private static void ConfigureInventoryItem(StoredItem item, ItemVisual visual)
    {
        item.RootVisual = visual;
        visual.style.visibility = Visibility.Hidden;
    }

    private void OnClickTakeButton(ClickEvent evt)
    {
        PlayerInventory.Instance.AddItemToInventory(ItemSelector.Instance.SelectingItem);
        RemoveItemToInventory(ItemSelector.Instance.SelectingItem);
    }
}