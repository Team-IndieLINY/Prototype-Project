using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Character_Master_Entity
{
    [SerializeField] private int Character_Code;
    [SerializeField] private string Char_Name;
    [SerializeField] private string Char_Relation;
    [SerializeField] private int Char_Type;
    [SerializeField] private int Scout_Type;
    [SerializeField] private int Stat_Code;
    [SerializeField] private string Stat_Name;
    [SerializeField] private int Stat_Type;
    [SerializeField] private int Stat_Basic_Value;
    [SerializeField] private int Routine_Type;
    [SerializeField] private int Routine_Value;
    [SerializeField] private int Routine_Goal_Type;
    [SerializeField] private int Routine_Goal_Value;
    [SerializeField] private int Routine_Reset_Type;
    [SerializeField] private int Routine_Reset_Value;
    [SerializeField] private int Routine_Delay_Type;
    [SerializeField] private int Routine_Delay_Value;
    [SerializeField] private int Routine_Plus_Delay_Type;
    [SerializeField] private int Routine_Plus_Delay_Value;
    [SerializeField] private int Routine_Repeat_Value;
    [SerializeField] private int Condition_Add_Type;
    [SerializeField] private int Condition_Limit_Type_1;
    [SerializeField] private int Condition_Limit_Value_1;
    [SerializeField] private int Condition_Value_1;
    [SerializeField] private int Condition_Limit_Type_2;
    [SerializeField] private int Condition_Limit_Value_2;
    [SerializeField] private int Condition_Value_2;

    public int CharacterCode => Character_Code;

    public string CharName => Char_Name;

    public string CharRelation => Char_Relation;

    public int CharType => Char_Type;

    public int ScoutType => Scout_Type;

    public int StatCode => Stat_Code;

    public string StatName => Stat_Name;

    public int StatType => Stat_Type;

    public int StatBasicValue => Stat_Basic_Value;

    public int RoutineType => Routine_Type;

    public int RoutineValue => Routine_Value;

    public int RoutineGoalType => Routine_Goal_Type;

    public int RoutineGoalValue => Routine_Goal_Value;

    public int RoutineResetType => Routine_Reset_Type;

    public int RoutineResetValue => Routine_Reset_Value;

    public int RoutineDelayType => Routine_Delay_Type;

    public int RoutineDelayValue => Routine_Delay_Value;

    public int RoutinePlusDelayType => Routine_Plus_Delay_Type;

    public int RoutinePlusDelayValue => Routine_Plus_Delay_Value;

    public int RoutineRepeatValue => Routine_Repeat_Value;

    public int ConditionAddType => Condition_Add_Type;

    public int ConditionLimitType1 => Condition_Limit_Type_1;

    public int ConditionLimitValue1 => Condition_Limit_Value_1;

    public int ConditionValue1 => Condition_Value_1;

    public int ConditionLimitType2 => Condition_Limit_Type_2;

    public int ConditionLimitValue2 => Condition_Limit_Value_2;

    public int ConditionValue2 => Condition_Value_2;
}
