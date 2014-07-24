using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RoadComponent : MonoBehaviour {

    const float SPAWN_COOLDOWN = 10f;
   
    public RoadItem roadItem;
    public RoadLook roadLook;
    public NodeItem[] Nodes;
    public RoadLook prevRoadLook;

    public List<TrafficVehicle> vehicles = new List<TrafficVehicle>();
    public List<TrafficVehicle> LeftVehicles {
        get {
            return (from actor in vehicles
                   where actor.leftHandDriving
                   orderby actor.dist descending 
                   select actor).ToList();
        }
    }
    public List<TrafficVehicle> RightVehicles {
        get {
            return (from actor in vehicles
                    where actor.leftHandDriving == false
                    orderby actor.dist descending
                    select actor).ToList();
        }
    }

    public int leftDriving = 0;
    public int rightDriving = 0;
    public int CurrentTrafficCount {get {return leftDriving + rightDriving;}}
    public float psuedoLength;
    public float spawnCooldown;

	private SegmentExtraInfo extraInfo;

    void Awake() {
        spawnCooldown = -1f;
		extraInfo = GetComponent<SegmentExtraInfo>();
        GeneratePsuedoLength();
    }

    void GeneratePsuedoLength() {
        psuedoLength = roadItem.length * roadLook.laneCount0;
    }

    void Update() { 
		if (StateMachine.Instance.IsInGameNotPaused()) {
	        // complicated spawning tactics.
	        spawnCooldown -= Time.deltaTime;
	        if (((CurrentTrafficCount / psuedoLength) < 0.03) &&
                !roadItem.NoTraffic) {
	            if (TrafficHandler.Instance.SpawnTraffic(this, extraInfo)) {
	                spawnCooldown = SPAWN_COOLDOWN;
	            }
	        }
		}
    }

    public void LeaveRoadSegment(TrafficVehicle actor ) {
        vehicles.Remove(actor);
        if (actor.leftHandDriving) {
            leftDriving--;
        } else {
            rightDriving--;
        }
        if (actor.backwordsCar != null) {
            if (actor.isFirst ) { 
                actor.backwordsCar.isFirst = true;
                actor.backwordsCar.forwardsCar = null;
            }
            actor.backwordsCar = null;
        }
    }

    public void EnterRoadSegment(TrafficVehicle actor) {
        actor.isFirst = false;
        if (actor.leftHandDriving) {
            if (leftDriving == 0) {
                actor.isFirst = true;
            } else {
                actor.forwardsCar = LeftVehicles.Last();
            }
            leftDriving++;
        } else {
            if (rightDriving == 0) {
                actor.isFirst = true;
            } else {
                actor.forwardsCar = RightVehicles.Last();
            }
            rightDriving++;
        }
        if (!actor.isFirst) {
            actor.forwardsCar.backwordsCar = actor;
        }
        vehicles.Add(actor);
    }

    void OnDrawGizmosSelected() {
		if(!MissionHandler.Instance){
			return;
		}
        Gizmos.color = Color.blue;
        float spawnDist = MissionHandler.Instance.trafficHandler.GetSpawnPosition(this, false);
        var lengthCoefficiont = roadItem.length / 3;
        var spawnInfo = Curves.SmoothCurve(
            Nodes[0].position, Nodes[0].direction * lengthCoefficiont,
            Nodes[1].position, Nodes[1].direction * lengthCoefficiont,
            spawnDist, true);
        var offset = (4.5f * 1 ) + roadLook.offset - (4.5f / 2);
        var offsetVector = -1 * TrafficHandler.TangentToNormal(spawnInfo[1]) * offset;
        Gizmos.DrawCube(spawnInfo[0] + offsetVector, new Vector3( 2, 2, 2 ));

        spawnDist = MissionHandler.Instance.trafficHandler.GetSpawnPosition(this, true);
        lengthCoefficiont = roadItem.length / 3;
        spawnInfo = Curves.SmoothCurve(
            Nodes[1].position, -1 * Nodes[1].direction * lengthCoefficiont,
            Nodes[0].position, -1 * Nodes[0].direction * lengthCoefficiont,
            spawnDist, true);
        offset = -1 * (4.5f * 1 ) + roadLook.offset - (4.5f / 2);
        offsetVector =  TrafficHandler.TangentToNormalNoY(spawnInfo[1]) * offset;
        Gizmos.DrawCube(spawnInfo[0] + offsetVector, new Vector3( 2, 2, 2 ));
    }

}
