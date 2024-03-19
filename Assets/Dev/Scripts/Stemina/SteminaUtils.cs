using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public enum EStatCode : int
{
    Health = 1,
    Stemina,
    Food,
    Thirsty,
    Temperature,
    MovementSpeed,
    SpeedOfUsingItem,
    
    First = Health,
    Last = SpeedOfUsingItem
}


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
