using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class BusStopSegment : ItemSegment {

	private static Dictionary<string, GameObject> CachedModels;

	public override GameObject CreateGameObject(ResourceServer server){
		if(CachedModels == null){
			ClearCachedModels();
		}
		var busStopSegment = new GameObject();
		var busStopExtraInfo = busStopSegment.AddComponent<SegmentExtraInfo>();
		busStopExtraInfo.id = id;
		busStopExtraInfo.lod = server.GetLoD(id);
		busStopSegment.name = "Segment" + id; 

		var busStopScript = AttachBusStopScript(busStopSegment, server, item);

		busStopSegment.transform.position = busStopScript.node.position;
		busStopExtraInfo.position = busStopSegment.transform.position;
		busStopExtraInfo.segmentType = Item.KIT_bus_stop;
		busStopSegment.transform.rotation.SetLookRotation(busStopScript.node.direction);
		busStopSegment.SetLayerRecursive(Layers.Terrain);

		AttachBusStopModel(busStopScript);
		AttachInactivePlate(busStopScript);
		AttachActivePlate(busStopScript);

		return busStopSegment;
	}
	
	public override void CalcBounds(){
		bounds = new Bounds (ResourceServer.Instance.nodes[((BusStopItem)item).index].position, Vector3.zero);
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
			modelFileName = modelFileName.Replace("/model", "Assets/_Prefabs");
			modelFileName = modelFileName.Remove(modelFileName.LastIndexOf(".")) + ".prefab";
			modelObject = (GameObject)AssetDatabase.LoadAssetAtPath(modelFileName, typeof(GameObject));
		}
		if(!modelObject){			
			modelFileName = modelFileName.Replace("/model", "Assets/_Prefabs");
			modelFileName = modelFileName.Remove(modelFileName.LastIndexOf(".")) + ".dae";
			if(!forceDae){
				Debug.Log("using raw dae file no prefab available " + modelFileName);
			}
			modelObject = (GameObject)AssetDatabase.LoadAssetAtPath(modelFileName, typeof(GameObject));
		}
		CachedModels[originalFileName] = modelObject;
		return modelObject;
	}

	private static void AttachInactivePlate(BusStopScript busStopScript) {
		var modelFileName = busStopScript.busStopLook.inactivePlate;
		const string NO_MODEL = "0";
		if (modelFileName != NO_MODEL) {
			GameObject inactivePlate = LoadModelFromPrefabsOrCache(modelFileName, true);
			if (inactivePlate == null) {
				Debug.LogError("Missing InactiveModel " + modelFileName);
			}
			inactivePlate.name = "InactivePlate";
			busStopScript.inactivePlatePrefab = inactivePlate;
		} 
	}

	private static void AttachActivePlate(BusStopScript busStopScript) {
		var modelFileName = busStopScript.busStopLook.activePlate;
		const string NO_MODEL = "0";
		if (modelFileName != NO_MODEL) {
			GameObject activePlate = LoadModelFromPrefabsOrCache(modelFileName, false);
			if (activePlate == null) {
				Debug.LogError("Missing ActiveModel " + modelFileName);
			}
			activePlate.name = "ActivePlate";
			busStopScript.activePlatePrefab = activePlate;
		} 
	}

	private static BusStopScript AttachBusStopScript(GameObject busStopSegment, ResourceServer server, Item item) {
		var busStop = (BusStopItem)item;
		var busStopScript = busStopSegment.AddComponent<BusStopScript>();
		busStopScript.busStopItem = busStop;
		busStopScript.node = server.nodes[busStop.index];
		busStopScript.busStopLook = server.busStopLooks[busStopScript.busStopItem.modelType];
		return busStopScript;
	}

	private static void AttachBusStopModel(BusStopScript busStopScript) {
		var modelFileName = busStopScript.busStopLook.busStopModel;
		const string NO_MODEL = "0";
		if (modelFileName != NO_MODEL) {
			GameObject stopModel = LoadModelFromPrefabsOrCache(modelFileName, true);
			if (stopModel == null) {
				Debug.LogError("missing bus stop model " + modelFileName);
			}
			stopModel.name = "BusStopModel";
			busStopScript.busStopModel = stopModel;
		}
	}
}
