using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {

	public NodeItem node{get; set;}	
    [SerializeField] protected GameObject greenLightOn;
    [SerializeField] protected GameObject greenLightOff;
    [SerializeField] protected GameObject redLightOn;
    [SerializeField] protected GameObject redLightOff;
    [SerializeField] protected GameObject yellowLightOn;
    [SerializeField] protected GameObject yellowLightOff;

	public TrafficLightController.LIGHT_STATES State {
		get{
			return state;
		}
		set{
			state = value;
			greenLightOn.SetActive	(value == TrafficLightController.LIGHT_STATES.GREEN);
			greenLightOff.SetActive	(value != TrafficLightController.LIGHT_STATES.GREEN);
			yellowLightOn.SetActive	(value == TrafficLightController.LIGHT_STATES.YELLOW);
	        yellowLightOff.SetActive(value != TrafficLightController.LIGHT_STATES.YELLOW);
			redLightOn.SetActive	(value == TrafficLightController.LIGHT_STATES.RED);
	        redLightOff.SetActive	(value != TrafficLightController.LIGHT_STATES.RED);
		}
	}
	private TrafficLightController.LIGHT_STATES state;

    void Awake() {
		State = TrafficLightController.LIGHT_STATES.NONE;
    }
}
