using UnityEngine;
using System;
using System.Collections;

public class ContinueButton : MonoBehaviour {
	public void OnClick() {
		Events.Instance.OnMissionSelectMenuLoaded(this, EventArgs.Empty);	
	}
}