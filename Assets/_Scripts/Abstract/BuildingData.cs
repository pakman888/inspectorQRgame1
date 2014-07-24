using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingData : ScriptableObject {

    public List<BuildingItem> buildingItems;
    public BuildingDefParser buildingsInfo;
    public List<NodeItem> allNodesList;
}
