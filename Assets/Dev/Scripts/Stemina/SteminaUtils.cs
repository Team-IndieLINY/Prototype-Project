using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class SteminaUtils
{
    public static bool Compare(Stat_Entity entity, EStatCode code)
    {
        Debug.Assert(entity != null);

        return entity.PlayerStatCode == (int)code;
    }


    public static void UpdateStemina(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.Properties;

        foreach (var fields in properties.GetRefAll())
        {
            if (fields is StatDataValue statValue)
            {
                StatUpdate(statValue, controller, table);
            }
        }
        
        if (properties
                .DoCondition<int>(EStatCode.Food, x=>x < 0, out _) ||
            properties
                .DoCondition<int>(EStatCode.Thirsty, x=>x<0, out _))
        {
            controller.Properties.GetRef<int>(EStatCode.Health).Value -= 1;
        }
    }

    private static void StatUpdate(StatDataValue statValue, SteminaController controller, StatTable table)
    {
        if (statValue.StatCode is
            EStatCode.Food or
            EStatCode.Thirsty)
        {
            statValue.Value -= 1;
        }
    }

    public static void UpdateValidation(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.Properties;
        
        if(properties.DoCondition(EStatCode.Food, x=>x < 0, out DataValueT<int> foodValue1))
        {
            foodValue1.Value = 0;
        }

        var maxFoodValue = table.Player_Stat_Master.First(y => y.PlayerStatCode == (int)EStatCode.Food).StatBasicValue;
        if(properties.DoCondition(EStatCode.Food, x=>x > maxFoodValue, out DataValueT<int> foodValue2))
        {
            foodValue2.Value = maxFoodValue;
        }
        
        
        
        if(properties.DoCondition(EStatCode.Thirsty, x=>x < 0, out DataValueT<int> thirstyValue1))
        {
            thirstyValue1.Value = 0;
        }

        var maxThirstyValue = table.Player_Stat_Master.First(y => y.PlayerStatCode == (int)EStatCode.Thirsty).StatBasicValue;
        if(properties.DoCondition(EStatCode.Food, x=>x > maxFoodValue, out DataValueT<int> thirstyValue2))
        {
            thirstyValue2.Value = maxThirstyValue;
        }
    }

    public static void UpdateView(SteminaController controller)
    {
        var view = controller.View;

        view.Food = controller.Properties.GetValue<int>(EStatCode.Food);
        view.Health = controller.Properties.GetValue<int>(EStatCode.Health);
        view.Thirsty = controller.Properties.GetValue<int>(EStatCode.Thirsty);
        
        view.UpdateView();
    }

    public static void SetBasicValue(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;

        for (int i = (int)EStatCode.First; i < (int)EStatCode.Last; i++)
        {
            controller.Properties.SetValue(
                (EStatCode)i, table.Player_Stat_Master
                    .First(x => x.PlayerStatCode == i)
                    .StatBasicValue);
        }
    }
}
