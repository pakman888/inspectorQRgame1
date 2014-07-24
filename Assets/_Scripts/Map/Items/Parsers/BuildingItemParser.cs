using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.IO;
using System.Collections.Generic;

public class BuildingItemParser {
    public static BuildingItem Parse( BinaryReader reader ) {
        BuildingItem buildingItem = new BuildingItem();
       
        //Kdop::write byte fat
        reader.ReadBytes(40);       
        buildingItem.flags = reader.ReadInt32();
        buildingItem.distType = reader.ReadByte();

        buildingItem.scheme = reader.ReadInt32(); 
        buildingItem.nodes = new List<int>();
        // The 4 comes form the buildings_item_u.h file  
        for (int i = 0; i < 4; i++) {
            buildingItem.nodes.Add(reader.ReadInt32());
        }
        buildingItem.length = reader.ReadSingle();
        buildingItem.firstBuilding = (int) reader.ReadUInt32();
        buildingItem.firstLook = (int) reader.ReadUInt32();
        buildingItem.randomSeed = (int) reader.ReadUInt32();
        buildingItem.scale = reader.ReadSingle();
        var idxsCount = reader.ReadUInt32(); // u32 - 4 bytes;
        var tempBuildingIndexList = new List<int>();
        for (uint j = 0; j < idxsCount; j++) {
            tempBuildingIndexList.Add((int)reader.ReadUInt32());
        }
        //Read variable length variant stuff.
        var variantCount = reader.ReadUInt32(); // u32 - 4 bytes;
        var tempBuildingVariantIndexList = new List<int>();
        for ( uint j = 0; j < variantCount; j++ ) {
            tempBuildingVariantIndexList.Add((int)reader.ReadUInt32());
        }
        buildingItem.buildingIndexComposed = tempBuildingIndexList;
        buildingItem.variantIdxs = tempBuildingVariantIndexList;
        //Get the model index's from the composed index list.
        buildingItem.buildingIndexDecomposed = new List<int>();
        foreach ( int composedIndex in buildingItem.buildingIndexComposed ) {
            buildingItem.buildingIndexDecomposed.Add( BuildingItem.GetModelIndexFromComposedIndex( composedIndex ));
            //Debug.Log("ComposedIndex: " + composedIndex + " ActualModelIndex: " + BuildingItem.GetModelIndexFromComposedIndex(composedIndex));
        }
		
		buildingItem.hookups = new List<Hookup>();
		uint hookupCount = reader.ReadUInt32();
		for(uint i = 0; i < hookupCount; i++){
			uint hookupType = reader.ReadUInt32();
			if(hookupType == 1){ //Tree leaf hookup type
				Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
				var materialStringLength = Convert.ToInt32(reader.ReadUInt32());
				var materialPath = new string(reader.ReadChars(materialStringLength));
				buildingItem.hookups.Add(new Hookup(
					position,
					materialPath
				));
			}			
		}
        return buildingItem;
    }
}

[System.Serializable]
public class Hookup{
	public Vector3 position;
	public string materialName;
	
	public Hookup(){
		
	}
	
	public Hookup(Vector3 pos, string matName){
		position = pos;
		materialName = matName;
	}
}
