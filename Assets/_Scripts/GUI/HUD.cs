using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : Singleton<HUD> {	
	public UILabel scoreDisplay; 
	public UILabel distanceDisplay;
	public UILabel timerDisplay;
	public UILabel stopsLeftDisplay;
	public GameObject openDoorsHint;
	public Speedometer speedometer;
	public List<UIHoldButton> holdButtons;
	public UIWidget steeringZone;
	public BrakeMeter brakeMeter;
	public ScoreEventDisplay scoreEventDisplay;
	public BusStopPassengerStatusDisplay busStopPassengerStatusDisplay;
	public UILabel happyPassengerDisplay, unhappyPassengerDisplay;
	
	public bool GetButton(string buttonName) {
		bool val = false;

		foreach (var button in holdButtons) {
			if (button.gameObject.name == buttonName) {
				val = button.BeingHeld;
				break;
			}
		}
		
		return val;
		
		// Note: The LINQ solution was crashing the project. In interests of saving time I re-coded it.
		// Investigate when there's more time.
		//return (from button in holdButtons where button.gameObject.name == buttonName select button.BeingHeld).FirstOrDefault();
	}
	
	public bool GetButtonDown(string buttonName) {
		bool val = false;
		
		foreach (var button in holdButtons) {
			if (button.gameObject.name == buttonName) {
				val = button.JustPressed;
				break;
			}
		}
		
		
		return val;
		//return (from button in holdButtons where button.gameObject.name == buttonName select button.JustPressed).FirstOrDefault();
	}
	
	public void ShowOpenDoorsHint() {
		openDoorsHint.SetActive(true);
	}
	
	public void HideOpenDoorsHint() {
		openDoorsHint.SetActive(false);
	}
	
	public void UpdateTimerDisplay(float time) {
		bool neg = time < 0;
		int mins = (int)(time / 60);
		int secs = Mathf.Abs(Mathf.FloorToInt(time - (mins * 60)));
		timerDisplay.text = (neg ? "-" : string.Empty) + Mathf.Abs(mins).ToString("00.") + ":" + secs.ToString("00.");
	}
	
	public void UpdateScoreDisplay(int score) {
		scoreDisplay.text = score.ToString();
	}
	
	public void UpdateDistanceDisplay(float distance) {
		distanceDisplay.text = distance.ToString("0.00");	
	}
	
	public void UpdateBusStopsLeftDisplay(int stopsLeft, int totalStops) {
		stopsLeftDisplay.text = stopsLeft.ToString();
	}
	
	public void UpdateSpeedometer(float speed) {
		speedometer.UpdateDisplay(speed);
	}
}