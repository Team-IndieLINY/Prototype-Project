using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptController
{
    public ActorSteminaData _steminaData;
    private ScriptData _model;
    private ScriptView _view;
    private SteminaProperties _properties;

    public ScriptController(ScriptData model, ScriptView view, SteminaProperties properties, ActorSteminaData steminaData)
    {
        _steminaData = steminaData;
        _model = model;
        _view = view;
        _properties = properties;

        foreach (var refValue in properties.GetRefAll())
        {
            if (refValue is not StatDataValue statValue) continue;

            statValue.OnChangedValue += OnChangedValue;
        };
    }

    private void OnChangedValue(int before, int after)
    {
        foreach (var entity in _model.Entities)
        {
            DoFilteredAction(entity);
        }
    }

    private void DoFilteredAction(ScriptEntity entity)
    {
        if (entity.CanDisplay &&
            entity.FoodPercentage >= _properties.GetValue<int>(EStatCode.Food) / (float)_steminaData.MaxFood &&
            entity.HealthPercentage >= _properties.GetValue<int>(EStatCode.Health) / (float)_steminaData.MaxHealth &&
            entity.ThristyPercentage >= _properties.GetValue<int>(EStatCode.Thirsty) / (float)_steminaData.MaxThristy)
        {
            _view.Show(entity);
            _view.StartCoroutine(CoEntityUpdate(entity));
        }
    }

    private IEnumerator CoEntityUpdate(ScriptEntity entity)
    {
        entity.CanDisplay = false;
        yield return new WaitForSeconds(entity.CoolTime);
        entity.CanDisplay = true;
    }
}