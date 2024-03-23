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

    private IEnumerator CoUpdate(ValueUpdaterParameter parameter)
    {
        var wait = new WaitForSeconds(parameter.UpdateIntervalMs * 0.001f);

        while (true)
        {
            parameter.OnUpdate?.Invoke(parameter);
            yield return wait;
        }
    }

    public void Start()
    {
        if (_running) return;
        
        _running = true;
        _factory.StartCoroutine(_coObj = CoUpdate(_parameter));
    }

    public void Stop()
    {
        _running = false;
        
        if(_coObj != null)
            _factory.StopCoroutine(_coObj);
        
        _coObj = null;
    }

    public void Reset()
    {
        
    }

    public void Release()
    {
        Stop();
        _factory = null;
        _parameter = null;
        _coObj = null;
        _running = false;
    }
}
