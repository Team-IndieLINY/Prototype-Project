
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBActorHP : IActorBehaviour
{
    public float HP { get; }
    public float MaxHP { get; }
    public void Heal(float healValue);
}

public interface IBActorStemina : IActorBehaviour
{
    public void Eat();
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public Item Item { get; }
    public void Collect();
}