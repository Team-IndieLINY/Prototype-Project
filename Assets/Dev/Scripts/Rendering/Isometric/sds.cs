using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class sds : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("asdasdasd"))
        {
            var objs =GetComponentsInChildren<OrderedObject>();

            var front = new GameObject("Front");
            front.transform.SetParent(transform);

            var front_collider = new GameObject("Collision");
            front_collider.transform.SetParent(front.transform);
            
            var back = new GameObject("Back");
            back.transform.SetParent(transform);

            var back_collider = new GameObject("Collision");
            back_collider.transform.SetParent(back.transform);
            
            var ceil = new GameObject("Ceil");
            ceil.transform.SetParent(transform);


            foreach (var obj in objs)
            {
                    switch (obj.Type)
                {
                    case EOrderedObjectType.Front:
                        obj.transform.SetParent(front.transform);
                        break;
                    case EOrderedObjectType.FrontCollision:
                        obj.transform.SetParent(front_collider.transform);
                        break;
                    case EOrderedObjectType.Back:
                        obj.transform.SetParent(back.transform);
                        break;
                    case EOrderedObjectType.BackCollision:
                        obj.transform.SetParent(back_collider.transform);
                        break;
                    case EOrderedObjectType.Floor:
                        obj.transform.SetParent(ceil.transform);
                        break;
                    case EOrderedObjectType.Stair:
                        break;
                    case EOrderedObjectType.Inner:
                        break;
                }
            }
        }
    }
}
