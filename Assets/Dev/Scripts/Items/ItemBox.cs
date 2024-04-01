using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using IndieLINY.Event;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리 연계 알아서 하도록
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ItemBox : MonoBehaviour, IBObjectItemBox, IBObjectHighlight
{
    [SerializeField] private Image _progress;
    
    [SerializeField] private Sprite _opend;
    [SerializeField] private Sprite _closed;
    [SerializeField] private SpriteRenderer _renderer;
    
    [SerializeField] private float _openDelaySec;
    [SerializeField] private List<ItemDefinition> _items;

    [SerializeField] private SpriteRenderer _outlineRenderer;
    public CollisionInteraction Interaction { get; private set; }
    
    public bool IsOpened { get; private set; }
    
    /// <summary>
    /// Task가 완료되면 상자가 열린 것.
    /// UniTask는 메인스레드에서 돌아가므로 비동기 관련 문제는 걱정하지 말길 바람
    /// </summary>
    public async UniTask<ItemBox> Open(CancellationTokenSource source)
    {
        if (IsOpened) return this;

        await CoOpen(source, OpenDelaySec).ToUniTask(coroutineRunner: this);

        if (source != null && source.IsCancellationRequested)
        {
            return null;
        }
        
        _renderer.sprite = _opend;

        IsOpened = true;
        return this;
    }

    private IEnumerator CoOpen(CancellationTokenSource source, float delay)
    {
        if (_progress == false) yield break;

        var wait = new WaitForEndOfFrame();
        float timer = 0f;
        _progress.gameObject.SetActive(true);
        var bar = _progress.transform.GetChild(0).GetComponentInChildren<Image>();
        
        
        while (timer < delay)
        {
            if (source.Token.IsCancellationRequested)
            {
                break;
            }
            bar.fillAmount = 1f - (timer / delay);
            timer += Time.deltaTime;
            
            yield return wait;
        }
        
        _progress.gameObject.SetActive(false);
    }

    public List<ItemDefinition> Items => _items;
    public float OpenDelaySec => _openDelaySec;

    private void Awake()
    {
        if (_renderer == false)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        if (_progress)
        {
            _progress.gameObject.SetActive(false);
        }

        _renderer.sprite = _closed;

        Interaction = GetComponentInChildren<CollisionInteraction>();
        Interaction.SetContractInfo(ObjectContractInfo.Create(transform, ()=>gameObject));

        if (Interaction.TryGetContractInfo(out ObjectContractInfo info))
        {
            info.AddBehaivour<IBObjectItemBox>(this);
            info.AddBehaivour<IBObjectHighlight>(this);
        }
    }

    private void Update()
    {
        _outlineRenderer.sortingOrder = _renderer.sortingOrder;
        var c = _outlineRenderer.color;
        c.a = _renderer.color.a;
        _outlineRenderer.color = c;

    }

    public bool CanInteractable { get; set; } = true;

    public bool Highlight
    {
        get
        {
            if (_outlineRenderer)
            {
                return _outlineRenderer.enabled;
            }

            return false;
        }

        set
        {
            if (CanInteractable == false) return;
            
            if (_outlineRenderer)
            {
                _outlineRenderer.enabled = value;
            }
        }
    }

    public void SetItemBoxItems(List<ItemDefinition> items)
    {
        _items = items;
    }

    private IEnumerator _co;
    private IEnumerator CoUpdate()
    {
        var wait = new WaitForEndOfFrame();
        
        while (true)
        {
            if (Highlight)
            {
                yield return wait;
                yield return wait;
                yield return wait;
                Highlight = false;
            }
            else
            {
                yield return wait;
            }
        }
    }
    public bool IsResetNextFrame
    {
        get => _co != null;
        set
        {
            if (CanInteractable == false) return;
            
            if (value)
            {
                if (_co == null)
                {
                    StartCoroutine(_co = CoUpdate());
                }
            }
            else
            {
                StopCoroutine(_co);
                _co = null;
            }
        }
    }
}
