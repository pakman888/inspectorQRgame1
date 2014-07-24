using UnityEngine;
using UnityEditor;
 
public class RacerStyleConfigAsset {
	[MenuItem("Assets/Create/DriveableVehicleData")]
	public static void CreateDriveableVehicleData() {
		ScriptableObjectUtility.CreateAsset<DriveableVehicleData>();
	}
	
	[MenuItem("Assets/Create/WheelData")]
	public static void CreateWheelData() {
		ScriptableObjectUtility.CreateAsset<WheelData>();
	}
	
	[MenuItem("Assets/Create/EngineData")]
	public static void CreateEngineData() {
		ScriptableObjectUtility.CreateAsset<EngineData>();
	}
	
	[MenuItem("Assets/Create/BrakesData")]
	public static void CreateBrakesData() {
		ScriptableObjectUtility.CreateAsset<BrakesData>();
	}
	
	[MenuItem("Assets/Create/TransmissionData")]
	public static void CreateTransmissionData() {
		ScriptableObjectUtility.CreateAsset<TransmissionData>();
	}
	
	[MenuItem("Assets/Create/ChassisData")]
	public static void CreateChassisData() {
		ScriptableObjectUtility.CreateAsset<ChassisData>();
	}
	
	[MenuItem("Assets/Create/SoundData")]
	public static void CreateSoundData() {
		ScriptableObjectUtility.CreateAsset<SoundData>();
	}
}