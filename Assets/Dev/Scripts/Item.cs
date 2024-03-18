using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[CreateAssetMenu(menuName = "IndieLINY/Item", fileName = "NewItem", order = 3)]
public class Item : ScriptableObject
{

    public string Name => _name;
    public Sprite Icon => _icon;

    public string _name;
    public Sprite _icon;

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
    
}
