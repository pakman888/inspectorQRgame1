using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChunkDef {
	public int chunkNumber = -1;
	public List<string> assetNames = new List<string>();
	
	public ChunkDef(){
		
	}
	
	public ChunkDef(int id){
		chunkNumber = id;
	}
		
	public void StripMissingScripts	() {
		foreach (var assetName in assetNames) {
			var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetName, typeof(GameObject));
			int i = 0;
			var comps = prefab.GetComponents<MonoBehaviour>();
			while (i < comps.Length) {
			//foreach (var component in prefab.GetComponents<MonoBehaviour>()) {
				var component = comps[i];
				if (component == null) {
					component = new MonoBehaviour();
					Object.DestroyImmediate(component, true);
					Debug.Log (prefab.name);
				}
				i++;
				EditorUtility.SetDirty(prefab);
			}
		}			
		EditorUtility.UnloadUnusedAssets();
		AssetDatabase.SaveAssets();
	}
	
	//FIXME: This method probably no longer works due to changes in segment/chunk tree depth
	public GeometryData GetGeometryData(){
		GeometryData result = new GeometryData() {meshes = new List<MeshGeometryData>()};
		result.chunk = this;
		foreach(var assetName in assetNames){
			var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(assetName, typeof(GameObject));
			foreach(Transform child in prefab.transform){
				var mf = child.GetComponent<MeshFilter>();
				if(mf && mf.sharedMesh != null){
					int tris = mf.sharedMesh.triangles.Length / 3;
					result.meshes.Add(new MeshGeometryData(mf.sharedMesh.name, tris));
					result.totalTriangles += tris;
				}
			}		
		}
		return result;
	}
	
	public struct GeometryData{
		public ChunkDef chunk;
		public int totalTriangles;
		public List<MeshGeometryData> meshes;
	}
	
	public struct MeshGeometryData{
		public string name;
		public int triangles;
		
		public MeshGeometryData(string name, int tris){
			this.name = name;
			triangles = tris;
		}
	}
}
