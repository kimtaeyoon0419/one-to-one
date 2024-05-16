using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class StatsDB : ScriptableObject
{
	public List<StatsDBEntity> Stats; // Replace 'EntityType' to an actual type that is serializable.
}
