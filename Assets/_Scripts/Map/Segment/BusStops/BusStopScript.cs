using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BusStopScript : MonoBehaviour {

    public NodeItem node;
    public BusStopItem busStopItem;
    public BusStopLook busStopLook;

    public GameObject busStopModel;
    public GameObject activePlatePrefab;
    public GameObject inactivePlatePrefab;

    public GameObject activePlateChild;
    public GameObject passengersChild;
	
    public int timeTableIndex;

    public bool onRoute;
    public bool HasPassengerModel {
        get {
            return MissionHandler.Instance.route.passengerModels[timeTableIndex] != null;
        }
    }

	public Vector3 PlateOffset{
		get{
			return new Vector3(0, 0.05f, 0);
		}
	}

    private const string NO_MODEL_ID = "0";
    
    MissionHandler mission;

    RouteItem route { get { return mission.route;} }

	private bool eventsRegistered;

    void Start() {
        GetMission();
        if (BusStopInMission()) {
            onRoute = true;
            InitializeMissionStop();
			//Chunk loaded while this is current stop (but this is not first stop i.e. starting location)
			if(mission.currentStopIndex == timeTableIndex && timeTableIndex != 0){
				ActivateBusStop();
			}
        } else {
            onRoute = false;
            InitializeNonMissionStop();
        }
		
		Events.Instance.BusStopCompleted += new BusStopEventHandler(OnBusStopCompleted);
		Events.Instance.StoppedAtBusStop += new BusStopEventHandler(OnStoppedAtStop);
		Events.Instance.MissionLoaded += new EventHandler(OnMissionLoaded);
		Events.Instance.MissionEnded += new EventHandler(OnMissionEnded);
		
		eventsRegistered = true;
    }
	
	void OnMissionEnded(object sender, EventArgs e) {
		DeregisterEvents();		
	}
	
	void OnDestroy(){
		if(eventsRegistered){
			DeregisterEvents();
		}
	}
	
	void DeregisterEvents(){
		Events.Instance.BusStopCompleted -= new BusStopEventHandler(OnBusStopCompleted);
		Events.Instance.StoppedAtBusStop -= new BusStopEventHandler(OnStoppedAtStop);
		Events.Instance.MissionLoaded -= new EventHandler(OnMissionLoaded);	
		Events.Instance.MissionEnded -= new EventHandler(OnMissionEnded);
		eventsRegistered = false;
	}
	
	void OnMissionLoaded(object sender, EventArgs e) { 
		if (timeTableIndex == 0){
			// The first stop always gets "completed" right away
			MissionHandler.Instance.bus.HandleFirstStop(this);
			DeactivateBusStop();
			DeactivateActivePlate();
		}
	}
	
	bool ThisStopFollows(BusStopScript other) {
		return other.timeTableIndex == timeTableIndex - 1;	
	}
	
	void OnStoppedAtStop(object sender, BusStopEventArgs e) {
		if (e.busStop == this) {
			DeactivateActivePlate();	
		}
	}
			
	void OnBusStopCompleted(object sender, BusStopEventArgs e) {
		if (e.busStop == this) {
			DeactivateBusStop();	
		}
		if (ThisStopFollows(e.busStop)) {
			ActivateBusStop();
		}
	}

    void GetMission() {
        mission = MissionHandler.Instance;
    }

    bool BusStopInMission() {
        for (int i = 0; i < route.orderedNodeIndexList.Count; i++ ) {
            if (route.orderedNodeIndexList[i] == busStopItem.stopUID) {
                timeTableIndex = i;
                return true;
            }
        }
        timeTableIndex = -1;
        return false;
    }

    void InitializeMissionStop() {
        InstantiateModel();
        InstantiateActivePlate();
        InstantiatePassengers();
        InstantiateInActivePlate();
        
		if (timeTableIndex != 0) {
            activePlateChild.SetActive(false);
        } 
    }

    void InstantiatePassengers() {
        //this information will be in the mission.
        // Note create a empty passenger prefab to instantiate when theer is no passenger, call it something like no passenger. 
        
        if (HasPassengerModel) {
	        passengersChild = (GameObject) Instantiate(mission.route.passengerModels[timeTableIndex], transform.position, Quaternion.LookRotation(node.direction));
            passengersChild.transform.Translate(-1f * Vector3.forward * busStopLook.modelOffset);
            passengersChild.transform.parent = transform;
            passengersChild.name = "passengers";
        }
    }

    GameObject Load(string path) {
        return new GameObject();
    }

    void InitializeNonMissionStop() {
        InstantiateModel();
        InstantiateInActivePlate();
    }

    void InstantiateActivePlate() {
        if (busStopLook.activePlate != NO_MODEL_ID) {
			activePlateChild = (GameObject)Instantiate(activePlatePrefab, transform.position + PlateOffset, Quaternion.LookRotation(node.direction));
			activePlateChild.transform.parent = transform;
			activePlateChild.transform.rotation = Quaternion.LookRotation(-1 * activePlateChild.transform.right);
			var activePlateScript = activePlateChild.GetComponent<ActiveBusStopScript>();
			if(activePlateScript){
				activePlateScript.busStop = this;
			}
			else{
				Debug.LogWarning("No ActiveBusStopScript on " + activePlateChild.name, activePlateChild);
			}

            if (timeTableIndex == 0) {
                foreach (Transform child in activePlateChild.transform) {
                    child.gameObject.SetActive(false);
                }
            }
			activePlateChild.name = "ActivePlate";
        }
    }

    void InstantiateInActivePlate() {
        if (busStopLook.inactivePlate != NO_MODEL_ID) {
            if (inactivePlatePrefab == null) {
                Debug.LogWarning("InactivePlate has string but no game object" + busStopLook.inactivePlate);
                return;
            }
            var loadingArea = (GameObject)Instantiate(inactivePlatePrefab, transform.position + PlateOffset, Quaternion.LookRotation(node.direction));
            loadingArea.transform.rotation = Quaternion.LookRotation(-1 * loadingArea.transform.right);
            loadingArea.transform.parent = transform;
            loadingArea.name = "InactivePlate";
        }
    }

    void InstantiateModel() {
        if (busStopLook.busStopModel != NO_MODEL_ID) {
            var model = (GameObject)Instantiate(busStopModel, transform.position, Quaternion.LookRotation(node.direction));
            if (model == null) {
                Debug.LogError("busstopmodel has value but is null");
            }
            model.transform.parent = transform;
            model.transform.position -= (model.transform.forward * busStopLook.modelOffset);
			var signRenderer = (from renderer in model.transform.GetComponentsInChildren<Renderer>()
							where renderer.material != null && renderer.material.name.Contains("sign")
							select renderer).FirstOrDefault();
			if(signRenderer){
				if (onRoute) {
					signRenderer.material = MissionHandler.Instance.activeBusSignMaterial;
				}
				else {
					signRenderer.material = MissionHandler.Instance.inactiveBusSignMaterial;
				}
			}						 
            model.name = "BusStopModel";
        }
    }

    public int OnPassengers {
        get {
            if (onRoute) {
                return route.enteringPassengersList[timeTableIndex];
            } else return -1;
        }
    }

    public int OffPassengers {
        get {
            if (onRoute) {
                return route.leavingPassengersList[timeTableIndex];
            }
            return -1;
        }
    }

    public void ActivateBusStop() {
        activePlateChild.SetActive(true);
    }

    public void DeactivateBusStop() {
        if ( passengersChild != null ) {
            passengersChild.SetActive(false);
		}
    } 
	
	public void DeactivateActivePlate() {
		activePlateChild.SetActive(false);
		activePlateChild.collider.enabled = false;
	}
	
	public bool skippedFirstExeunt = false;
	public int groupToExeunt = 0;
	public void ExeuntPassengerGroup() {
		passengersChild.transform.GetChild(groupToExeunt).gameObject.SetActive(false);
		groupToExeunt++;
	}
}