using System;
using System.Collections;
using System.Collections.Generic;
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

[ExcelAsset]
public class StatTable : ScriptableObject
{
	public List<Stat_Entity> Player_Stat_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Condition_Entity> Condition_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Item_Entity> Item_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Build_Entity> Build_Master; // Replace 'EntityType' to an actual type that is serializable.
}
