using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Bus : MonoBehaviour {
    public GameObject leftBlindSpot;
    public GameObject rightBlindSpot;
	public SoundData soundData;
	GameObject door;
	
	public float cameraDistance = 8.0f;
	public float cameraHeight = 3.75f;
	public float blinkerCameraDistance = 10f;
	
	[SerializeField] bool doorsOpen;
	public bool DoorsOpen {  
		get { return doorsOpen; } 
	}
	
	[SerializeField] TurnSignal currentSignal;
	public TurnSignal CurrentSignal {
		get { return currentSignal; }
	}
	
	public GameObject leftBlinker;
	public GameObject rightBlinker;
	
	bool doorsMoving;
	
	[SerializeField] BusStopScript insideBusStop;
	public BusStopScript InsideBusStop { 
		get { return insideBusStop; }
	}	
	
	public bool CanOpenDoors { 
		get {
			var momentum = collider.rigidbody.velocity.magnitude;
			return insideBusStop && momentum < 1 && !doorsOpen && !doorsMoving;
		}
	}

    void Start() {
		currentSignal = TurnSignal.off;
		SetBlinkerEnabled(TurnSignal.left, false);
		SetBlinkerEnabled(TurnSignal.right, false);
		
		door = (from trs in transform.GetChild(0).GetComponentsInChildren<Transform>()
			   where trs.gameObject.name.Equals("traffic")
			   select trs.gameObject).FirstOrDefault();
    }
	
	void Update() {
		if (StateMachine.Instance.IsInGameNotPaused()) {
			if (!doorsMoving && (CanOpenDoors || (DoorsOpen && !MissionHandler.Instance.LastStop))) {
				HUD.Instance.ShowOpenDoorsHint();
			}
			else {
				HUD.Instance.HideOpenDoorsHint();	
			}
			
			const float MS_TO_KMH = 3.6f;
			HUD.Instance.UpdateSpeedometer(rigidbody.velocity.magnitude * MS_TO_KMH);
		}
	}
	
	public void OnEnterBusStop(BusStopScript busStop){ 
		//We're assuming there are never overlapping bus stop triggers
		insideBusStop = busStop;	
	}
	
	public void OnExitBusStop(BusStopScript busStop) {
		insideBusStop = null;	
	}
	
	public void OnDoorsButtonPressed() {
		if( doorsMoving){
			return;
		}
		if (!doorsOpen) {
			if (CanOpenDoors) {
				StartCoroutine(HandleStoppedAtBusStop());
                //Handle good parking bonus;
			}
			else {
                ScoreManager.Instance.OpenDoorPenalty();
			}
		}
		else {
			// If the doors are open, you are implicity inside a bus stop
			if (!MissionHandler.Instance.LastStop) {
				StartCoroutine(HandleLeavingBusStop());
				
			}
		}
	}
	
	public void HandleFirstStop(BusStopScript stop) {
		Events.Instance.OnBusStopCompleted(this, new BusStopEventArgs(stop));
		insideBusStop = null;
	}
	
	IEnumerator HandleStoppedAtBusStop() {
		Events.Instance.OnStoppedAtBusStop(this, new BusStopEventArgs(insideBusStop));
		FreezeBusAndPlaceInCenterOfStop();
		yield return StartCoroutine(OpenDoor());
		
	}
	
	IEnumerator HandleLeavingBusStop() {
		yield return StartCoroutine(CloseDoor());
		UnFreezeBus();
		if(insideBusStop != null){
			Events.Instance.OnBusStopCompleted(this, new BusStopEventArgs(insideBusStop));
		}
		else{
			Debug.LogWarning("Bus trying to leave null stop");
		}
		insideBusStop = null;
	}
	
	IEnumerator OpenDoor() {
		doorsMoving = true;
		doorsOpen = true;
		SoundMaster.Instance.PlayDoorOpen();
		yield return new WaitForSeconds(0.5f);
		if (door != null) {
			door.SetActive(false);
		}
		doorsMoving = false;
	}
	
	IEnumerator CloseDoor() {
		doorsMoving = true;
		SoundMaster.Instance.PlayDoorClose();
		yield return new WaitForSeconds(0.5f);
		if (door != null) {
			door.SetActive(true);
		}
		yield return new WaitForSeconds(0.25f);
		doorsOpen = false;
		doorsMoving = false;
	}
	
	void FreezeBusAndPlaceInCenterOfStop() {
    	gameObject.rigidbody.velocity = Vector3.zero;
        gameObject.rigidbody.angularVelocity = Vector3.zero;
        gameObject.rigidbody.isKinematic = true;
		
		foreach (var wheel in GetComponent<CarController>().wheels) {
			wheel.angularVelocity = 0;
		}
		
		gameObject.transform.forward = -insideBusStop.activePlateChild.transform.forward;
		gameObject.transform.position = insideBusStop.transform.position;
	}
	
	void UnFreezeBus() {
    	gameObject.rigidbody.isKinematic = false;	
	}
	
	Coroutine<bool> blinkRoutine;
	
	public void OnTurnSignalButtonPressed(TurnSignal signal) {
		if (blinkRoutine != null) {
			blinkRoutine.Cancel();
			SetBlinkerEnabled(TurnSignal.left, false);
			SetBlinkerEnabled(TurnSignal.right, false);
		}
		if (currentSignal == signal) {
			SoundMaster.Instance.StopBlinker();
			currentSignal = TurnSignal.off;
		}
		else {
			SoundMaster.Instance.PlayBlinker();
			blinkRoutine = StartCoroutine<bool>(Blink(signal));
			currentSignal = signal;
		}
        Events.Instance.OnBlinkreChange(this, new BusEventArgs(this));
	}
	
	IEnumerator Blink(TurnSignal signal) {
		float time = 0;
		bool on = true;
		while (true) {
			SetBlinkerEnabled(signal, on);
			time += Time.deltaTime;
			if (time > 0.5f) {
				time -= 0.5f;
				on = !on;
			}
			yield return null;	
		}
	}
	
	void SetBlinkerEnabled(TurnSignal signal, bool on) {
		if (signal == TurnSignal.left) {
			leftBlinker.SetActive(on);
		}
		if (signal == TurnSignal.right) {
			rightBlinker.SetActive(on);
		}
	}
}

public enum TurnSignal {
	off, left, right
}
