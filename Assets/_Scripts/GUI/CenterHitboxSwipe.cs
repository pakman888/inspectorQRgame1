using UnityEngine;
using System.Collections;

public class CenterHitboxSwipe : MonoBehaviour {
	float startTime;
	float maxSwipeTime = 1;
	Rect hitbox;
	int swipeFingerId = -1;
	
	void Start() {
		hitbox = new Rect(Screen.width / 3, Screen.height / 3, Screen.width / 3, Screen.height / 3); 
	}
	
	void Update() {
		if (StateMachine.Instance.IsInGameNotPaused()) {
			Touch[] touches = Input.touches;
			if(touches.Length == 0){
				CancelSwipe();				
			}
			for(int i = 0; i < touches.Length; i++) {
				var touch = touches[i];
				if(swipeFingerId < 0 && touch.phase == TouchPhase.Began && hitbox.Contains(touch.position)){
					swipeFingerId = touch.fingerId;
	            	startTime = Time.time;	
				}
				if(touch.fingerId == swipeFingerId){
					if (touch.phase == TouchPhase.Stationary || Time.time - startTime > maxSwipeTime) {
						CancelSwipe();
					}
					else if (touch.phase == TouchPhase.Ended){
						var bus = MissionHandler.Instance.bus;
						if (touch.position.x > hitbox.xMax && touch.position.y > hitbox.yMin && touch.position.y < hitbox.yMax) {
							MissionHandler.Instance.bus.OnDoorsButtonPressed();	
						}
						else if (!bus.CanOpenDoors && bus.InsideBusStop && touch.position.x < hitbox.xMin && touch.position.y > hitbox.yMin && touch.position.y < hitbox.yMax) {
							MissionHandler.Instance.bus.OnDoorsButtonPressed();	
						}
						/*
						//Swipe for map disabled for now
						else if (touch.position.y < hitbox.yMin && touch.position.x > hitbox.xMin && touch.position.x < hitbox.xMax) {
							UIManager.Instance.PullDownMap();		
						}
						else if (touch.position.y > hitbox.yMax && touch.position.x > hitbox.xMin && touch.position.x < hitbox.xMax) {
							UIManager.Instance.PushUpMap();		
						}
						*/
						CancelSwipe();
					}
				}
			}
		}
		else{
			CancelSwipe();
		}
	}
	
	void CancelSwipe(){
		swipeFingerId = -1;
	}
}
