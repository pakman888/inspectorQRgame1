using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour {
	/*
	This camera smoothes out rotation around the y-axis and height.
	Horizontal Distance to the target is always fixed.
	
	There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.
	
	For every of those smoothed values we calculate the wanted value and the current value.
	Then we smooth it using the Lerp function.
	Then we apply the smoothed values to the transform's position.
	*/
	
	// The target we are following
	public Drivetrain target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	
	public float verticalAngle = 0.0f;
	public float offsetAngle = 0.0f;
	public float offsetAngleMax = -10.0f;
	
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public float distanceDamping = 3.0f;

    public float blinkerAngle = 6f;
    public float blinkerFollow = 12f;

    public TurnSignal signal = TurnSignal.off;

	private float distanceLerp;

    void Start() {
		Events.Instance.Overbraking += new BusEventHandler(Overbraking);
        Events.Instance.BlinkerChange += new BusEventHandler(BlinkerChange);
    }

    void BlinkerChange(object sender, BusEventArgs e) {
        signal = e.bus.CurrentSignal;
    }

    void Overbraking(object sender, BusEventArgs e) {
        transform.position -= (target.transform.forward - target.transform.up);
    }

	public void ApplyTransformationImmediately() {
		// Early out if we don't have a target
		if (!target)
			return;
		
		// Calculate the current rotation angles
		var wantedRotationAngle = target.transform.eulerAngles.y;
		var wantedHeight = target.transform.position.y + height;
		
		var currentRotation = Quaternion.Euler (0, wantedRotationAngle, 0);
		transform.position = target.transform.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		transform.position = new Vector3(transform.position.x, wantedHeight, transform.position.z);
		
		transform.LookAt(target.transform);
	}

	void LateUpdate () {
		// Early out if we don't have a target
		if (!target)
			return;
		// Calculate the current rotation angles
        var wantedRotationAngle = target.transform.eulerAngles.y;
        if (signal == TurnSignal.left) {
            wantedRotationAngle -= blinkerAngle;
        } else if (signal == TurnSignal.right) {
            wantedRotationAngle += blinkerAngle;
        }

		var wantedHeight = target.transform.position.y + height;
			
		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// damped value between distance and blinkerFollow meters behind the target, depending on signalling state
		distanceLerp = Mathf.Lerp(distanceLerp, signal == TurnSignal.off ? 0 : 1, distanceDamping * Time.deltaTime);
        var currentDistance = Mathf.Lerp(distance, blinkerFollow, distanceLerp);
        transform.position = target.transform.position;
		transform.position -= currentRotation * Vector3.forward * currentDistance;
		
		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
		
		// Always look at the target
        transform.LookAt(target.transform);
		float wantedOffsetAngle;
		if (!target.InReverse) {
			wantedOffsetAngle = Mathf.Lerp (0, offsetAngleMax, Mathf.Clamp ((target.rigidbody.velocity.magnitude * 1.64f) / 24.0f, 0.0f, 1.0f));
		}
		else {
			wantedOffsetAngle = 0;	
		}
		offsetAngle = Mathf.LerpAngle (offsetAngle, wantedOffsetAngle, rotationDamping * Time.deltaTime);
	    //up down;	
		transform.Rotate(Vector3.right, verticalAngle + offsetAngle);
	}
}