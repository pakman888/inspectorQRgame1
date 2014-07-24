using UnityEngine;
using System.Collections;

public class WheelParameters : ScriptableObject {

	// Wheel radius in meters
	public float radius = 0.34f;
	// Wheel suspension travel in meters
	public float suspensionTravel = 0.2f;
	// Damper strength in kg/s
	public float damping = 5000;
	// Wheel angular inertia in kg * m^2
	public float inertia = 2.2f;
	// Maximal braking torque (in Nm)
	public float brakeFrictionTorque = 4000;
	// Maximal handbrake torque (in Nm)
	public float handbrakeFrictionTorque = 0;
	// Base friction torque (in Nm)
	public float frictionTorque = 10;
	// Maximal steering angle (in degrees)
	public float maxSteeringAngle = 28f;
	// Fraction of the car's mass carried by this wheel
	public float massFraction = 0.25f;
	// Pacejka coefficients
	public float[] a={1.0f,-60f,1688f,4140f,6.026f,0f,-0.3589f,1f,0f,-6.111f/1000f,-3.244f/100f,0f,0f,0f,0f};
	public float[] b={1.0f,-60f,1588f,0f,229f,0f,0f,0f,-10f,0f,0f};
	//slip ratio below which ABS kicks in (-1 = no ABS)
	public float antilockBrakeThreshold = -0.03f;

	private bool slipMaximaCalculated;
	private float maxSlip{
		get{
			if(!slipMaximaCalculated){
				InitSlipMaxima();
			}
			return _maxSlip;
		}
	}
	private float _maxSlip;
	
	private float maxAngle{
		get{
			if(!slipMaximaCalculated){
				InitSlipMaxima();
			}
			return _maxAngle;
		}
	}
	private float _maxAngle;
	
	public float CalcLongitudinalForce(float Fz,float slip)
	{
		Fz*=0.001f;//convert to kN
		slip*=100f; //covert to %
		float uP=b[1]*Fz+b[2];
		float D=uP*Fz;	
		float B=((b[3]*Fz+b[4])*Mathf.Exp(-b[5]*Fz))/(b[0]*uP);
		float S=slip+b[9]*Fz+b[10];
		float E=b[6]*Fz*Fz+b[7]*Fz+b[8];
		float Fx=D*Mathf.Sin(b[0]*Mathf.Atan(S*B+E*(Mathf.Atan(S*B)-S*B)));
		return Fx;
	}
	
	public float CalcLateralForce(float Fz,float slipAngle)
	{
		Fz*=0.001f;//convert to kN
		slipAngle*=(360f/(2*Mathf.PI)); //convert angle to deg
		float uP=a[1]*Fz+a[2];
		float D=uP*Fz;
		float B=(a[3]*Mathf.Sin(2*Mathf.Atan(Fz/a[4])))/(a[0]*uP*Fz);
		float S=slipAngle+a[9]*Fz+a[10];
		float E=a[6]*Fz+a[7];
		float Sv=a[12]*Fz+a[13];
		float Fy=D*Mathf.Sin(a[0]*Mathf.Atan(S*B+E*(Mathf.Atan(S*B)-S*B)))+Sv;
		return Fy;
	}
	
	public float CalcLongitudinalForceUnit(float Fz,float slip)	{
		if(!slipMaximaCalculated){
			InitSlipMaxima();
		}
		return CalcLongitudinalForce(Fz,slip*maxSlip);
	}
	
	public float CalcLateralForceUnit(float Fz,float slipAngle) {
		if(!slipMaximaCalculated){
			InitSlipMaxima();
		}
		return CalcLongitudinalForce(Fz,slipAngle*maxAngle);
	}
	
	public Vector3 CombinedForce(float Fz,float slip,float slipAngle, Vector3 localVelo, Vector3 groundNormal)
	{
		float unitSlip = slip/maxSlip;
		float unitAngle = slipAngle/maxAngle;
		float p = Mathf.Sqrt(unitSlip*unitSlip + unitAngle*unitAngle);
		if(p > Mathf.Epsilon)
		{
			if (slip < -0.8f)
				return -localVelo.normalized * (Mathf.Abs(unitAngle/p * CalcLateralForceUnit(Fz,p)) + Mathf.Abs(unitSlip/p * CalcLongitudinalForceUnit(Fz,p)));
			else
			{
				Vector3 forward = new Vector3( 0, -groundNormal.z, groundNormal.y);
				return Vector3.right * unitAngle/p * CalcLateralForceUnit(Fz,p) + forward * unitSlip/p * CalcLongitudinalForceUnit(Fz,p);
			}
		}
		else
			return Vector3.zero;
	}
	
	public void InitSlipMaxima()
	{
		const float stepSize = 0.001f;
		const float testNormalForce = 4000f;
		float force = 0;
		for (float slip = stepSize;;slip += stepSize)
		{
			float newForce = CalcLongitudinalForce(testNormalForce,slip);
			if (force<newForce)
				force = newForce;
			else {
				_maxSlip = slip-stepSize;
				break;
			}
		}
		force = 0;
		for (float slipAngle = stepSize;;slipAngle += stepSize)
		{
			float newForce = CalcLateralForce(testNormalForce,slipAngle);
			if (force<newForce)
				force = newForce;
			else {
				_maxAngle = slipAngle-stepSize;
				break;
			}
		}
		slipMaximaCalculated = true;
	}
}
