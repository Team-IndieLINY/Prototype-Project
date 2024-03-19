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


    public static void UpdateStemina(SteminaController controller)
    {
        if (controller.Enabled == false) return;
        var properties = controller.Properties;
        
        var food = properties.GetRef<int>(EStatCode.Food);
        food.Value -= 1;

        if (food.Value <= 0)
        {
            var health = properties.GetRef<int>(EStatCode.Health);
            health.Value -= 1;
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
    }

    public static void UpdateView(SteminaController controller)
    {
        var view = controller.View;

        view.Food = controller.Properties.GetValue<int>(EStatCode.Food);
        view.Health = controller.Properties.GetValue<int>(EStatCode.Health);
        
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
