using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour {
	public UILabel speedDisplay;
	public Transform needle;
	public float needleMinAngle;
	public float needleMaxAngle;
	public float needleMaxSpeed;

	float currentSpeed;
	float displayChangeDelay = 0.1f;
	float time = 0.0f;
	
	public void UpdateDisplay(float speed) {
		currentSpeed = speed;
		needle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(needleMinAngle, needleMaxAngle, currentSpeed/needleMaxSpeed));
	}	
	
	void Update() {
		time += Time.deltaTime;
		// add delay in order to not have the number spazz out
		if (time > displayChangeDelay) {
			speedDisplay.text = currentSpeed.ToString("0.");	
			time -= displayChangeDelay;
		}
	}
}
