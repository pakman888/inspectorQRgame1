using UnityEngine;
using System.Collections;

public class ModelComponent : MonoBehaviour {

    public ModelItem modelItem;
    public NodeItem node;
    // Use this for first everythings
    void Awake() {
        if (InRange(modelItem.modelLookIndex, 200, 204)) {
            gameObject.SetActive(false);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    static bool InRange(int val, int min, int max) {
        return (val >= min && val <= max);
    }
}
