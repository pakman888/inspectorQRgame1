using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : Singleton<ScoreManager> {
	public GUISkin skin;
	
	//In m/s. Bus currently tops out at 10 m/s (36 kph), probably want to revisit these numbers
	public const float CrashVelocityHigh = 15, 
						CrashVelocityMedium = 5,
						CrashVelocityLow = 1;

    public float totalPenalties;
    public float totalBonus;
    public int bonusCount;
    public int penaltyCount;
    public float Score { get { return totalBonus + totalPenalties; } }

    public float[] distanceWithout;
    
    const int WTH_offence = 0,
              WTH_unhappy = 1,
              WTH_crash = 2,
              WTH_all = 3;

    const float ONE_MILE_IN_METERS = 1609.34f;
	
    private float lastBadBlinkerCooldown;
	private float lastGreatBlinkerCooldown;
    private float lastWrongWay;
    const float BAD_BLINKER_PENALTY_TIMER = 5f;

    const int SCORE_PER_PASSENGER = 50;
    //per passenger who could not get off at stop. 
    const float PASSENGERS_LEAVE_PENALTY = -100.0f;
    //per passenger left at a stop;
    const float PASSENGERS_ENTER_PENALTY = -90.0f;
    //per passenger who is allowed to leave before the time table allows. 
    const float EARLY_EXIT_PENALTY = -80.0f;
    //wrong way driving
    const float DRIVING_OFFENCE_PENALTY = -40.0f;
    //overbreaking
    const float UNHAPPY_PASSENGER_PENALTY = -100.0f;
    //low speed non vehicle
    const float NON_VEHICLE_CRASH_PENALTY_LOW = -50.0f;
    //medium speed non vehicle
    const float NON_VEHICLE_CRASH_PENALTY_MEDIUM = -100.0f;
    //high speed non vehicle
    const float NON_VEHICLE_CRASH_PENALTY_HIGH = -200.0f;
    //small non vehicle objects pylons etc
    const float NON_VEHICLE_CRASH_PENALTY_SMALL_OBJECT = -10.0f;
    //bonus for completing a busstop
    const float NEXT_STOP_BONUS = 100.0f;
    //bonus for completing a checkpoint... I don't know if I have ever seen this.
    //each succesfully finished route == these bonuses. 
    const float NEXT_CHECKPOINT_BONUS = 100.0f;
    const float BAD_BLINKER_PENALTY = -10.0f;
    //blinker usage on crossings
    const float GREAT_BLINKER_BONUS = 20.0f;
    //blinker usage on lanechanges
    const float GREAT_BLINKER_ROAD_BONUS = 10.0f;
    //getting to far away from the chopper.
    const float FOLLOW_CHOPPER_PENALTY = -10.0f;
    //for opening the door on the open road.
    const float OPEN_DOOR_PENALTY = -5.0f;
    const float RED_LIGHT_OFFENCE_PENALTY = -200.0f;
    const float GREEN_LIGHT_BONUS = 30.0f;
    //crashes with other vehicles low - high speeds
    const float VEHICLE_CRASH_PENALTY_LOW = -50.0f;
    const float VEHICLE_CRASH_PENALTY_MEDIUM = -100.0f;
    const float VEHICLE_CRASH_PENALTY_HIGH = -200.0f;
    //us of language heer is bad without penalty should be used. 
    const float MILE_WITHOUT_CRASH_BONUS = 15.0f;
    const float MILE_WITHOUT_OFFENCE_BONUS = 40.0f;
    const float MILE_WITHOUT_UNHAPPY_BONUS = 90.0f;
    const float MILE_WITHOUT_ANY_BONUS = 170.0f;
    const float ROUTE_TIME_BONUS = 1000f;
    //incase you break the bus? can that even happen. 
    // also can you finish maps early?
    const float CRASH_PENALTY = -50000.0f;
    const float PERFECT_STOP_BONUS = 20.0f;

    /* The messages to be printed to screen */
    const string PASSENGERS_LEAVE_MESSAGE = "Unhappy Passengers Left Early";
    const string PASSENGERS_ENTER_MESSAGE = "Passengers Left Behind At Stop";
    const string EARLY_EXIT_MESSAGE = "Left Stop Early";
    const string DRIVING_OFFENCE_MESSAGE = "Driving Against Traffic";
    const string INTENSIVE_BRAKING_MESSAGE = "Extreme Braking";
    const string OBSTACLE_CRASH_MESSAGE_LOW = "Scratched the Paint";
    const string OBSTACLE_CRASH_MESSAGE_MEDIUM = "Fender Bender";
    const string OBSTACLE_CRASH_MESSAGE_HIGH = "Major Crash";
    const string OBSTACLE_CRASH_MESSAGE_SMALL_OBJECT = "@@ui_obstacle_crash_small_object@@";
    const string NEXT_STOP_MESSAGE_NAME = "Stop Complete";
    const string NEXT_STOP_MESSAGE = "Stop Complete";
    const string NEXT_CHECKPOINT_MESSAGE = "Checkpoint Reached";
    const string BAD_BLINKER_MESSAGE = "Incorrect Blinkers";
    const string GREAT_BLINKER_MESSAGE = "Great Blinkers";
    const string GREAT_BLINKER_ROAD_MESSAGE = "Great Blinkers";
    const string FOLLOW_CHOPPER_MESSAGE = "Follow the helicopter";
    const string OPEN_DOOR_MESSAGE = "Opened door not at stop";
    const string RED_LIGHT_OFFENCE_MESSAGE = "Ran Red Light";
    const string GREEN_LIGHT_BONUS_MESSAGE = "Green Light Bonus";
    const string VEHICLE_CRASH_MESSAGE_LOW = "Minor Collision";
    const string VEHICLE_CRASH_MESSAGE_MEDIUM = "Medium Collision";
    const string VEHICLE_CRASH_MESSAGE_HIGH = "Major Collision";
    const string MILE_WITHOUT_CRASH_MESSAGE = "Mile with no Crash";
    const string MILE_WITHOUT_OFFENCE_MESSAGE = "Mile with no Offence";
    const string MILE_WITHOUT_UNHAPPY_MESSAGE = "Mile with no Unhappy Passengers";
    const string MILE_WITHOUT_ANY_MESSAGE = "Perfect Mile";
    const string PERFECT_STOP_BONUS_MESSAGE = "Perfect Stop";

	//Keys for looking up UI messages... later.
    //const string PASSENGERS_LEAVE_MESSAGE = "@@ui_passengers_leave_penalty@@";
    //const string PASSENGERS_ENTER_MESSAGE = "@@ui_passengers_enter_penalty@@";
    //const string EARLY_EXIT_MESSAGE = "@@ui_follow_time_table@@";
    //const string DRIVING_OFFENCE_MESSAGE = "@@ui_driving_offence@@";
    //const string INTENSIVE_BRAKING_MESSAGE = "@@ui_intensive_braking@@";
    //const string OBSTACLE_CRASH_MESSAGE_LOW = "@@ui_obstacle_crash_low@@";
    //const string OBSTACLE_CRASH_MESSAGE_MEDIUM = "@@ui_obstacle_crash_medium@@";
    //const string OBSTACLE_CRASH_MESSAGE_HIGH = "@@ui_obstacle_crash_high@@";
    //const string OBSTACLE_CRASH_MESSAGE_SMALL_OBJECT = "@@ui_obstacle_crash_small_object@@";
    //const string NEXT_STOP_MESSAGE_NAME = "@@ui_great_next_stop_name@@";
    //const string NEXT_STOP_MESSAGE = "@@ui_great_next_stop@@";
    //const string NEXT_CHECKPOINT_MESSAGE = "@@ui_great_next_checkpoint@@";
    //const string BAD_BLINKER_MESSAGE = "@@ui_use_blinkers@@";
    //const string GREAT_BLINKER_MESSAGE = "@@ui_great_blinkers@@";
    //const string GREAT_BLINKER_ROAD_MESSAGE = "@@ui_great_blinkers_road@@";
    //const string FOLLOW_CHOPPER_MESSAGE = "@@ui_follow_chopper@@";
    //const string OPEN_DOOR_MESSAGE = "@@ui_open_door_penalty@@";
    //const string RED_LIGHT_OFFENCE_MESSAGE = "@@ui_red_light_offence@@";
    //const string GREEN_LIGHT_BONUS_MESSAGE = "@@ui_green_light_bonus@@";
    //const string VEHICLE_CRASH_MESSAGE_LOW = "@@ui_vehicle_crash_low@@";
    //const string VEHICLE_CRASH_MESSAGE_MEDIUM = "@@ui_vehicle_crash_medium@@";
    //const string VEHICLE_CRASH_MESSAGE_HIGH = "@@ui_vehicle_crash_high@@";
    //const string MILE_WITHOUT_CRASH_MESSAGE = "@@ui_mile_without_crash@@";
    //const string MILE_WITHOUT_OFFENCE_MESSAGE = "@@ui_mile_without_offence@@";
    //const string MILE_WITHOUT_UNHAPPY_MESSAGE = "@@ui_mile_without_unhappy@@";
    //const string MILE_WITHOUT_ANY_MESSAGE = "@@ui_mile_without_any@@";
    //const string PERFECT_STOP_BONUS_MESSAGE = "@@ui_perfect_stop_bonus@@";
    public Queue<ScoreEvent> scoreEventQueue;
	float time = 0f;
	
	void Start() {
		Events.Instance.BusInstantiated += new BusEventHandler(OnBusInstantiated);
        distanceWithout = new float[4];
		scoreEventQueue = new Queue<ScoreEvent>();
		ResetStats();
		        
		Events.Instance.MissionLoadingStarted += OnMissionSelected;
	}

    void OnBusInstantiated( object sender, BusEventArgs args) {

    }
	
	public int GetPassengersTransportedBonus() {
		return MissionHandler.Instance.passengersTransported * SCORE_PER_PASSENGER;	
	}
	
	void OnMissionSelected(object sender, MissionEventArgs e) {
        ResetStats();
	}
	
	void ResetStats(){
		scoreEventQueue.Clear();
        totalPenalties = 0;
        totalBonus = 0;
		time = 0;
		penaltyCount = 0;
        bonusCount = 0;
		for(int i = 0; i < distanceWithout.Length; i++){
			distanceWithout[i] = 0;
		}
	}
	
    private float displayTime = 0f;
    private float DISPLAY_MAXTIME = 3f;

	// Update is called once per frame
	void Update () {
		if (StateMachine.Instance.IsInGameNotPaused()) {
	        HandleDistanceBasedBonuses();
	        UpdateCoolDowns();
	        HandleQueuedPenalties();
			HUD.Instance.UpdateScoreDisplay((int)Score);
			if (!StateMachine.Instance.IsInState(GameState.ScoreTally)) {
				time += Time.deltaTime;
			}
		}
	}

    public int PassengerBonus() {
        return MissionHandler.Instance.passengersTransported * SCORE_PER_PASSENGER;
    }

    public int TimeTableBonus() {
        float percentage = GetOnTimePercentage();
        float bonus = (percentage * ROUTE_TIME_BONUS) / 100;
        return (int) bonus;
    }

    public float GetOnTimePercentage() {
        float low = MissionHandler.Instance.route.timeLimit;
        float high = MissionHandler.Instance.route.bonusTimeLimit;
        
        if (time > high) {
             return 0f;
        }
        float relativeTime = Mathf.Max(0f, Mathf.InverseLerp(low, high, time));
        float relativeBonus = Mathf.Pow(1f - relativeTime, 1.5f);
        float percentage = Mathf.Round(100f * relativeBonus);;
		
		return percentage;
    }

    void UpdateCoolDowns() {
        lastBadBlinkerCooldown -= Time.deltaTime;
		lastGreatBlinkerCooldown -= Time.deltaTime;
    }

    void HandleQueuedPenalties() {
        if (scoreEventQueue.Count > 0) {
            displayTime -= Time.deltaTime;
            var currentEvent = scoreEventQueue.Peek();
            if (!currentEvent.applied) {
                ApplyEvent(currentEvent);
            }
            if (displayTime < 0) {
                scoreEventQueue.Dequeue();
                displayTime = DISPLAY_MAXTIME;
            }
        }

    }

    void ApplyEvent(ScoreEvent currentEvent) {
		HUD.Instance.scoreEventDisplay.DisplayEvent(currentEvent);
        if (currentEvent.PointValue < 0) {
            penaltyCount++;
            totalPenalties += currentEvent.PointValue;
        } else {
            totalBonus += currentEvent.PointValue;
            bonusCount++;
        }
        currentEvent.applied = true;
    }

    void HandleDistanceBasedBonuses() {
        for( int i = 0; i < distanceWithout.Length; i++ ) {
            var bus = MissionHandler.Instance.bus;
            if (bus == null) {
                return;
            }
            var busRigidBody = bus.rigidbody;

            distanceWithout[i] += 
                busRigidBody.velocity.magnitude
                * Time.deltaTime;
            if (distanceWithout[i] > ONE_MILE_IN_METERS) {
                if (i == WTH_all) {
                    MileWithOutAnyProblems();
                    distanceWithout[i] = 0.0f;
                } else if (i == WTH_crash) {
                    MileWithOutCrash();
                    distanceWithout[i] = 0.0f;
                } else if (i == WTH_offence) {
                    MileWithOutOffence();
                    distanceWithout[WTH_all] = 0.0f;
                } else if (i == WTH_unhappy) {
                    MileWithOutAnyUnhappy();
                    distanceWithout[WTH_all] = 0.0f;
                }
            }
        }
    }

    public void PassengerLeft(int count) {
		SoundMaster.Instance.PlayMinusBonus();
        float netPenalty = count * PASSENGERS_LEAVE_PENALTY;
        var scoreEvent = new ScoreEvent(PASSENGERS_LEAVE_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }

    public void PassengersLeftAtStop(int count) {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty = count * PASSENGERS_ENTER_PENALTY;
        var scoreEvent = new ScoreEvent(PASSENGERS_ENTER_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }

    public void PassengerEarlyExit() {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty =  EARLY_EXIT_PENALTY;
        var scoreEvent = new ScoreEvent(EARLY_EXIT_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }
    
    public void WrongWayDriving() {
		SoundMaster.Instance.PlayMinusBonus();
        // there code has something about penalty offence levels. 
        var netPenalty = DRIVING_OFFENCE_PENALTY;
        var scoreEvent = new ScoreEvent(DRIVING_OFFENCE_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);
        PassengerSystem.Instance.UpdateMood(0.2f);

        distanceWithout[WTH_offence] = 0.0f;
        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }

    public void FailedToFollowChopper() {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty = FOLLOW_CHOPPER_PENALTY;
        var scoreEvent = new ScoreEvent(FOLLOW_CHOPPER_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_all] = 0.0f;
    }

    public void NextStopName() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = NEXT_STOP_BONUS;
        var scoreEvent = new ScoreEvent(NEXT_STOP_MESSAGE_NAME, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);
    }

    public void NextStop() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = NEXT_STOP_BONUS;
        var scoreEvent = new ScoreEvent(NEXT_STOP_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);
    }

    public void CheckPointBonus() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = NEXT_CHECKPOINT_BONUS;
        var scoreEvent = new ScoreEvent(NEXT_CHECKPOINT_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);
    }
    
    public void RanRedLight() {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty = RED_LIGHT_OFFENCE_PENALTY;
        var scoreEvent = new ScoreEvent(RED_LIGHT_OFFENCE_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);
    }

    public void BadBlinker() {
        if (lastBadBlinkerCooldown < 0.0f) {
			SoundMaster.Instance.PlayMinusBonus();
            var netPenalty = BAD_BLINKER_PENALTY;
            lastBadBlinkerCooldown = BAD_BLINKER_PENALTY_TIMER;
            var scoreEvent = new ScoreEvent( BAD_BLINKER_MESSAGE, netPenalty );
            scoreEventQueue.Enqueue(scoreEvent);
            distanceWithout[WTH_all] = 0.0f;
			distanceWithout[WTH_offence] = 0.0f;
        }
    }

    public void GreatBlinker() {
		if (lastGreatBlinkerCooldown < 0.0f) {
			SoundMaster.Instance.PlayPlusBonus();
			lastGreatBlinkerCooldown = BAD_BLINKER_PENALTY_TIMER;
	        var netBonus = GREAT_BLINKER_BONUS;
	        var scoreEvent = new ScoreEvent(GREAT_BLINKER_MESSAGE, netBonus);
	        scoreEventQueue.Enqueue(scoreEvent);
		}
    }

    public void GreatBlinkerRoad() {
		if (lastGreatBlinkerCooldown < 0.0f) {
			SoundMaster.Instance.PlayPlusBonus();
			lastGreatBlinkerCooldown = BAD_BLINKER_PENALTY_TIMER;
	        var netBonus = GREAT_BLINKER_ROAD_BONUS;
	        var scoreEvent = new ScoreEvent(GREAT_BLINKER_ROAD_MESSAGE, netBonus);
	        scoreEventQueue.Enqueue(scoreEvent);
		}
    }

    public void OpenDoorPenalty() {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty = OPEN_DOOR_PENALTY;
        var scoreEvent = new ScoreEvent(OPEN_DOOR_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_all] = 0.0f;
    }

    public void IntensiveBreaking() {
        var netPenalty = UNHAPPY_PASSENGER_PENALTY;
        var scoreEvent = new ScoreEvent(INTENSIVE_BRAKING_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);
        PassengerSystem.Instance.UpdateMood(0.3f);
        
        distanceWithout[WTH_all] = 0.0f;
        distanceWithout[WTH_unhappy] = 0.0f;
    } 

    public void ObstacleCrash(float speed) {
		if(speed >= CrashVelocityHigh){
			scoreEventQueue.Enqueue(new ScoreEvent(OBSTACLE_CRASH_MESSAGE_HIGH, NON_VEHICLE_CRASH_PENALTY_HIGH));
		}
		else if(speed >= CrashVelocityMedium){
			scoreEventQueue.Enqueue(new ScoreEvent(OBSTACLE_CRASH_MESSAGE_MEDIUM, NON_VEHICLE_CRASH_PENALTY_MEDIUM));
		}
		else if(speed >= CrashVelocityLow){
			scoreEventQueue.Enqueue(new ScoreEvent(OBSTACLE_CRASH_MESSAGE_LOW, NON_VEHICLE_CRASH_PENALTY_LOW));
		}

        // distance update stuff 
        // passenger update stuff. 
    }

    public void RedLightOffence() {
		SoundMaster.Instance.PlayMinusBonus();
        var netPenalty = RED_LIGHT_OFFENCE_PENALTY;
        var scoreEvent = new ScoreEvent(RED_LIGHT_OFFENCE_MESSAGE, netPenalty);
        scoreEventQueue.Enqueue(scoreEvent);

        PassengerSystem.Instance.UpdateMood(0.1f);

        distanceWithout[WTH_offence] = 0.0f;
        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }

    public void GreenLightBonus() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = GREEN_LIGHT_BONUS;
        var scoreEvent = new ScoreEvent(GREEN_LIGHT_BONUS_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);
    }
 
    public void VehicleCrash(float speed) {
        Debug.Log(speed);
        if(speed >= CrashVelocityHigh){
			scoreEventQueue.Enqueue(new ScoreEvent(VEHICLE_CRASH_MESSAGE_HIGH, VEHICLE_CRASH_PENALTY_HIGH));
		}
		else if(speed >= CrashVelocityMedium){
			scoreEventQueue.Enqueue(new ScoreEvent(VEHICLE_CRASH_MESSAGE_MEDIUM, VEHICLE_CRASH_PENALTY_MEDIUM));
		}
		else if(speed >= CrashVelocityLow){
			scoreEventQueue.Enqueue(new ScoreEvent(VEHICLE_CRASH_MESSAGE_LOW, VEHICLE_CRASH_PENALTY_LOW));
		}
        PassengerSystem.Instance.UpdateMood(0.6f);

        distanceWithout[WTH_crash] = 0.0f;
        distanceWithout[WTH_offence] = 0.0f;
        distanceWithout[WTH_unhappy] = 0.0f;
        distanceWithout[WTH_all] = 0.0f;
    }

    public void MileWithOutCrash() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = MILE_WITHOUT_CRASH_BONUS;
        var scoreEvent = new ScoreEvent(MILE_WITHOUT_CRASH_MESSAGE,netBonus);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_crash] = 0.0f;
    }

    public void MileWithOutOffence() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = MILE_WITHOUT_OFFENCE_BONUS;
        var scoreEvent = new ScoreEvent(MILE_WITHOUT_OFFENCE_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_offence] = 0.0f;
    }

    public void MileWithOutAnyUnhappy() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = MILE_WITHOUT_UNHAPPY_BONUS;
        var scoreEvent = new ScoreEvent(MILE_WITHOUT_UNHAPPY_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_unhappy] = 0.0f;
    }

    public void MileWithOutAnyProblems() {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = MILE_WITHOUT_ANY_BONUS;
        var scoreEvent = new ScoreEvent(MILE_WITHOUT_ANY_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);

        distanceWithout[WTH_all] = 0.0f;
    }

    public void PerfectStop(int value) {
		SoundMaster.Instance.PlayPlusBonus();
        var netBonus = value;
        var scoreEvent = new ScoreEvent(PERFECT_STOP_BONUS_MESSAGE, netBonus);
        scoreEventQueue.Enqueue(scoreEvent);
    }

}

public class ScoreEvent {
    float pointValue;
    string message;
    public bool applied = false;
    public string Message { get { return message; }}
    public float PointValue { 
        get {
            return pointValue;
        }
    }

    public ScoreEvent(string message, float pointValue) {
        this.message = message;
        this.pointValue = pointValue;
    }

    public string toString() {
        return message + " " + pointValue;
    }
}
