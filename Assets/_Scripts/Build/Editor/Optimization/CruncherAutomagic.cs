using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace CruncherPlugin {
	public class CruncherAutomagic {
		public static float QUALITY = 0.6f;
		
		public static Mesh Crunch(Mesh originalMesh) {
			if (CruncherPluginManager.Startup() == false) {
				Debug.LogError("Cruncher won't start");
				return null;
			}
			
			CruncherIO cruncherIO = new CruncherIO();
			cruncherIO.vertices = (Vector3[])originalMesh.vertices.Clone();
			cruncherIO.triangles = (int[])originalMesh.triangles.Clone();
			cruncherIO.normals = (Vector3[])originalMesh.normals.Clone();
			cruncherIO.uv0s = (Vector2[])originalMesh.uv.Clone();
			
			/*
			cruncherIO.vertices = originalMesh.vertices;
			cruncherIO.triangles = originalMesh.triangles;
			cruncherIO.normals = originalMesh.normals;
			cruncherIO.uv0s = originalMesh.uv;
			*/
			
			cruncherIO.subMeshTriangles = new int[originalMesh.subMeshCount][];
			for(int i = 0; i < cruncherIO.subMeshTriangles.Length; i++) {
				cruncherIO.subMeshTriangles[i] = (int[])originalMesh.GetTriangles(i).Clone();
			}

			CruncherMeshConfiguration meshConfiguration = new CruncherMeshConfiguration();
			meshConfiguration.joinVertices = true;
			meshConfiguration.joinNormals = true;
			meshConfiguration.removeTJunctions = true;
			meshConfiguration.recalculateNormals = true;
			meshConfiguration.joinUvs = true;
			
			int meshId = CruncherPluginManager.AddMesh(cruncherIO, meshConfiguration);
			
			Mesh outputMesh = null;
			
			var startTime = Time.realtimeSinceStartup;
			if (meshId != -1) {
				CruncherAdjustment cruncherAdjustment = new CruncherAdjustment();
				cruncherAdjustment.type = CruncherAdjustment.Type.TargetQuality;
				cruncherAdjustment.quality = QUALITY;
				bool crunched = CruncherPluginManager.AdjustMeshes(cruncherAdjustment);
				
				if (crunched) {
					cruncherIO = CruncherPluginManager.RetrieveMesh(meshId, true, false);
					
					outputMesh = GetUnityMeshFromCruncherIO(cruncherIO);
					outputMesh.name = originalMesh.name;
			
					LogMeshSavings(originalMesh, outputMesh, startTime);
				}
				else {
					LogUnCrunchedMesh(originalMesh, startTime);
					outputMesh = originalMesh;
				}
			}
			else {
				LogUnCrunchedMesh(originalMesh, startTime);
				outputMesh = originalMesh;
			}
			
			CruncherPluginManager.Shutdown();
			
			return outputMesh;
		}
		
		static Mesh GetUnityMeshFromCruncherIO(CruncherIO cruncherIO) {
			var crunchedMesh = new Mesh();
			crunchedMesh.vertices = cruncherIO.vertices;
 			crunchedMesh.normals = cruncherIO.normals;
			crunchedMesh.uv = cruncherIO.uv0s;
			crunchedMesh.triangles = cruncherIO.triangles;
			return crunchedMesh;
		}
		
		public static void LogMeshSavings(Mesh originalMesh, Mesh crunchedMesh, float startTime) {
			var timeTaken = Time.realtimeSinceStartup - startTime;
			var line = originalMesh.name  + ": New Vertex Count: " + crunchedMesh.vertexCount + " vs Vertex Count: " +  originalMesh.vertexCount + ", Index Count: " + originalMesh.triangles.Length + " vs. New Index Count: " + crunchedMesh.triangles.Length + " using quality: " + QUALITY + ", vertex ratio: " + ((float)crunchedMesh.vertexCount / originalMesh.vertexCount).ToString() + " Time taken: " + timeTaken + "\n";;
			File.AppendAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + WorldConversion.logsFolder + "MeshSavings.log", line);	
		}
		
		public static void LogUnCrunchedMesh(Mesh originalMesh, float startTime) {
			var timeTaken = Time.realtimeSinceStartup - startTime;
			var line = originalMesh.name  + " was unable to be crunched. Time taken: " + timeTaken + "\n";
			File.AppendAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + WorldConversion.logsFolder + "MeshSavings.log", line);	
		}
	}
}
