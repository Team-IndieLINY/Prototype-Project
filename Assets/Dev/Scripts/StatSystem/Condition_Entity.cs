using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition_Entity
{
    [SerializeField] private int Condition_Code;
    [SerializeField] private string Condition_Name;
    [SerializeField] private int Buff_Stat_Type;
    [SerializeField] private int Change_Type;
    [SerializeField] private int Change_Value;
    [SerializeField] private int Buff_Overlap_Type;
    [SerializeField] private int Change_Goal_Type_1;
    [SerializeField] private int Change_Goal_Value_1;
    [SerializeField] private int Change_Goal_Type_2;
    [SerializeField] private int Change_Goal_Value_2;
    [SerializeField] private int Change_Delay_Type;
    [SerializeField] private int Change_Delay_Value;
    [SerializeField] private int Change_Repeat_Value;
    [SerializeField] private int Condition_Limit_Value;
    [SerializeField] private int Condition_Value;

    public int ConditionCode => Condition_Code;

    public string ConditionName => Condition_Name;

    public int BuffStatType => Buff_Stat_Type;

    public int ChangeType => Change_Type;

    public int ChangeValue => Change_Value;

    public int BuffOverlapType => Buff_Overlap_Type;

    public int ChangeGoalType1 => Change_Goal_Type_1;

    public int ChangeGoalValue1 => Change_Goal_Value_1;

    public int ChangeGoalType2 => Change_Goal_Type_2;

    public int ChangeGoalValue2 => Change_Goal_Value_2;

    public int ChangeDelayType => Change_Delay_Type;

    public int ChangeDelayValue => Change_Delay_Value;

    public int ChangeRepeatValue => Change_Repeat_Value;

    public int ConditionLimitValue => Condition_Limit_Value;

    public int ConditionValue => Condition_Value;
}
