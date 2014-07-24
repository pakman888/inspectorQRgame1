using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ChunkPipeline {
	
	public const string sceneDirectoryRelativeToProjectFolder = "Assets/GameData/Scenes";
	public const string chunkContentsDirectoryRelativeToProjectFolder = "Assets/GameData/ChunkContents";
	public const string chunkContentsFileName = "ChunkContents.asset";
	
	static void CheckInputDirectoryAndLogErrorAndCreateItIfNonExistent() {
		DirectoryInfo di = new DirectoryInfo(chunkContentsDirectoryRelativeToProjectFolder);
		if (!di.Exists) {
			di.Create();
			Debug.LogError("You must put your ChunkContents into " + chunkContentsDirectoryRelativeToProjectFolder + " in order to build. This folder did not exist, and has now been created for you.");
			AssetDatabase.Refresh();
			return;
		}
	}
	
	static void CreateSceneOutputFolderIfNonExistent() {
		DirectoryInfo di = new DirectoryInfo(sceneDirectoryRelativeToProjectFolder);
		if (!di.Exists) {
			di.Create();
		}
	}
	
	static void ClearOldScenes(){
		//Delete pre-existing scenes
		var sceneDir = new DirectoryInfo(sceneDirectoryRelativeToProjectFolder);
		FileInfo[] files = sceneDir.GetFiles("*.unity");
		for(int i = 0; i < files.Length; i++){
			if(files[i].Exists){
				files[i].Delete();
			}
			if(File.Exists(files[i].FullName + ".meta")){
				File.Delete(files[i].FullName + ".meta");
			}
		}
		//Clean up scene list
		List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
		sceneList = sceneList.FindAll(x => File.Exists(x.path));
		EditorBuildSettings.scenes = sceneList.ToArray();
	}
	
	public static ChunkContents GetOrCreateChunkContents() {
		string filePath = Path.Combine(chunkContentsDirectoryRelativeToProjectFolder, chunkContentsFileName);
		ChunkContents chunk = AssetDatabase.LoadMainAssetAtPath(filePath) as ChunkContents;
		if (chunk == null) {
			var directory = chunkContentsDirectoryRelativeToProjectFolder;
	        if (!Directory.Exists(directory)){
	            Directory.CreateDirectory(directory);
	        }

			var so = ScriptableObject.CreateInstance<ChunkContents>();
			AssetDatabase.CreateAsset(so, filePath);
			AssetDatabase.SaveAssets();
			return so;
		}
		return chunk;
	}
	
	public static void ChunkWorld() {
		var files = Directory.GetFiles(WorldConversion.segmentsFolder, "*.prefab", SearchOption.AllDirectories);
		
		try{
			AssetDatabase.StartAssetEditing();
			//Load and clear existing chunkcontents assets
			ChunkContents chunkContents = GetOrCreateChunkContents();
			chunkContents.Clear();

			//Rechunk
			for(int i = 0; i < files.Length; i++){
				int chunkId = ChunkNumberFromPrefabPath(files[i]);
				chunkContents.AddAssetToChunk(files[i], chunkId);
			}
		}
		finally{
			AssetDatabase.SaveAssets();
			AssetDatabase.StopAssetEditing();
		}
	}
	
	public static void BuildChunkScenes(){
		if(!EditorApplication.SaveCurrentSceneIfUserWantsTo()){
			return;
		}
		
		CheckInputDirectoryAndLogErrorAndCreateItIfNonExistent();
		
		CreateSceneOutputFolderIfNonExistent();
		ClearOldScenes();
		ChunkContents chunkContents = GetOrCreateChunkContents();
		List<int> chunkIds = new List<int>();
		List<float> chunkCosts = new List<float>();
		
		try{
			List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
			for(int i = 0; i < chunkContents.chunks.Count; i++){				
				EditorUtility.DisplayProgressBar("Building Scenes", (i+1) + "/" + chunkContents.chunks.Count, i/(float)chunkContents.chunks.Count);
				float chunkCost = BuildChunkScene(chunkContents.chunks[i]);
				chunkIds.Add(chunkContents.chunks[i].chunkNumber);
				chunkCosts.Add(chunkCost);
				string scenePath = PathForSceneForChunkDef(chunkContents.chunks[i]);
				if(!sceneList.Exists(x => x.path == scenePath)){
					sceneList.Add(new EditorBuildSettingsScene(scenePath, true));
				}
			}
			EditorBuildSettings.scenes = sceneList.ToArray();
			WorldBounds.Instance.SetChunkInfo(chunkIds, chunkCosts);
		}
		finally{
			EditorUtility.ClearProgressBar();
		}
	}
	
	public static string PathForSceneForChunkDef(ChunkDef chunkDef){
		return Path.Combine(sceneDirectoryRelativeToProjectFolder, chunkDef.chunkNumber + ".unity");
	}
	
	public static int ChunkNumberFromPrefabPath(string prefabPath){
		string fileName = Path.GetFileName(prefabPath).Substring("ChunkRoot".Length);
		return int.Parse(fileName.Substring(0, fileName.IndexOf('.')));
	}
	
	public static float BuildChunkScene(ChunkDef chunkDef){
		float totalCost = 0;
		var fullOutputPath = PathForSceneForChunkDef(chunkDef);
		
		EditorApplication.NewScene();
		
		//Blow up main camera that comes with every new scene
		Object.DestroyImmediate(Camera.main.gameObject);
		
		foreach (var name in chunkDef.assetNames) {
			GameObject obj = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath(name)) as GameObject;
			Chunk chunk = obj.GetComponent<Chunk>();
			if(chunk){
				totalCost += chunk.Cost;
			}
			else{
				Debug.LogError(name + " is missing a Chunk component");
			}
		}
		
		EditorApplication.SaveScene(fullOutputPath);		
		return totalCost;
	}

    public static string GetPathForFullCityScene() {
        return Path.Combine(sceneDirectoryRelativeToProjectFolder, "EntireCity.unity");
    }

    public static void BuildMulitpleChunksIntoScene(List<ChunkDef> chunkDefs) {
        Debug.Log(chunkDefs.Count); 
        var fullOutputPath = GetPathForFullCityScene();
		
		EditorApplication.NewScene();
		
		//Blow up main camera that comes with every new scene
        try {
            int count = 0;
            foreach( var chunkDef in chunkDefs ) { 
                foreach (var name in chunkDef.assetNames) {
                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath(name));
                }
                count++;
				EditorUtility.DisplayProgressBar("Adding Chunks", (count + 1) + "/" + chunkDefs.Count, count/(float)chunkDefs.Count);
            }
        }
        finally {
            EditorUtility.ClearProgressBar();
        }

		EditorApplication.SaveScene(fullOutputPath);		
    }

    public static void BuildGlobalSegmentScene() {
        if (!EditorApplication.SaveCurrentSceneIfUserWantsTo()) {
            return;
        }
        CheckInputDirectoryAndLogErrorAndCreateItIfNonExistent();
        CreateSceneOutputFolderIfNonExistent();
        ChunkContents chunkContents = GetOrCreateChunkContents();
        BuildMulitpleChunksIntoScene(chunkContents.chunks);
    }
	
	private static void MarkStaticRecursive(Transform t){
		t.gameObject.isStatic = true;
		foreach(Transform child in t){
			MarkStaticRecursive(child);
		}
	}
}

