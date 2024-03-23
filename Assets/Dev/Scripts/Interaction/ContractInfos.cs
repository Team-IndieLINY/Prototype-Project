
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBActorStemina : IActorBehaviour
{
    public PropertiesConatiner<StatDataValue, int> StatProperties { get; }
    public void Eat(Item item);
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public Item Item { get; }
    public void Collect();
}