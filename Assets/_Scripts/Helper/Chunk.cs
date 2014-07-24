using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	const int kTransparentRenderQueue = 3000;

	public float Cost;
	
	public void CalculateCost(){
		Cost = CalculateCostRecursive(transform);
	}
	
	private float CalculateCostRecursive(Transform t){
		float cost = 0;
		
		//Mesh costs
		MeshFilter filter = t.GetComponent<MeshFilter>();
		if(filter && filter.sharedMesh != null){
			int tris = filter.sharedMesh.triangles.Length / 3;
			if(t.renderer && t.renderer.sharedMaterial){
				if(t.renderer.sharedMaterial.renderQueue == kTransparentRenderQueue){
					cost += tris * 2f; //Transparent elements are more expensive
				}
				else{
					cost += tris;
				}
			}
		}
		
		if(t.GetComponent<BusStopScript>()){
			cost += 3500; //Vaguely correct cost for most stops
		}
		
		LeafParticles leaves = t.GetComponent<LeafParticles>();
		if(leaves){
			cost += leaves.LeafPositions.Count * 8; //2 tris * 2 for transparency * fudge factor for high overdraw
		}
		
		foreach(Transform child in t){
			cost += CalculateCostRecursive(child);
		}
		return cost;
	}
}
