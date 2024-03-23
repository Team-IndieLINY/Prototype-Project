using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ValueUpdaterParameter
{
    public BaseValue Value;
    public int UpdateIntervalMs;
    public Action<ValueUpdaterParameter> OnUpdate;
}

public interface IValueUpdater
{
    public void Setup(ValueUpdaterParameter parameter);
    public void Start();
    public void Stop();
    public void Reset();
    public void Release();
}

public enum EStatCode : int
{
    Health = 1,
    Stemina = 2,
    Food = 3,
    Thirsty = 4,
    Temperature = 5,
    MovementSpeed = 6,
    SpeedOfItemUsage = 7
}

public interface IValueUpdaterFactory
{
    public IValueUpdater Create();
}

public static class StatUtils
{
    public static void FrameUpdateStemina(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.StatProperties;
        
        
    }

    public static void UpdateValidation(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.StatProperties;
        
        //TODO: 스텟 값 유효성 검사 코드 작성
    }

    public static void UpdateView(SteminaController controller)
    {
        var view = controller.View;
        var properties = controller.StatProperties;
        
        //TODO: stat을 view에 적용하는 코드 작성
        view.Health = properties.GetValue((int)EStatCode.Health);
        view.Food = properties.GetValue((int)EStatCode.Food);
        view.Thirsty = properties.GetValue((int)EStatCode.Thirsty);
        
        //TODO: Stat의 max 값을 view에 적용하는 코드 작성
        view.MaxHealth = properties.GetRef((int)EStatCode.Health).Entity.StatBasicValue;
        view.MaxFood = properties.GetRef((int)EStatCode.Food).Entity.StatBasicValue;
        view.MaxThirsty = properties.GetRef((int)EStatCode.Thirsty).Entity.StatBasicValue;
        
        view.UpdateView();
    }

    public static PropertiesConatiner<StatDataValue, int> CreateProperties(int characterCode, IValueUpdaterFactory factory, StatTable table)
    {
        var list = table.Character_Master
            .Where(x => x.CharacterCode == characterCode)
            .ToList();
        int length = list.Count;

        list.Sort((a, b) =>
        {
            if (a.StatCode > b.StatCode) return 1;
            else if (a.StatCode < b.StatCode) return -1;
            else return 0;
        });
        
        StatDataValue[] allRef = new StatDataValue[length];

        for (int i = 0; i < length; i++)
        {
            IValueUpdater updater = factory.Create();
            StatDataValue refValue = new StatDataValue(list[i], updater);
            Character_Master_Entity entity = refValue.Entity;
            
            //TODO:스텟 초기화 로직 작성
            refValue.Value = entity.StatBasicValue;
            
            updater.Setup(new ValueUpdaterParameter()
            {
                Value =  refValue,
                OnUpdate = UpdateProperty,
                
                //TODO: ms단위로 반복 단위를 테이블에서 참조하여 설정
                UpdateIntervalMs = 1000
            });
            updater.Start();
            
            allRef[i] = refValue;
        }

        return new PropertiesConatiner<StatDataValue, int>(allRef);
    }

    private static void UpdateProperty(ValueUpdaterParameter parameter)
    {
        if (parameter.Value is not StatDataValue refValue) return;
        
        //TODO: property 갱신 로직 작성
        refValue.Value -= 10;
    }

    public static void Release(SteminaController controller)
    {
        foreach (var refValue in controller.StatProperties.GetRefAll())
        {
            refValue.Updater.Release();
        }
    }
}
