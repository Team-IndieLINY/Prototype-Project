using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SteminaProperties
{
    public class BaseValue {}
    public class ValueT<T> : BaseValue
        where T : struct
    {
        public T Value;

        public ValueT()
        {
            Value = default;
        }
    }
    
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
            _array[index] = new ValueT<T>();
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
        
        if (_array[index] is ValueT<T> v)
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
        
        if (_array[index] is ValueT<T> v)
        {
            v.Value = value;
        }
        else
        {
            Debug.Assert(false, "EStateCode와 매칭되지 않는 T");
        }
    }

    public ValueT<T> GetRef<T>(EStatCode code) where T : struct
    {
        TryAlloc<T>(code);
        CheckValid(code, out int index);

        if (_array[index] is ValueT<T> v)
        {
            return v;
        }
        
        Debug.Assert(false, "EStateCode와 매칭되지 않는 T");
        return default;
    }
}
