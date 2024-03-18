
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBActorStemina : IActorBehaviour
{
    public ActorSteminaData SteminaData { get; }
    public float Food { get; set; }
    public float Health { get; set; }
    public void Eat(Item item);
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public Item Item { get; }
    public void Collect();
}