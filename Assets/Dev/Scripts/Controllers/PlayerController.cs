using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using IndieLINY.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XRProject.Utils.Log;

[Serializable]
public class PlayerSpriteDir
{
    public Sprite Sprite;
    public Vector2Int Dir;
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private SteminaView _steminaView;
    [SerializeField] private PlayerSpriteDir[] _sprites;

    // TODO: it's dummy code, delete later
    [SerializeField] private Image _sprintSteminaFillImage;
    
    public ActorSteminaData _steminaData;
    
    public float MovementSpeed;
    public bool UsePresetRigidBodyOptions;
    public float ItemCollectRadius;

    public Rigidbody2D Rigid2D;
    public PlayerInventory Inventory;
    public SteminaController Stemina { get; private set; }

    public bool IsStopped { get; set; }

    [SerializeField]
    private ItemBoxInventory _itemBoxInventory;


    private CancellationTokenSource _itemboxCancelSource;
    
    private void Awake()
    {
        Interaction = GetComponentInChildren<CollisionInteraction>();
        Stemina = new SteminaController(Interaction, _steminaView, _steminaData);
        
        if (Interaction == false)
        {
            XLog.LogError("PlayerController에서 CollisionInteraction를 찾을 수 없습니다.", "default");
            return;
        }
        
        Interaction.SetContractInfo(ActorContractInfo.Create(
            transform,
            ()=>gameObject == false
            ));
        
        if(Interaction.ContractInfo is ActorContractInfo info)
        {
            info
                .AddBehaivour<IBActorStemina>(Stemina)
                ;
        }
    }

    private void Start()
    {
        if (Rigid2D == false)
            Rigid2D = GetComponent<Rigidbody2D>();

        if (UsePresetRigidBodyOptions)
        {
            Rigid2D.simulated = true;
            Rigid2D.gravityScale = 0f;
            Rigid2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            Rigid2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        StartCoroutine(Stemina.UpdatePerSec());
    }

    private void Update()
    {
        Move();

        WorldInteraction();
        SelfInteraction();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Test");
        }

        if (Stemina.Properties.GetValue<int>(EStatCode.Health) <= 0)
        {
            SceneManager.LoadScene("Test");
        }
    }

    private bool _isOpen = false;
    private void SelfInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerInventory.Instance.CloseInventory();
            ItemBoxInventory.Instance.CloseInventory();
            _isOpen = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isOpen)
            {
                PlayerInventory.Instance.CloseInventory();
                ItemBoxInventory.Instance.CloseInventory();
                _isOpen = false;
            }
            else
            {
                PlayerInventory.Instance.OpenInventory();
                _isOpen = true;
            }

        }
        
        if (Input.GetKeyDown(KeyCode.E) == false) return;
        // if(Inventory.Cursor.TryGetItem(out var item))
        // {
        //     Inventory.RemoveItem(item);
        //     Stemina.Eat(item);
        //     
        //     if (TryGetComponent(out AudioSource source))
        //     {
        //         source.Play();
        //     }
        // }
    }
    
    private void WorldInteraction()
    {
        var cols = Physics2D.OverlapCircleAll(transform.position, ItemCollectRadius);
        Collider2D collider = null;
        float dis = Mathf.Infinity;
        foreach (var col in cols)
        {
            if (col.TryGetComponent<CollisionInteraction>(out var tInteraction))
            {
                if (tInteraction == this.Interaction) continue;
                float tempDis = Vector2.Distance(col.transform.position, transform.position);
                if (tempDis <= dis)
                {
                    collider = col;
                    dis = tempDis;
                }
            }
        }
        if (collider == null) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hits = Physics2D.RaycastAll(
            ray.origin,
            ray.direction,
            Mathf.Infinity
        );

        if (Input.GetMouseButtonDown(0))
        {
            var results = hits
                    .Where(x =>
                    {
                        return x.transform.TryGetComponent<CollisionInteraction>(out var interaction) &&
                               interaction.TryGetContractInfo(out ObjectContractInfo info) &&
                               info.TryGetBehaviour(out IBObjectItemBox box);
                    })
                    .Select(x =>
                    {
                        if (x.transform.TryGetComponent<CollisionInteraction>(out var interaction) &&
                            interaction.TryGetContractInfo(out ObjectContractInfo info) &&
                            info.TryGetBehaviour(out IBObjectItemBox box))
                        {
                            return box;
                        }
                
                        Debug.Assert(false);
                        return null;
                    })
                    .ToList()
                ;
            if (results.Count > 0)
            {
                IsStopped = true;
                _itemboxCancelSource = new CancellationTokenSource();
                results[0].Open(_itemboxCancelSource).ContinueWith(DoInteractItemBox);
            }
        }
        
        
        if (collider.TryGetComponent<CollisionInteraction>(out var ttInteraction))
        {
            ObjectContractInfo objInfo = null;
            // 키 누르지 않아도 작동되는 코드
            if(ttInteraction.TryGetContractInfo(out objInfo))
            {
                if (objInfo.TryGetBehaviour(out IBObjectHighlight highlight))
                {
                    highlight.Highlight = true;
                    highlight.IsResetNextFrame = true;
                }
            }
            
            // 키 눌러야 작동되는 코드
            if (Input.GetKeyDown(KeyCode.F) == false) return;
            if(ttInteraction.TryGetContractInfo(out objInfo))
            {
                if (objInfo.TryGetBehaviour(out IBObjectFieldItem item))
                {
                    DoInteractFieldItem(item);
                }
                if (objInfo.TryGetBehaviour(out IBObjectItemBox itemBox))
                {
                    IsStopped = true;
                    _itemboxCancelSource = new CancellationTokenSource();
                    itemBox.Open(_itemboxCancelSource).ContinueWith(DoInteractItemBox);
                }
            }
            else if (ttInteraction.TryGetContractInfo(out ActorContractInfo actorInfo) &&
                     actorInfo.TryGetBehaviour<IBActorStemina>(out var stemina))
            {
                DoInteractNPC(stemina);
            }
        }
    }

    /// <summary>
    /// 인벤토리 연계는 이쪽에서 하길 바람
    /// 플레이어가 상자를 열었을 때, 상자에 들어있는 아이템을 넘겨받음
    /// </summary>
    /// <param name="itemList"></param>
    private void DoInteractItemBox(ItemBox itemBox)
    {
        if (itemBox == null)
        {
            IsStopped = false;
            return;
        }

        IsStopped = false;
        _isOpen = true;
        Inventory.OpenInventory();
        
        _itemBoxInventory.CloseInventory();

        _itemBoxInventory.OpenInventory(itemBox);

        
    }

    private void DoInteractFieldItem(IBObjectFieldItem item)
    {
        PlayerInventory.Instance.AddItemToInventory(item.Item);
        item.Collect();
    }

    private void DoInteractNPC(IBActorStemina stemina)
    {
        // if (Inventory.Cursor.TryGetItem(out var item))
        // {
        //     stemina.Eat(item);
        //     Inventory.RemoveItem(item);
        // }
    }
    
    private void Move()
    {
        
        var dir = new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };

        if (dir.sqrMagnitude > 0f)
        {
            _itemboxCancelSource?.Cancel();
        }
        
        if (IsStopped) return;

        Vector2Int iDir = Vector2Int.zero;

        if (dir.x > 0f) iDir.x = 1;
        else if (dir.x < 0f) iDir.x = -1;
        else iDir.x = 0;
        
        if (dir.y > 0f) iDir.y = 1;
        else if (dir.y < 0f) iDir.y = -1;
        else iDir.y = 0;

        int x = iDir.x;
        int y = iDir.y;

        foreach (var spriteDir in _sprites)
        {
            if (x == spriteDir.Dir.x && y == spriteDir.Dir.y)
            {
                if(TryGetComponent<SpriteRenderer>(out var renderer))
                {
                    renderer.sprite = spriteDir.Sprite;
                }
            }
        }


        var curSpeed = MovementSpeed;
        var currentStemina = Stemina.Properties.GetRef<float>(EStatCode.Stemina);
        if (Input.GetKey(KeyCode.LeftShift) && currentStemina.Value > 0)
        {
            currentStemina.Value -= _steminaData.DecraseSprintSteminaPerSec * Time.deltaTime;
            curSpeed *= 1.5f;
        }
        else
        {
            currentStemina.Value += _steminaData.IncreaseSprintSteminaPerSec* Time.deltaTime;
        }

        _sprintSteminaFillImage.fillAmount = (float)currentStemina.Value / _steminaData.MaxSprintStemina;
        
    
        Rigid2D.velocity = dir * curSpeed;
    }

    #region ActorBehviour
    public CollisionInteraction Interaction { get; private set; }
    [SerializeField] 
    private float _maxHp;
    
    #region HP
    public float HP { get; private set; }
    public float MaxHP => _maxHp;
    public void Heal(float healValue)
    {
        HP += healValue;
    }
    #endregion
    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ItemCollectRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _steminaData.BeginIncreaseTemperatureRadius);
        
    }
}