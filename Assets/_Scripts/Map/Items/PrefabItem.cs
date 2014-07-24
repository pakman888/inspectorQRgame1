using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PrefabItem : Item {
	public int prefabID;
	public int prefabLookID;
	public int[] nodeIndices;
	public int trLightModels;
	public int originIndex;
	public int prefabRotY;
	public Int16 prefabRotZ;
	public uint[] terrainFlags;
	public Int16[] terrainMaterialIDs;
	public float[] terrainProfileCoefficients;
	public Int16[] terrainVegetation;
	public PrefabDesc prefabDesc;
    public List<NodeItem> nodes;
}
