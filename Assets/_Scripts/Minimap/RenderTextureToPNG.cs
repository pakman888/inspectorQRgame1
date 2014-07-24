using UnityEngine;
using System.IO;
using System.Collections;

public class RenderTextureToPNG : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.LookRotation(Vector3.down);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void WriteToPNG() {

        var camera = GetComponent<Camera>();
        if (camera == null) {
            Debug.LogError("No Camera");
            return;
        }

        var renderTexture = camera.targetTexture;
        if (renderTexture == null) {
            Debug.LogError("No attached render texture");
            return;
        }
        RenderTexture.active = renderTexture;

        Texture2D textured2D = new Texture2D(2048, 2048);
        textured2D.ReadPixels(new Rect(0, 0, 2048, 2048), 0, 0);
        textured2D.Apply();

        byte[] bytes = textured2D.EncodeToPNG();
        Debug.Log("trying to writ to " + Application.dataPath + "/SavedScreen");
        File.WriteAllBytes(Application.dataPath + "/SavedScreen.png", bytes);

    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 40, 40), "button")) {
            WriteToPNG();
        }

    }
}
