using UnityEngine;
using System.Collections;

// This class simulates a single car's wheel with tire, brake and simple
// suspension (basically just a single, independant spring and damper).
public class Wheel : MonoBehaviour {
	// Workaround for Mission 1 Build: Instead of sorting this garbage out, 
	// just mulitply added force by a less-than-1 multipler to slwo the bus down
	float forceMultiplier = 1f;
	
	//Wheel parameter set
	public WheelParameters wheelParams;
	//parameter accessors
	public float massFraction 			   { get{ return wheelParams.massFraction; }}
	public float damping				   { get{ return wheelParams.damping; }}
	public float radius 				   { get{ return wheelParams.radius; }}
	public float inertia 				   { get{ return wheelParams.inertia; }}
	public float brakeFrictionTorque	   { get{ return wheelParams.brakeFrictionTorque; }}
	public float handbrakeFrictionTorque   { get{ return wheelParams.handbrakeFrictionTorque; }}
	public float maxSteeringAngle		   { get{ return wheelParams.maxSteeringAngle; }}
	public float frictionTorque			   { get{ return wheelParams.frictionTorque; }}
	public float suspensionTravel		   { get{ return wheelParams.suspensionTravel; }}
	
	// Graphical wheel representation (to be rotated accordingly)
	public GameObject model;
	//Side of vehicle the wheel is on; -1 for left +1 for right
	public float side;
	
	// inputs
	// engine torque applied to this wheel
	public float driveTorque = 0;
	// engine braking and other drivetrain friction torques applied to this wheel
	public float driveFrictionTorque = 0;
	// brake input
	public float brake = 0;
	// handbrake input
	public float handbrake = 0;
	// underpowered drivewheel brake input
	public float lazyBrake = 0;
	// steering input
	public float steering = 0;
	// drivetrain inertia as currently connected to this wheel
	public float drivetrainInertia = 0;
	// suspension force externally applied (by anti-roll bars)
	public float suspensionForceInput = 0;
	// Coeefficient of grip - this is simly multiplied to the resulting forces, 
	// so it is not quite realitic, but an easy way to quickly change handling characteritics
	public float grip = 1.0f;
	
	// output
	public float angularVelocity;
	public float slipRatio;
	public float peakSlipRatio;
	public float slipVelo;
	public float slipAngle;
	public float compression;
	
	// state
	float fullCompressionSpringForce;
	Vector3 wheelVelo;
	Vector3 localVelo;
	Vector3 groundNormal;
	float rotation;
	float normalForce;
	Vector3 suspensionForce;
	Vector3 roadForce;
	Vector3 up, right, forward;
	Quaternion localRotation = Quaternion.identity;
	Quaternion inverseLocalRotation = Quaternion.identity;		
	int lastSkid = -1;
	float antilockMultiplier = 1;
	
	// cached values
	Rigidbody body;	
	float oldAngle;	
	Skidmarks skid;	
		
	void Start () {
		Transform trs = transform;
		while (trs != null && trs.rigidbody == null)
			trs = trs.parent;
		if (trs != null)
			body = trs.rigidbody;

		skid = FindObjectOfType(typeof(Skidmarks)) as Skidmarks;
		fullCompressionSpringForce = body.mass * massFraction * 2.0f * -Physics.gravity.y;
		
		if(Application.isEditor){
			wheelParams.InitSlipMaxima();
		}
	}
	
	Vector3 SuspensionForce () {
		float springForce = compression * fullCompressionSpringForce;
		normalForce = springForce;
		
		float damperForce = Vector3.Dot(localVelo, groundNormal) * damping;

		return (springForce - damperForce + suspensionForceInput) * up;
	}
	
	float SlipRatio ()
	{
		const float fullSlipVelo = 4.0f;

		float wheelRoadVelo = Vector3.Dot (wheelVelo, forward);
		if (wheelRoadVelo == 0)
			return 0;
		
		float absRoadVelo = Mathf.Abs (wheelRoadVelo);
		float damping = Mathf.Clamp01( absRoadVelo / fullSlipVelo );
		
		float wheelTireVelo = angularVelocity * radius;
		return (wheelTireVelo - wheelRoadVelo) / absRoadVelo * damping;
	}

	float SlipAngle ()
	{
		const float fullAngleVelo = 2.0f;
		
		Vector3 wheelMotionDirection = localVelo;
		wheelMotionDirection.y = 0;

		if (wheelMotionDirection.sqrMagnitude < Mathf.Epsilon)
			return 0;
				
		float sinSlipAngle = wheelMotionDirection.normalized.x;
		Mathf.Clamp(sinSlipAngle, -1, 1); // To avoid precision errors.

		float damping = Mathf.Clamp01( localVelo.magnitude / fullAngleVelo );
		
		return -Mathf.Asin(sinSlipAngle) * damping * damping;
	}
	
	public float GetBrakeFactor(){
		return Mathf.Max(brake, lazyBrake) * antilockMultiplier;
	}
	
	Vector3 RoadForce () {
		int slipRes=(int)((100.0f-Mathf.Abs(angularVelocity))/(10.0f));
		if (slipRes < 1)
			slipRes = 1;
		float invSlipRes = (1.0f/(float)slipRes);
		
		float totalInertia = inertia + drivetrainInertia;
		float driveAngularDelta = driveTorque * Time.deltaTime * invSlipRes / totalInertia;
		float totalFrictionTorque = brakeFrictionTorque * GetBrakeFactor() + handbrakeFrictionTorque * handbrake + frictionTorque + driveFrictionTorque;
		float frictionAngularDelta = totalFrictionTorque * Time.deltaTime * invSlipRes / totalInertia;

		Vector3 totalForce = Vector3.zero;
		float newAngle = maxSteeringAngle * steering;
		peakSlipRatio = 0;
		for (int i=0; i<slipRes; i++)
		{
			float f = i * 1.0f/(float)slipRes;
			localRotation = Quaternion.Euler (0, oldAngle + (newAngle - oldAngle) * f, 0); 		
			inverseLocalRotation = Quaternion.Inverse(localRotation);
			forward = transform.TransformDirection (localRotation * Vector3.forward);
			right = transform.TransformDirection (localRotation * Vector3.right);
			
			slipRatio = SlipRatio ();
			peakSlipRatio = Mathf.Max(peakSlipRatio, Mathf.Abs(slipRatio));
			slipAngle = SlipAngle ();
			Vector3 force = invSlipRes * grip * wheelParams.CombinedForce (normalForce, slipRatio, slipAngle, localVelo, groundNormal);
			Vector3 worldForce = transform.TransformDirection (localRotation * force);
			angularVelocity -= (force.z * radius * Time.deltaTime) / totalInertia;
			angularVelocity += driveAngularDelta;
			if (Mathf.Abs(angularVelocity) > frictionAngularDelta)
				angularVelocity -= frictionAngularDelta * Mathf.Sign(angularVelocity);
			else
				angularVelocity = 0;
			
			wheelVelo += worldForce* (1/body.mass) * Time.deltaTime * invSlipRes;
			totalForce += worldForce;
		}

		float longitunalSlipVelo = Mathf.Abs(angularVelocity * radius - Vector3.Dot (wheelVelo, forward));	
		float lateralSlipVelo = Vector3.Dot (wheelVelo, right);
		slipVelo = Mathf.Sqrt(longitunalSlipVelo * longitunalSlipVelo + lateralSlipVelo * lateralSlipVelo);
		
		oldAngle = newAngle;
		return totalForce;
	}
	
	void FixedUpdate () { 
		Vector3 pos = transform.position;
		up = transform.up;
		RaycastHit hit;
		bool onGround = Physics.Raycast( pos, -up, out hit, suspensionTravel + radius);
		
		if (onGround && hit.collider.isTrigger)
		{
			onGround = false;float dist = suspensionTravel + radius;
			RaycastHit[] hits = Physics.RaycastAll( pos, -up, suspensionTravel + radius);
			foreach(RaycastHit test in hits)
			{
				if (!test.collider.isTrigger && test.distance <= dist)
				{
					hit = test;
					onGround = true;
					dist = test.distance;
				}
			}
		}

		if (onGround)
		{
			groundNormal = transform.InverseTransformDirection (inverseLocalRotation * hit.normal);
			compression = 1.0f - ((hit.distance - radius) / suspensionTravel);
			wheelVelo = body.GetPointVelocity (pos);
			localVelo = transform.InverseTransformDirection (inverseLocalRotation * wheelVelo);
			suspensionForce = SuspensionForce ();
			roadForce = RoadForce ();
			body.AddForceAtPosition ((suspensionForce + forceMultiplier * roadForce), pos);
		}
		else
		{
			compression = 0.0f;
			suspensionForce = Vector3.zero;
			roadForce = Vector3.zero;
			float totalInertia = inertia + drivetrainInertia;
			float driveAngularDelta = driveTorque * Time.deltaTime / totalInertia;
			float totalFrictionTorque = brakeFrictionTorque * GetBrakeFactor() + handbrakeFrictionTorque * handbrake + frictionTorque + driveFrictionTorque;
			float frictionAngularDelta = totalFrictionTorque * Time.deltaTime / totalInertia;
			angularVelocity += driveAngularDelta;
			if (Mathf.Abs(angularVelocity) > frictionAngularDelta)
				angularVelocity -= frictionAngularDelta * Mathf.Sign(angularVelocity);
			else
				angularVelocity = 0;
			//angularVelocity = Mathf.Clamp(angularVelocity, -WHEEL_AVEL_LIMIT, WHEEL_AVEL_LIMIT);
			
			slipRatio = 0;
			slipVelo = 0;
		}
		
		//Update ABS
		if(slipRatio < wheelParams.antilockBrakeThreshold){
			antilockMultiplier *= 0.5f;
		}
		else{
			antilockMultiplier = Mathf.Min(1, antilockMultiplier * 2);
		}		
		
		if (skid != null && Mathf.Abs(slipRatio) > 0.2)
			lastSkid = skid.AddSkidMark(hit.point, hit.normal, Mathf.Abs(slipRatio) - 0.2f,lastSkid);
		else
			lastSkid = -1;
			
		compression = Mathf.Clamp01 (compression);
		rotation += angularVelocity * Time.deltaTime;
		if (model != null)
		{
			model.transform.localPosition = Vector3.up * (compression - 1.0f) * suspensionTravel;
			model.transform.localRotation = Quaternion.Euler (Mathf.Rad2Deg * rotation, maxSteeringAngle * steering, 0);
		}
	}
}
