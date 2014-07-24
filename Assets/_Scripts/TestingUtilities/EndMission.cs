using UnityEngine;
using System;
using System.Collections;

public class EndMission : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			Events.Instance.OnMissionSelectMenuLoaded(this, EventArgs.Empty);
		}
	}
}
