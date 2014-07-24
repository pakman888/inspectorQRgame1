using System;
using UnityEngine;
using System.Collections;

public class MissionSelectMenu : MonoBehaviour {
	public void OnBackButtonPressed() {
		Events.Instance.OnMainMenuLoaded(this, EventArgs.Empty);	
	}
}
