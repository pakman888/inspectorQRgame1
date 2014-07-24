using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntersectionComponent : MonoBehaviour {

    public PrefabItem intersectionInfo;
    public List<NodeItem> nodes;
    public TrafficLightController trafficLightController;
    public GameObject trafficLightPrefab3 { get { return MissionHandler.Instance.trafficLight2;} }
    public GameObject trafficLightPrefab2 { get { return MissionHandler.Instance.trafficLight1; } }

    const int USES_TRAFFIC_LIGHTS_FLAG = 0x00080000;
    public bool UsesTrafficLight { 
        get { 
            return (USES_TRAFFIC_LIGHTS_FLAG & intersectionInfo.flags ) > 0; 
        }
    }

    const int FULL_NIBBLE_MASK = 0xf;
    const int ONE_NIBBLE_OFFSET = 4;

    public void Awake() {
        //LogRelevantInfo();
        if (UsesTrafficLight) {
            if (gameObject.GetComponent<TrafficLightController>() == null) {
                trafficLightController = gameObject.AddComponent<TrafficLightController>();
            } else {
                trafficLightController = gameObject.GetComponent<TrafficLightController>();
            }
            for (int i = 0; i < nodes.Count; i++) {
                var node = nodes[i];
                if ((intersectionInfo.flags & USES_TRAFFIC_LIGHTS_FLAG) > 0) {
                    AttachTrafficLight(node, i);
                }
            }
            trafficLightController.SetGroups(nodes);
        } else {
            var trafficLightController = gameObject.GetComponent<TrafficLightController>();
            Destroy(trafficLightController);
        }
        // hacks for prefab6 and prefab7
        if (intersectionInfo.prefabID == 6 || intersectionInfo.prefabID == 7) {
            Prefab6Hack();
        }
    }

    private void Prefab6Hack() {
        var info = GetComponent<SegmentExtraInfo>();
        if (info.id == 1633 || info.id == 3689 ) {
            var temp = nodes[0];
            nodes[0] = nodes[2];
            nodes[2] = nodes[1];
            nodes[1] = temp;
        } else if (info.id == 6859 || info.id == 6731|| info.id == 1880) {
            var temp = nodes[2];
            nodes[2] = nodes[1];
            nodes[1] = temp;
        } else if (info.id != 167 && info.id != 156) {
            Debug.LogError("unhandled prefab 6 at " + info.id);
        }
    }
    

    private GameObject GetTrafficLightPrefab(NodeItem node) {
        
        if (node.forwardIsRoad) {
            if (node.forwardRoadLook.laneCount0 >= 3) {
                return (GameObject) Instantiate(trafficLightPrefab3);
            } else {
                return (GameObject)Instantiate(trafficLightPrefab2);
            }
        }
        else {
            if (node.backwardRoadLook.laneCount1 >= 3) {
                return (GameObject) Instantiate(trafficLightPrefab3);
            } else {
                return (GameObject)Instantiate(trafficLightPrefab2);
            }
        }
        
    }

    float GetRoadWidth(NodeItem node) {
        var road = node.forwardIsRoad ? node.forwardRoadLook : node.backwardRoadLook;
        var width = road.size;
        width += road.offset;
        width *= 2;
        return width  ;
    }

    void AttachTrafficLight( NodeItem node, int nodeIndex) {
        float width = 0f;
        for ( int i = 0; i < nodes.Count; i++ ) {
            if (Vector3.Dot(node.direction, nodes[i].direction) < 0.9f) {
                width = GetRoadWidth(nodes[i]);
                break;    
            }
        }
        //------------------------------
        //******************************
        //     HAX <3 Simon Hacks
        //******************************
        //------------------------------
        if (intersectionInfo.prefabID == 6 || intersectionInfo.prefabID == 7 ) {
            width = 3;
        }
        // end hax
        var trafficLight = GetTrafficLightPrefab(node);
        trafficLight.transform.parent = transform;
        /* 
         * Traffic lights are weird, 
         * They control the nodes in which traffic can enter an intersection, 
         * We can tweak the positions of the trafficLights manually later
         */
        const float ARBITRARY_OFFSET_HELPER = 3.5f;
        const float ESTIMATED_CROSSWALK_WIDTH = 12f;
        if ( node.backwardIsRoad ) {  // bacwards nodes imply incoming road. 
            var lightOffset = node.backwardRoadLook.offset + node.backwardRoadLook.size + ARBITRARY_OFFSET_HELPER;
            var normal = new Vector3( node.direction.z * (-1),
                                      node.direction.y,
                                      node.direction.x);
            trafficLight.transform.position = node.position;
            trafficLight.transform.position += node.direction.normalized * (width + ESTIMATED_CROSSWALK_WIDTH + ARBITRARY_OFFSET_HELPER);
            trafficLight.transform.position -= lightOffset * normal.normalized;
            trafficLight.transform.rotation = Quaternion.LookRotation(node.direction );
        }

        if (node.forwardIsRoad) {
            if (node.forwardRoadLook.oneWay) { // if it is one way then traffic can't leave via this node
                trafficLight.SetActive(false);
                return;
            }
            var lightOffset = node.forwardRoadLook.offset + node.forwardRoadLook.size + ARBITRARY_OFFSET_HELPER;
            var normal = new Vector3( node.direction.z * (-1f),
                                      node.direction.y,
                                      node.direction.x);
            trafficLight.transform.position = node.position;
            trafficLight.transform.position += lightOffset * normal.normalized; 
            trafficLight.transform.position -= node.direction.normalized * (width + ESTIMATED_CROSSWALK_WIDTH + ARBITRARY_OFFSET_HELPER);
            trafficLight.transform.rotation = Quaternion.LookRotation(node.direction * -1f);
        }

        var trafficLightComponent = trafficLight.GetComponent<TrafficLight>();
		trafficLightComponent.node = node;
		trafficLightController.AddTrafficLight(trafficLightComponent);

        if (node.forwardIsRoad && node.backwardIsRoad) {
            Debug.LogWarning("more then one road on this intersection node");
        }
    }

    private void LogRelevantInfo() {
        if (intersectionInfo.flags > 0) {
            Debug.Log(string.Format("segment {0} ", name));
            Debug.Log("->" + intersectionInfo.flags);
            Debug.Log("->" + (intersectionInfo.flags & USES_TRAFFIC_LIGHTS_FLAG));
           
        }
        if (intersectionInfo.trLightModels > 0) {
            Debug.Log(string.Format("segment {0} ", name));
            Debug.Log("->rawValue" + intersectionInfo.trLightModels);
        }
    }

   void OnDrawGizmos() {
        if (nodes.Count == 3) {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(nodes[0].position, nodes[1].position);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(nodes[2].position, nodes[1].position);
        }
    
   }
}
