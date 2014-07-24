using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameCameraManager : Singleton<GameCameraManager> {
	public List<Camera> busStopCameras;
	public FollowCamera gameCamera;
    public GameObject skybox;
	
	void Start() {
		Events.Instance.MissionLoaded += (src, e) => SpawnBusStopCameras();
		
		Events.Instance.SwitchedStateContext += (object src, StateContextEventArgs e) => { 
			if (e.stateContext == StateContext.Menu) OnMenuLoaded();
			else if (e.stateContext == StateContext.Game) gameCamera.gameObject.SetActive(true);
		};
		
		Events.Instance.BusInstantiated += OnBusInstantiated;
		Events.Instance.StoppedAtBusStop += (src, e) => EnableCurrentBusStopCamera(e);
		Events.Instance.BusStopCompleted += (src, e) => DisableCurrentBusStopCamera(e);
	}
	
	public void OnBusInstantiated(object sender, BusEventArgs e) {
        var skyBoxScript = gameCamera.gameObject.AddComponent<SkyBoxCameraScript>();
        skyBoxScript.skybox = skybox;
        skyBoxScript.yVal = 93;
        foreach ( var stop in busStopCameras ) {
            var skyBoxScriptStop = stop.gameObject.AddComponent<SkyBoxCameraScript>();
            skyBoxScriptStop.yVal = 93;
            skyBoxScriptStop.skybox = skybox;
        }

		gameCamera.target = e.bus.GetComponent<Drivetrain>();
		gameCamera.distance = e.bus.cameraDistance;
		gameCamera.blinkerFollow = e.bus.blinkerCameraDistance;
		gameCamera.height = e.bus.cameraHeight;
		gameCamera.transform.position = e.bus.transform.position;
		gameCamera.ApplyTransformationImmediately();
	}
	
	public void OnMenuLoaded() {
		foreach (var cam in busStopCameras) {
			Destroy(cam.gameObject);	
		}
		busStopCameras.Clear();
		gameCamera.target = null;
		gameCamera.gameObject.SetActive(false);
	}
		
	void EnableCurrentBusStopCamera(BusStopEventArgs e) {
		busStopCameras[e.busStop.timeTableIndex].gameObject.SetActive(true);
	}
	
	void DisableCurrentBusStopCamera(BusStopEventArgs e) {
		busStopCameras[e.busStop.timeTableIndex].gameObject.SetActive(false);
	}
	
	void SpawnBusStopCameras() {
		var index = 0;
		foreach (var posRot in MissionHandler.Instance.route.busStopCameras) {
			var cam = new GameObject();
			cam.name = "BusStopCamera" + index;
			cam.AddComponent<Camera>();
			cam.transform.position = posRot.pos;
			cam.transform.localRotation = posRot.rot;
			cam.SetActive(false);
			busStopCameras.Add(cam.GetComponent<Camera>());
			index++;
		}
	}
}
