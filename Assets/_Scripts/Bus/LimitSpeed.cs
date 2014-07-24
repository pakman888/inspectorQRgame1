using UnityEngine;
using System.Collections;

public class LimitSpeed : MonoBehaviour {

	 public float maxSpeed = 200f;//Replace with your max speed
	 
	  void FixedUpdate()
	  {
	        if(rigidbody.velocity.magnitude > maxSpeed)
	        {
	               rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
	        }
	  }

}
