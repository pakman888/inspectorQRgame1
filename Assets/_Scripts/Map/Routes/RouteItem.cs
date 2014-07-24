using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RouteItem : ScriptableObject {
    public string routeName;
    public string controllerType;
    public string longDescription;
    public string shortDescription;
    public string mapName;
    public int timeLimit;
    public int bonusTimeLimit;
    public int sunCount;
    public List<int> sunIndexes;
    public int skyCount;
    public List<int> skyIndexes;
    public int tier;
    public int rank;
    public int overlayOffsetX;
    public int overlayOffsetY;
    public string vehicle;
	public GameObject vehiclePrefab;
    public string timeTableName;
    public int iconCount;
    public List<string> iconsList;
    public int busStopCamerasCount;
    public List<PosRot> busStopCameras;
    public int enteringPassengersCount;
    public List<int> enteringPassengersList;
    public int leavingPassengersCount;
    public List<int> leavingPassengersList;
    public int drivingTimesCount;
    public List<int> drivingTimesList;
    public int stopTimesCount;
    public List<int> stopTimesList;
    public int passengerCount;
    public List<string> passengerList;
    public List<GameObject> passengerModels;
    public List<int> orderedNodeIndexList;
    public Vector3 startPos;

    public RouteItem() {
        sunIndexes = new List<int>();
        skyIndexes = new List<int>();
        iconsList = new List<string>();
		busStopCameras = new List<PosRot>();
        enteringPassengersList = new List<int>();
        leavingPassengersList = new List<int>();
        drivingTimesList = new List<int>();
        stopTimesList = new List<int>();
        passengerList = new List<string>();
        orderedNodeIndexList = new List<int>();
        startPos = new Vector3();
    } 
}

[System.Serializable]
public class PosRot {
	public Vector3 pos;
	public Quaternion rot;
	
	public PosRot(Vector3 position, Quaternion rotation) {
		pos = position;
		rot = rotation;
	}
}