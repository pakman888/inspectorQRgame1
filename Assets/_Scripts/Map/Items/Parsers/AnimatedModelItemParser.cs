using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class AnimatedModelItemParser {
	public static AnimatedModelItem Parse(BinaryReader reader) { 
        var item = new AnimatedModelItem();
    
        item.flags = reader.ReadInt32();
        item.distType = reader.ReadByte();
        reader.ReadBytes(40);

        item.tName = reader.ReadUInt64();
        var nodeCount = reader.ReadUInt32();
        item.nodes = new int[nodeCount];
        for (int i = 0; i < nodeCount; i++) {
            item.nodes[i] = reader.ReadInt32();
        }

		return item; 
    }
}
