
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBActorStemina : IActorBehaviour
{
    public PropertiesConatiner<StatDataValue, int> StatProperties { get; }
    public void Eat(ItemDefinition item);
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public ItemDefinition Item { get; }
    public void Collect();
}