using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class WorldConversion : Editor {
	public static string gameDataFolder = "Assets/GameData/";
	public static string segmentsFolder = gameDataFolder + "Segments/"; //See also GetSegmentDirectoryPath and GetSegmentPrefabPath
	public static string meshesFolder = gameDataFolder + "Meshes/";
	public static string materialsFolder = gameDataFolder + "Materials/";
	public static string anonymousMaterialsFolder = materialsFolder + "Anonymous/";
	public static string logsFolder = "Assets/Logs/";
	public static string sourceBaseFolder = "Assets/base/";
    public static string routeSourceFolder = "Assets/base/def/route/";
	public static string parserTargetFolder = "Segments/bus1";

	[MenuItem("WorldConversion/Build Phase 0", false, 0)]
	public static void BuildPhase0() {		
		GenerateMaterials();
		AssetDatabase.Refresh();
		
		var parser = new SegmentParser();
		parser.ParseSegments(parserTargetFolder, true, true);
		
		GenerateAndOptimizeWorldGeometry(parser);
		AssetDatabase.Refresh();
		
		CreateSegmentPrefabs(parser);
		AssetDatabase.Refresh();
		
		CreateStops();
		AssetDatabase.Refresh();
		
		MissionRepository.CreateOrUpdateMissionRepository();
		AssetDatabase.Refresh();
	}

	[MenuItem("WorldConversion/Build Phase 1", false, 0)]
	public static void BuildPhase1() {
		ChunkWorld();
		BuildScenes();
	}

	[MenuItem("WorldConversion/Build All Phases", false, 0)]
	public static void BuildAllPhases() {
		BuildPhase0();
		BuildPhase1();
	}
	
	[MenuItem("WorldConversion/Generate Materials", false, 11)]
	public static void GenerateMaterials() {
		MaterialGenerator.Generate();
	}

	[MenuItem("WorldConversion/Generate and Optimize World Geometry", false, 11)]	
	public static void GenerateAndOptimizeWorldGeometry(){
		GenerateAndOptimizeWorldGeometry(null);
	}
	
	public static void GenerateAndOptimizeWorldGeometry(SegmentParser parser) {
		if(parser == null){
			parser = new SegmentParser();
			parser.ParseSegments(parserTargetFolder, false, true);
		}
		parser.BuildGeometry();
	}
	
	[MenuItem("WorldConversion/Create Segment Prefabs", false, 11)]
	public static void CreateSegmentPrefabs(){
		CreateSegmentPrefabs(null);
	}
	
	public static void CreateSegmentPrefabs(SegmentParser parser){
		if(parser == null){
			parser = new SegmentParser();
			parser.ParseSegments(parserTargetFolder, true, false);
		}
		parser.BuildSegments(ResourceServer.Instance);
	}

	// Phase 1
	[MenuItem("WorldConversion/Chunk World", false, 22)]
	public static void ChunkWorld() {
		ChunkPipeline.ChunkWorld();
	}
	
	[MenuItem("WorldConversion/Build Chunks into Scenes", false, 22)]
	public static void BuildScenes() {
		ChunkPipeline.BuildChunkScenes();
	}
	
	[MenuItem("WorldConversion/Create Mission Repository", false, 99)]
	public static void CreateMissionRepository() {
		MissionRepository.CreateOrUpdateMissionRepository();
	}
	
	// Misc
    [MenuItem("WorldConversion/(Re)Generate Missions", false, 99)]
    public static void ParseAllRoutes() {
        var foldername = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + routeSourceFolder;
        foldername = foldername.Replace('/', Path.DirectorySeparatorChar);
        var files = Directory.GetFiles(foldername);
        foreach (string filename in files) {
            if (filename.EndsWith(".meta")) {
                continue;
            }
            RouteParser.ParseAndWrite(filename,ResourceServer.Instance);
        }
		MissionRepository.CreateOrUpdateMissionRepository();
    } 

	public static string GetSegmentPrefabPath(int segmentId){
		return GetSegmentDirectory(segmentId) + "ChunkRoot" + segmentId +  ".prefab";
	}
	
	public static string GetSegmentDirectory(int segmentId){
		return segmentsFolder + (segmentId / 100) + "/";
	}

	private static Mesh FixMesh(Mesh mesh) {
		var vertices = new Vector3[mesh.vertexCount];
		var newMesh = mesh;
		for(int i = 0; i < mesh.vertexCount; i++) {
			vertices[i] = InverseZValue(mesh.vertices[i]);
		}
		newMesh.vertices = vertices;
		return newMesh;
	}

	private static Vector3 InverseZValue(Vector3 values) {
		return new Vector3(values.x, values.y, values.z * (-1)); 
	}

    [MenuItem("WorldConversion/Create Stops", false, 11)]
    public static void CreateStops() {
        BusStopModification.CreateBusSegments(ResourceServer.Instance);
    }

    [MenuItem("WorldConversion/CreateMap", false, 99)]
    public static void GenerateMap() {
        ChunkPipeline.BuildGlobalSegmentScene();
    }
	
	[MenuItem("WorldConversion/CreateMissionDescriptions", false, 99)]
    public static void crea() {
    	Debug.Log (MissionDescriptions.Instance);
    }

    [MenuItem("WorldConversion/PokeResourceServer",false, 99)]
    public static void poke() {
        var thing = ResourceServer.Instance;
    }
}
