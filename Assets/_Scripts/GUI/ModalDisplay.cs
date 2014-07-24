using UnityEngine;
using System.Collections;

public class ModalDisplay : MonoBehaviour {
	public UILabel scoreDisplay;
	public UILabel scoreMessage;
	TweenAlpha alphaTween;
	
	void Start() {
		alphaTween = GetComponent<TweenAlpha>();
		alphaTween.onFinished.Add(new EventDelegate(this, "TweenFinished"));
	}
	
	public void TweenFinished() {
		//Debug.Log ("tweened");	
	}
	
	public void StartDisplay(ScoreEvent scoreEvent) {
		
	}
}
