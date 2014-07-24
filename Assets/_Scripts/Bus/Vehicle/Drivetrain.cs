using UnityEngine;
using System.Collections;

// This class simulates a car's engine and drivetrain, generating
// torque, and applying the torque to the wheels.
public class Drivetrain : MonoBehaviour {
	
	// All the wheels the drivetrain should power
	public Wheel[] poweredWheels;
	
	//Shared drivetrain specifications
	public DrivetrainParameters drivetrainParams;
	
	// inputs
	// engine throttle
	public float throttle = 0;
	// engine throttle without traction control (used for automatic gear shifting)
	public float throttleInput = 0;
	
	// shift gears automatically?
	public bool automatic = true;

	// state
	public int gear = 2;
	public float rpm;
	public float slipRatio = 0.0f;
	float engineAngularVelo;
	
	
	float Sqr (float x) { return x*x; }
	
	public bool CanReverse{
		get{
			return throttleInput < 0 && rpm <= drivetrainParams.minRPM;
		}
	}
	
	public bool InReverse{
		get{
			return gear == 0;
		}
	}
	
	void FixedUpdate () 
	{
		float ratio = drivetrainParams.gearRatios[gear] * drivetrainParams.finalDriveRatio;
		float inertia = drivetrainParams.engineInertia * Sqr(ratio);
		float engineFrictionTorque = drivetrainParams.engineBaseFriction + rpm * drivetrainParams.engineRPMFriction;
		float engineTorque = (drivetrainParams.CalcEngineTorque(rpm, gear) + Mathf.Abs(engineFrictionTorque)) * throttle;
		slipRatio = 0.0f;		
		
		if (ratio == 0)
		{
			// Neutral gear - just rev up engine
			float engineAngularAcceleration = (engineTorque-engineFrictionTorque) / drivetrainParams.engineInertia;
			engineAngularVelo += engineAngularAcceleration * Time.deltaTime;
			
			// Apply torque to car body
			rigidbody.AddTorque(-drivetrainParams.engineOrientation * engineTorque);
		}
		else
		{
			float drivetrainFraction = 1.0f/poweredWheels.Length;
			float averageAngularVelo = 0;	
			foreach(Wheel w in poweredWheels)
				averageAngularVelo += w.angularVelocity * drivetrainFraction;

			// Apply torque to wheels
			foreach(Wheel w in poweredWheels)
			{
				float tankControlMultiplier = 1 - w.steering * w.side * drivetrainParams.tankControlsFactor;
				w.lazyBrake = Mathf.Clamp01(w.steering * w.side * drivetrainParams.tankControlsFactor) * drivetrainParams.tankLazyWheelBrake;
				float lockingTorque = (averageAngularVelo - w.angularVelocity) * drivetrainParams.differentialLockCoefficient;
				w.drivetrainInertia = inertia * drivetrainFraction;
				w.driveFrictionTorque = engineFrictionTorque * Mathf.Abs(ratio) * drivetrainFraction;
				w.driveTorque = engineTorque * ratio * drivetrainFraction * tankControlMultiplier + lockingTorque;

				slipRatio += w.slipRatio * drivetrainFraction;
			}
			
			// update engine angular velo
			engineAngularVelo = averageAngularVelo * ratio;
		}
		
		// update state
		slipRatio *= Mathf.Sign ( ratio );
		rpm = engineAngularVelo * (60.0f/(2*Mathf.PI));
		
		// very simple simulation of clutch - just pretend we are at a higher rpm.
		float minClutchRPM = drivetrainParams.minRPM;
		//if (gear == 2)
		//	minClutchRPM += throttle * 2000;
		if (rpm < minClutchRPM)
			rpm = minClutchRPM;
			
		// Automatic gear shifting. Bases shift points on rpm.
		if (automatic)
		{
			if(rpm >= drivetrainParams.shiftUpRPM && gear >= 2){
				ShiftUp();
			}
			else if(rpm <= drivetrainParams.shiftDownRPM && gear > 2){
				ShiftDown();
			}
		}
	}
		
	public void ShiftUp () {
		if (gear < drivetrainParams.gearRatios.Length - 1){
			gear ++;
			//Debug.Log("Shift into gear " + gear + " @" + (rigidbody.velocity.magnitude * 3.6f).ToString("f2") + " kph");
		}
	}

	public void ShiftDown () {
		if (gear > 0){
			gear --;
			//Debug.Log("Shift into gear " + gear + " @" + (rigidbody.velocity.magnitude * 3.6f).ToString("f2") + " kph");
		}
	}
	
	public void ShiftReverse(){
		gear = (gear == 0 ? 2 : 0);
		if (gear == 0) {
			SoundMaster.Instance.PlayReverse();
		}
		else {
			SoundMaster.Instance.StopReverse();	
		}
	}
	/*
	void OnGUI(){
		GUI.Label(new Rect(512, 0, 120, 32), (rigidbody.velocity.magnitude * 3.6f).ToString("f2") + " kph");
		GUI.Label(new Rect(512, 32, 200, 32), "Current Gear: " + gear + " RPM " + rpm.ToString("f2"));
		for(int i = 2; i < drivetrainParams.gearRatios.Length; i++){
			float rpmInGear = rpm * drivetrainParams.gearRatios[i] / drivetrainParams.gearRatios[gear];
			if(rpmInGear >= drivetrainParams.shiftUpRPM){
				GUI.color = Color.green;
			}
			if(rpmInGear <= drivetrainParams.shiftDownRPM){
				GUI.color = Color.red;
			}
			GUI.Label(new Rect(512, 32 * i, 200, 32), "Gear " + i + " RPM " + rpmInGear.ToString("f2"));
			GUI.color = Color.white;
		}		
	}
	*/
}