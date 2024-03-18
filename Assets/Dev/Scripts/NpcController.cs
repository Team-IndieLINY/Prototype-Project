using System;
using System.Collections;
using System.Collections.Generic;
using IndieLINY.Event;
using UnityEngine;

public class NpcController : MonoBehaviour, IBActorStemina
{
    [SerializeField] private NpcInventory _inventory;

    [SerializeField] private CollisionInteraction _interaction;
    public CollisionInteraction Interaction => _interaction;

    private void Awake()
    {
        Interaction.SetContractInfo(ActorContractInfo.Create(
            transform,
            () => gameObject == false)
        );

        if (Interaction.ContractInfo is ActorContractInfo info)
        {
            info
                .AddBehaivour<IBActorStemina>(this)
                ;
        }
    }

    public void Eat()
    {
    }
}
