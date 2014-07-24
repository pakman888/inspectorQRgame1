using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

public class WorldBounds : DerivedSingleton<WorldBounds> {
	
	public static Vector3 Min{
		get{
			return Instance.min;
		}
#if UNITY_EDITOR
		set{
			Instance.min = value;
			EditorUtility.SetDirty(Instance);
		}
#endif
	}	
	[SerializeField] private Vector3 min;
	
	public static Vector3 Max{
		get{
			return Instance.max;
		}
#if UNITY_EDITOR
		set{
			Instance.max = value;
			EditorUtility.SetDirty(Instance);
		}
#endif
	}
	[SerializeField] private Vector3 max;
	
	public static int Subdivisions{
		get{
			return Instance.subdivisions;
		}
	}
	[SerializeField] private int subdivisions = 30;
	
	public static System.Collections.ObjectModel.ReadOnlyCollection<int> GridDimensions{
		get{
			return Instance.gridDimensions.AsReadOnly();
		}
	}
	[SerializeField] private List<int> gridDimensions; //This is used as a Vector2i
	
	public static float UnitSize{
		get{
			return Instance.unitSize;
		}
	}
	[SerializeField] private float unitSize;

	[SerializeField] private int[] validChunks;
	[SerializeField] private float[] chunkCosts;
	private Dictionary<int, float> validChunkCosts;

	public static int GetChunkNumberForPosition(Vector3 pos) {
		int chunkNumber = (int)((Subdivisions * (Mathf.FloorToInt((pos.x - Min.x) / UnitSize))) + 
								          (Mathf.FloorToInt((pos.z - Min.z) / UnitSize)));
		if (chunkNumber < 0 || chunkNumber >= Instance.gridDimensions[0] * Instance.gridDimensions[1]) {
			return -1;
		}
		else {
			return chunkNumber;	
		}
	}
	
	public static Vector3 GetCenterOfChunkForPosition(Vector3 pos){
		return new Vector3(
			Mathf.FloorToInt((pos.x - Min.x) / UnitSize) * UnitSize + Min.x + UnitSize/2,
			0,
			Mathf.FloorToInt((pos.z - Min.z) / UnitSize) * UnitSize + Min.z + UnitSize/2
		);
	}
	
	public static Vector3 GetCenterOfChunk(int chunkNumber){
		int x = chunkNumber % Subdivisions;
		int z = chunkNumber / Subdivisions;
		return new Vector3((x + 0.5f) * UnitSize - Min.x, 0, (z + 0.5f) * UnitSize - Min.z);
	}
	
	public static Rect GetGridBoundsForPosition(Vector3 pos){
		return new Rect(
			Mathf.FloorToInt((pos.x - Min.x) / UnitSize) * UnitSize + Min.x,
			Mathf.FloorToInt((pos.z - Min.z) / UnitSize) * UnitSize + Min.z,
			UnitSize,
			UnitSize
		);
	}
	
	public static bool IsValidChunk(int chunkId){
		if(Instance.validChunkCosts == null){
			Instance.BuildValidChunks();
		}
		return Instance.validChunkCosts.ContainsKey(chunkId); 
	}
	
	public static float GetChunkCost(int chunkId){
		if(Instance.validChunkCosts == null){
			Instance.BuildValidChunks();
		}
		float cost = 0;
		Instance.validChunkCosts.TryGetValue(chunkId, out cost);
		return cost;
	}

	[ContextMenu("Calculate Unit Size and Grid Dimensions")]
	public void CalculateUnitSizeAndGridDimensions() {
		Vector3 total = max - min;
		var xLength = total.x;
		var zLength = total.z;
		
		unitSize = xLength / subdivisions;
		gridDimensions = new List<int>{ subdivisions, Mathf.CeilToInt(zLength / unitSize) };
#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif
	}
	
#if UNITY_EDITOR
	public void SetChunkInfo(IList<int> ids, IList<float> costs){
		validChunks = new int[ids.Count];
		chunkCosts = new float[costs.Count];
		for(int i = 0; i < ids.Count; i++){
			validChunks[i] = ids[i];
			chunkCosts[i] = costs[i];
		}
		EditorUtility.SetDirty(this);
	}
#endif
	
	private void BuildValidChunks(){
		validChunkCosts = new Dictionary<int, float>();
		for(int i = 0; i < validChunks.Length; i++){
			validChunkCosts.Add(validChunks[i], chunkCosts[i]);
		}
	}
}
