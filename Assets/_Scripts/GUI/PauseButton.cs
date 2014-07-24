using UnityEngine;
using System;
using System.Collections;

public class PauseButton : MonoBehaviour {

	//TODO: Make this pause game and pop up in-game menu instead of returning to main menu
	public void OnPressed(){
		Events.Instance.OnMissionSelectMenuLoaded(this, EventArgs.Empty);
	}
}
