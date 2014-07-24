using UnityEngine;
using System.Collections;

public class SetRoadLayer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        var stuff =(GameObject[]) FindObjectsOfType(typeof(GameObject));
        Debug.Log(stuff.Length);
        foreach (var thing in stuff) {
            if (thing.renderer != null) {
                var mat = thing.renderer.sharedMaterial;
                if (mat != null) {
                    Texture tex = mat.GetTexture("_MainTex");
                    if (tex.name.Contains("road")) {
                        Debug.Log(tex.name);
                        thing.layer = 10;
                        Debug.Log("set");
                    }
                }
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
