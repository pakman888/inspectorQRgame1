using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CutPlaneItem : Item {
	public static int BYTE_SIZE = 49 + 8;
	public int[] nodeIndices;
}