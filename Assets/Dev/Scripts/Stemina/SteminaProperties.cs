using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseValue {}
public class DataValueT<T> : BaseValue
    where T : struct
{
    public T Value;

    public DataValueT()
    {
        Value = default;
    }
}
public class SteminaProperties
{
    
    private BaseValue[] _array;

    public SteminaProperties()
    {
        _array = new BaseValue[(int)EStatCode.Last];
    }

    private void TryAlloc<T>(EStatCode code)
    where T : struct
    {
        CheckValid(code, out int index);

        if (_array[index] == null)
        {
            _array[index] = new DataValueT<T>();
        }
    }

    private void CheckValid(EStatCode code, out int index)
    {
        index = (int)code;
        
        Debug.Assert(_array != null);
        Debug.Assert(_array.Length > index);
    }
    
    public T GetValue<T>(EStatCode code) where T : struct
    {
        CheckValid(code, out int index);
        TryAlloc<T>(code);
        
        if (_array[index] is DataValueT<T> v)
        {
            return v.Value;
        }
        
        Debug.Assert(false, "EStateCode와 매칭되지 않는 T");
        return default;
    }

    public void SetValue<T>(EStatCode code, T value) where T : struct
    {
        TryAlloc<T>(code);
        CheckValid(code, out int index);
        
        if (_array[index] is DataValueT<T> v)
        {
            v.Value = value;
        }
        else
        {
            Debug.Assert(false, "EStateCode와 매칭되지 않는 T");
        }
    }

    public DataValueT<T> GetRef<T>(EStatCode code) where T : struct
    {
        TryAlloc<T>(code);
        CheckValid(code, out int index);

        if (_array[index] is DataValueT<T> v)
        {
            return v;
        }
        
        Debug.Assert(false, "EStateCode와 매칭되지 않는 T");
        return default;
    }

    public bool DoCondition<T>(EStatCode code, Func<T, bool> callback, out DataValueT<T> refDataValue) where T : struct
    {
        TryAlloc<T>(code);
        CheckValid(code, out int index);

        
        refDataValue = null;
        if (_array[index] is DataValueT<T> v)
        {
            var rtv = callback?.Invoke(v.Value);
            if (rtv.HasValue == false || rtv.Value == false)
            {
                return false;
            }

            refDataValue = v;
            return true;
        }
        return false;
    }

    public void DoAction<T>(EStatCode code, Func<T, T> callback) where T : struct
    {
        TryAlloc<T>(code);
        CheckValid(code, out int index);

        if (_array[index] is DataValueT<T> v)
        {
            var rtv = callback?.Invoke(v.Value);
            if (rtv.HasValue == false)
            {
                return;
            }

            v.Value = rtv.Value;

        }
    }
}
