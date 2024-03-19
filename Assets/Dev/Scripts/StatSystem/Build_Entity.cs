using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Build_Entity
{
    [SerializeField] private int Build_Group_Code;
    [SerializeField] private string Build_Group_Name;
    [SerializeField] private string Build_Name;
    [SerializeField] private int Build_Main_Type;
    [SerializeField] private int Item_Need_Type;
    [SerializeField] private int Item_Need_Value;

    public int BuildGroupCode => Build_Group_Code;

    public string BuildGroupName => Build_Group_Name;

    public string BuildName => Build_Name;

    public int BuildMainType => Build_Main_Type;

    public int ItemNeedType => Item_Need_Type;

    public int ItemNeedValue => Item_Need_Value;
}
