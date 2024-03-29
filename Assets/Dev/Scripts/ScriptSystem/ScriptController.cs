using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptController
{
    private ScriptData _model;
    private ScriptView _view;
    private PropertiesConatiner<StatDataValue, int> PropertiesConatiner;

    public ScriptController(ScriptData model, ScriptView view, PropertiesConatiner<StatDataValue, int> propertiesConatiner)
    {
        _model = model;
        _view = view;
        PropertiesConatiner = propertiesConatiner;

        foreach (var refValue in propertiesConatiner.GetRefAll())
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
        //TODO: 엑터의 스텟 상태 관찰에 의한 스크립트 출력 기능 구현
    }

    private IEnumerator CoEntityUpdate(ScriptEntity entity)
    {
        entity.CanDisplay = false;
        yield return new WaitForSeconds(entity.CoolTime);
        entity.CanDisplay = true;
    }
}