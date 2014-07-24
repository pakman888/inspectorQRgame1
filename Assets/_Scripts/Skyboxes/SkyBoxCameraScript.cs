using UnityEngine;
using System.Collections;

public class SkyBoxCameraScript : MonoBehaviour {

    public float yVal;
    public GameObject skybox;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnPreRender() {
        var newPos = transform.position;
        skybox.transform.position = new Vector3(newPos.x, yVal, newPos.z);
    }
}
