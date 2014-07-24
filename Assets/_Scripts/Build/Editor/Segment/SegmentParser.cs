using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SegmentParser  {
	
	private string lastCrunchedLogPath = Path.Combine(WorldConversion.logsFolder, "LastCrunched.log");
	private Dictionary<PrismMaterial, string> anonymousMaterialsCache = new Dictionary<PrismMaterial, string>();
	private List<StaticSegment> staticSegments;

	int ReadOrCreateLastCrunchedLog() {
		int returnValue;
		if (File.Exists(lastCrunchedLogPath)) {
			var text = File.ReadAllText(lastCrunchedLogPath);
			if (text.Length > 0) {
				returnValue = Convert.ToInt32(text);
			} else {
				returnValue = 0;
			}
		}
		else {
			File.Create(lastCrunchedLogPath);
			returnValue = 0;
		}
		return returnValue;
	}
	
	void WriteLastCrunchedLog(int index) {
		File.WriteAllText(lastCrunchedLogPath, index.ToString());
	}
	
	public void ParseSegments(string foldername, bool buildAnonMaterials, bool loadRawGeometry){
		staticSegments = ParseStaticSegments(foldername, buildAnonMaterials, loadRawGeometry);
	}
	
	public void BuildGeometry(){
		if(staticSegments == null){
			Debug.LogError("Call ParseSegments first");
		}
		try{
			CreateDirectoryIfDoesNotExist(WorldConversion.meshesFolder);
			AssetDatabase.StartAssetEditing();

			Dictionary<string, List<Proto>> blobs = new Dictionary<string, List<Proto>>();
			EditorUtility.DisplayProgressBar("Loading Protos", "", 0);
			for(int i = 0; i < staticSegments.Count; i++){
				var segment = staticSegments[i];
				string blobPath = segment.GetMeshBlobPath();
				if(blobs.ContainsKey(blobPath)){
					blobs[blobPath].AddRange(segment.protos);
				}
				else{
					blobs[blobPath] = new List<Proto>(segment.protos);
				}
			}
			
			int exportedBlobs = 0;
			foreach(KeyValuePair<string, List<Proto>> kvp in blobs){
				EditorUtility.DisplayProgressBar("Exporting Meshes", exportedBlobs+"/"+blobs.Count, exportedBlobs/(float)blobs.Count);
				ProtoMeshExporter meshExporter = new ProtoMeshExporter();
				meshExporter.AddRange(kvp.Value);
				meshExporter.Export(kvp.Key);
				exportedBlobs += 1;
			}
		}
		finally{
			EditorUtility.ClearProgressBar();
			AssetDatabase.StopAssetEditing();
			AssetDatabase.SaveAssets();			
		}
	}
	
	public void BuildSegments(ResourceServer server) {
		Dictionary<int, List<Segment>> chunks = new Dictionary<int, List<Segment>>();
		int segmentCount;
		try{
			//Load segment geometry and calculate bounds
			int segmentsProcessed = 0;
			Bounds newWorldBounds = new Bounds(Vector3.zero, Vector3.zero);			
			
			IEnumerable<Segment> segments = staticSegments.Cast<Segment>().Concat(BusStopModification.CreateBusSegments(server).Cast<Segment>());
			segmentCount = segments.Count();
			
			foreach(Segment segment in segments){
				if(segmentsProcessed % 100 == 0){
					EditorUtility.DisplayProgressBar("Loading Geometry", segmentsProcessed+"/"+segmentCount, segmentsProcessed/(float)segmentCount);
				}
				segment.CalcBounds();
				newWorldBounds.Encapsulate(segment.bounds);
				segmentsProcessed++;
			}
			
			WorldBounds.Min = newWorldBounds.min;
			WorldBounds.Max = newWorldBounds.max;
			WorldBounds.Instance.CalculateUnitSizeAndGridDimensions();
			
			//Chunk world
			foreach(Segment segment in segments){
				if(server.GetLoD(segment.id) == SegmentLoD.Exclude){
					continue;
				}
				int chunkId = WorldBounds.GetChunkNumberForPosition(segment.bounds.center);
				if(!chunks.ContainsKey(chunkId)){
					chunks[chunkId] = new List<Segment>();
				}
				chunks[chunkId].Add(segment);
			}
		}
		finally{
			EditorUtility.ClearProgressBar();
		}
		VegetationCreator.ClearCachedModels();	
		try{		
			System.Console.WriteLine("**** START ASSET EDITING");
			AssetDatabase.StartAssetEditing();
			
			double startTime = EditorApplication.timeSinceStartup;			
			int segmentsProcessed = 0;
			
			foreach(KeyValuePair<int, List<Segment>> chunk in chunks){
				GameObject root = new GameObject("ChunkRoot" + chunk.Key);
				Chunk chunkComponent = root.AddComponent<Chunk>();
				foreach(Segment segment in chunk.Value){								
					if(segmentsProcessed % 10 == 0){
						double timePerSegment = 0;
						if(segmentsProcessed != 0){
						 	timePerSegment = (EditorApplication.timeSinceStartup - startTime)/(segmentsProcessed);
						}
						double timeRemaining = (segmentCount - segmentsProcessed) * timePerSegment;
						EditorUtility.DisplayProgressBar("Building Segments", segmentsProcessed+"/"+segmentCount + ", Est. " + timeRemaining.ToString("f0"), segmentsProcessed/(float)segmentCount);
					}

					var go = segment.CreateGameObject(server);
					go.transform.parent = root.transform;					
	
					segmentsProcessed++;
				}
				chunkComponent.CalculateCost();
				//Save asset
				CreateDirectoryIfDoesNotExist(WorldConversion.GetSegmentDirectory(chunk.Key));
				PrefabUtility.CreatePrefab(WorldConversion.GetSegmentPrefabPath(chunk.Key), root);
				//Cleanup
				UnityEngine.Object.DestroyImmediate(root);				
				//EditorUtility.UnloadUnusedAssets();
			}
		}
		finally{
			EditorUtility.ClearProgressBar();
			System.Console.WriteLine("**** SAVE ASSETS");
			AssetDatabase.SaveAssets();
			System.Console.WriteLine("**** STOP ASSET EDITING");
			AssetDatabase.StopAssetEditing();						
		}
	}

	SegmentVertexT ParseSegmentVertex(BinaryReader reader, out bool proceduralUv) {
		var sv = new SegmentVertexT();

		sv.pos = new Vector3(
			reader.ReadSingle(),
			reader.ReadSingle(),
			reader.ReadSingle() * (-1)
		);

		sv.norm = new Vector3(
			reader.ReadSingle(),
			reader.ReadSingle(),
			reader.ReadSingle() * (-1)
		);

		sv.clr = reader.ReadUInt32();
		
		//Sometimes UVs export as 0xCDCDCDCD, which we're assuming is a flag
		//for using procedural UVs. Here we're using (x,z) position coordinates
		//for uv coordinates.
		float proceduralUvScaleFactor = 0.25f; //Trial and error'd
		byte[] texXbytes = reader.ReadBytes(4);
		byte[] texYbytes = reader.ReadBytes(4);
		
		proceduralUv = true;
		for(int i = 0; i < texXbytes.Length; i++){
			if(texXbytes[i] != 0xCD || texYbytes[i] != 0xCD){
				proceduralUv = false;
				break;
			}
		}
		if(proceduralUv){
			sv.tex.x = sv.pos.x * proceduralUvScaleFactor;
			sv.tex.y = sv.pos.z * proceduralUvScaleFactor;
		}
		else{
			sv.tex.x = BitConverter.ToSingle(texXbytes, 0);
			sv.tex.y = BitConverter.ToSingle(texYbytes, 0);
		}
		
		return sv;
	}
	
	StaticSegment ParseSegment(string filename, bool buildAnonMaterials, bool loadRawGeometry) {
		var reader = new BinaryReader(File.Open(filename, FileMode.Open), new System.Text.ASCIIEncoding());

		var segmentType = reader.ReadInt32();
		var protoCount = reader.ReadUInt32();
		var vertexCount = new uint[protoCount];
		var indexCount = new uint[protoCount];
		
		for (uint i = 0; i < protoCount; i++) {
			vertexCount[i] = reader.ReadUInt32();
			indexCount[i] = reader.ReadUInt32();
		}

		var segment = new StaticSegment();
		var name = filename.Remove(filename.LastIndexOf('.')).Substring(filename.LastIndexOf(Path.DirectorySeparatorChar) + 1);
		segment.id = Convert.ToInt32(name);
		segment.segmentType = segmentType;
		for (uint i = 0; i < protoCount; i++) {
			if (vertexCount[i] == 0) {
				continue;
			}
			List<SegmentVertexT> segmentVertices;
			List<UInt16> indices;
			List<bool> proceduralUvs;
			if(loadRawGeometry){
				segmentVertices = new List<SegmentVertexT>();
				indices = new List<UInt16>();
				proceduralUvs = new List<bool>();

				bool isProceduralUv;

				for (uint j = 0; j < vertexCount[i]; j++) {
					segmentVertices.Add(ParseSegmentVertex(reader, out isProceduralUv));
					proceduralUvs.Add(isProceduralUv);
				}
				for (uint j = 0; j < indexCount[i]; j++) {
					indices.Add(reader.ReadUInt16());
				}
			}
			else{
				reader.BaseStream.Seek(SegmentVertexT.ByteLength * vertexCount[i] + 2 * indexCount[i], SeekOrigin.Current);
				segmentVertices = null;
				indices = null;
				proceduralUvs = null;
			}
			
			var materialStringLength = Convert.ToInt32(reader.ReadUInt32());
			var materialPath = new string(reader.ReadChars(materialStringLength));
			if(materialPath == "anonymous"){
				materialPath = ReadAnonymousMaterialData(reader, buildAnonMaterials);
			}
			else{
				ReadAnonymousMaterialData(reader, false);
				materialPath = "Assets/GameData/Materials" + materialPath;
			}		

			segment.protos.Add(new Proto {
				segment = segment,
				protoId = (int)i,
				segmentVertices = segmentVertices,
				indices = indices,
				materialPath = materialPath,
				proceduralUvs = proceduralUvs });
		}
		
		uint spriteCount = reader.ReadUInt32();
		SpriteDef[] sprites = new SpriteDef[spriteCount];
		for(int i = 0; i < spriteCount; i++){
			Vector3 pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			uint model = reader.ReadUInt32();
			sprites[i] = new SpriteDef(pos, model);
		}
		segment.sprites = sprites;
		
		return segment;
	}

	List<StaticSegment> ParseStaticSegments(string foldername,
									bool buildAnonMaterials,
									bool loadRawGeometry) {		
		if(buildAnonMaterials && Directory.Exists(WorldConversion.anonymousMaterialsFolder)){
			try{
				EditorUtility.DisplayProgressBar("Clearing old anon materials", "", 0);
				//Delete pre-existing materials
				var materialDir = new DirectoryInfo(WorldConversion.anonymousMaterialsFolder);
				FileInfo[] materialFiles = materialDir.GetFiles("*.mat");
				for(int i = 0; i < materialFiles.Length; i++){
					if(materialFiles[i].Exists){
						materialFiles[i].Delete();
					}
					if(File.Exists(materialFiles[i].FullName + ".meta")){
						File.Delete(materialFiles[i].FullName + ".meta");
					}
				}
				AssetDatabase.Refresh();
			}			
			finally{
				EditorUtility.ClearProgressBar();
			}
		}
	
		var segments = new List<StaticSegment>();
		
		foldername = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + foldername;
		foldername = foldername.Replace('/', Path.DirectorySeparatorChar);
		var files = Directory.GetFiles(foldername, "*.sbd");
		if(buildAnonMaterials){
			AssetDatabase.StartAssetEditing();
		}
		try{
			for(int i = 0; i < files.Length; i++){
				if(i%100 == 0){
					EditorUtility.DisplayProgressBar("Parsing Segments", i+"/"+files.Length, i/(float)files.Length);
				}
				string filename = files[i];
				var segment = ParseSegment(filename, buildAnonMaterials, loadRawGeometry);
				segments.Add(segment); 
			}
		}
		finally{
			EditorUtility.ClearProgressBar();
			if(buildAnonMaterials){
				AssetDatabase.StopAssetEditing();
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}
		return segments;
	}
	
	//If build is true, finds/builds a material matching the material data block
	//coming up in the passed stream. If build is false, just reads past the block.
	//Returns path to the anonymous material asset (empty if could not create asset) or null if build is false.
	private string ReadAnonymousMaterialData(BinaryReader reader, bool build){
		string effectName = ReadLengthPrefixedString(reader);
		//Find errors from segment exporting process
		if(effectName == "matconfailed"){
			Debug.LogError("Can't create anonymous material, material could not be exported");
			return build ? "" : null;
		}
		if(effectName == "effectconfailed"){
			Debug.LogError("Can't create anonymous material, effect_unit could not be exported");
			return build ? "" : null;
		}
		
		if(!build){
			//Skip data
			SkipLengthPrefixedStringArray(reader);
			SkipLengthPrefixedStringArray(reader);
			SkipLengthPrefixedString(reader);
			reader.BaseStream.Seek(+68, SeekOrigin.Current); //4 colors (16B each) + single(4B)
			return null;
		}
		
		//Load material data
		PrismMaterial pMat = new PrismMaterial();
		pMat.alias = effectName;
		pMat.tobjPaths.AddRange(ReadLengthPrefixedStringArray(reader));
		pMat.normalmapTobjPaths.AddRange(ReadLengthPrefixedStringArray(reader));
		pMat.specularTobjPath = ReadLengthPrefixedString(reader);
		pMat.LoadTexturePathsFromTObjPaths();
		
		pMat.ambient = ReadColor(reader);
		pMat.diffuse = ReadColor(reader);
		pMat.specular = ReadColor(reader);
		pMat.emission = ReadColor(reader);
		pMat.shininess = reader.ReadSingle();
		
		//Create material asset
		if(anonymousMaterialsCache.ContainsKey(pMat)){
			return anonymousMaterialsCache[pMat];
		}
				
		CreateDirectoryIfDoesNotExist(WorldConversion.anonymousMaterialsFolder);
		string uniquePath = AssetDatabase.GenerateUniqueAssetPath(WorldConversion.anonymousMaterialsFolder + "anonymous.mat");
		AssetDatabase.CreateAsset(pMat.CreateUnityMaterial(), uniquePath);
		anonymousMaterialsCache[pMat] = uniquePath;
		
		return uniquePath;
	}
	
	//Helper function to read a string that is prefixed by its length (as a uint32) from the stream
	private string ReadLengthPrefixedString(BinaryReader reader){
		int length = Convert.ToInt32(reader.ReadUInt32());
		return new string(reader.ReadChars(length));
	}
	
	//Helper function to read an array of length-prefixed strings that is
	//prefixed by its length (as a uint32) from the stream
	private string[] ReadLengthPrefixedStringArray(BinaryReader reader){
		int length = Convert.ToInt32(reader.ReadUInt32());
		string[] result = new string[length];
		for(int i = 0; i < length; i++){
			result[i] = ReadLengthPrefixedString(reader);
		}
		return result;
	}
	
	//Helper functions to skip over length-prefixed strings without reading them
	private void SkipLengthPrefixedString(BinaryReader reader){
		reader.BaseStream.Seek(reader.ReadUInt32(), SeekOrigin.Current);
	}
	
	private void SkipLengthPrefixedStringArray(BinaryReader reader){
		uint length = reader.ReadUInt32();
		for(int i = 0; i < length; i++){
			SkipLengthPrefixedString(reader);
		}
	}
	
	//Helper function to read a color as 4 floats
	private Color ReadColor(BinaryReader reader){
		return new Color(
			reader.ReadSingle(),
			reader.ReadSingle(),
			reader.ReadSingle(),
			reader.ReadSingle()
		);
	}
	
	void CreateDirectoryIfDoesNotExist(string directory) {
		if (!Directory.Exists(directory)){
			Debug.Log(string.Format("Creating directory {0}", directory));
			Directory.CreateDirectory(directory);
		} 
	}
}

[System.Serializable]
public class SegmentVertexT {
	//byte length in segment files
	public const int ByteLength = 36;
	
	public Vector3 pos;
	public Vector3 norm;
	public uint clr;
	public Vector2 tex;
}