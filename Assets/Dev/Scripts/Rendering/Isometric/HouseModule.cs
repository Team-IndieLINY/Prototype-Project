using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HouseModule : MonoBehaviour
{
    [HideInInspector] public List<SpriteRenderer> fronts;
    [HideInInspector] public List<SpriteRenderer> backs;
    [HideInInspector] public List<SpriteRenderer> floors;
    [HideInInspector] public List<SpriteRenderer> fronts_collider;
    [HideInInspector] public List<SpriteRenderer> backs_collider;
    [HideInInspector] public StairLine[] stairs;

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
                default:
                    Debug.Assert(false);
                    break;
            }
            
            addingArray.Add(obj.Renderer);
        }
    }
}
