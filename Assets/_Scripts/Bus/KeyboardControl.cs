using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class KeyboardControl : MonoBehaviour {
	public KeyCode forward;
	public KeyCode backward;
	public KeyCode left;
	public KeyCode right;
	public float speed;
	public float rotationSpeed;
	float angle;
	
	void FixedUpdate() {
		if (Input.GetKey(forward)) {
			rigidbody.AddForce(transform.forward * speed);
			//transform.Translate(transform.forward * speed * Time.deltaTime);	
		}
		if (Input.GetKey(backward)) {
			rigidbody.AddForce(-transform.forward * speed);
			//transform.Translate(-transform.forward * speed * Time.deltaTime);	
		}
		if (Input.GetKey(left)) {
			transform.Rotate(transform.up, -rotationSpeed * Time.deltaTime);
		}
		if (Input.GetKey(right)) {
			transform.Rotate(transform.up,  rotationSpeed * Time.deltaTime);
		}
	}
}
