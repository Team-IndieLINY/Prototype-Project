
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public interface IBActorStemina : IActorBehaviour
{
    public SteminaProperties Properties { get; }
    public void Eat(Item item);
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public Item Item { get; }
    public void Collect();
}

[Serializable]
public class ItemBoxSlot
{
    public Item Item;
    public Vector2Int Position;
}
public interface IBObjectItemBox : IObjectBehaviour
{
    public List<ItemBoxSlot> Items { get; }
    public UniTask<List<ItemBoxSlot>> Open();
    public float OpenDelaySec { get; }
}