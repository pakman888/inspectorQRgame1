using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NodeItem {
	public static int BYTE_SIZE = 0x25;
	public static float NODE_DIR_LEN_COEF = 0.33333333333f;
    public Vector3 position;
    public Vector3 direction;
    public int forwardIndex;
    public int backwardIndex;
    public int flags;
    public byte roadNameIndex;
    public bool forwardIsRoad;
    public bool backwardIsRoad;
    public RoadItem forwardRoadItem;
    public RoadLook forwardRoadLook;
    public RoadItem backwardRoadItem;
    public RoadLook backwardRoadLook;
}