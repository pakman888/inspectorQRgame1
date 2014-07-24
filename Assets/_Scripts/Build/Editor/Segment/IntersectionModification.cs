using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class IntersectionModification {
    
    public static void AddIntersectionInfoToSegment(ref GameObject segment, ResourceServer server, SegmentExtraInfo extraInfo) {
        var intersectionComp = segment.AddComponent<IntersectionComponent>();
        intersectionComp.intersectionInfo = (PrefabItem)server.GetItem(extraInfo.id);
        
        intersectionComp.nodes = new List<NodeItem>();
        AssignTrafficLightPrefabs(ref intersectionComp);
 
        foreach (int nodeId in intersectionComp.intersectionInfo.nodeIndices) {
            PopulateRoadInformation(server, nodeId); 
            intersectionComp.nodes.Add(server.nodes[nodeId]);
        }
        AttachTrafficLightController(ref segment, server, intersectionComp.intersectionInfo);
        intersectionComp.intersectionInfo.nodes = intersectionComp.nodes;

        foreach (Transform child in segment.transform) {
            var material = child.gameObject.renderer.sharedMaterial;
            Texture texture = material.GetTexture("_MainTex");
            if (texture.name == "line_white_cross") {
                child.gameObject.collider.enabled = false;
                child.transform.Translate(Vector3.up * 0.05f);
            }
        }
        
    }

    static void AttachTrafficLightController(ref GameObject segment, ResourceServer server, PrefabItem intersectionInfo) {
        var trafficLightController = segment.AddComponent<TrafficLightController>();
        trafficLightController.intersectionInfo = intersectionInfo;
    }
    
    static void PopulateRoadInformation(ResourceServer server, int nodeId) {
        var node = server.nodes[nodeId];
        if (node.backwardIndex != 0) {
            if (server.GetItem(node.backwardIndex).kitType == Item.KIT_road) {
                node.backwardIsRoad = true;
                node.backwardRoadItem = (RoadItem)server.GetItem(node.backwardIndex);
                node.backwardRoadLook = server.roadLooks[node.backwardRoadItem.roadLookID];
            }
        }
        if (node.forwardIndex != 0) {
            if (server.GetItem(node.forwardIndex).kitType == Item.KIT_road) {
                node.forwardIsRoad = true;
                node.forwardRoadItem = (RoadItem)server.GetItem(node.forwardIndex);
                node.forwardRoadLook = server.roadLooks[node.forwardRoadItem.roadLookID];
            }
        }
    }

    static private void AssignTrafficLightPrefabs(ref IntersectionComponent intersectionComp) {
		/*
        intersectionComp.trafficLightPrefab3 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/traffic_light/city3.prefab", typeof(GameObject));
        if (intersectionComp.trafficLightPrefab3 == null) Debug.Log("notfound streetLight3");
        intersectionComp.trafficLightPrefab2 = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/traffic_light/city1.prefab", typeof(GameObject));
        if (intersectionComp.trafficLightPrefab2 == null) Debug.Log("notfound streetLight2");
        */
    }
}
