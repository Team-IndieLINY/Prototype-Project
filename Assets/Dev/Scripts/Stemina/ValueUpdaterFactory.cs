using System.Collections;
using System.Collections.Generic;
using IndieLINY.Singleton;
using UnityEngine;

public class ValueUpdaterFactory : MonoBehaviour, IValueUpdaterFactory
{
    public IValueUpdater Create()
        => new ValueUpdater(this);
}

public class ValueUpdater : IValueUpdater
{
    private ValueUpdaterFactory _factory;
    private ValueUpdaterParameter _parameter;
    private IEnumerator _coObj;

    public float Speed
    {
        get => _parameter.Speed;
        set => _parameter.Speed = value;
    }

    private bool _running;

    public ValueUpdater(ValueUpdaterFactory factory)
    {
        _factory = factory;
        _running = false;
    }
    public void Setup(ValueUpdaterParameter parameter)
    {
        _parameter = parameter;
    }

    public void Start()
    {
        if (_running) return;

        _running = true;

        _parameter.CurrentCallback = _parameter.EntryPoint;
        _factory.StartCoroutine(_coObj = CoUpdate());
    }

    private IEnumerator CoUpdate()
    {
        yield return null;
        
        IEnumerator enumerator = _parameter.CurrentCallback(_parameter);
        
        while (true)
        {
            if (_running == false)
            {
                yield return null;
                continue;
            }
            
            if (enumerator == null)
            {
                break;
            }

            yield return enumerator.Current;
            if (enumerator.MoveNext() == false)
            {
                break;
            }
        }

        _factory.StartCoroutine(_coObj = CoUpdate());
    }

    public void Pause()
    {
        _running = false;
    }

    public void Resume()
    {
        _running = true;
    }

    public void Reset()
    {
        _running = false;
        _parameter.CurrentCallback = _parameter.EntryPoint;
        _factory.StopCoroutine(_coObj);
        _factory.StartCoroutine(CoUpdate());
    }

    public void Release()
    {
        _factory = null;
        _parameter = null;
        _coObj = null;
        _running = false;
    }
}
