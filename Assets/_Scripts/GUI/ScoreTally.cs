using UnityEngine;
using System;
using System.Collections;

public class ScoreTally : MonoBehaviour {
	public UILabel passengersTransported;
	public UILabel drivingPenalty;
	public UILabel onTimeBonus;
	public UILabel drivingBonus;
	public UILabel score;
	public UILabel highScoreLabel;
	
	void Start() {
		Events.Instance.MissionEnded += (sender, e) => UpdateLabels();	
	}
	
	void UpdateLabels() {
		passengersTransported.text = ScoreManager.Instance.GetPassengersTransportedBonus().ToString();
		drivingPenalty.text = ((int)ScoreManager.Instance.totalPenalties).ToString();
		onTimeBonus.text = ScoreManager.Instance.TimeTableBonus().ToString();
		drivingBonus.text = ((int)ScoreManager.Instance.totalBonus).ToString();
		
		int finalScore = (int)ScoreManager.Instance.Score + ScoreManager.Instance.GetPassengersTransportedBonus() + ScoreManager.Instance.TimeTableBonus();
		
		score.text = finalScore.ToString();
		
		var route = MissionHandler.Instance.route;
		var prefKey = String.Format("highscore{0}{1}", route.tier, route.rank); 
		var highScore = PlayerPrefs.GetInt(prefKey, -999999);
		if (finalScore > highScore) {
			highScore = finalScore;
			PlayerPrefs.SetInt(prefKey, highScore); 
		}	
		highScoreLabel.text = highScore.ToString();
	}
}