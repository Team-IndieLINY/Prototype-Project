using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseValue
{
    public IValueUpdater Updater { get; private set; }

    public BaseValue(IValueUpdater updater)
    {
        this.Updater = updater;
    }
}

public class DataValueT<T> : BaseValue
    where T : struct
{
    private T _value;

    public T Value
    {
        get => _value;
        set
        {
            T temp = this._value;
            this._value = value;
            OnUpdate(temp, this._value);
        }
    }
    
    public event Action<T, T> OnChangedValue;

    public DataValueT(IValueUpdater updater) : base(updater)
    {
        Value = default;
    }

    protected virtual void OnUpdate(T before, T after)
    {
        OnChangedValue?.Invoke(before, after);
    }
}

public class StatDataValue : DataValueT<int>
{
    public int StatCode { get; private set; }
    public Character_Master_Entity Entity { get; private set; }

    public StatDataValue(Character_Master_Entity entity, IValueUpdater updater) : base(updater)
    {
        this.StatCode = entity.StatCode;
        this.Entity = entity;
    }
    
    public event Action<int, int, int> OnChangedValueType;

    protected override void OnUpdate(int before, int after)
    {
        base.OnUpdate(before, after);
        OnChangedValueType?.Invoke(before, after, StatCode);
    }
}

public class PropertiesConatiner<TRefValue, TN> 
    where TRefValue : DataValueT<TN> 
    where TN : struct
{
    private TRefValue[] _array;

    public PropertiesConatiner(TRefValue[] values)
    {
        _array = values;
    }

    private void CheckValid(int index)
    {
        Debug.Assert(_array != null);
        Debug.Assert(_array.Length > index);
    }
    
    public TN GetValue(int index) 
    {
        CheckValid(index);

        return _array[index].Value;
    }

    public void SetValue(int index, TN value)
    {
        CheckValid(index);
        _array[index].Value = value;
    }

    public TRefValue GetRef(int index)
    {
        CheckValid(index);

        return _array[index];
    }

    public bool DoCondition(int index, Func<TRefValue, bool> callback, out TRefValue refDataValue)
    {
        CheckValid(index);
        
        refDataValue = null;
        var v = _array[index];
        if (callback.Invoke(v) == false) return false;

        refDataValue = v;
        return true;
    }
    public bool DoCondition(int index, Func<TRefValue, bool> callback, Action<TRefValue> trueCallback)
    {
        CheckValid(index);

        var v = _array[index];
        
        var rtv = callback?.Invoke(v);
        if (rtv.HasValue == false || rtv.Value == false)
        {
            return false;
        }

        trueCallback?.Invoke(v);
        return true;
    }

    public void DoAction(int index, Func<TRefValue, TN> callback)
    {
        CheckValid(index);
        
        var v = _array[index];
        var rtv = callback?.Invoke(v);
        if (rtv == null)
        {
            return;
        }

        v.Value = rtv.Value;
    }

    public TRefValue[] GetRefAll()
        => _array;
}
