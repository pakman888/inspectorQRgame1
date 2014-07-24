using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/** Static segments are built from exported map data and consist of a collection of "protos." */
public class StaticSegment : Segment {

	public const int segmentsPerMeshBlob = 100;
	
	public static string GetMeshBlobPath(int segmentId, int segmentType){
		return WorldConversion.meshesFolder + "Blob " + (segmentId / segmentsPerMeshBlob) + ".obj";
	}
	
	public List<Proto> protos = new List<Proto>();

	public override GameObject CreateGameObject(ResourceServer server){
		var go = new GameObject();
		go.name = "Segment " + id;

		var extraInfo = go.AddComponent<SegmentExtraInfo>();
		extraInfo.segmentType = segmentType;
		extraInfo.id = id;
		extraInfo.lod = server.GetLoD(id);

		var protoGOs = BuildAllProtos();
		foreach (var protoGO in protoGOs) {
			protoGO.transform.parent = go.transform;
		}
		go.SetLayerRecursive(Layers.Terrain);
		
		if (segmentType == 0) {
			RoadModification.AttachRoadComponent(ref go, server);
		}
		if (segmentType == 2){
			BuildingModification.ModifyBuildingSegment(ref go, server, extraInfo);
		}				

		if (segmentType == 3) { //is prefab/intersection
			IntersectionModification.AddIntersectionInfoToSegment(ref go, server, extraInfo);
		}

		if (segmentType == 4) {
			ModelModification.AddModelInfoToSegment(ref go, server, extraInfo);
		}
		
		extraInfo.position = bounds.center;
		
		foreach(SpriteDef sprite in sprites){
			var spriteObject = VegetationCreator.Instantiate(sprite);
			spriteObject.transform.parent = go.transform;
			spriteObject.transform.position = Vector3.Scale(sprite.Position, new Vector3(1, 1, -1));
		}
		
		return go;
	}
	
	public int GetProtoNumFromGameObjectName(string objectName){
		return int.Parse(objectName.Substring(5));
	}
	
	public string GetMeshBlobPath(){
		return GetMeshBlobPath(id, segmentType);
	}
	
	public override void CalcBounds() {
        int count = 0;
        foreach (Proto p in protos) {
			p.mesh = p.GetMesh();
			if(p.mesh != null){
				if(count == 0){
					bounds = p.mesh.bounds;
				}
				else{
					bounds.Encapsulate(p.mesh.bounds);
				}		
                count++;
            }
        }
    }

	GameObject BuildProto(Proto proto, int protoNum) {
		
		var protoGO = new GameObject();
		Material material = null;
		bool skipMesh = false;
		
		File.AppendAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + WorldConversion.logsFolder + "SegmentMaterialRequests.log", proto.materialPath + '\n');

		if (!String.IsNullOrEmpty(proto.materialPath)){
			//This material is A) missing and B) used exclusively on crazy one-triangle meshes which we do not want.
			if(proto.materialPath == "Assets/GameData/Materials/automat/01/f64552.mat"){
				skipMesh = true;
			}
			else{
				material = AssetDatabase.LoadAssetAtPath(proto.materialPath, typeof(Material)) as Material;
			}
		}
		
		if(!skipMesh){
			if(proto.mesh == null){
				Debug.LogError("Can't fetch mesh for " + proto.segment.id + "-" + protoNum);
			}
			else{		
				//mesh = CruncherPlugin.CruncherAutomagic.Crunch(mesh);		   
				protoGO.AddComponent<MeshRenderer>();
				protoGO.AddComponent<MeshFilter>();
				protoGO.GetComponent<MeshFilter>().sharedMesh = proto.mesh;
				protoGO.AddComponent<MeshCollider>();
				protoGO.GetComponent<MeshCollider>().sharedMesh = proto.mesh;
		
				if (material) {
					protoGO.renderer.sharedMaterial = material;
				}
				else {
					protoGO.renderer.sharedMaterial = null;
				} 
			}
		}

		return protoGO;
	}

	List<GameObject> BuildAllProtos() {
		var protoGOs = new List<GameObject>();
		
		for (int i = 0; i < protos.Count; i++) {
			var proto = protos[i];
			var protoGO = BuildProto(proto, i);
			protoGO.name = "Proto" + i;
			protoGOs.Add(protoGO);
		}

		return protoGOs; 
	}
}
