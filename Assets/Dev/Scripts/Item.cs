using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(menuName = "IndieLINY/Item", fileName = "NewItem", order = 3)]
public class Item : ScriptableObject
{

    [SerializeField] private string _name;
    [SerializeField]private Sprite _icon;

    [SerializeField]private int _fillFood; 
    [SerializeField]private int _fillHealth;
    [SerializeField]private int _fillThirsty;

    public int FillFood => _fillFood;
    public int FillHealth => _fillHealth;
    public int FillThirsty => _fillThirsty;

    public string Name => _name;
    public Sprite Icon => _icon;


    #region Functions
    public override bool Equals(object obj)
    {
        if (obj is not Item item) return false;

        if (item.Name == Name) return true;

        return false;
    }
    

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), _name);
    }
    public static bool operator==(Item a, Item b)
    {
        if (Object.ReferenceEquals(a, b))
        {
            return true;
        }
        else
        {
            if (object.ReferenceEquals(a, null)) return false;
            return a.Equals(b);
        }
    }
    public static bool operator!=(Item a, Item b)
    {
        if (Object.ReferenceEquals(a, b))
        {
            return false;
        }
        else
        {
            if (object.ReferenceEquals(a, null)) return true;
            return !a.Equals(b);
        }
    }
    #endregion
    
}
