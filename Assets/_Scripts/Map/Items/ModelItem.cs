using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ModelItem : Item {
	public static int BYTE_SIZE = 0x55;
	public int modelIndex;
	public int modelLookIndex;
	public int nodeIndex;
	public Vector3 eulerAngles;
	public Vector3 scale;
}
