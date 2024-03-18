using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;
using XRProject.Utils.Log;

public class FieldItem : MonoBehaviour, IBObjectFieldItem
{
    [SerializeField] private Item _item;
    
    private void Awake()
    {
        Interaction = GetComponentInChildren<CollisionInteraction>();
        if (Interaction == false)
        {
            XLog.LogError("FieldItem CollisionInteraction를 찾을 수 없습니다.", "default");
            return;
        }
        
        Interaction.SetContractInfo(ObjectContractInfo.Create(
            transform,
            ()=>gameObject == false
        ));
        
        if(Interaction.ContractInfo is ObjectContractInfo info)
        {
            info
                .AddBehaivour<IBObjectFieldItem>(this)
                ;
        }
    }

    public CollisionInteraction Interaction { get; private set; }
    public Item Item => _item;

    public void Collect()
    {
        Destroy(gameObject);
    }
}
