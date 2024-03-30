using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IndieLINY.Event;
using UnityEngine;

/// <summary>
/// 인벤토리 연계 알아서 하도록
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ItemBox : MonoBehaviour, IBObjectItemBox
{
    [SerializeField] private Sprite _opend;
    [SerializeField] private Sprite _closed;
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private float _openDelaySec;
    [SerializeField] private List<ItemBoxSlot> _items;
    public CollisionInteraction Interaction { get; private set; }
    
    public bool IsOpened { get; private set; }
    
    /// <summary>
    /// Task가 완료되면 상자가 열린 것.
    /// UniTask는 메인스레드에서 돌아가므로 비동기 관련 문제는 걱정하지 말길 바람
    /// </summary>
    public async UniTask<List<ItemBoxSlot>> Open()
    {
        if (IsOpened) return Items;
        
        await UniTask.Delay((int)(OpenDelaySec * 1000f));
        _renderer.sprite = _opend;

        IsOpened = true;
        return Items;
    }

    public List<ItemBoxSlot> Items => _items;
    public float OpenDelaySec => _openDelaySec;

    private void Awake()
    {
        if (_renderer == false)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        _renderer.sprite = _closed;

        Interaction = GetComponentInChildren<CollisionInteraction>();
        Interaction.SetContractInfo(ObjectContractInfo.Create(transform, ()=>gameObject));

        if (Interaction.TryGetContractInfo(out ObjectContractInfo info))
        {
            info.AddBehaivour<IBObjectItemBox>(this);
        }
    }
    
}
