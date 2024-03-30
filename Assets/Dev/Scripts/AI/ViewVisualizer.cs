using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Rendering;

public class ViewVisualizer : MonoBehaviour
{
    public int _iteration;

    public MeshFilter Filter;
    public MeshRenderer Renderer;

    public LayerMask _LayerMask;
    
    private Mesh _mesh;
    
    private void Awake()
    {
        _mesh = Filter.mesh;
    }

    public void UpdateView(Vector2 leftRay, float angle, float distance)
    {
        var list = new List<Vector2>();
        if (_iteration < 1) return;
        
        for (int i = 0; i < _iteration + 1; i++)
        {
            float rAngle = i / (float)_iteration * angle;
            
            var q = Quaternion.Euler(0f, 0f, rAngle);
            Vector2 dir = q * leftRay;
            dir = dir.normalized;

            var result = Physics2D.Raycast(transform.position, dir, distance, _LayerMask);

            if (result)
            {
                list.Add(result.point);
            }
            else
            {
                list.Add((Vector2)transform.position + dir * distance);
            }
        }
        var mesh = GenerateMesh(list);
        Filter.mesh = mesh;
    }

    private Mesh GenerateMesh(List<Vector2> hittedPoints)
    {
        List<Vector3> v = new List<Vector3>();
        List<int> indices = new List<int>();
        v.Add(Vector3.zero);
        for (int i = 0; i < hittedPoints.Count; i++)
        {
            var pos = transform.position;
            pos.z = 0f;
            v.Add((Vector3)hittedPoints[i] - pos);
        }

        for (int i = 0; i < hittedPoints.Count; i++)
        {
            indices.Add(0);
            indices.Add(i);
            indices.Add(i + 1);
        }

        _mesh.vertices = v.ToArray();
        _mesh.triangles = indices.ToArray();
        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
        return _mesh;
    }

}