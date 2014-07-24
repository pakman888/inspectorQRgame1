using UnityEngine;
using System.Collections;

public class ActiveBusStopScript : MonoBehaviour {

    public BusStopScript busStop;
	
    void OnTriggerEnter(Collider other) {
		var bus = other.GetComponent<Bus>();
        if (bus) {
			bus.OnEnterBusStop(busStop);	
		}
    } 
	
	void OnTriggerExit(Collider other) {
		var bus = other.GetComponent<Bus>();
        if (bus) {
			bus.OnExitBusStop(busStop);	
		}
    }
}
