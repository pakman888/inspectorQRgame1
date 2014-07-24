using UnityEngine;
using System.Collections;

public class MapHandler : Singleton<MapHandler> {
    public Camera mapCamera;
    
    public GameObject floatingBusToken;
	public Vector3 floatingBusTokenOffset;
    public GameObject currentStopToken;
	public Vector3 currentStopTokenOffset;
    public GameObject nextStopArrow;

    Vector3 stopPosition;
    public bool showMap;

    public GameObject targetToFollow;

	// Use this for initialization
	void Start () {
		Events.Instance.BusInstantiated += new BusEventHandler(OnBusInstantiated);
        Events.Instance.StoppedAtBusStop += new BusStopEventHandler(OnStoppedAtBusStop);
	}

    void OnBusInstantiated(object sender, BusEventArgs e) {
        targetToFollow = MissionHandler.Instance.bus.gameObject;
        SetNextStop(1);
        SetUpCamera();
    }

    void SetNextStop(int nextStop) {
        var stopUID = 
            MissionHandler.Instance.route.orderedNodeIndexList[nextStop];
        var nodeIndex = 
            ResourceServer.Instance.stopUIDsToNodeIndices[stopUID];
        stopPosition =  ResourceServer.Instance.nodes[nodeIndex].position;
        currentStopToken.transform.position = stopPosition;
        currentStopToken.transform.position += Vector3.up * 50 + currentStopTokenOffset;
        currentStopToken.SetActive(true);
    }

    void OnStoppedAtBusStop(object sender, BusStopEventArgs e) {
        currentStopToken.SetActive(false);
        var nextStop = MissionHandler.Instance.currentStopIndex + 1;
        if (nextStop < MissionHandler.Instance.route.orderedNodeIndexList.Count) {
            SetNextStop(nextStop);
        }
    }

	// Update is called once per frame
	void Update () {
        if (targetToFollow != null) {
            MoveFollowToken();
            MoveCamera();
            DisplayNextStopArrow();
        }
	}

    void DisplayNextStopArrow() {
        var stopVector = stopPosition -targetToFollow.transform.position;
        if (stopVector.magnitude > mapCamera.orthographicSize) {
            nextStopArrow.SetActive(true);
            var stopDirectionNormalized = stopVector.normalized;
            nextStopArrow.transform.rotation = Quaternion.LookRotation(stopDirectionNormalized) ;
            nextStopArrow.transform.Rotate(Vector3.right, 90f);
            nextStopArrow.transform.position = ((targetToFollow.transform.position + stopDirectionNormalized * (mapCamera.orthographicSize * 2 / 3)) + Vector3.up * 100);
        } else {
            nextStopArrow.SetActive(false);
        }

    }

    void SetUpCamera() {
        mapCamera.transform.rotation = Quaternion.LookRotation(Vector3.down);
        MoveCamera();
        MoveFollowToken();
    }

    void MoveFollowToken() {
        floatingBusToken.transform.position = targetToFollow.transform.position + (Vector3.up * 50)  + floatingBusTokenOffset;
        //floatingBusToken.transform.rotation = targetToFollow.transform.rotation;
        //floatingBusToken.transform.Rotate(Vector3.right, 90.0f);
    }

    void MoveCamera() {
        mapCamera.transform.position = targetToFollow.transform.position;
        mapCamera.transform.position += Vector3.up * 200;
    }
}