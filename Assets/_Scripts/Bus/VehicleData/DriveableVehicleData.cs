using UnityEngine;
using System.Collections;

[System.Serializable]
public class DriveableVehicleData : BaseVehicleData {
		public ChassisData chasis_data;  
        public BrakesData brakes_data;
        public EngineData engine_data;
        public TransmissionData transmission_data;
        public WheelData wheel_data;
	
        public SoundData sdata_int;
        public SoundData sdata_ext;
	
        public GameObject fwheel_model;
        public GameObject wipers;
		public GameObject door_open;
        public GameObject door_close;
	
		public Camera camera_behind;
        public Camera camera_front;
        public Camera camera_door;
        public Camera camera_start;   
        public Vector3 fwheel_positions;   
        public int  vehicle_class; 
        public Vector3  driver_pos;    
        public int capacity_sitting;
        public int capacity_standing;
        public Vector3[] interior_mirror_left; 
        public Vector3[] interior_mirror_right;

		// This value will move the game camera more to the vehicle 
        public float camera_Z_coef;      
        public float camera_Y_ofs;
        public Transform   camera_front_placement;

        public int capacity;
}
