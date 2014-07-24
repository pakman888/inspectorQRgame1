using UnityEngine;
using System.Collections;

public static class Layers {

	public const int Terrain = 8,
					 Vehicles = 9;
					
	public static void SetLayerRecursive(this GameObject obj, int layer){
		obj.layer = layer;
		foreach(Transform child in obj.transform){
			SetLayerRecursive(child.gameObject, layer);
		}
	}
	
	public static int GetLayerMaskFromLayer(int layer){
		return 1 << layer;
	}
}
