using UnityEngine;
using System.Collections;

public class webcam : MonoBehaviour {

	WebCamTexture front;

	// Use this for initialization
	void Start () {
		front = new WebCamTexture();

		renderer.material.mainTexture = front;
		front.Play ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
