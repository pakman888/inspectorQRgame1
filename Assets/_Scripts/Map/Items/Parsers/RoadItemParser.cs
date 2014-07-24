using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class RoadItemParser {
	public static RoadItem Parse(BinaryReader reader) {
		var item = new RoadItem();	

        reader.ReadBytes(40);
		item.flags = reader.ReadInt32();
		item.distType = reader.ReadByte();
		item.roadLookID = reader.ReadInt32();
		item.roadMaterialID = reader.ReadInt32();
		item.nodeIndices = new int[] {reader.ReadInt32(), reader.ReadInt32 ()};

		item.rightRailingID = reader.ReadInt16();
		item.rightRailingOffset = reader.ReadInt16();
		item.rightModelID = reader.ReadInt16();
		item.rightModelOffset = reader.ReadInt16();
		item.rightModelDist = reader.ReadInt16();
		item.rightMaterialID = reader.ReadInt16();
		item.rightQCount = reader.ReadInt16();
		item.rightProfileID = reader.ReadInt16();
		item.rightProfileCoef = reader.ReadSingle();
		item.rightVegetation = reader.ReadInt16();
		item.rightSidewalk = reader.ReadInt16();
		item.rightRoadHeight = reader.ReadInt32();

		item.leftRailingID = reader.ReadInt16();
		item.leftRailingOffset = reader.ReadInt16();
		item.leftModelID = reader.ReadInt16();
		item.leftModelOffset = reader.ReadInt16();
		item.leftModelDist = reader.ReadInt16();
		item.leftMaterialID = reader.ReadInt16();
		item.leftQCount = reader.ReadInt16();
		item.leftProfileID = reader.ReadInt16();
		item.leftProfileCoef = reader.ReadSingle();
		item.leftVegetation = reader.ReadInt16();
		item.leftSidewalk = reader.ReadInt16();
		item.leftRoadHeight = reader.ReadInt32();

		item.centerMaterialID = reader.ReadInt16();
		item.length = reader.ReadSingle();
		item.roadAIID = reader.ReadInt32();

        var signItems = new List<SignItem>();
        var signCount = reader.ReadUInt32();	
        for (int i = 0; i < signCount; i++) {
            signItems.Add(SignItemParser.Parse(reader.ReadBytes(SignItem.BYTE_SIZE)));
        }

        var stampItems = new List<StampItem>();
        var stampCount = reader.ReadUInt32();
        for (int i = 0; i < stampCount; i++) {
            stampItems.Add(StampItemParser.Parse(reader.ReadBytes(StampItem.BYTE_SIZE)));
        }

        item.signItems = signItems;
        item.stampItems = stampItems;

		return item;
	}
}
