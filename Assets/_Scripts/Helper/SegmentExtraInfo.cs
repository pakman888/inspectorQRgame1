using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SegmentExtraInfo : MonoBehaviour {
	
    public Vector3 position;
    public int segmentType;
    ///road 0
	///terrain 1    
	///buildings 2
    /// prefab 3
	/// model 4
	public int id;

	// 0 never, should really have been pre-stripped
	// 1 half chunk
	// 2 full chunk
	// 3 2 chunks
	// 4 4 chunks
	// 5 8 chunks / "always"
	public int lod = 2;
	private List<Renderer>[] lodRenderers;
	
	private IEnumerator Start(){
		GatherRenderers();
		while(true){
			if(WorldChunkMemoryManager.Instance.bus != null){
				float busDistanceSquare = (position - WorldChunkMemoryManager.Instance.bus.position).sqrMagnitude;
				for(int i = 0; i < lodRenderers.Length; i++){
					bool lodEnabled = busDistanceSquare <= GetDrawDistanceForLodLevel(i);
					foreach(Renderer r in lodRenderers[i]){
						r.enabled = lodEnabled;
					}
				}
			}
			yield return new WaitForSeconds(2);
		}
	}
	
	private void GatherRenderers(){
		lodRenderers = new List<Renderer>[6];
		for(int i = 0; i < lodRenderers.Length; i++){
			lodRenderers[i] = new List<Renderer>();
		}
		GatherRenderersRecursive(transform);
	}
	
	private void GatherRenderersRecursive(Transform t){
		if(segmentType == 2 || segmentType == 4){
			if(t.renderer){
				lodRenderers[lod].Add(t.renderer);
			}
		}
		if(segmentType == 0){
			//Handle various medians and lines
			Renderer r = t.renderer;
			if(r && r.sharedMaterial){
				if(r.sharedMaterial.name == "lines"){
					lodRenderers[2].Add(r);
				}
				else if(r.sharedMaterial.name == "stripes"){
					lodRenderers[2].Add(r);
				}
				else if(r.sharedMaterial.name == "f9b09e"){ //Generic grassy median
					lodRenderers[2].Add(r);
				}
				else if(r.sharedMaterial.name == "7f7226"){ //Generic guardrail
					lodRenderers[2].Add(r);
				}
				else if(r.sharedMaterial.name == "8e6846"){ //Tramway median
					lodRenderers[2].Add(r);
				}
				else if(r.sharedMaterial.name == "f0eef9"){ //Painted traffic island (Zigzag stripes)
					lodRenderers[2].Add(r);
				}
			}
		}
		
		foreach(Transform child in t){
			GatherRenderersRecursive(child);
		}
	}
	
	private float GetDrawDistanceForLodLevel(int lod){
		if(lod <= 0){
			return 0;
		}
		float d = WorldBounds.UnitSize * 0.5f * (1 << (lod-1));
		return d*d;
	}
}
