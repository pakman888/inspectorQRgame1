using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficVehicle : MonoBehaviour {
    const float LANE_CHANGE_SPACE_MIN = 5f;
    const float LANE_CHANGE_SPACE_MAX = 10f;
    const float CRASHED_DESTROY_DISTANCE = 100f;
    const int VEHICLE_LAYER = 9;
	const float OBSTACLE_CHECK_INTERVAL = 0.2f;

    public GameObject currentSegment;
    public GameObject model;

    float TimeSinceLastLaneChange;
    float TimeSinceBlocked;
	float TimeSinceLastObstacleCheck;

    public NodeItem targetNode;

    public bool trafficMerging;
    public bool crashed;
    public bool isBlocked;
    public bool atRed;
	public bool isBig;

    public List<GameObject> leftWheels;
    public List<GameObject> rightWheels;


    public float MinimumSafeDistance {
        get {
            return 5f;
        }
    }

    public float prevPercent;
    public float length;
    public float baseSpeed;
    public float bonusSpeed;
    public float targetSpeed;

    public bool isFirst;
    public TrafficVehicle forwardsCar;
    public TrafficVehicle backwordsCar;

    // for defining the curve that the cars will drive along. 
    public Vector3 p1, p2, t1, t2;
    // Distance along custom curve. 
    public float prevDist;
    public float dist;
    public int currentLane;
    public bool leftHandDriving;
    public float speed = TrafficHandler.CITY_SPEED_LIMIT_MPS;
    
    public float CurveLength {
        get { return (p1 - p2).magnitude; }
    }

	// Use this for initialization
	void Start () {
        transform.localScale *= 1.2f;
        rigidbody.mass = 800;
        gameObject.SetLayerRecursive(9);
        length = collider.bounds.extents.z;
        crashed = false;
        isBlocked = false;
        atRed = false;

        baseSpeed = TrafficHandler.CITY_SPEED_LIMIT_MPS;
        //bonusSpeed = UnityEngine.Random.Range(0f, 5f);
        bonusSpeed = 0;	
		UpdateVehiclePosition(true);
		
		if (isBig) {
			audio.clip = SoundMaster.Instance.GetRandomBigCarClip();
			audio.Play();
		}
		else {
			audio.clip = SoundMaster.Instance.GetRandomSmallCarClip();
			audio.Play();
		}
	}

    void RemoveCrashedVehicle() {
        var distance =
            (MissionHandler.Instance.bus.transform.position - transform.position).magnitude;
        if (distance > CRASHED_DESTROY_DISTANCE) {
            GameObject.Destroy(this);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        if ( crashed ) {
			return;
        }		       
        if (!crashed && !atRed && !isBlocked && !trafficMerging) {
            TimeSinceBlocked += Time.fixedDeltaTime;
            if (TimeSinceBlocked < 1) {

            } else {
                speed = TimeSinceBlocked < 2 ? baseSpeed * (TimeSinceBlocked - 1): baseSpeed;
                Drive();
            }
        }
	}
	
	void Update(){
		if(!crashed){
			if(TimeSinceLastObstacleCheck > OBSTACLE_CHECK_INTERVAL){
				LookForObstacles();
				TimeSinceLastObstacleCheck = 0;
			}        
			else{
				TimeSinceLastObstacleCheck += Time.deltaTime;
			}
			CheckForBusSignal();
	        // spin at reds.  
	        if (atRed) {
	            UpdateTrafficLightInfo();
	        }
	        if (atRed || isBlocked) {
	            TimeSinceBlocked = 0;
	        }
		}
        TurnWheels();
	}

    void TurnWheels() {
        if (crashed || isBlocked) return;
        var distTraveled = dist - prevDist;
        var circumferenc = 2f * Mathf.PI * leftWheels[0].GetComponent<SphereCollider>().radius * leftWheels[0].transform.localScale.y;
        foreach (var wheel in leftWheels) {
            wheel.transform.Rotate(distTraveled + circumferenc, 0f, 0f); 

        }
        foreach (var wheel in rightWheels) {
            wheel.transform.Rotate(distTraveled - circumferenc, 0f, 0f); 

        }
         
    }

    private bool TryGetCurrentSegment() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward * 2f + Vector3.up, Vector3.down, out hit, 5f)) {
            currentSegment = hit.collider.gameObject.transform.parent.gameObject;
            return true;
        } else {
            currentSegment = null;
            return false;
        }
    }

    public void Drive() {
        float distanceTraveled = speed * Time.fixedDeltaTime;
        float percentOfCurve = (float) distanceTraveled / CurveLength;
        prevPercent = percentOfCurve;
        prevDist = dist;
        dist += percentOfCurve;
        if (dist >= 1) {
            SwitchSegment();
        }
        UpdateVehiclePosition();
    }


    void ChangeSpeed() {
        // get the space between the vehicles.

        if (!isFirst) {
            var space = (transform.position - forwardsCar.transform.position).magnitude
                - (length / 2)
                - (forwardsCar.length / 2);
            targetSpeed = space < MinimumSafeDistance ?
                forwardsCar.speed :
                baseSpeed;//+ bonusSpeed;
        } else {
            targetSpeed = baseSpeed + bonusSpeed;
        }
        
        if ( isBlocked || atRed) {
            targetSpeed = 0;
            prevDist = 0f;
        }

        speed = targetSpeed;

    }

    void CheckForBusSignal() {
        trafficMerging = false;
        var bus = MissionHandler.Instance.bus;
        if (bus.CurrentSignal == TurnSignal.off) {
            return;
        }
        var busInfo = bus.GetComponent<TrafficRulesDetection>();
        if (busInfo.inIntersection) {
            return;
        }
        var busToCar = transform.position - bus.transform.position;
        //Make sure the vehicle is behind the bus.
        if (Vector3.Dot(bus.transform.forward, transform.forward) > 0.5
            && Vector3.Dot(busToCar.normalized, bus.transform.forward) < 0 ) {

            var distToBus = (transform.position - bus.transform.position).magnitude;
            if (TrafficHandler.InRange(distToBus, LANE_CHANGE_SPACE_MIN, LANE_CHANGE_SPACE_MAX)) {
                if (bus.CurrentSignal == TurnSignal.left) {
                    if (currentLane <= busInfo.currentLane || busInfo.currentLane == 0) {
                        trafficMerging = true;
                        return;
                    }
                }
                if (bus.CurrentSignal == TurnSignal.right) {
                    if (currentLane >= busInfo.currentLane) {
                        trafficMerging = true;
                        return;
                    }
                }
            }
        } 
        trafficMerging = false;

    }

	//Immediate flag: Do not wait until end of physics update step
    void UpdateVehiclePosition(bool immediate = false) {
        float lengthCoefficient = CurveLength / 3;
        var localCurveInfo = Curves.SmoothCurve(
            p1, t1 * lengthCoefficient, 
            p2, t2 * lengthCoefficient, 
            dist, true);
		if(immediate){
			gameObject.transform.position = localCurveInfo[0] + ( Vector3.up * 0.25f );
	        gameObject.transform.rotation = Quaternion.LookRotation(localCurveInfo[1]);	
		}
		else{
        	rigidbody.position = localCurveInfo[0] + ( Vector3.up * 0.25f );
        	rigidbody.rotation = Quaternion.LookRotation(localCurveInfo[1]);
		}
    }
    
    void SwitchSegment() {
        var roadComponent = currentSegment.GetComponent<RoadComponent>();
        var intersectionComponent = currentSegment.GetComponent<IntersectionComponent>();
        if (roadComponent != null) {
            roadComponent.LeaveRoadSegment(this);
        } else if (intersectionComponent) {
            // Cleanup intersection leaving stuff. 
        }

        TryGetCurrentSegment();
        if (currentSegment == null) {
            MissionHandler.Instance.trafficHandler.RemoveVehicle(gameObject);
            return;
        }
        // this should change slightly maybe have a previous segment. 
        gameObject.transform.parent = currentSegment.transform;
        // for readability sake maybe dup this code in the handle methods. 
        p1 = p2;
        t1 = t2;
        dist -= 1;

        var extraInfo = currentSegment.GetComponent<SegmentExtraInfo>();

        if (extraInfo.segmentType == 3) {
            HandleIntersection();
        } else if (extraInfo.segmentType == 0) {
            HandleRoad();
        } else {
            Debug.LogError("Car trying to drive on unknown type.");
            Debug.LogError(currentSegment.name);
            Destroy(gameObject);
        }
    }

    NodeItem GetArrivalNode(List<NodeItem> nodes) {
        var arrivalNodes = (from NodeItem node in nodes
                            where node.position == targetNode.position
                            select node);
        if (arrivalNodes.Count() != 1) {
            Debug.LogError("no arrival node");
        }
        return arrivalNodes.First();
    }

    List<NodeItem> GetListOfPossibleNodes(List<NodeItem> nodes, NodeItem arrivalNode) {
        List<NodeItem> returnlist = new List<NodeItem>();
        foreach (var node in nodes) {
            if (node.position == arrivalNode.position) {
                continue;
            }
            if (node.forwardIsRoad) {
                if (!node.forwardRoadItem.NoTraffic) {
                    returnlist.Add(node);
                }
            }
            else if (node.backwardIsRoad) {
                if (!node.backwardRoadItem.NoTraffic &&
                     !node.backwardRoadLook.oneWay) {
                    returnlist.Add(node);
                }
            }
        }
        return returnlist;
    }

    
    void HandleIntersection() {

        isFirst = true;

        var intersectionComponent = currentSegment.GetComponent<IntersectionComponent>();
        var nodes = intersectionComponent.nodes;

        bool oneway = true;
        foreach (var node in nodes) {
            if (node.backwardIsRoad) {
                if (!node.backwardRoadLook.oneWay) {
                    oneway = false;
                }
                if (node.forwardIsRoad) {
                    if (!node.forwardRoadLook.oneWay) {
                        oneway = false;
                    }
                }
            }
        }

        var arrivalNode = GetArrivalNode(nodes);
        var possible = GetListOfPossibleNodes(nodes, arrivalNode);

        NodeItem target;
        //-------------------------------------------------------
        // Hax for threeway intersection;
        // simon
        //-------------------------------------------------------
        if (intersectionComponent.intersectionInfo.prefabID == 6 
            || intersectionComponent.intersectionInfo.prefabID == 7) {
            target = Prefab6And7Hax(nodes, arrivalNode);
        }else if (oneway && nodes.Count == 3) {
            target = HandleOneWayPrefabs(nodes, arrivalNode, possible); 
        } else {
            target = GetTarget(possible, arrivalNode);
        }
        if (target.position == arrivalNode.position) {
            Debug.LogError("turning on self");
        }
        if (intersectionComponent.UsesTrafficLight) {
            UpdateTrafficLightInfo();
        }
        targetNode = target; 
        // handles new curve info 
        NewCurveInfo(target);
    }

    NodeItem Prefab6And7Hax(List<NodeItem> nodes, NodeItem arrivalNode) {
        //var info = currentSegment.GetComponent<SegmentExtraInfo>();
        int index = nodes.IndexOf(arrivalNode);
        if (index == 0 || index == 2) {
            return nodes[1];
        } else {
            return nodes[0];
        }
    }

    NodeItem HandleOneWayPrefabs(List<NodeItem> nodes, NodeItem arrivalNode, List<NodeItem> possible) {
        if (possible.Count == 1 ) {
            // could be  <-> <-
            //               ->
            // or 
            //            <- <-
            //               <-
            NodeItem otherNode = (from node in nodes 
                        where node.position != possible[0].position
                        where node.position != arrivalNode.position
                        select node).First();
            bool arrivalIsOneWay = arrivalNode.backwardIsRoad ? arrivalNode.backwardRoadLook.oneWay : arrivalNode.forwardRoadLook.oneWay;
            if ( arrivalIsOneWay ) {
                // this is case two
                var cross = Vector3.Cross(
                    (arrivalNode.position - possible[0].position).normalized, 
                    (otherNode.position - possible[0].position).normalized);
                if (cross.y > 0) {
                    currentLane += otherNode.backwardRoadLook.laneCount0;
                } 
            }
            return possible[0];
        }
        if (possible.Count == 2) {
            var cross = Vector3.Cross(arrivalNode.direction.normalized, (arrivalNode.position - possible[0].position).normalized);
            var lastLaneCount = arrivalNode.backwardRoadLook.laneCount0;
            NodeItem rightNode, leftNode;
            if (cross.y < 0) {
                rightNode = possible[0];
                leftNode = possible[1];
            } else {
                rightNode = possible[1];
                leftNode = possible[0];
            }
            if (currentLane > lastLaneCount - rightNode.forwardRoadLook.laneCount0) {
                currentLane -= lastLaneCount - rightNode.forwardRoadLook.laneCount0;  
                return rightNode;
            } else {
                return leftNode;
            }
        }
        return possible[0];
    }

    void UpdateCurrentLane() {
        var roadComponent = currentSegment.GetComponent<RoadComponent>();
        var distanceFromNode = (transform.position - p2).magnitude;
        currentLane = Mathf.FloorToInt(distanceFromNode / 4.5f);
        currentLane = Mathf.Clamp(currentLane, 1, roadComponent.roadLook.laneCount0);
    }


    void NewCurveInfo(NodeItem target) {

        float offset;
        if (target.forwardIsRoad) {
            leftHandDriving = false;
            t2 = target.direction;
            offset = GetOffset(target.forwardRoadLook);
        } else {
            leftHandDriving = true;
            t2 = -1 * target.direction;
            offset = GetOffset(target.backwardRoadLook);
        }

        p2 = (-1 * TrafficHandler.TangentToNormalNoY(t2).normalized * offset);
        p2 += target.position;

    }

    NodeItem GetTarget(List<NodeItem> possible, NodeItem nearest){
        NodeItem target = possible.ToArray()[UnityEngine.Random.Range(0, possible.Count)];
        // we might be in the out sideLane

        foreach (var node in possible) {
            if (Mathf.Abs(Vector3.Dot(node.direction, nearest.direction)) >= 0.9) {
                target = node;
            }
        }
        return target;
    }

    void HandleRoad() {
        var roadInfo = currentSegment.GetComponent<RoadComponent>();
        roadInfo.EnterRoadSegment(this);   

        if (leftHandDriving) {
            t2 = -1 * roadInfo.Nodes[0].direction;
            p2 = roadInfo.Nodes[0].position ;
            targetNode = roadInfo.Nodes[0];
        } else {
            t2 = roadInfo.Nodes[1].direction;
            p2 = roadInfo.Nodes[1].position;
            targetNode = roadInfo.Nodes[1];
        }

        var offset = GetOffset(roadInfo.roadLook);

        p2 += -1 * (TrafficHandler.TangentToNormalNoY(t2).normalized * offset);

    }

    public float GetOffset(RoadLook roadLook) {
        var offset = ( 4.5f * currentLane ) + roadLook.offset - ( 4.5f / 2 );
        if (roadLook.oneWay) {
            offset -= 4.5f * (Mathf.FloorToInt(roadLook.laneCount0 + 1) / 2);
        }
        return offset;
    }
    
    public bool Merge() {
        var roadComponent = currentSegment.GetComponent<RoadComponent>();
        if (isFirst) {
            if (backwordsCar != null) {
                backwordsCar.isFirst = true;
            }
        } else {
            if (backwordsCar != null) {
                backwordsCar.forwardsCar = forwardsCar;
                forwardsCar.backwordsCar = backwordsCar;
            } else {
                forwardsCar.backwordsCar = null;
            }
        }

        var traffic = leftHandDriving ?
            roadComponent.LeftVehicles:
            roadComponent.RightVehicles;

        var list = from car in traffic 
                   where car.currentLane == currentLane
                   select car;
        if (list.Count() == 0) {
            forwardsCar = null;
            backwordsCar = null;
            isFirst = true;
            return true;
        }
        foreach (var car in list) {
            if (car.isFirst) {
                if ( dist > car.dist ) {
                    backwordsCar = car;
                    car.forwardsCar = this;
                    car.isFirst = false;
                    isFirst = true;
                    return true;
                }
            }
            if (car.backwordsCar == null ) {
                if (dist < car.dist) {
                    forwardsCar = car;
                    car.backwordsCar = this;
                    isFirst = false;
                    return true;
                }
            } else {
                if ( dist < car.dist && dist > car.backwordsCar.dist ) {
                    var backCar = car.backwordsCar;
                    backwordsCar = backCar;
                    backCar.forwardsCar = this;

                     forwardsCar = car;
                    car.backwordsCar = this;
                    return true;
                }
            }
        }
        return false;
    }

    void LookForObstacles() {
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position + transform.up, gameObject.transform.forward, out rayHit, 6f)) {
            if (rayHit.collider != null) {
                isBlocked = true;
            }
        } else if ( isBlocked ){
            isBlocked = false;
        }
    }

    void UpdateTrafficLightInfo() {
        var trafficLightController = currentSegment.GetComponent<TrafficLightController>();
        if (trafficLightController != null) {
            var state = trafficLightController.RedLightPenalty(gameObject);
            if (state == TrafficLightController.LIGHT_STATES.GREEN) {
                atRed = false;
            } else if (state == TrafficLightController.LIGHT_STATES.RED) {
                atRed = true;
            }
        } else {
            Debug.LogError("No Traffic Light controller on " + currentSegment.name);
        }

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer == VEHICLE_LAYER) {
            crashed = true;
            rigidbody.useGravity = true;
        }
    }

    void OnDestroy() {
        if (forwardsCar != null) {
            if (backwordsCar != null) {
                forwardsCar.backwordsCar = backwordsCar;
                backwordsCar.forwardsCar = forwardsCar;
            } else {
                forwardsCar.backwordsCar = null;
            }
        } else if (backwordsCar != null) {
            backwordsCar.forwardsCar = null;
        }
        TrafficHandler.Instance.trafficCount--;
    }
    
    public void OnDrawGizmos() {
        if ( isFirst) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, p2);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + transform.up * 3, 2);
        }
        if (!isFirst) {
            if (forwardsCar == null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + transform.up * 3, 2);

            } else {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, forwardsCar.transform.position);

            }
        }
    }
}

public static class SuperMagic {
    public static T MinBy<T, C>(this IEnumerable<T> items, Func<T, C> selector) {
        var comparer = Comparer<C>.Default;
        C best = default(C);
        T bestItem = default(T);

        bool first = true;
        foreach (T item in items) {
            if (first) {
                bestItem = item;
                best = selector(item);
                first = false;
            } else {
                C current = selector(item);
                if (comparer.Compare(best, current) > 0) {
                    bestItem = item;
                    best = current;
                }
            }
        }
        if (first) {
            throw (new ArgumentException("no Items") );
        }
        return bestItem;
    }

    public static Bounds EncapsulateChildren(this IEnumerable<Bounds> bounds) {
        return bounds.Aggregate((box1, box2) => {
            box1.Encapsulate(box2);
            return box1;
        });
    }

}
