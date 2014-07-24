using System;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public TweenPosition playButton;
	public Transform playAnchor;
	public TweenPosition muteButton;
	public Transform muteAnchor;
	
	void Start() { 
		StartCoroutine(StartTweening());	
	}
	
	IEnumerator StartTweening() {
		yield return new WaitForEndOfFrame();
		playButton.to = playAnchor.transform.localPosition;
		playButton.enabled = true;
		muteButton.to = muteAnchor.transform.localPosition;
		muteButton.enabled = true;
	}
	
	public void OnMuteButtonPressed() {
		Events.Instance.OnMuteButtonPressed(this, EventArgs.Empty);	
	}
	
	public void OnPlayButtonPressed() {
		Events.Instance.OnMissionSelectMenuLoaded(this, EventArgs.Empty);
	}
}
