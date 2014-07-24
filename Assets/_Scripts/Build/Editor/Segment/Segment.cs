using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public abstract class Segment {		
	public int segmentType;
    public int id;
	public Bounds bounds;
	public SpriteDef[] sprites;
		
	public abstract void CalcBounds();
	public abstract GameObject CreateGameObject(ResourceServer server);
}
