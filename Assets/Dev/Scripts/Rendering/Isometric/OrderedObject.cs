using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EOrderedObjectType
{
    Front,
    FrontCollision,
    Back,
    BackCollision,
    Floor,
    Stair,
    Inner
}
public class OrderedObject : MonoBehaviour
{
    [SerializeField] private EOrderedObjectType _type;
    public EOrderedObjectType Type => _type;

    [SerializeField] private SpriteRenderer _renderer;
    private Material _backupMaterial;
    private Material _createdMaterial;

    public int Stencil
    {
        get => _renderer.material.GetInt("_Stencil");
    }

    public void SetStencilState(bool enabled)
    {
        if (enabled)
        {
            _createdMaterial.SetInt("_Stencil", 1);
            _renderer.material = _createdMaterial;
        }
        else
        {
            _renderer.sharedMaterial = _backupMaterial;
        }
    }
    
    public SpriteRenderer Renderer
    {
        get
        {
            if (_renderer == false)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }

            return _renderer;
        }
    }

    private void Awake()
    {
        if (_renderer == false)
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        _backupMaterial = _renderer.sharedMaterial;
        _backupMaterial.SetInt("Stencil", 2);
        _createdMaterial = _renderer.material;
        _renderer.material = _backupMaterial;
    }
}
