using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NoWeatherItem : Item {
	public static int BYTE_SIZE = 49 + 12;
	
	public float width;
	public float height;
	public int nodeIndex;
}