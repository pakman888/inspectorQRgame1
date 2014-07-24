using UnityEngine;
using System.Collections;

public class BrakeMeter : MonoBehaviour {

	public float minX, maxX;
	
	public void UpdateDisplay(float brake){
		transform.localPosition = new Vector3(Mathf.Lerp(minX, maxX, brake), transform.localPosition.y, transform.localPosition.z);
	}
}
