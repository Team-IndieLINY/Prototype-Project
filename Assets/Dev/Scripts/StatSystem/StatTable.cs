using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class StatTable : ScriptableObject, ITableConatinedItem
{
	public List<Character_Master_Entity> Character_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Condition_Entity> Condition_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Item_Entity> Item_Master; // Replace 'EntityType' to an actual type that is serializable.
	public List<Build_Entity> Build_Master; // Replace 'EntityType' to an actual type that is serializable.

	public string TableKey => "Stat";
}
