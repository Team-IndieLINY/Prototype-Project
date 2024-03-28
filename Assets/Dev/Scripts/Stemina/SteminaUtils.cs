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
        var steminaData = controller.SteminaData;

        var healthRef = controller.Properties.GetRef<int>(EStatCode.Health);

        if (properties
            .DoCondition<int>(EStatCode.Food, x => x <= 0, out _))
        {
            healthRef.Value -= steminaData.DecreaseHealthWhenNotEnoughFoodPerSec;
        }

        if (properties
            .DoCondition<int>(EStatCode.Thirsty, x => x <= 0, out _))
        {
            healthRef.Value -= steminaData.DecreaseHealthWhenNotEnoughThirstyPerSec;
        }
        

        if (properties
            .DoCondition<int>(EStatCode.Temperature, x => x <= controller.SteminaData.MinTemperature, out _))
        {
            healthRef.Value -= steminaData.DecreaseHealthWhenNotEnoughTemperaturePerSec;
        }
        
        
        properties.DoAction<int>(EStatCode.Food, x=>x - steminaData.DecreaseFoodPerSec);
        properties.DoAction<int>(EStatCode.Thirsty, x=>x - steminaData.DecreaseThristyPerSec);

        properties.DoAction<int>(EStatCode.Temperature, x =>
        {
            if (Physics2D.OverlapCircle(controller.Interaction.transform.position,
                    steminaData.BeginIncreaseTemperatureRadius, LayerMask.GetMask("Campfire")))
            {
                return x + steminaData.IncreaseTemperaturePerSec;
            }
            else
            {
                return x - steminaData.DecreaseTemperaturePerSec;
            }
        });
    }

    public static void UpdateValidation(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.Properties;
        var steminaData = controller.SteminaData;
        
        
        //health validations
        properties.DoCondition<int>(EStatCode.Health, x => x > steminaData.MaxHealth, refValue =>
        {
            refValue.Value = steminaData.MaxHealth;
        });
        properties.DoCondition<int>(EStatCode.Health, x => x < 0, refValue =>
        {
            refValue.Value = 0;
        });

        //food validations
        properties.DoCondition<int>(EStatCode.Food, x => x > steminaData.MaxFood, refValue =>
        {
            refValue.Value = steminaData.MaxFood;
        });
        properties.DoCondition<int>(EStatCode.Food, x => x < 0, refValue =>
        {
            refValue.Value = 0;
        });
        
        //thirsty validations
        properties.DoCondition<int>(EStatCode.Thirsty, x => x > steminaData.MaxThristy, refValue =>
        {
            refValue.Value = steminaData.MaxThristy;
        });
        properties.DoCondition<int>(EStatCode.Thirsty, x => x < 0, refValue =>
        {
            refValue.Value = 0;
        });
        
        //temperature validations
        properties.DoCondition<int>(EStatCode.Temperature, x => x > steminaData.MaxTemperature, refValue =>
        {
            refValue.Value = steminaData.MaxTemperature;
        });
        properties.DoCondition<int>(EStatCode.Temperature, x => x < steminaData.MinTemperature, refValue =>
        {
            refValue.Value = steminaData.MinTemperature;
        });
        
        //temperature validations
        properties.DoCondition<float>(EStatCode.Stemina, x => x > steminaData.MaxSprintStemina, refValue =>
        {
            refValue.Value = steminaData.MaxSprintStemina;
        });
        properties.DoCondition<float>(EStatCode.Stemina, x => x < 0, refValue =>
        {
            refValue.Value = 0;
        });
    }

    public static void UpdateView(SteminaController controller)
    {
        var view = controller.View;

        view.Food = controller.Properties.GetValue<int>(EStatCode.Food);
        view.Health = controller.Properties.GetValue<int>(EStatCode.Health);
        view.Thirsty = controller.Properties.GetValue<int>(EStatCode.Thirsty);
        view.Temperature = controller.Properties.GetValue<int>(EStatCode.Temperature);

        view.MaxFood = controller.SteminaData.MaxFood;
        view.MaxHealth = controller.SteminaData.MaxHealth;
        view.MaxThirsty = controller.SteminaData.MaxThristy;
        view.MaxTemperature = controller.SteminaData.MaxTemperature;
        
        view.UpdateView();
    }

    public static void SetBasicValue(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;

        var properties = controller.Properties;
        var data = controller.SteminaData;
        
        properties.SetValue(EStatCode.Health, data.MaxHealth);
        properties.SetValue(EStatCode.Food, data.MaxFood);
        properties.SetValue(EStatCode.Thirsty, data.MaxThristy);
        properties.SetValue(EStatCode.Temperature, data.MaxTemperature);
        properties.SetValue<float>(EStatCode.Stemina, data.MaxSprintStemina);
    }
}
