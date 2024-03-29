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
    }
}
