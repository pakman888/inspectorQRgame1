using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(RoadComponent))]
[CanEditMultipleObjects]
public class RoadComponentInspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
	}
	
	void OnSceneGUI(){
		RoadComponent road = target as RoadComponent;
		Handles.color = Color.white;
		foreach(NodeItem node in road.Nodes){
			Handles.ArrowCap(0, node.position, Quaternion.FromToRotation(Vector3.forward, node.direction), 3);
		}
	}
}
