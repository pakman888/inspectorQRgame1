using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RoadLook : Look{
	public const int NoLookId = 0; //ID corresponding to "no look"
	
	public int id;
	public float size;
	public float offset;
	public float textureLeft;
	public float textureRight;
	public bool doubleLine;
	public int whiteStyle;
	public int centerLine;
	public bool useWhite;
	public int laneCount0;
	public int laneCount1;
	public bool oneWay;
	public string name;
}