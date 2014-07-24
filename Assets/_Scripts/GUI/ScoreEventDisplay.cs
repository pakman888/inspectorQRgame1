using UnityEngine;
using System.Collections;

public class ScoreEventDisplay : MonoBehaviour {

	public UISprite penaltyBackground;
	public UISprite bonusBackground;
	public UILabel scoreModLabel;
	public UILabel eventDescriptionLabel;

	private void OnEnable(){
		//Jump to frame 0 of appearance animation
		animation.Play();
		animation.Sample();
		animation.Stop();
	}

	public void DisplayEvent(ScoreEvent currentEvent){
		penaltyBackground.gameObject.SetActive(currentEvent.PointValue < 0);
		bonusBackground.gameObject.SetActive(currentEvent.PointValue >= 0);
		scoreModLabel.text = currentEvent.PointValue.ToString("f0");
		eventDescriptionLabel.text = currentEvent.Message;
		animation.Play();
	}
}
