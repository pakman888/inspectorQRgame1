using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	void OnWillRenderObject(){
		transform.rotation = Camera.current.transform.rotation;
	}

}
