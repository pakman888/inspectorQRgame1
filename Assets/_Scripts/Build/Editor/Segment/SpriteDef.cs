using UnityEngine;
using System.Collections;

public class SpriteDef {

	public Vector3 Position;
	public uint ModelIndex;
	
	public SpriteDef(){
		
	}
	
	public SpriteDef(Vector3 pos, uint model){
		Position = pos;
		ModelIndex = model;
	}

}
