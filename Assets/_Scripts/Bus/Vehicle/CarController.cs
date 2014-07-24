using UnityEngine;
using System.Collections;

// This class is repsonsible for controlling inputs to the car.
// Change this code to implement other input types, such as support for analogue input, or AI cars.
[RequireComponent (typeof (Drivetrain))]
public class CarController : MonoBehaviour {		
	// Add all wheels of the car here, so brake and steering forces can be applied to them.
	public Wheel[] wheels;
	
	// A transform object which marks the car's center of gravity.
	// Cars with a higher CoG tend to tilt more in corners.
	// The further the CoG is towards the rear of the car, the more the car tends to oversteer. 
	// If this is not set, the center of mass is calculated from the colliders.
	public Transform centerOfMass;

	// A factor applied to the car's inertia tensor. 
	// Unity calculates the inertia tensor based on the car's collider shape.
	// This factor lets you scale the tensor, in order to make the car more or less dynamic.
	// A higher inertia makes the car change direction slower, which can make it easier to respond to.
	public float inertiaFactor = 1.5f;
	
	// current input state
	float brake;
    public float Brake { get { return brake; } }
	float throttle;
	float throttleInput;
	float steering;
	float lastShiftTime = -1;
	float handbrake;
	float finalSteering;
	int steeringFingerId = -1;
	Vector2 steeringTouchStart;
	
	public float Throttle { 
		get { return throttle; }
	}
		
	// cached Drivetrain reference
	Drivetrain drivetrain;
	
	// How long the car takes to shift gears
	public float shiftSpeed = 0.8f;
	

	// These values determine how fast throttle value is changed when the accelerate keys are pressed or released.
	// Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.
	// There are different values for when the wheels have full traction and when there are spinning, to implement 
	// traction control schemes.
		
	// How long it takes to fully engage the throttle
	public float throttleTime = 1.0f;
	// How long it takes to fully engage the throttle 
	// when the wheels are spinning (and traction control is disabled)
	public float throttleTimeTraction = 10.0f;
	// How long it takes to fully release the throttle
	public float throttleReleaseTime = 0.5f;
	// How long it takes to fully release the throttle 
	// when the wheels are spinning.
	public float throttleReleaseTimeTraction = 0.1f;

	// Turn traction control on or off
	public bool tractionControl = true;
	
	
	// These values determine how fast steering value is changed when the steering keys are pressed or released.
	// Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.
	
	// How long it takes to fully turn the steering wheel from center to full lock
	public float steerTime = 1.2f;
	// This is added to steerTime per m/s of velocity, so steering is slower when the car is moving faster.
	public float veloSteerTime = 0.1f;

	// How long it takes to fully turn the steering wheel from full lock to center
	public float steerReleaseTime = 0.6f;
	// This is added to steerReleaseTime per m/s of velocity, so steering is slower when the car is moving faster.
	public float veloSteerReleaseTime = 0f;
	// When detecting a situation where the player tries to counter steer to correct an oversteer situation,
	// steering speed will be multiplied by the difference between optimal and current steering times this 
	// factor, to make the correction easier.
	public float steerCorrectionFactor = 4.0f;
	
	//Steering sensitivity for smudge controls
	public float touchSteerSensitivity = 0.8f;

	// Used by SoundController to get average slip velo of all wheels for skid sounds.
	public float slipVelo {
		get {
			float val = 0.0f;
			foreach(Wheel w in wheels)
				val += w.slipVelo / wheels.Length;
			return val;
		}
	}

	// Initialize
	void Start () 
	{
		if (centerOfMass != null)
			rigidbody.centerOfMass = centerOfMass.localPosition;
		rigidbody.inertiaTensor *= inertiaFactor;
		drivetrain = GetComponent (typeof (Drivetrain)) as Drivetrain;
	}
	
	bool gasButtonPressed;
	public void OnGasButton(bool pressedOrReleased) {
		gasButtonPressed = pressedOrReleased;
	}
	
	bool brakeButtonPressed;
	bool brakeBegan;
	public void OnBrakeButton(bool pressedOrReleased) {
		brakeButtonPressed = pressedOrReleased;
	}
	
	bool leftButtonPressed;
	public void OnLeftButton(bool pressedOrReleased) {
		leftButtonPressed = pressedOrReleased;
	}
	
	bool rightButtonPressed;
	public void OnRightButton(bool pressedOrReleased) {
		rightButtonPressed = pressedOrReleased;
	}
	
	void HandleKeyboardInput() {		
		gasButtonPressed = Input.GetKey (KeyCode.UpArrow);
		brakeButtonPressed = Input.GetKey (KeyCode.DownArrow);		
		brakeBegan = Input.GetKeyDown(KeyCode.DownArrow);
		
		//Swap "accelerate" and "brake" controls when in reverse gear
		if (drivetrain.automatic && drivetrain.gear == 0)
		{
			gasButtonPressed = Input.GetKey(KeyCode.DownArrow);
			brakeButtonPressed = Input.GetKey(KeyCode.UpArrow);
			brakeBegan = Input.GetKeyDown(KeyCode.UpArrow);
		}
		
		leftButtonPressed = Input.GetKey(KeyCode.LeftArrow);
		rightButtonPressed = Input.GetKey(KeyCode.RightArrow);
		
		// Steering
		Vector3 carDir = transform.forward;
		float fVelo = rigidbody.velocity.magnitude;
		Vector3 veloDir = rigidbody.velocity * (1/fVelo);
		float angle = -Mathf.Asin(Mathf.Clamp( Vector3.Cross(veloDir, carDir).y, -1, 1));
		float optimalSteering = angle / (wheels[0].maxSteeringAngle * Mathf.Deg2Rad);
		if (fVelo < 1)
			optimalSteering = 0;
		
		float steerInput = 0;
		if (leftButtonPressed) steerInput = -1;
		else if (rightButtonPressed) steerInput = 1;
		
		if (steerInput < steering)
		{
			float steerSpeed = (steering>0) ? 
								  (1 / (steerReleaseTime + veloSteerReleaseTime*fVelo))
								: (1 / (steerTime + veloSteerTime*fVelo));
			if (steering > optimalSteering)
				steerSpeed *= 1 + (steering-optimalSteering) * steerCorrectionFactor;
			steering -= steerSpeed * Time.deltaTime;
			if (steerInput > steering)
				steering = steerInput;
		}
		else if (steerInput > steering)
		{
			float steerSpeed = (steering<0) ?
								  (1/(steerReleaseTime + veloSteerReleaseTime * fVelo)) 
								: (1/(steerTime + veloSteerTime * fVelo));
			if (steering < optimalSteering)
				steerSpeed *= 1 + (optimalSteering-steering) * steerCorrectionFactor;
			steering += steerSpeed * Time.deltaTime;
			if (steerInput < steering)
				steering = steerInput;
		}
		
		//Piecewise steering -- extended range for ideal wheel slip angle
		float idealSteeringWheelSlipAngle = 1/6f; //Magnic number that produces "good" steering
		if(steering > 0){
			if(steering <= idealSteeringWheelSlipAngle){
				finalSteering = steering;
			}
			else if(steering > idealSteeringWheelSlipAngle && steering < 0.75f){
				finalSteering = idealSteeringWheelSlipAngle;
			}
			else{
				finalSteering = Mathf.Lerp(idealSteeringWheelSlipAngle, 1, (steering - 0.75f) * 4);
			}
		}
		else if(steering < 0){
			if(steering >= -idealSteeringWheelSlipAngle){
				finalSteering = steering;
			}
			else if(steering < -idealSteeringWheelSlipAngle && steering > -0.75f){
				finalSteering = -idealSteeringWheelSlipAngle;
			}
			else{
				finalSteering = Mathf.Lerp(-idealSteeringWheelSlipAngle, -1, (-steering - 0.75f) * 4);
			}
		}
		else{
			finalSteering = 0;
		}
	}
	
	void HandleTouchscreenInput() {
		gasButtonPressed = HUD.Instance.GetButton("Gas");
		brakeButtonPressed = HUD.Instance.GetButton("Brake");
		brakeBegan = HUD.Instance.GetButtonDown("Brake");
			
		//Swap "accelerate" and "brake" controls when in reverse gear
		if (drivetrain.automatic && drivetrain.gear == 0) {
			gasButtonPressed = HUD.Instance.GetButton("Brake");
			brakeButtonPressed = HUD.Instance.GetButton("Gas");
			brakeBegan = HUD.Instance.GetButtonDown("Gas");
		}
		
		//Steering Zone
		Touch[] touches = Input.touches;
		Camera uiCam = UICamera.mainCamera;
		Vector3[] steerZoneWorldCorners = HUD.Instance.steeringZone.worldCorners;
		Vector3 topRightSteerZoneScreenCorner = uiCam.WorldToScreenPoint(steerZoneWorldCorners[2]);
		Vector3 bottomLeftSteerZoneScreenCorner = uiCam.WorldToScreenPoint(steerZoneWorldCorners[0]);
		Rect steerZoneScreenRect = Rect.MinMaxRect(bottomLeftSteerZoneScreenCorner.x,
													bottomLeftSteerZoneScreenCorner.y,
													topRightSteerZoneScreenCorner.x,
													topRightSteerZoneScreenCorner.y);
		float steerInput = 0;
		for(int i = 0; i < touches.Length; i++){
			//No fingerId assigned to steering: Check for touch began in zone
			if(steeringFingerId < 0 && touches[i].phase == TouchPhase.Began && steerZoneScreenRect.Contains(touches[i].position)){
				steeringFingerId = touches[i].fingerId;
				steeringTouchStart = touches[i].position;
			}
			//Finger id assigned: monitor touch
			if(steeringFingerId >= 0){
				if(touches[i].fingerId != steeringFingerId){
					continue;
				}
				else if(touches[i].phase == TouchPhase.Ended || touches[i].phase == TouchPhase.Canceled){
					steeringFingerId = -1;
				}
				else{
					steerInput = Mathf.Clamp((touches[i].position.x - steeringTouchStart.x) * 2 * touchSteerSensitivity/steerZoneScreenRect.width, -1, 1);
				}
			}
		}
		steering = finalSteering = steerInput;
	}
	
	void Update () 
	{
		// Extra key bindings
		if (Input.GetKeyDown(KeyCode.O)) {
			GetComponent<Bus>().OnDoorsButtonPressed();	
		}
		if (Input.GetKeyDown(KeyCode.LeftBracket)) {
			GetComponent<Bus>().OnTurnSignalButtonPressed(TurnSignal.left);		
		}
		if (Input.GetKeyDown(KeyCode.RightBracket)) {
			GetComponent<Bus>().OnTurnSignalButtonPressed(TurnSignal.right);	
		}
		
#if UNITY_EDITOR
		HandleKeyboardInput();
#else
		HandleTouchscreenInput();
#endif

		if (gasButtonPressed)
		{
			if (drivetrain.slipRatio < 0.10f)
				throttle += Time.deltaTime / throttleTime;
			else if (!tractionControl)
				throttle += Time.deltaTime / throttleTimeTraction;
			else
				throttle -= Time.deltaTime / throttleReleaseTime;

			if (throttleInput < 0)
				throttleInput = 0;
			throttleInput += Time.deltaTime / throttleTime;
			brake = 0;
		}
		else 
		{
			if (drivetrain.slipRatio < 0.2f)
				throttle -= Time.deltaTime / throttleReleaseTime;
			else
				throttle -= Time.deltaTime / throttleReleaseTimeTraction;
		}
		throttle = Mathf.Clamp01 (throttle);

		if (brakeButtonPressed)
		{
			if (drivetrain.slipRatio < 0.2f)
				brake += Time.deltaTime / throttleTime;
			else
				brake += Time.deltaTime / throttleTimeTraction;
			throttle = 0;
			throttleInput -= Time.deltaTime / throttleTime;
		}
		else 
		{
			if (drivetrain.slipRatio < 0.2f)
				brake -= Time.deltaTime / throttleReleaseTime;
			else
				brake -= Time.deltaTime / throttleReleaseTimeTraction;
		}
		brake = Mathf.Clamp01 (brake);
		HUD.Instance.brakeMeter.UpdateDisplay(brake);
		
		throttleInput = Mathf.Clamp (throttleInput, -1, 1);
		
		// Gear shifting
		float shiftThrottleFactor = Mathf.Clamp01((Time.time - lastShiftTime)/shiftSpeed);
		drivetrain.throttle = throttle * shiftThrottleFactor;
		drivetrain.throttleInput = throttleInput;
		if(drivetrain.CanReverse && brakeBegan) {
			drivetrain.ShiftReverse();
		}
		if (!drivetrain.InReverse) { SoundMaster.Instance.StopReverse(); }

		// Apply inputs
		foreach(Wheel w in wheels)
		{
			w.brake = brake;
			w.steering = finalSteering;
		}
	}
	
/*
	public GUISkin debugGuiSkin;
	private void OnGUI(){
		GUI.Label(new Rect(512, 0, 120, 32), "Steering " + finalSteering);
		GUI.skin = debugGuiSkin;
		float angleScale = 3;
		float slipScale = 300;
		float refSlipAngle = 10; //degrees
		float refSlipRatio = 0.1f;
		for(int i = 0; i < wheels.Length; i++){
			Wheel w = wheels[i];
			//Slip angle bar
			GUI.color = Color.green;
			float barHeight = Mathf.Abs(w.slipAngle * Mathf.Rad2Deg * angleScale);
			GUI.Label(new Rect(640 + 10 * i, 140 - barHeight, 4, barHeight), "", "test_touch");			
			//Slip % bar
			GUI.color = Color.white;
			barHeight = Mathf.Abs(w.peakSlipRatio * slipScale);
			GUI.Label(new Rect(640 + 10 * i + 4, 140 - barHeight, 4, barHeight), "", "test_touch");
		}
		GUI.color = Color.white;
		GUI.Label(new Rect(638, 140, wheels.Length * 10 + 2, 2), "", "test_touch");
		GUI.color = Color.green;
		GUI.Label(new Rect(638, 140 - refSlipAngle * angleScale, wheels.Length * 10 + 2, 1), "", "test_touch");
		GUI.color = Color.white;
		GUI.Label(new Rect(638, 140 - refSlipRatio * slipScale, wheels.Length * 10 + 2, 1), "", "test_touch");
	}
	*/
}