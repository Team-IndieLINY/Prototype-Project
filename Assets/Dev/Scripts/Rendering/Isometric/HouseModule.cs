using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HouseModule : MonoBehaviour
{
    [NonSerialized] public List<SpriteRenderer> fronts = new();
    [NonSerialized] public List<SpriteRenderer> backs = new();
    [NonSerialized] public List<SpriteRenderer> floors = new();
    [NonSerialized] public List<SpriteRenderer> fronts_collider = new();
    [NonSerialized] public List<SpriteRenderer> backs_collider = new();
    [NonSerialized] public List<SpriteRenderer> inners = new();
    [NonSerialized] public StairLine[] stairs;

    public int floor;

    private void Awake()
    {
        var objects = GetComponentsInChildren<OrderedObject>(true);

        foreach (var obj in objects)
        {
            if (obj == false) continue;

            List<SpriteRenderer> addingArray = null;

            switch (obj.Type)
            {
                case EOrderedObjectType.Front:
                    addingArray = fronts;
                    break;
                case EOrderedObjectType.FrontCollision:
                    addingArray = fronts_collider;
                    break;
                case EOrderedObjectType.Back:
                    addingArray = backs;
                    break;
                case EOrderedObjectType.BackCollision:
                    addingArray = backs_collider;
                    break;
                case EOrderedObjectType.Floor:
                    addingArray = floors;
                    break;
                case EOrderedObjectType.Stair:
                    break;
                case EOrderedObjectType.Inner:
                    addingArray = inners;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            
            Debug.Assert(addingArray != null) ;
            addingArray.Add(obj.Renderer);
        }
    }
}
