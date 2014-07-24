using UnityEngine;
using System.Collections;

public class MissionModelItemComponent : MonoBehaviour {
    
    public MissionModelItem missionModelItem;
    public bool onRoute;
    public RouteItem missionRoute; 

    void Awake() {

        missionRoute = MissionHandler.Instance.route;
        if (missionRoute == null) {
            return;
        }
        if (IsUsedInCurrentMission()) {
            CheckArrows();
            onRoute = true;
            /* we can always reenable this but I wrote more specialized function in CheckArrows
			foreach (var collider in GetComponentsInChildren<Collider>()) {
				collider.enabled = false;
			}
            */
        } else {
            onRoute = false;
            DisableThisMissionItem();
        }
    }

    bool IsUsedInCurrentMission() {
        foreach (var missionName in missionModelItem.missionList) {
            if ( missionRoute.routeName.Contains(missionName)){
                return true;
            }
        }
        return false;
    }

    void DisableThisMissionItem() {
            gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
        // should update the arrow animation here. 	
	}

    static bool InRange(int val, int min, int max) {
        return (val >= min && val <= max);
    }

    void CheckArrows() {
        if (InRange(missionModelItem.modelId, 200, 204)) {
            foreach ( Transform child in transform ) {
                var collider = child.gameObject.collider;
                if (collider != null) {
                    Destroy(collider);
                }
                child.transform.Translate(Vector3.up * 0.05f);
            }
        }
    }
}