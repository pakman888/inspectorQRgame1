using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Proto {
	//Segment identifier
	public StaticSegment segment;
	public int protoId;
	
	//Raw Geometry
    public List<SegmentVertexT> segmentVertices;
    public List<UInt16> indices;
	public List<bool> proceduralUvs;
	
	//Processed Geometry
	public Mesh mesh;
	
	public string materialPath;
    
	
	//Mesh name is not allowed to contain spaces
	public string GetMeshName(){
		return segment.id + "-" + protoId;
	}
	
	public Mesh GetMesh(){
		UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(segment.GetMeshBlobPath());
		return System.Array.Find(objs, x => x.name == GetMeshName() && x.GetType() == typeof(Mesh)) as Mesh;
	}
	
	public Mesh CreateMesh() {
		var mesh = new Mesh();
		var vertexCount = segmentVertices.Count;
		
        var vertices = new Vector3[vertexCount];
        var normals = new Vector3[vertexCount];
        // TODO: colors
        //var colors = new Color[vertexCount];
        var uv = new Vector2[vertexCount];
        var triangles = new int[indices.Count];

		Vector2 proceduralUvTotal = Vector2.zero;
		int proceduralUvCount = 0;
		
        for (int i = 0; i < vertexCount; i++) {
            var segmentVertex = segmentVertices[i];
            vertices[i] = segmentVertex.pos;
            normals[i] = segmentVertex.norm;
            // TODO: colors
            uv[i] = new Vector2((segmentVertex.tex.x), (1 - segmentVertex.tex.y));
			if(proceduralUvs[i]){
				proceduralUvTotal += uv[i];
				proceduralUvCount += 1;
			}
        }
		
		//Procedural UVs are derived from world position and can be relatively large (100s).
		//Low uv precision on device hardware causes texture "swimming"
		//so subtract a baseline value from all procedural uvs (which all tile) in the mesh to reduce uv magnitude
		if(proceduralUvCount > 0){
			Vector2 proceduralUvOffset = proceduralUvTotal / proceduralUvCount;
			proceduralUvOffset.x = Mathf.FloorToInt(proceduralUvOffset.x);
			proceduralUvOffset.y = Mathf.FloorToInt(proceduralUvOffset.y);
			for(int i = 0; i < uv.Length; i++){
				if(proceduralUvs[i]){
					uv[i] = uv[i] - proceduralUvOffset;
				}
			}
		}		

        for (int i = 0; i < indices.Count; i += 3) {
            triangles[i] = Convert.ToInt32(indices[i]);
            triangles[i+1] = Convert.ToInt32(indices[i+1]);
            triangles[i+2] = Convert.ToInt32(indices[i+2]);
		}
		
		mesh.vertices = vertices;
        mesh.normals = normals;
        // TODO: colors
        mesh.uv = uv;
        mesh.triangles = triangles;
		
		return mesh;
	}
}
