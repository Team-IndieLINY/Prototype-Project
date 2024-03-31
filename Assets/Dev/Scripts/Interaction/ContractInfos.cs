
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public interface IBActorStemina : IActorBehaviour
{
    public SteminaProperties Properties { get; }
    public void Eat(ItemDefinition item);
}

public interface IBObjectFieldItem : IObjectBehaviour
{
    public ItemDefinition Item { get; }
    public void Collect();
}public interface IBObjectHighlight : IObjectBehaviour
{
    public bool Highlight { get; set; }
    public bool  IsResetNextFrame { get;  set;}
}public interface IBObjectItemBox : IObjectBehaviour
{
    public UniTask<List<ItemDefinition>> Open();
}