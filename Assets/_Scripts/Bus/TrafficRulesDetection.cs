using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficRulesDetection : MonoBehaviour {
    const float COLLISION_COOLDOWN_TIME = 1f;
    float collisionTimer = COLLISION_COOLDOWN_TIME;

	private enum LaneAlignment{
		With, //Vehicle aligned traffic
		Against, //Vehicle aligned against traffic
		Across //Vehicle aligned across traffic (perpendicular)
	};
	
	private enum TrafficEntry{
		None,
		Left,
		Right
	};

	Bus bus;
    CarController busController;
	
    float lastBrakeReport = 0.0f;
	public bool wrongWayDriving;
    public bool supressWrongWayDriving;
	public bool onRoad;
	public bool inIntersection;
	public bool runningRedLight;
	public GameObject currentSegment;
	public float currentLane;
	public ScoreManager scoreManager;
	public bool leftOfCurve; //Whether the vehicle is on the left of the curve node1 -> node2. Traffic drives from node1 to node2 on the right of the curve and vis versa on the left.

	public const float WRONG_WAY_DRIVING_COOLDOWN_PENALTY_TIME = 10f;
	public const float LANE_CHANGE_PENALTY_COOLDOWN_TIME = 10f; 
	public float wrongWayDrivingCooldown;
	public float lanePenaltyCooldown;

	private LaneAlignment laneAlignment;
	private TrafficEntry currentTrafficEntry;
	private bool trafficEntryStartedLeftOfCurve;

	void Awake() {
		bus = GetComponent<Bus>();
        busController = GetComponent<CarController>();
	}
	
	// Use this for initialization
	void Start () {
		GetScoreManager();
		wrongWayDrivingCooldown = 0f;
		lanePenaltyCooldown = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        collisionTimer -= Time.deltaTime;
		TryGetCurrentSegment();
		UpdateRoadPenalties();
        DetectOverBraking();
		UpdateIntersectionPenalties();
	}

    void DetectOverBraking() {
        var brake = busController.Brake;
        if (brake > 0.9 && MissionHandler.Instance.Passengers >= 1 && rigidbody.velocity.magnitude > 5.0f) {
            lastBrakeReport += Time.deltaTime;
            if (lastBrakeReport > 0.1f) {
                lastBrakeReport = -5.0f;
                scoreManager.IntensiveBreaking();
                //play sound
                //camera effect.
                Events.Instance.OnOverBraking(this, new BusEventArgs(bus));

            }
            if (lastBrakeReport < 0.0f) {
                lastBrakeReport += Time.deltaTime;
                if (lastBrakeReport > 0.0f) {
                    lastBrakeReport = 0.0f;
                }
            }
        } 
    }

	void GetScoreManager() {
		scoreManager = ScoreManager.Instance;
		if (scoreManager == null) {
			Debug.LogError("No Score Manager");
		}
	}
	
	private void UpdateIntersectionPenalties() {
		if (currentSegment == null) {
			if (inIntersection) {
				Debug.Log("Exited Intersection");
			}
			inIntersection = false;
			runningRedLight = false;
			return;
		}
		var segInfo = currentSegment.GetComponent<SegmentExtraInfo>();
		if ( segInfo.segmentType != 3 ) {
			if (inIntersection) {
				//HRM
			}
			inIntersection = false;
			runningRedLight = false;
			return;
		}
		if (!inIntersection) {
			inIntersection = true;
            currentLane = -1;
			var trafficControllerComponent = currentSegment.GetComponent<TrafficLightController>();
			if (trafficControllerComponent != null) {
				TrafficLightController.LIGHT_STATES state = trafficControllerComponent.RedLightPenalty(gameObject);
				if (state == TrafficLightController.LIGHT_STATES.GREEN) {
					scoreManager.GreenLightBonus();
				} else if (state == TrafficLightController.LIGHT_STATES.RED) {
					scoreManager.RedLightOffence();
					runningRedLight = true;
				}
				// else do nothing
			}
		}
	}

	private bool TryGetCurrentSegment() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 5f, Layers.GetLayerMaskFromLayer(Layers.Terrain))) {
			currentSegment = hit.collider.gameObject.transform.parent.gameObject;
			return true;
		} else {
			currentSegment = null;
			return false;
		}
	}

	void OnTriggerEnter(Collider trigger) {
	}
	void OnCollisionEnter(Collision collision) {
        if (collisionTimer > 0f) return;
        collisionTimer = COLLISION_COOLDOWN_TIME;
        var collisionSpeed = collision.relativeVelocity.magnitude;
		if(collision.gameObject.layer == Layers.Vehicles){
			scoreManager.VehicleCrash(collisionSpeed);
		}
		else{
			scoreManager.ObstacleCrash(collisionSpeed);
		}
		
		SoundMaster.Instance.PlayCrash(collisionSpeed);
	} 

	void OnCollisionStay(Collision collider) {

	}

	void OnCollisionExit(Collision collider) {

	}
	//handles wrong way driving and lane change violations.
	void UpdateRoadPenalties() {
		// if there is no road, there is no traffic penalty
		if (currentSegment == null) {
			onRoad = false;
			wrongWayDriving = false;
			return;
		}
		// again if there is no road there is no traffic penalty 
		var segmentInfo = currentSegment.GetComponent<SegmentExtraInfo>();
		if (!segmentInfo || segmentInfo.segmentType != 0) {
			onRoad = false;
			wrongWayDriving = false;
			return;
		}

		RoadComponent roadComponent = currentSegment.GetComponent<RoadComponent>();
        supressWrongWayDriving = roadComponent.roadLook.oneWay ? true : false;
		NodeItem firstNode = roadComponent.Nodes[0];
		NodeItem secondNode = roadComponent.Nodes[1];

		float lengthCoefficient = roadComponent.roadItem.length * (1 / 3f);

		Vector3 node1Pos = firstNode.position;
		Vector3 node2Pos = secondNode.position;

		Vector3 point1 = node1Pos;
		Vector3 point2 = node2Pos;

		Vector3 tangent1 = firstNode.direction * lengthCoefficient;
		Vector3 tangent2 = secondNode.direction * lengthCoefficient;

		float relativePoint1 = 0f;
		float relativePoint2 = 1f;

		Vector3 pos = Vector3.zero;
		Vector3 tan = Vector3.zero;
		float bestRelativePoint = 0f;

		// Bisect the curve to find an approximate point near the bus.
		Vector3 position = transform.position;
		int iterationCount = 15;		
		for (int i = 0; i < iterationCount; i++) {
			var distSquared1 = (position - point1).sqrMagnitude;
			var distSquared2 = (position - point2).sqrMagnitude;
			bestRelativePoint = (relativePoint1 + relativePoint2) * 0.5f;

			var pointOnLine = Curves.SmoothCurve(node1Pos, tangent1, node2Pos, tangent2, bestRelativePoint, true);

			pos = pointOnLine[0];
			tan = pointOnLine[1];

			if (distSquared1 < distSquared2) {
				point2 = pos;
				relativePoint2 = bestRelativePoint;
			} else {
				point1 = pos;
				relativePoint1 = bestRelativePoint;
			}
		}
		
		tan = tan.normalized;
		//Vector perpendicular to the curve
		Vector3 normal = new Vector3(tan.z, tan.y, -tan.x);
		
		leftOfCurve = Vector3.Dot(normal, (transform.position - pos)) < 0;
		
		laneAlignment = GetLaneAlignment(laneAlignment, tan);

		//get the lane the actor is in.
		float newLane = (transform.position - pos).magnitude;

		RoadLook road = roadComponent.roadLook;
		var middleLaneSize = road.offset;

		//roads have var middle lane length. 
		RoadLook prevRoad = roadComponent.prevRoadLook ;
		if (prevRoad.id != RoadLook.NoLookId) {
			var roadDelta = prevRoad.offset - road.offset;
			var relPositionInv = 1f - bestRelativePoint;

			middleLaneSize += roadDelta * relPositionInv;
		}

		// we are driving in the middle lane. 
		if (Mathf.Abs(newLane) < middleLaneSize) {
			onRoad = false;
			newLane = 0f;
            if (currentLane != -1) {
                DetectLaneChange(newLane, currentLane);
            }
			currentLane = newLane;
			return;
		} 
		
		// Debug lane display
		//DebugDrawLanes(pos, tan, normal, middleLaneSize, road.laneCount1);

		newLane -= middleLaneSize;

		newLane /= 4.5f; // LANE WIDTH Constant

		onRoad = (newLane < (road.laneCount1 + 1 ));
		if(!onRoad){
			newLane = road.laneCount1 + 1;
		}

		if ( laneAlignment == LaneAlignment.Against) {
			HandleWrongWayDriving();
		} else {
			wrongWayDriving = false;
		}
		HandleTrafficEntry(road);
		if(currentTrafficEntry == TrafficEntry.None){
            if (currentLane != -1) {
                DetectLaneChange(newLane, currentLane);
            }
		}
		currentLane = newLane;

	}

	void HandleWrongWayDriving() {
		if (!supressWrongWayDriving && onRoad) {
			if (wrongWayDriving) {
				wrongWayDrivingCooldown += Time.deltaTime;
				if (wrongWayDrivingCooldown > WRONG_WAY_DRIVING_COOLDOWN_PENALTY_TIME) {
					wrongWayDrivingCooldown = 0f;
					scoreManager.WrongWayDriving();
				}
			} else {
				wrongWayDriving = true;
				wrongWayDrivingCooldown = 0f;
				scoreManager.WrongWayDriving();
			}
		}
	}

	void DetectLaneChange(float current, float last) {		
		if ( Mathf.Floor(current) > Mathf.Floor(last) ){
			// right lane change check blinkers. 
			if (onRoad && !wrongWayDriving) {
				if (bus.CurrentSignal == TurnSignal.right) {
					scoreManager.GreatBlinkerRoad();
				}
				else {
					scoreManager.BadBlinker();	
				}
			}
		}
		if ( Mathf.Floor(current) < Mathf.Floor(last)) {
			// left lane change check blinkers. 
			if (onRoad && !wrongWayDriving) {
				if (bus.CurrentSignal == 
					(laneAlignment == LaneAlignment.Across ? TurnSignal.right : TurnSignal.left)
					) {
					scoreManager.GreatBlinkerRoad();
				}
				else {
					scoreManager.BadBlinker();	
				}
			}
		}
	}
	
	void HandleTrafficEntry(RoadLook road){
		//Begin traffic entry
		if(!onRoad && currentTrafficEntry == TrafficEntry.None && laneAlignment == LaneAlignment.Across){
			if(bus.CurrentSignal == TurnSignal.left){
				currentTrafficEntry = TrafficEntry.Left;
				trafficEntryStartedLeftOfCurve = leftOfCurve;
			}
			if(bus.CurrentSignal == TurnSignal.right){
				currentTrafficEntry = TrafficEntry.Right;
				trafficEntryStartedLeftOfCurve = leftOfCurve;
			}
		}
		if(currentTrafficEntry == TrafficEntry.Left){
			//Cancel maneuver
			if(bus.CurrentSignal != TurnSignal.left){
				currentTrafficEntry = TrafficEntry.None;
			}
			//Check maneuver success
			else if(onRoad && leftOfCurve != trafficEntryStartedLeftOfCurve && laneAlignment == LaneAlignment.With){
				scoreManager.GreatBlinkerRoad();
				currentTrafficEntry = TrafficEntry.None;
			}
			//Check maneuver failure
			else if(onRoad && laneAlignment == LaneAlignment.Against || (laneAlignment == LaneAlignment.With && leftOfCurve == trafficEntryStartedLeftOfCurve)){
				scoreManager.BadBlinker();
				currentTrafficEntry = TrafficEntry.None;
			}
		}
		if(currentTrafficEntry == TrafficEntry.Right){
			//Cancel maneuver
			if(bus.CurrentSignal != TurnSignal.right){
				currentTrafficEntry = TrafficEntry.None;
			}
			//Check maneuver success
			else if(onRoad && laneAlignment == LaneAlignment.With){
				scoreManager.GreatBlinkerRoad();
				currentTrafficEntry = TrafficEntry.None;
			}
			//Check maneuver failure
			else if(onRoad && laneAlignment == LaneAlignment.Against || leftOfCurve != trafficEntryStartedLeftOfCurve){
				scoreManager.BadBlinker();
				currentTrafficEntry = TrafficEntry.None;
			}
		}
	}

	LaneAlignment GetLaneAlignment(LaneAlignment oldAlignment, Vector3 tan){
		Vector3 forward = transform.forward;
		float dotTan = Vector3.Dot(forward, tan);
		
		if(oldAlignment == LaneAlignment.Across){
			if(dotTan >= 0.8f){
				return leftOfCurve ? LaneAlignment.Against : LaneAlignment.With;
			}
			else if(dotTan <= -0.8f){
				return leftOfCurve ? LaneAlignment.With : LaneAlignment.Against;
			}
			else {
				return LaneAlignment.Across;
			}
		} else {
			if(dotTan < 0.7f && dotTan > -0.7f){
				return LaneAlignment.Across;
			}
			else if(dotTan >= 0.7f){
				return leftOfCurve ? LaneAlignment.Against : LaneAlignment.With;
			}
			else{
				return leftOfCurve ? LaneAlignment.With : LaneAlignment.Against;
			}
		}
	}  

	void DebugDrawLanes(Vector3 position, Vector3 tangent, Vector3 normal, float middleLaneSize, int laneCount){
		position = position + Vector3.up * 0.2f;
		Debug.DrawRay(position - tangent, tangent * 2, Color.green, 0, true);
		Debug.DrawRay(position + middleLaneSize * normal, normal * (laneCount + 1) * 4.5f, Color.red, 0, true);
		Debug.DrawRay(position - middleLaneSize * normal, -normal * (laneCount + 1) * 4.5f, Color.blue, 0, true);
		for(int i = 0; i <= laneCount + 1; i++){
			Debug.DrawRay(position + (middleLaneSize + 4.5f * i) * normal - tangent, tangent*2, Color.magenta, 0, true);
			Debug.DrawRay(position - (middleLaneSize + 4.5f * i) * normal - tangent, tangent*2, Color.cyan, 0, true);
		}
	}
}