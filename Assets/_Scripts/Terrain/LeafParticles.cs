using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeafParticles : MonoBehaviour {

	public float LeafSize;
	public List<Vector3> LeafPositions = new List<Vector3>();

	void Start () {
		var emitter = GetComponent<ParticleEmitter>();
		foreach(Vector3 pos in LeafPositions){
			emitter.Emit(pos, Vector3.zero, LeafSize, Mathf.Infinity, Color.white);
		}
	}
}
