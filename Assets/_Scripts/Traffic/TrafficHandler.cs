using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TrafficHandler : Singleton<TrafficHandler> {
    const int TRAFFIC_LAYER = 8;
    const float MAXIMUM_DISTANCE = 200;
    const float MINIMUM_DISTANCE = 100;
    const float MAXIMUM_ROAD_DENSITY = 1f / 25f;
    const float MAX_TRAFFIC = 10;
    public int trafficCount;

    public GameObject[] vehiclePrefabs;
    public List<GameObject> trafficList;
    public List<GameObject> crashedList;

    public const float CITY_SPEED_LIMIT_KPH = 30f;
    public const float CITY_SPEED_LIMIT_MPS = 30 / 3.6f;

	// Use this for initialization
	void Start () {
        trafficList = new List<GameObject>();
        trafficCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    // HandleCrashed
	}


    public static bool InRange(float value, float min, float max) {
        return (value >= min && value <= max); 
    }

    public bool SpawnTraffic(RoadComponent roadComponent, SegmentExtraInfo info) {
        var distance = (info.position - MissionHandler.Instance.bus.transform.position).magnitude;
        // possibly do this eles where. 
        // maybe have a list of loaded segments. 
        if (distance > MINIMUM_DISTANCE &&
            distance < MAXIMUM_DISTANCE &&
            trafficCount < MAX_TRAFFIC){ 
                // Get a spawn location;
                // confirm space 
                // then. 
               if ( !roadComponent.roadLook.oneWay ) 
                   InstantiateActor(roadComponent.gameObject, true);
               InstantiateActor(roadComponent.gameObject, false);
            return true;
        } 
        return false;
       //DebugCurvePlacement();
    }

    Vector3 GetPositionOfLaneOffset(Vector3 tan, RoadComponent road) {
        return road.Nodes[0].position + (TangentToNormalNoY(tan.normalized)) * (4.5f + road.roadLook.offset);
    }

    public static Vector3 TangentToNormalNoY(Vector3 tan) {
        return new Vector3(-1 * tan.z, 0, tan.x);
    }

    public static Vector3 TangentToNormal(Vector3 tan){ 
        tan = tan.normalized;
        return new Vector3(-1 * tan.z, tan.y, tan.x);
    }

    public float GetSpawnPosition(RoadComponent road, bool leftHandDriving) {
        List<TrafficVehicle> traffic;
        if ( leftHandDriving ) {
            traffic = road.LeftVehicles;
        } else {
            traffic = road.RightVehicles;
        }
        float newDist = 0.5f;
        if (traffic.Count > 0) {
            var maxSpace = 0f;
            for (int i = 0; i < traffic.Count; i++) {
                var entity = traffic[i];
                if (i == 0) {
                    var curveSpace = 1 - entity.dist;
                    // Spawn is forward;
                    if (curveSpace > maxSpace) {
                        maxSpace = curveSpace;
                        newDist = entity.dist + (curveSpace / 2);
                    }
                } 
                if ( i > 0 ) {
                    var forwards = traffic[i - 1];
                    var curveSpace = forwards.dist - entity.dist;
                    if (curveSpace > maxSpace) {
                        maxSpace = curveSpace;
                        newDist = entity.dist + (curveSpace / 2);
                    }
                }
                if (i == traffic.Count - 1) {
                    //Spawn is backwards
                    var curveSpace = entity.dist;
                    if (curveSpace > maxSpace) {
                        maxSpace = curveSpace;
                        newDist = curveSpace / 2;
                    }
                }
            }
        }
        return newDist;
    }

    void InstantiateActor( GameObject road, bool leftHandDriving) {

        trafficCount++;

        GameObject vehicle = (GameObject)Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)]);
        
        var actor = vehicle.GetComponent<TrafficVehicle>();

        var roadComponent = road.GetComponent<RoadComponent>();

        PopulateVehiclePositionInfo(ref actor, roadComponent, leftHandDriving);

        actor.currentSegment = road;
        
        AlignActorOnRoad(vehicle, actor);
        
        vehicle.transform.parent = road.transform;
        actor.currentSegment = road;
    }

    void AlignActorOnRoad ( GameObject vehicle, TrafficVehicle actor) {
        if (actor.currentSegment != null) {
            var roadInfo = actor.currentSegment.GetComponent<RoadComponent>();
            roadInfo.EnterRoadSegment(actor);
        } else {
            Debug.LogError("No CurrentSegment on Vehicle");
        }
        actor.Merge();
        actor.Drive();
    }

    void DebugCurvePlacement(RoadComponent roadComponent) {
        float lengthCoefficient = roadComponent.roadItem.length / 3;
        var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.renderer.material.color = Color.red;
        var pointOnCurve = Curves.SmoothCurve(
            roadComponent.Nodes[0].position, roadComponent.Nodes[0].direction * lengthCoefficient, 
            roadComponent.Nodes[1].position, roadComponent.Nodes[1].direction * lengthCoefficient, 
            0.5f, true);
        block.transform.position = pointOnCurve[0];
          
        block.transform.rotation = Quaternion.LookRotation(pointOnCurve[1]);
        block.transform.position += block.transform.up;

        var block2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block2.renderer.material.color = Color.blue;
        Vector3 offsetvalue = TangentToNormalNoY( pointOnCurve[1]).normalized * (4.5f + roadComponent.roadLook.offset);
        block2.transform.position = pointOnCurve[0] + offsetvalue;
        block2.transform.rotation = Quaternion.LookRotation(pointOnCurve[1]);

    }

    void PopulateVehiclePositionInfo(ref TrafficVehicle actor, 
             RoadComponent roadComponent, bool leftHandDriving) {

        actor.leftHandDriving = leftHandDriving;
        
        actor.dist = GetSpawnPosition(roadComponent, leftHandDriving);
        var lanes = roadComponent.roadLook.laneCount0;
        actor.currentLane = Random.Range(1, lanes + 1);
        var offset = (4.5f * actor.currentLane) + roadComponent.roadLook.offset - (4.5f / 2);
        offset -= roadComponent.roadLook.oneWay ?
            4.5f * Mathf.Floor((roadComponent.roadLook.laneCount0 + 1) / 2) :
            0;

        if (actor.leftHandDriving) {
            actor.p1 = roadComponent.Nodes[1].position;
            actor.t1 = -1 * roadComponent.Nodes[1].direction;
            actor.p2 = roadComponent.Nodes[0].position;
            actor.t2 = -1 * roadComponent.Nodes[0].direction;
            actor.targetNode = roadComponent.Nodes[0];
        } else {
            actor.p1 = roadComponent.Nodes[0].position;
            actor.t1 = roadComponent.Nodes[0].direction;
            actor.p2 = roadComponent.Nodes[1].position;
            actor.t2 = roadComponent.Nodes[1].direction;
            actor.targetNode = roadComponent.Nodes[1];

        }
        actor.p1 += -1 * (TangentToNormalNoY(actor.t1).normalized * offset);
        actor.p2 += -1 * (TangentToNormalNoY(actor.t2).normalized * offset);

    }

    public void RemoveVehicle(GameObject vehicle) {
        DestroyObject(vehicle);
    }

    public static float MpsToKph(float meters) {
        return meters * 3.6f;
    }

    public static float KphToMps(float kilometers) {
        return kilometers / 3.6f;
    }

}
