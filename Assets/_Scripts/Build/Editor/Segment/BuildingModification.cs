using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildingModification {

	public const string LeafPrefabPath = "Assets/_Prefabs/Terrain/LeafParticles.prefab";
	

	public static void ModifyBuildingSegment(ref GameObject segment, ResourceServer server, SegmentExtraInfo info){
		foreach (Transform child in segment.transform) {
            if (child.renderer != null) {
                var material = child.gameObject.renderer.sharedMaterial;
				if(!material){
					Debug.LogWarning(child.name + " in building segment " + segment.name + " has no material");
				}	
				else{
 	               Texture texture = material.GetTexture("_MainTex");
	                if (texture && texture.name == "sf_rails") {
	                    child.gameObject.collider.enabled = false;
	                    child.transform.Translate(Vector3.up * 0.06f); //Slightly higher than crosswalks / road lines
	                }
				}
            }
        }
		
		BuildingItem buildingItem = server.GetItem(info.id) as BuildingItem;
		if(buildingItem.hookups.Count > 0){
			GameObject leafObject = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath(LeafPrefabPath)) as GameObject;
			leafObject.transform.parent = segment.transform;
			leafObject.transform.position = info.position;
			var leaves = leafObject.GetComponent<LeafParticles>();
			foreach(Hookup hookup in buildingItem.hookups){
				leaves.LeafPositions.Add(Vector3.Scale(hookup.position, new Vector3(1, 1, -1)));
			}
			//TODO: Better way to fetch this material?
			string matPath = Path.Combine(WorldConversion.materialsFolder, buildingItem.hookups[0].materialName.Substring(1));
			Material mat = AssetDatabase.LoadMainAssetAtPath(matPath) as Material;
			if(!mat){
				Debug.LogError("No hookup material " + matPath);
			}
			else{
				leafObject.renderer.material = mat;
			}
		}
	}
}