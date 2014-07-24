using UnityEngine;
using System.Collections;
//engine_data_u

public class EngineData : ScriptableObject {
	/** Base torque constant for generating the engine torque curve. */
    public float torque;
	/** Does the engine power the front (or rear) wheels? */
    public bool front_wheel_engine;  
}
