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
        
        
    }

    public static void UpdateValidation(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;
        var properties = controller.Properties;
        
        //TODO: 스텟 값 유효성 검사 코드 작성
    }

    public static void UpdateView(SteminaController controller)
    {
        var view = controller.View;
        
        //TODO: stat을 view에 적용하는 코드 작성
        
        //TODO: Stat의 max 값을 view에 적용하는 코드 작성
        
        view.UpdateView();
    }

    public static void SetBasicValue(SteminaController controller, StatTable table)
    {
        if (controller.Enabled == false) return;

        // 이후 데이터 테이블 적용시 다시 사용
        //for (int i = (int)EStatCode.First; i < (int)EStatCode.Last; i++)
        //{
        //    controller.Properties.SetValue(
        //        (EStatCode)i, table.Player_Stat_Master
        //            .First(x => x.PlayerStatCode == i)
        //            .StatBasicValue);
        //}
        
        //TODO: 기본값 설정 코드 작성
    }
}
