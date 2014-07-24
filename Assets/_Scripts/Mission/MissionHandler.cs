using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MissionHandler : Singleton<MissionHandler> {	
    public RouteItem route;
    public TrafficHandler trafficHandler { get { return TrafficHandler.Instance; } }
    public ScoreManager scoreManager;
    public GameObject trafficLight1;
    public GameObject trafficLight2;
    public Bus bus;
    
	BusStopScript currentStop;
    public int currentStopIndex;
    [Obsolete("This is no longer correnct, use PassengerSystem.Instance.Passengers")]
    public int passengersSitting { get { return PassengerSystem.Instance.Passengers; } }
    public int passengersWaitingAtStop;
    public int passengersWaitingToLeave;
    public int passengersTransported;
	
    float missionTime;
	
	int soundCounter = 0;
	
	public Material activeBusSignMaterial;
	public Material inactiveBusSignMaterial;
   
    public float loadingDeltas;
    [Obsolete("Please use PassengerSystem.Instance.Passengers")]
    public int Passengers {
        get { return PassengerSystem.Instance.Passengers; }
	}
		
	public bool LastStop { 
		get { return currentStopIndex == route.stopTimesCount - 1; }	
	}
	
	public void SelectMission(RouteItem mission) {
		route = mission;	
	}
	
	public void StartMission() {
		Events.Instance.OnMissionLoadingStarted(this, new MissionEventArgs(route));	
	}
    
	const float PASSENGER_BASE_SPEED = 2f;
	
    void Start() {	
		Events.Instance.StoppedAtBusStop += OnStoppedAtBusStop;
		Events.Instance.BusStopCompleted += (sender, e) => StopCompleted(e.busStop);
		Events.Instance.MissionLoadingStarted += OnMissionLoadingStarted;
		Events.Instance.SwitchedStateContext += (object src, StateContextEventArgs e) => { 
			if (e.stateContext == StateContext.Menu) Reset();
		};
		route = MissionRepository.Instance.GetRoute(0,0);
    }
	
	void Reset() {
        currentStopIndex = 0;
        loadingDeltas = 0f;
        PassengerSystem.Instance.Reset();
		passengersWaitingAtStop = 0;
		missionTime = 0;
		passengersWaitingToLeave = 0;
		passengersTransported = 0;
		
		if (bus != null) {
			Destroy(bus.gameObject);	
		}
	}
	
	public void OnMissionLoadingStarted(object sender, MissionEventArgs e) {
		route = e.mission;
		InstantiateBusAtMissionStart();
	}
	
	public void OnStoppedAtBusStop(object sender, BusStopEventArgs e) {
		currentStop = e.busStop;	
		soundCounter = 0;
	}

    void Update() {
		if (StateMachine.Instance.IsInGameNotPaused()) {
			if (!StateMachine.Instance.IsInState(GameState.ScoreTally)) { missionTime += Time.deltaTime; }
			
			//if doors are open, we are implicitly at stop;
	        if (bus.DoorsOpen) {
	            UpdatePassengers(); 
	        }
			
			HUD.Instance.UpdateTimerDisplay(missionTime);
			HUD.Instance.UpdateDistanceDisplay(GetDistanceToCurrentStop());
		}
    }
	
	void InstantiateBusAtMissionStart() {
		int nodeIndex;
		ResourceServer.Instance.stopUIDsToNodeIndices.TryGetValue(route.orderedNodeIndexList[0], out nodeIndex);
		
		var stopNode = ResourceServer.Instance.nodes[nodeIndex];
		var busGO = Instantiate(route.vehiclePrefab, stopNode.position, Quaternion.LookRotation(stopNode.direction)) as GameObject;
		busGO.transform.rotation = Quaternion.LookRotation(busGO.transform.right);
		bus = busGO.GetComponent<Bus>();
		Events.Instance.OnBusInstantiated(this, new BusEventArgs(bus));
	}

	//This function is called after the doors are closed at a stop. 
    public void StopCompleted(BusStopScript stop) {		
        if (currentStopIndex != 0) {
			ScoreManager.Instance.NextStop();
        }
        
        if (passengersWaitingAtStop > 0) {
            //penalize for passengers who couldn't get on. 
            //works.
            ScoreManager.Instance.PassengersLeftAtStop(passengersWaitingAtStop);
            // Handle penalties;
        }
        if (passengersWaitingToLeave > 0) {
            //penalizeForpassengers who didn't get off the bus. 
            ScoreManager.Instance.PassengerLeft(passengersWaitingToLeave);
            // HandlePenalties;
        }
		
        currentStopIndex++;
		HUD.Instance.UpdateBusStopsLeftDisplay(route.stopTimesCount - currentStopIndex, route.stopTimesCount);
		HUD.Instance.busStopPassengerStatusDisplay.Hide();
		missionTime -= (route.drivingTimesList[currentStopIndex - 1] + route.stopTimesList[currentStopIndex]);
        if (currentStopIndex < route.stopTimesCount) {
            // this basically sais if it's my turn activaet.
	        loadingDeltas = 0f;
	        passengersWaitingAtStop = route.enteringPassengersList[currentStopIndex];
	        passengersWaitingToLeave = route.leavingPassengersList[currentStopIndex];	
		}
		else {
			EndMission();
		}
    }
	
	void EndMission() {
		Events.Instance.OnMissionEnded(this, EventArgs.Empty);
	}
	
    void UpdatePassengers() {		
        loadingDeltas += Time.deltaTime;
        if (passengersWaitingToLeave > 0 && Passengers > 0) {
            if (loadingDeltas > 1f / PASSENGER_BASE_SPEED) {
                PassengerLeaves();
                loadingDeltas = 0f;
            }
        } else if (passengersWaitingAtStop > 0) {
            if (loadingDeltas > 1f / PASSENGER_BASE_SPEED) {
                PassengerEnters();
                loadingDeltas = 0f;
            }
        }

		HUD.Instance.busStopPassengerStatusDisplay.UpdateDisplay(
			passengersWaitingAtStop,
			LastStop ? PassengerSystem.Instance.Passengers : passengersWaitingToLeave //See last stop assumption, below
		);

		if (LastStop) {
			// Assumption: all passengers get off at last stop
			// Reason had to be made: passengers waiting to leave number is all messed.
			if (PassengerSystem.Instance.Passengers == 0 && passengersWaitingAtStop == 0) {
				EndMission();	
			}
		}
    }

    void PassengerLeaves() {
		PlaySoundOnEverySecondPassenger();
        passengersWaitingToLeave--;
        passengersTransported++;
        PassengerSystem.Instance.PassengerLeaves();
    }
	
	void PlaySoundOnEverySecondPassenger() {
		soundCounter++;
		if (soundCounter == 2) {
			SoundMaster.Instance.PlayBusStopSound();
			soundCounter = 0;
		}
	}

    void PassengerEnters() {
		PlaySoundOnEverySecondPassenger();
        passengersWaitingAtStop--;
        PassengerSystem.Instance.PassengerEnters(UnityEngine.Random.Range(0.8f, 1f));
		
		float ratio = (float)route.enteringPassengersList[currentStopIndex] / (float)currentStop.passengersChild.transform.childCount;
		if (passengersWaitingAtStop % Mathf.Floor(ratio) == 0) {
			// Special case when the total number of actual passengers isn't divided evenly by the number of
			// passenger models, we have to skip the first exeunt
			if (!currentStop.skippedFirstExeunt && currentStop.groupToExeunt == 0 && ratio % 1 != 0){ 
				currentStop.skippedFirstExeunt = true;
			}
			else {
				currentStop.ExeuntPassengerGroup();	
			}
		}
    } 
	
	public float GetDistanceToCurrentStop() {
		int nodeIndex;
		ResourceServer.Instance.stopUIDsToNodeIndices.TryGetValue(route.orderedNodeIndexList[currentStopIndex], out nodeIndex);
		var stopPos = ResourceServer.Instance.nodes[nodeIndex].position;
		stopPos.ReplaceY(0);
		var busPos = bus.transform.position;
		busPos.ReplaceY(0);
		
		const float KILOMETER_CONVERSION = 0.001f;
		return Vector3.Distance(stopPos, busPos) * KILOMETER_CONVERSION;
 	}
}
