using UnityEngine;
using System.Collections;

public class TurnSignalButton : MonoBehaviour {
	public TurnSignal signal;
	
	public void OnPressed() {
		MissionHandler.Instance.bus.OnTurnSignalButtonPressed(signal);
	}
}
