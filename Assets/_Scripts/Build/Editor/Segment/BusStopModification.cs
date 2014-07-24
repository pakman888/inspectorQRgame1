using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class BusStopModification {
	public static List<BusStopSegment> CreateBusSegments(ResourceServer server) {
		BusStopSegment.ClearCachedModels();
		var result = new List<BusStopSegment>();
		for(int i = 0; i < server.busStopItems.Count; i++){
			BusStopItem item = server.busStopItems[i];
			BusStopSegment busStop = new BusStopSegment();
			busStop.item = item;
			busStop.segmentType = item.kitType;
			busStop.id = i;
			busStop.bounds = new Bounds(server.nodes[((BusStopItem)item).index].position, Vector3.zero);
			
			result.Add(busStop);
		}
		return result;
	}
}
