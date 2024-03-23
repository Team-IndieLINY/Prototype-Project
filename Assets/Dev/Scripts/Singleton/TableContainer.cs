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
        if (Resources.Load<T>(path) is ITableConatinedItem item)
        {
            Debug.Assert(_tableDict.ContainsKey(item.TableKey) == false, "이미 중복된 테이블 키: " + item.TableKey);
            _tableDict.Add(item.TableKey, item);
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