using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    public ActorSteminaData _steminaData;
    
    public float MovementSpeed;
    public bool UsePresetRigidBodyOptions;
    public float ItemCollectRadius;

    public Rigidbody2D Rigid2D;
    public PlayerInventory Inventory;
    public SteminaController Stemina { get; private set; }

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void SelfInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) == false) return;
        
        if(Inventory.Cursor.TryGetItem(out var item))
        {
            Inventory.RemoveItem(item);
            Stemina.Eat(item);
            
            if (TryGetComponent(out AudioSource source))
            {
                source.Play();
            }
        }
    }

    private void WorldInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) == false) return;
        
        var cols = Physics2D.OverlapCircleAll(transform.position, ItemCollectRadius);

        Collider2D collider = null;
        float dis = Mathf.Infinity;
        foreach (var col in cols)
        {
            if (col.TryGetComponent<CollisionInteraction>(out var tInteraction))
            {
                if (tInteraction == this.Interaction) continue;
            }
            float tempDis = Vector2.Distance(col.transform.position, transform.position);
            if (tempDis <= dis)
            {
                collider = col;
                dis = tempDis;
            }
        }

        if (collider == null) return;
        if (collider.TryGetComponent<CollisionInteraction>(out var ttInteraction))
        {
            if(ttInteraction.TryGetContractInfo(out ObjectContractInfo objInfo) &&
               objInfo.TryGetBehaviour(out IBObjectFieldItem item))
            {
                DoInteractFieldItem(item);
            }
            else if (ttInteraction.TryGetContractInfo(out ActorContractInfo actorInfo) &&
                     actorInfo.TryGetBehaviour<IBActorStemina>(out var stemina))
            {
                DoInteractNPC(stemina);
            }
        }
    }

    private void DoInteractFieldItem(IBObjectFieldItem item)
    {
        Inventory.AddItem(item.Item);
        item.Collect();
    }

    private void DoInteractNPC(IBActorStemina stemina)
    {
        if (Inventory.Cursor.TryGetItem(out var item))
        {
            stemina.Eat(item);
            Inventory.RemoveItem(item);
        }
    }
    
    private void Move()
    {
        var dir = new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };

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

        Rigid2D.velocity = dir * MovementSpeed;
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