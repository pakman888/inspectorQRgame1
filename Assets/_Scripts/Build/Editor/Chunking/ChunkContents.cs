using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ChunkContents : ScriptableObject {
	
	public List<ChunkDef> chunks = new List<ChunkDef>();
	
	private Dictionary<int, ChunkDef> chunksById;
	
	[ContextMenu(@"Identify Prefabs With Missing Scripts")]
	public void StripMissingScripts(){
		foreach(ChunkDef chunk in chunks){
			if(chunk != null){
				chunk.StripMissingScripts();
			}
		}
	}
	
	[ContextMenu(@"Log Geometry Stats to Console")]
	public void LogGeometryStats(){
		System.Console.WriteLine("Logging geometry stats...");
		List<ChunkDef.GeometryData> geoDatas = new List<ChunkDef.GeometryData>();
		foreach(ChunkDef chunk in chunks){
			geoDatas.Add(chunk.GetGeometryData());
			EditorUtility.UnloadUnusedAssets();
		}
		foreach(var result in geoDatas.OrderBy(geoData => -geoData.totalTriangles)){
			System.Console.WriteLine("Chunk " + result.chunk.chunkNumber + ": " + result.totalTriangles + " triangles");
			foreach(var meshData in result.meshes.OrderBy(x => -x.triangles)){
				System.Console.WriteLine(">" + meshData.name + " " + meshData.triangles + " (" + (meshData.triangles/(float)result.totalTriangles).ToString("P") + ")");
			}
		}
	}
	
	public ChunkDef GetChunkDefWithId(int id){
		if(chunksById == null || chunks.Count != chunksById.Count){
			BuildChunkDictionary();
		}
		ChunkDef result;
		if(!chunksById.TryGetValue(id, out result)){
			chunksById[id] = result = new ChunkDef(id);
			chunks.Add(result);
			EditorUtility.SetDirty(this);
		}
		return result;
	}
	
	public void AddAssetToChunk(string assetPath, int chunkNumber){
		GetChunkDefWithId(chunkNumber).assetNames.Add(assetPath);
		EditorUtility.SetDirty(this);
	}
	
	public void Clear(){
		chunks.Clear();
		chunksById = null;
		EditorUtility.SetDirty(this);
	}
	
	private void BuildChunkDictionary(){
		chunksById = new Dictionary<int, ChunkDef>();
		foreach(ChunkDef def in chunks){
			if(def != null){
				chunksById[def.chunkNumber] = def;
			}
		}
	}
}