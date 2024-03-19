using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_Entity
{
    [SerializeField] private int Item_Group_Code;
    [SerializeField] private string Item_Group_Name;
    [SerializeField] private int Item_Code;
    [SerializeField] private string Item_Name;
    [SerializeField] private int Item_Main_Type;
    [SerializeField] private int Item_Sub_Type;
    [SerializeField] private int Item_Value;
    [SerializeField] private int Buff_Stat_Type;

    public int ItemGroupCode => Item_Group_Code;

    public string ItemGroupName => Item_Group_Name;

    public int ItemCode => Item_Code;

    public string ItemName => Item_Name;

    public int ItemMainType => Item_Main_Type;

    public int ItemSubType => Item_Sub_Type;

    public int ItemValue => Item_Value;

    public int BuffStatType => Buff_Stat_Type;
}
