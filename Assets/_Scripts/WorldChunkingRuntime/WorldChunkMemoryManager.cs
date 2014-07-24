using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class WorldChunkMemoryManager : Singleton<WorldChunkMemoryManager> { 	
	
	private enum ChunkAllocationMode {MaxCount, Budgeted};
	
	public Transform bus;
	
	private Dictionary<int, LoadedChunkRecord> loadedChunks;
	private int localGridDimension = 5;	
	private bool loadingUnloading = false;
	private PrioritizedOffset[] prioritizedOffsets;
	
	private ChunkAllocationMode allocMode = ChunkAllocationMode.MaxCount;
	private float chunkBudget = 200000; //Max chunk budget in Budgeted mode
	private int maxChunks = 18;			//Max number of chunks in MaxCount mode	
	
	private AsyncOperation ongoingResourceUnload;

    protected override void Awake() {
		SingletonSetup();
		
		loadedChunks = new Dictionary<int, LoadedChunkRecord>();
		PopulateOffsetGrid();
	}
	
	void Start() {
		Events.Instance.BusInstantiated += new BusEventHandler(OnBusInstantiated);
		Events.Instance.SwitchedStateContext += (object src, StateContextEventArgs e) => { 
			if (e.stateContext == StateContext.Menu) OnMenuLoaded();
		};
	}
	
	void PopulateOffsetGrid() {
		//Populate offset grid
		prioritizedOffsets = new PrioritizedOffset[localGridDimension * localGridDimension];
		for(int x = 0; x < localGridDimension; x++){
			for(int z = 0; z < localGridDimension; z++){
				PrioritizedOffset po = new PrioritizedOffset();
				po.offset = new Vector3(
					(x - localGridDimension/2) * WorldBounds.UnitSize,
					0,
					(z - localGridDimension/2) * WorldBounds.UnitSize
				);
				po.isCenter = (x-localGridDimension/2 == 0) && (z-localGridDimension/2 == 0);
				po.isInnerRing = (Mathf.Abs(x-localGridDimension/2) <= 1) && (Mathf.Abs(z-localGridDimension/2) <= 1);
				prioritizedOffsets[x + z * localGridDimension] = po;
			}
		}
	}
			
	void OnBusInstantiated(object sender, BusEventArgs e) {
		bus = e.bus.transform;
		StartCoroutine(FirstWorldLoad());
	}
	
	void OnMenuLoaded() {
		bus = null;
		UnloadAllChunks(); 
	}
	
	void UnloadAllChunks() {
		StartCoroutine(LoadAndUnloadChunks(new List<int>(), loadedChunks.Keys.ToList()));
	}
	
	IEnumerator FirstWorldLoad() {
		//Wait for starting chunks to load, then activate bus
		
		if(bus) {
			bus.gameObject.SetActive(false);
			ComputeNeededChunksAndStartLoadingProcess();
			while(loadingUnloading){
				yield return 0;
			}
			bus.gameObject.SetActive(true);
		}
		
		Events.Instance.OnMissionLoaded(this, EventArgs.Empty);
	}
	
	public void Update() {
		if (StateMachine.Instance.IsInContext(StateContext.Game)) {
			ComputeNeededChunksAndStartLoadingProcess();
		}
	}
	
	void ComputeNeededChunksAndStartLoadingProcess() {
		var neededChunks = GetNeededChunksForCurrentBusPosition();
		
		var chunksToLoad = new List<int>();
		for (int i = 0; i < neededChunks.Count; i++) {
			if (!loadedChunks.ContainsKey(neededChunks[i])) {
				chunksToLoad.Add(neededChunks[i]);
			}
		}
		
		var chunksToUnload = new List<int>();
		foreach (int loadedChunk in loadedChunks.Keys) {
			if (!neededChunks.Contains(loadedChunk)) {
				chunksToUnload.Add(loadedChunk);
			}
		}
		
		if (chunksToLoad.Count > 0 || chunksToUnload.Count > 0) {
			StartCoroutine(LoadAndUnloadChunks(chunksToLoad, chunksToUnload));
		}	
	}
	
	private IEnumerator LoadAndUnloadChunks(List<int> chunksToLoad, List<int> chunksToUnload) {
		if (!loadingUnloading) {
			loadingUnloading = true;
			// hax for synchronization reasons
			foreach (var chunkNumber in chunksToLoad) {
				loadedChunks.Add(chunkNumber, null);
			}
			
			foreach (var chunkNumber in chunksToUnload) {
				yield return StartCoroutine(UnloadChunk(chunkNumber));	
			}
			foreach (var chunkNumber in chunksToLoad) {
				yield return StartCoroutine(LoadChunk(chunkNumber));	
			}			
			loadingUnloading = false;
		}
		if(ongoingResourceUnload == null || ongoingResourceUnload.isDone){
			ongoingResourceUnload = Resources.UnloadUnusedAssets();
		}
		yield break;		
	}
	
	private IEnumerator LoadChunk(int chunkNumber) {
		if(!WorldBounds.IsValidChunk(chunkNumber)){
			yield break;
		}
		AsyncOperation loadOp = Application.LoadLevelAdditiveAsync(chunkNumber.ToString());
		if(loadOp == null){
			Debug.LogWarning("No chunk scene " + chunkNumber);
			yield break;
		}
		loadedChunks[chunkNumber] = new LoadedChunkRecord(chunkNumber);
		
		//Wait for scene to load
		while(!loadOp.isDone){
			yield return 0;
		}
		loadedChunks[chunkNumber].isDone = true;		
		loadedChunks[chunkNumber].root = GameObject.Find("/ChunkRoot" + chunkNumber);
	}
	
	private IEnumerator UnloadChunk(int chunkNumber) {
		LoadedChunkRecord c;
		if(loadedChunks.TryGetValue(chunkNumber, out c)){
			if(c != null){
				if(c.isDone){
					Destroy(c.root);
					loadedChunks.Remove(chunkNumber);
				}
				//If chunk is not done loading, don't remove it from loaded chunks
			}
			else{
				loadedChunks.Remove(chunkNumber);
			}
		}
		yield break;
	}

	float totalChunkCost;
	//Returns prioritized list of chunks, first = most important
	private List<int> GetNeededChunksForCurrentBusPosition() {
		PrioritizeChunkOffsets();
		totalChunkCost = 0;
		var neededChunks = new List<int>();
		int innerRingMarkedNeeded = 0; //How many inner ring chunks we've seen, we want to see all 9
		for (int i = 0; i < prioritizedOffsets.Length && ((allocMode == ChunkAllocationMode.Budgeted ? totalChunkCost < chunkBudget : neededChunks.Count < maxChunks) || innerRingMarkedNeeded < 9); i++) {			
			if(prioritizedOffsets[i].isInnerRing){
				innerRingMarkedNeeded++;
			}
			
			var needed = WorldBounds.GetChunkNumberForPosition(bus.position + prioritizedOffsets[i].offset);
			
			//check chunk is acceptable: valid and affordable (or required)
			if (needed != -1 && WorldBounds.IsValidChunk(needed)){
				float chunkCost = WorldBounds.GetChunkCost(needed);
				bool canAfford = allocMode == ChunkAllocationMode.MaxCount || totalChunkCost + chunkCost < chunkBudget;
				if(prioritizedOffsets[i].isInnerRing || canAfford) {
					neededChunks.Add(needed);
					totalChunkCost += chunkCost;
				}
#if UNITY_EDITOR
				//Debug: Draw grid segment bounds
				Rect bounds = WorldBounds.GetGridBoundsForPosition(bus.position + prioritizedOffsets[i].offset);
				Color color = Color.Lerp(Color.green, Color.red, i/(float)(prioritizedOffsets.Length-1));
				Debug.DrawRay(new Vector3(bounds.x, bus.position.y, bounds.y), new Vector3(bounds.width, 0, 0), color);
				Debug.DrawRay(new Vector3(bounds.x, bus.position.y, bounds.y), new Vector3(0, 0, bounds.height), color);
				Debug.DrawRay(new Vector3(bounds.x, bus.position.y, bounds.yMax), new Vector3(bounds.width, 0, 0), color);
				Debug.DrawRay(new Vector3(bounds.xMax, bus.position.y, bounds.y), new Vector3(0, 0, bounds.height), color);
				if(!canAfford){
					Debug.DrawRay(new Vector3(bounds.x, bus.position.y, bounds.y), new Vector3(bounds.width, 0, bounds.height), color);
					Debug.DrawRay(new Vector3(bounds.x, bus.position.y, bounds.yMax), new Vector3(bounds.width, 0, -bounds.height), color);
				}
#endif
			}
		}
		
		return neededChunks;		
	}
	
	//Prioritizes grid offsets based on current bus direction
	private void PrioritizeChunkOffsets(){
		Vector3 busChunkCenter = WorldBounds.GetCenterOfChunkForPosition(bus.position);
		Vector3 busOffset = bus.position - busChunkCenter;
		busOffset.y = 0;
		//Calculate angle multiplier such that being directly behind is like being on the other side of the grid
		float angleMultiplier = WorldBounds.UnitSize / (180f / (localGridDimension / 2));
		
		Vector3 flatForward = bus.forward;
		flatForward.y = 0;
		
		foreach(PrioritizedOffset po in prioritizedOffsets){
			float dist = (po.offset - busOffset).magnitude;
			float angle = Vector3.Angle(po.offset, flatForward);
			po.priority = angle * angleMultiplier + dist;
		}
		
		System.Array.Sort(prioritizedOffsets, (a, b) => {			
			//central square is always top priority
			if(a.isCenter){
				return -1;
			}
			if(b.isCenter){
				return 1;
			}
			if(a.isInnerRing && !b.isInnerRing){
				return -1;
			}
			if(b.isInnerRing && !a.isInnerRing){
				return 1;
			}
			return a.priority.CompareTo(b.priority);
		});
	}
	
	/*
	void OnGUI(){
		GUI.Label(new Rect(512, 0, 240, 32), "Chnk " + totalChunkCost.ToString("f0") + " / " + chunkBudget.ToString("f0"));
		if(GUI.Button(new Rect(900, 0, 124, 64), "40k")){
			chunkBudget = 40000;
		}
		if(GUI.Button(new Rect(900, 64, 124, 64), "80k")){
			chunkBudget = 80000;
		}
		if(GUI.Button(new Rect(900, 128, 124, 64), "120k")){
			chunkBudget = 120000;
		}
		if(GUI.Button(new Rect(900, 192, 124, 64), "160k")){
			chunkBudget = 160000;
		}
		if(GUI.Button(new Rect(900, 256, 124, 64), "200k")){
			chunkBudget = 200000;
		}
	}
	*/
	
	private class PrioritizedOffset{
		public Vector3 offset;
		public bool isCenter; //Is the center square, i.e. offset 0,0
		public bool isInnerRing; //Is center or is one of the 8 squares immediately surrounding the center square
		
		//sorting priority, lower is higher priority
		public float priority;
		
		public PrioritizedOffset(){
			
		}
		
		public PrioritizedOffset(Vector3 offset){
			this.offset = offset;
		}
	}
}

class LoadedChunkRecord {	
	public int number;
	public GameObject root;
	public bool isDone;
	
	public LoadedChunkRecord(int number) {
		this.number = number;
	}
}