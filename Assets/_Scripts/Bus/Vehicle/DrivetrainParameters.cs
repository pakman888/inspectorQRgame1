using UnityEngine;
using System.Collections;

public class DrivetrainParameters : ScriptableObject {

	// The gear ratios, including neutral (0) and reverse (negative) gears
	public float[] gearRatios;
	
	//Hacked in torque multipliers for various gears
	public float[] torqueRatio; 
	
	// The final drive ratio, which is multiplied to each gear ratio
	public float finalDriveRatio = 3.23f;
	
	// The engine's torque curve characteristics. Since actual curves are often hard to come by,
	// we approximate the torque curve from these values instead.

	// powerband RPM range
	public float minRPM = 800;
	public float maxRPM = 6400;

	// engine's maximal torque (in Nm) and RPM.
	public float maxTorque = 664;
	public float torqueRPM = 4000;

	// engine's maximal power (in Watts) and RPM.
	public float maxPower = 317000;
	public float powerRPM = 5000;

	// engine inertia (how fast the engine spins up), in kg * m^2
	public float engineInertia = 0.3f;
	
	// engine's friction coefficients - these cause the engine to slow down, and cause engine braking.

	// constant friction coefficient
	public float engineBaseFriction = 25f;
	// linear friction coefficient (higher friction when engine spins higher)
	public float engineRPMFriction = 0.02f;

	// Engine orientation (typically either Vector3.forward or Vector3.right). 
	// This determines how the car body moves as the engine revs up.	
	public Vector3 engineOrientation = Vector3.forward;
	
	// Coefficient determining how much torque is transfered between the wheels when they move at 
	// different speeds, to simulate differential locking.
	public float differentialLockCoefficient = 0;
	
	//RPM to automatically up/downshift at
	public float shiftUpRPM = 3000, shiftDownRPM = 2000;
	
	//Proportion of power shifted to opposite side drive wheels when steering
	//Not physically realistic but improves turning at slow speeds
	public float tankControlsFactor = 0;
	//Brakes applied to the underpowered drivewheel(s)
	public float tankLazyWheelBrake = 0;
	
	private float Sqr(float x){
		return x * x;
	}
	
	// Calculate engine torque for current rpm and throttle values.
	public float CalcEngineTorque (float rpm, int gear) 
	{
		float result;
		if(rpm < torqueRPM)
			result = maxTorque*(-Sqr(rpm / torqueRPM - 1) + 1);
		else {
			float maxPowerTorque = maxPower/(powerRPM*2*Mathf.PI/60);
			float aproxFactor = (maxTorque-maxPowerTorque)/(2*torqueRPM*powerRPM-Sqr(powerRPM)-Sqr(torqueRPM));
			float torque = aproxFactor * Sqr(rpm-torqueRPM)+maxTorque;
			result=torque>0?torque:0;
		} 
		if(rpm > maxRPM)
		{
			result *= 1-((rpm-maxRPM) * 0.006f);
			if(result<0)
				result=0;
		}
		if(rpm<0)
			result=0;
		return result * torqueRatio[gear];
	}
}
