using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class ItemVisual : VisualElement
{
    private readonly ItemDefinition m_Item;
    public ItemVisual(ItemDefinition item)
    {
        m_Item = item;
        name = $"{m_Item.FriendlyName}";
        style.height = m_Item.SlotDimension.Height * 
            PlayerInventory.SlotDimension.Height;
        style.width = m_Item.SlotDimension.Width * 
            PlayerInventory.SlotDimension.Width;
        style.visibility = Visibility.Hidden;
        VisualElement icon = new VisualElement
        {
            style = { backgroundImage = m_Item.Icon.texture }
        };
        Add(icon);
        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");
        RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }

    ~ItemVisual()
    {
        UnregisterCallback<MouseDownEvent>(OnMouseDownEvent);
    }
    
    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.top = pos.y;
    }
    
    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        if (m_Item == null)
        {
            Debug.Log("m_Item is null");
            return;
        }

        if (panel.visualTree.name != "ItemBoxInventoryPanelSetting")
        {
            return;
        }
        
        ItemBoxInventory.Instance.UnhighlightAllItem();
        AddToClassList("visual-icon-container-highlighted");
        ItemSelector.Instance.SetSelectingItem(m_Item);
    }
}