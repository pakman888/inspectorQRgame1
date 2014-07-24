using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SceneDisabler{

	[MenuItem("Debug/DisableAllScenes")]
	public static void DisableAllScenes(){
		SetAllScenesEnabled(false);
	}
	
	[MenuItem("Debug/EnableAllScenes")]
	public static void EnableAllScenes(){
		SetAllScenesEnabled(true);
	}
	
	public static void SetAllScenesEnabled(bool enabled){
		List<EditorBuildSettingsScene> sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
		foreach(EditorBuildSettingsScene scene in sceneList){
			scene.enabled = enabled;
		}
		EditorBuildSettings.scenes = sceneList.ToArray();
	}
}
