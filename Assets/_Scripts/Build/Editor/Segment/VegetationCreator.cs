using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class VegetationCreator {

	private static Dictionary<string, GameObject> CachedModels;

	public static GameObject Instantiate(SpriteDef sprite){
		if(CachedModels == null){
			ClearCachedModels();
		}
		return PrefabUtility.InstantiatePrefab(LoadModelFromPrefabsOrCache(ResourceServer.Instance.vegetationPaths[(int)sprite.ModelIndex], false)) as GameObject;
	}
	
	public static void ClearCachedModels(){
		CachedModels = new Dictionary<string, GameObject>();
	}

	private static GameObject LoadModelFromPrefabsOrCache(string modelFileName, bool forceDae){
		if(CachedModels.ContainsKey(modelFileName)){
			return CachedModels[modelFileName];
		}
		GameObject modelObject = null;
		string originalFileName = modelFileName;
		if(!forceDae){
			string prefabFileName = modelFileName.Replace("base/model", "Assets/_Prefabs") + ".prefab";
			modelObject = (GameObject)AssetDatabase.LoadAssetAtPath(prefabFileName, typeof(GameObject));
		}
		if(!modelObject){			
			string daeFileName = modelFileName.Replace("base/model", "Assets/_Prefabs") + ".dae";
			if(!forceDae){
				Debug.Log("using raw dae file no prefab available " + modelFileName);
			}
			modelObject = (GameObject)AssetDatabase.LoadAssetAtPath(daeFileName, typeof(GameObject));
		}
		CachedModels[originalFileName] = modelObject;
		return modelObject;
	}
}
