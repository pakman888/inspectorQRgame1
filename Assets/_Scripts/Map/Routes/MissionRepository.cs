using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MissionRepository : DerivedSingleton<MissionRepository> {
	public RouteItem[] tiers;
	
	public RouteItem GetRoute(int tierIndex, int missionIndex) {
		return tiers[6 * tierIndex + missionIndex];
	}
	
	#if UNITY_EDITOR
	public static void CreateOrUpdateMissionRepository() {
		CreateAndSave();
	}
	
	protected override void OnCreate() {
		tiers = new RouteItem[36];
		PopulateRepository();	
	}
	
	void PopulateRepository() {
		var routeTargetFolder = "Assets/GameDataPersistent/Missions/";
		var dir = Directory.GetCurrentDirectory() + "/" + routeTargetFolder;
        foreach (string file in Directory.GetFiles(dir)) {
			if (!file.Contains(".meta")) {
				var filename = routeTargetFolder + new FileInfo(file).Name;
				Debug.Log (filename);
				RouteItem route = (RouteItem)AssetDatabase.LoadMainAssetAtPath(filename);
				tiers[6 * route.tier + route.rank] = route;
			}
        }
	}
	#endif
}
