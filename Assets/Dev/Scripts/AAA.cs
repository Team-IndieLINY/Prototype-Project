using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AAA : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("ksdgsldfsd"))
        {
            List<GameObject> list = new List<GameObject>();
            
            SetObject(transform, list);
            
            foreach (var obj in list)
            {
                if (Regex.IsMatch(obj.name, @"Collider"))
                {
                    obj.layer = LayerMask.NameToLayer("Obstacle");
                    if (obj.GetComponent<NavMeshModifier>() == false)
                    {
                        var com = obj.AddComponent<NavMeshModifier>();
                        com.overrideArea = true;
                        com.area = NavMesh.GetAreaFromName("Not Walkable");
                    }   
                }

                obj.isStatic = true;
                var pos = obj.transform.position;
                pos.z = 0f;
                obj.transform.position = pos;
                
            }
        }
    }

    private void SetObject(Transform transform, List<GameObject> list)
    {
        list.Add(transform.gameObject);
        
        if (transform.childCount == 0) return;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i);
        
            SetObject(t, list);
        }
    }
}
