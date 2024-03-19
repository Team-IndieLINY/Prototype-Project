using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat_Entity
{
    [SerializeField] private int Player_Stat_Code;
    [SerializeField] private string Player_Stat_Name;
    [SerializeField] private int Stat_Type;
    [SerializeField] private int Stat_Basic_Value;
    [SerializeField] private int Change_Type;
    [SerializeField] private int Change_Value;
    [SerializeField] private int Change_Goal_Type;
    [SerializeField] private int Change_Reset_Type;
    [SerializeField] private int Change_Reset_Value;
    [SerializeField] private int Change_Delay_Type;
    [SerializeField] private int Change_Delay_Value;
    [SerializeField] private int Change_Plus_Delay_Type;
    [SerializeField] private int Change_Plus_Delay_Value;
    [SerializeField] private int Change_Repeat_Value;
    [SerializeField] private int Condition_Limit_Value;
    [SerializeField] private int Condition_Value;

    public int PlayerStatCode => Player_Stat_Code;

    public string PlayerStatName => Player_Stat_Name;

    public int StatType => Stat_Type;

    public int StatBasicValue => Stat_Basic_Value;

    public int ChangeType => Change_Type;

    public int ChangeValue => Change_Value;

    public int ChangeGoalType => Change_Goal_Type;

    public int ChangeResetType => Change_Reset_Type;

    public int ChangeResetValue => Change_Reset_Value;

    public int ChangeDelayType => Change_Delay_Type;

    public int ChangeDelayValue => Change_Delay_Value;

    public int ChangePlusDelayType => Change_Plus_Delay_Type;

    public int ChangePlusDelayValue => Change_Plus_Delay_Value;

    public int ChangeRepeatValue => Change_Repeat_Value;

    public int ConditionLimitValue => Condition_Limit_Value;

    public int ConditionValue => Condition_Value;
}
