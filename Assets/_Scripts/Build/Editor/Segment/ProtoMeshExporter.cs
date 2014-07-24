using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ProtoMeshExporter{	
	private List<Proto> protos = new List<Proto>();
	
	public void Add(Proto p){
		protos.Add(p);
	}
	
	public void AddRange(IEnumerable<Proto> e){
		protos.AddRange(e);
	}
	
	public void Export(string path){
		using(StreamWriter writer = new StreamWriter(path)){
			int indexOffset = 1; //.obj file indices are 1-based
			foreach(Proto p in protos){
				Mesh mesh = p.CreateMesh();
				indexOffset = WriteMeshData(writer, p.GetMeshName(), mesh.vertices, mesh.uv, mesh.normals, mesh.triangles, indexOffset);				
				UnityEngine.Object.DestroyImmediate(mesh);
			}
		}
		protos.Clear();
	}
	
	//Exports the mesh with an offset, rotation, and scale
	public void Export(string path, Vector3 offset, Quaternion rotation, Vector3 scale){
		Matrix4x4 trs = Matrix4x4.TRS(offset, rotation, scale);
		using(StreamWriter writer = new StreamWriter(path)){
			int indexOffset = 1; //.obj file indices are 1-based
			foreach(Proto p in protos){
				Mesh mesh = p.CreateMesh();
				Vector3[] vertices = mesh.vertices;
				Vector3[] normals = mesh.normals;
				for(int i = 0; i < vertices.Length; i++){
					vertices[i] = trs.MultiplyPoint3x4(vertices[i]);
					normals[i] = trs.MultiplyVector(normals[i]);
				}
				indexOffset = WriteMeshData(writer, p.GetMeshName(), vertices, mesh.uv, normals, mesh.triangles, indexOffset);
				UnityEngine.Object.DestroyImmediate(mesh);
			}
		}
		protos.Clear();		
	}
	
	//Returns new index offset
	private int WriteMeshData(StreamWriter writer, string meshName, Vector3[] vertices, Vector2[] uv, Vector3[] normals, int[] triangles, int indexOffset){
		writer.WriteLine("g " + meshName);
				
		for(int i = 0; i < vertices.Length; i++){
			//OBJs have flipped X coordinates
			writer.WriteLine("v " + -vertices[i].x + " " + vertices[i].y + " " + vertices[i].z);
		}
		for(int i = 0; i < uv.Length; i++){
			writer.WriteLine("vt " + uv[i].x + " " + uv[i].y);			
		}
		for(int i = 0; i < normals.Length; i++){
			//OBJs have flipped X coordinates
			writer.WriteLine("vn " + -normals[i].x + " " + normals[i].y + " " + normals[i].z);
		}
		for(int i = 0; i < triangles.Length/3; i++){
			//OBJs have counterclockwise vertices, Unity uses clockwise
			int[] indices = new int[]{triangles[i*3]+indexOffset, triangles[i*3+2]+indexOffset, triangles[i*3+1]+indexOffset};
			writer.WriteLine("f " 
				+ indices[0] + "/" + indices[0] + "/" + indices[0] + " " 
				+ indices[1] + "/" + indices[1] + "/" + indices[1] + " " 
				+ indices[2] + "/" + indices[2] + "/" + indices[2]);
		}
		return indexOffset + vertices.Length;
	}
}
