using UnityEngine;
using System.Collections;

public class BusStopPassengerStatusDisplay : MonoBehaviour {

	public UILabel passengersExitingLabel;
	public UILabel passengersBoardingLabel;

	void Start(){
		Hide();
	}
	
	public void Hide(){
		gameObject.SetActive(false);
	}

	public void UpdateDisplay(int boarding, int exiting){
		gameObject.SetActive(true);
		passengersBoardingLabel.text = boarding.ToString();
		passengersExitingLabel.text = exiting.ToString();
	}
	
}
