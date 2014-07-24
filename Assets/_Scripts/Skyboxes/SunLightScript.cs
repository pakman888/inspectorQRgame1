using UnityEngine;
using System.Collections;

public class SunLightScript : MonoBehaviour {

    bool useFog = false;

    void Start() {
        Events.Instance.MissionLoadingStarted += OnMissionLoadStarted;
    }

    void OnMissionLoadStarted(object o, MissionEventArgs e) {
        if (e.mission.sunCount == 0) {
            light.color = Color.white;
            return;
        }
        var index = e.mission.sunIndexes[0];
        var sunItem = ResourceServer.Instance.sunItems[index];
        RenderSettings.ambientLight = VecToColor(sunItem.ambient);
        light.color = VecToColor(sunItem.diffuse);
        transform.rotation = Quaternion.LookRotation(sunItem.sunDirection);
        if (useFog == true) {
            var skyIndex = e.mission.skyIndexes[0];
            var skyItem = ResourceServer.Instance.skyBoxDefs[skyIndex];
            RenderSettings.fog = true;
            RenderSettings.fogColor = VecToColor(skyItem.fogColor);
            RenderSettings.fogDensity = skyItem.fogDensity;
        }

    }

    Color VecToColor( Vector3 vec ) {
        //handle overbright.
        return new Color(Mathf.Clamp01(vec.x / 255f),Mathf.Clamp01( vec.y / 255f), Mathf.Clamp01(vec.z/255f)); 
    }
}