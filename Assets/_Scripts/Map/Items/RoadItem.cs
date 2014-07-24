using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadItem : Item {
	public int roadLookID;
	public int roadMaterialID;
	public int[] nodeIndices;
	
	public int rightRailingID;
	public int rightRailingOffset;
	public int rightModelID;
	public int rightModelOffset;
	public int rightModelDist;
	public int rightMaterialID;
	public int rightQCount;
	public int rightProfileID;
	public float rightProfileCoef;
	public int rightVegetation;
	public int rightSidewalk;
	public int rightRoadHeight;
	
	public int leftRailingID;
	public int leftRailingOffset;
	public int leftModelID;
	public int leftModelOffset;
	public int leftModelDist;
	public int leftMaterialID;
	public int leftQCount;
	public int leftProfileID;
	public float leftProfileCoef;
	public int leftVegetation;
	public int leftSidewalk;
	public int leftRoadHeight;
	
	public int centerMaterialID;
	public float length;
	public int roadAIID;

    public bool NoTraffic {
        get {
            return ((flags & RoadFlags.RDF_NO_TRAFFIC) > 0);
        }
    }
	
	public List<SignItem> signItems;
	public List<StampItem> stampItems;
	
	public int GetSegmentCount() {
		if ((flags & RoadFlags.RDF_HALF_STEP) == RoadFlags.RDF_HALF_STEP) {
			if (length < RoadFlags.SEG_LENGTH1_HALF) {
				return 0;
			}
			return (int)Math.Ceiling(length * RoadFlags.SEG_LENGTH1_INV);
		}
		else {
			if (length < RoadFlags.SEG_LENGTH2_HALF) {
				return 0;
			}
			return (int)Math.Ceiling(length* RoadFlags.SEG_LENGTH2_INV);
		}
	}
}
