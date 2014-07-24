using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SkyBoxGeoScript : MonoBehaviour {
    public GameObject top;
    public GameObject sides;
    public List<Material> materials; 

    void Start() {
        Events.Instance.MissionLoadingStarted += OnMissionStart;
    }

    void OnMissionStart(object o, MissionEventArgs e) {
        int index = e.mission.skyIndexes[0];
        var skyBox = ResourceServer.Instance.skyBoxDefs[index];
        var textureToUseName = skyBox.texture.Replace(".tobj", string.Empty).Trim(); 
        var textureToUse = materials.Where( mat => mat.name == textureToUseName ).FirstOrDefault();
        var topToUseName = skyBox.top.Replace(".tobj", string.Empty).Trim();
        var topToUse = materials.Where( mat => mat.name == topToUseName).FirstOrDefault();
        if (textureToUse != null) {
            sides.renderer.material = textureToUse;
        } else {
            Debug.Log("missing side texture");
            Debug.Log(textureToUseName);
        }
        if (topToUse != null) {
            top.renderer.material = topToUse;
        } else {
            Debug.Log("Missing top texture");
            Debug.Log(topToUseName);
        }
    }
}
