using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PrefabItemParser : MonoBehaviour {
	public static PrefabItem Parse(BinaryReader reader, ResourceServer server) {
		var item = new PrefabItem();	
		
        reader.ReadBytes(40);
		item.flags = reader.ReadInt32();
		item.distType = reader.ReadByte();
		item.prefabID = reader.ReadInt32();
		item.prefabLookID = reader.ReadInt32();
		
		var def = server.prefabDefs[item.prefabID];
		var prefabDesc = def.prefabDesc;
		item.nodeIndices = new int[prefabDesc.nodeCount];
		
		for (int i = 0; i < prefabDesc.nodeCount; i++) {
			item.nodeIndices[i] = reader.ReadInt32();
		}
		item.trLightModels = reader.ReadInt16();
		item.originIndex = reader.ReadInt16();
		//Debug.Log(item.OriginIndex);
		item.prefabRotY = (180 - reader.ReadInt16()) % 360;
		item.prefabRotZ = reader.ReadInt16();
		
		item.terrainFlags = new uint[prefabDesc.nodeCount];
		item.terrainMaterialIDs = new Int16[prefabDesc.nodeCount];
		item.terrainProfileCoefficients = new float[prefabDesc.nodeCount];
		item.terrainVegetation = new Int16[prefabDesc.nodeCount];
		for (int i = 0; i < prefabDesc.nodeCount; i++) {
			item.terrainFlags[i] = reader.ReadUInt32();
			item.terrainMaterialIDs[i] = reader.ReadInt16();
			item.terrainProfileCoefficients[i] = reader.ReadSingle();
			item.terrainVegetation[i] = reader.ReadInt16();
		}
		
		// REFACTOR: does the item need this now?
		item.prefabDesc = prefabDesc;
		
		return item;
	}
}
