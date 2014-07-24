using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class MoverItem : Item {
    public int modelId;
    public float speed;
    public float endDelay;
    public int lengthsCount;
    public List<float> lengthsList;
    public List<int> nodeIndices;
}
