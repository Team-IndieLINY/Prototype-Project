using System.Collections;
using System.Collections.Generic;
using IndieLINY.Singleton;
using JetBrains.Annotations;
using UnityEngine;
using XRProject.Utils.Log;

public interface ITableConatinedItem
{
    public string TableKey { get; }
}

[Singleton(ESingletonType.Global, 0)]
public class TableContainer : MonoBehaviourSingleton<TableContainer>
{
    private Dictionary<string, ITableConatinedItem> _tableDict;
    public override void PostInitialize()
    {
        _tableDict = new();
        
        Load();
    }

    private void WrappLoad<T>(string path)
        where T : ScriptableObject
    {
        if (Resources.Load<StatTable>("Data/StatTable") is ITableConatinedItem statTable)
        {
            if (_tableDict.TryAdd(path, statTable) == false)
            {
                Debug.Assert(false);
            }
        }
    }

    private void Load()
    {
        WrappLoad<StatTable>("Data/StatTable");
    }
    public override void PostRelease()
    {
        _tableDict.Clear();
        _tableDict = null;
    }

    [CanBeNull]
    public T Get<T>(string key) where T : class, ITableConatinedItem
        => _tableDict[key] as T;

    public bool TryGet<T>(string key, out T table) where T : class, ITableConatinedItem
    {
        if (_tableDict.TryGetValue(key, out var temp))
        {
            if (temp is T rtv)
            {
                table = rtv;
                return true;
            }
        }
        
        table = null;
        return false;
    }
}
