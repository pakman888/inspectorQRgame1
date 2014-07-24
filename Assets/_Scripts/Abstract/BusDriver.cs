using UnityEngine;
using System.Collections;

public partial class BusDriver : MonoBehaviour {
    static BusDriver instance;
    static BusDriver Instance {
        get {
            if (!instance) {
                instance = FindObjectOfType(typeof(BusDriver)) as BusDriver;
            }
            return instance;
        }
    }

	static public void Reset(bool paul) {
		Application.LoadLevel(0);
	}
}	
