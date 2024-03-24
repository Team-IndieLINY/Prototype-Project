using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PannelItemSlotView : MonoBehaviour, IInventroyView
{
    [SerializeField] private GraphicRaycaster _raycaster;
    private List<PlayerInventorySlot> _slots;

    // <before pos, after pos, moved item, 반환값: 성공 여부>
    public event Func<Vector2Int, Vector2Int, Item, bool> OnRequireItemMoved;
    // <추가할 위치, 추가할 아이템, 반환값: 추가 성공 여부>
    public event Func<Vector2Int, Item, bool> OnRequireToAddItem;
    // <버리려는 아이템 위치, 버리려는 아이템, 반환값: 버리기 성공 여부>
    public event Func<Vector2Int, Item, bool> OnRequireDropItem;
    
    private void Awake()
    {
        _slots = GetComponentsInChildren<PlayerInventorySlot>().ToList();
    }

    private void Update()
    {
        var results = new List<RaycastResult>();
        _raycaster.Raycast(new PointerEventData(EventSystem.current), results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out PlayerInventorySlot slot))
            {
            }
        }
    }
}
