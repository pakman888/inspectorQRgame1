using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MissionModelItem : Item {
    public int modelId;
    public int model_look_id;
    public int index;
    public Vector3 rotation;
    public Vector3 scale;
    public int missionCount;
    public List<string> missionList;
}
