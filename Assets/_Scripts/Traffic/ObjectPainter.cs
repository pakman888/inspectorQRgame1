using UnityEngine;
using System.Collections;

public class ObjectPainter : MonoBehaviour {

    Color[] colorOptions = { 
       Color.red,
       Color.blue,
       Color.green,
       Color.yellow,
       Color.white
    };

	// Use this for initialization
	void Start () {
        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null) {
            renderer.material.color = GetRandomColor();
        }   
	}

    Color GetRandomColor() {
        return colorOptions[Random.Range(0, colorOptions.Length )];
    }
}
