using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class SegmentRankerWindow : EditorWindow {

	[MenuItem("Debug/Segment Ranker")]
	public static void OpenWindow(){
		EditorWindow.GetWindow(typeof(SegmentRankerWindow), true, "Segment Ranking", true);
	}

	int currentSegmentIndex = 0;
	int jumpSegmentIndex;
	GameObject currentSegment;
	const int segmentCount = 7197;
	int currentVertexCount;
	int lastRankedSegmentIndex = -1;
	float currentChunkCost = 0;
	bool blinkRenderer;
	Dictionary<int, int> rankings;

	public void OnEnable(){
		rankings = SegmentLoD.LoadRawData();
		currentSegmentIndex = -1;
		GetNextUnrankedSegment();
		FocusCurrentSegment();	
	}
	
	public void OnInspectorUpdate(){
		//Blink selected segment
		if(blinkRenderer && focusedWindow == this && currentSegment != null){
			if(Mathf.FloorToInt((float)EditorApplication.timeSinceStartup) % 2 == 0){
				SetRendererEnabledRecursive(currentSegment, true);
			}
			else{
				SetRendererEnabledRecursive(currentSegment, false);
			}
		}
	}
	
	public void OnLostFocus(){
		if(currentSegment != null){
			SetRendererEnabledRecursive(currentSegment, true);
		}
	}

	public void OnGUI(){
		GUILayout.Label("Viewing Segment " + currentSegmentIndex);
		//Jump to specific segment
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Jump To")){
			JumpToSegment(jumpSegmentIndex);
		}
		jumpSegmentIndex = EditorGUILayout.IntField("", jumpSegmentIndex);
		GUILayout.EndHorizontal();		
		
		if(GUILayout.Button("Re-focus current")){
			FocusCurrentSegment();
		}
		
		GUILayout.Label("Vertices: " + currentVertexCount);
		GUILayout.Label("Est. Chunk Cost: " + GetChunkCostDescriptor(currentChunkCost) + " (" + currentChunkCost + ")");
		if(rankings.ContainsKey(currentSegmentIndex)){
			GUILayout.Label("This segment has already been ranked " + rankings[currentSegmentIndex]);
			GUILayout.Label("Ranking it again will replace the current ranking.");
			if(GUILayout.Button("Next unranked segment")){
				GetNextUnrankedSegment();
				FocusCurrentSegment();
			}
		}
		
		//Rankings:		
		bool[] rankingKeys = new bool[6];
		if(Event.current.type == EventType.KeyDown){
			if(Event.current.keyCode == KeyCode.Keypad0 || Event.current.keyCode == KeyCode.Alpha0){
				rankingKeys[0] = true;
				Event.current.type = EventType.Used;
			}
			if(Event.current.keyCode == KeyCode.Keypad1 || Event.current.keyCode == KeyCode.Alpha1){
				rankingKeys[1] = true;
				Event.current.type = EventType.Used;
			}
			if(Event.current.keyCode == KeyCode.Keypad2 || Event.current.keyCode == KeyCode.Alpha2){
				rankingKeys[2] = true;
				Event.current.type = EventType.Used;
			}
			if(Event.current.keyCode == KeyCode.Keypad3 || Event.current.keyCode == KeyCode.Alpha3){
				rankingKeys[3] = true;
				Event.current.type = EventType.Used;
			}
			if(Event.current.keyCode == KeyCode.Keypad4 || Event.current.keyCode == KeyCode.Alpha4){
				rankingKeys[4] = true;
				Event.current.type = EventType.Used;
			}
			if(Event.current.keyCode == KeyCode.Keypad5 || Event.current.keyCode == KeyCode.Alpha5){
				rankingKeys[5] = true;
				Event.current.type = EventType.Used;
			}
		}
		GUILayout.BeginHorizontal();
		for(int i = 0; i <= 5; i++){
			if(GUILayout.Button(i.ToString())){
				rankingKeys[i] = true;
			}
		}
		GUILayout.EndHorizontal();
		for(int i = 0; i <= 5; i++){
			if(rankingKeys[i]){
				rankings[currentSegmentIndex] = i;
				lastRankedSegmentIndex = currentSegmentIndex;
				SegmentLoD.SaveRawData(rankings);
				GetNextUnrankedSegment();
				FocusCurrentSegment();
				break;
			}
		}
		
		if(lastRankedSegmentIndex >= 0){
			GUILayout.BeginHorizontal();
			GUILayout.Label("Last segment was ranked " + rankings[lastRankedSegmentIndex]);
			if(GUILayout.Button("Jump To")){
				JumpToSegment(lastRankedSegmentIndex);
			}
			GUILayout.EndHorizontal();
		}
		
		GUILayout.FlexibleSpace();
		bool newBlink = GUILayout.Toggle(blinkRenderer, "Blink Segment");
		if(blinkRenderer && !newBlink && currentSegment != null){
			SetRendererEnabledRecursive(currentSegment, true);
		}
		blinkRenderer = newBlink;
	}
	
	private void JumpToSegment(int index){
		var jumpObj = GameObject.Find("Segment " + index);
		if(jumpObj){
			if(currentSegment != null){
				SetRendererEnabledRecursive(currentSegment, true);
			}
			currentSegmentIndex = index;
			currentSegment = jumpObj;
			currentVertexCount = GetVertexCountRecursive(currentSegment.transform);
			currentChunkCost = GetChunkCost(currentSegment.transform);
			FocusCurrentSegment();
		}
		else{
			Debug.LogWarning("Segment " + index + " could not be found");
		}
	}
	
	private void GetNextUnrankedSegment(){
		if(currentSegment != null){
			SetRendererEnabledRecursive(currentSegment, true);
		}
		while(currentSegmentIndex < segmentCount-1){
			currentSegmentIndex++;
			if(rankings.ContainsKey(currentSegmentIndex)){
				continue;
			}
			currentSegment = GameObject.Find("Segment " + currentSegmentIndex);
			if(!currentSegment || currentSegment.GetComponent<BusStopScript>() || currentSegment.GetComponent<IntersectionComponent>()){
				continue;
			}
			var info = currentSegment.GetComponent<SegmentExtraInfo>();
			if(info.segmentType == 0 || info.segmentType == 1){
				continue;
			}
			else{
				break;
			}
		}
		currentVertexCount = GetVertexCountRecursive(currentSegment.transform);
		currentChunkCost = GetChunkCost(currentSegment.transform);
	}
	
	private void FocusCurrentSegment(){
		Selection.activeGameObject = currentSegment;		
		var view = SceneView.currentDrawingSceneView;
		if(view != null) {
			view.FrameSelected();
	    }
	}
	
	private int GetVertexCountRecursive(Transform t){
		int result = 0;
		MeshFilter filter = t.GetComponent<MeshFilter>();
		if(filter && filter.sharedMesh != null){			
			result = filter.sharedMesh.vertices.Length;			
		}
		foreach(Transform child in t){
			result += GetVertexCountRecursive(child);
		}
		return result;
	}
	
	private float GetChunkCost(Transform t){
		while(t != null){
			Chunk chunk = t.GetComponent<Chunk>();
			if(chunk){
				return chunk.Cost;
			}
			t = t.parent;
		}
		return 0;
	}
	
	private string GetChunkCostDescriptor(float cost){
		if(cost < 12000){
			return "Very Low";
		}
		if(cost < 20000){
			return "Low";
		}
		if(cost < 30000){
			return "Moderate";
		}
		if(cost < 40000){
			return "High";
		}
		return "Very High";
	}
	
	private void SetRendererEnabledRecursive(Transform t, bool enabled){
		if(t.renderer){
			t.renderer.enabled = enabled;
		}
		foreach(Transform child in t){
			SetRendererEnabledRecursive(child, enabled);
		}
	}
	
	private void SetRendererEnabledRecursive(GameObject obj, bool enabled){
		SetRendererEnabledRecursive(obj.transform, enabled);
	}
}
