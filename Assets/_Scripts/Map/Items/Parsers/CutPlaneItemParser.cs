using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CutPlaneItemParser {
	public static CutPlaneItem Parse(BinaryReader reader) {
        var item = new CutPlaneItem();
        
        reader.ReadBytes(40); // 40 bytes for kdop::write
        item.flags = reader.ReadInt32();
        item.distType = reader.ReadByte();
		
		var nodeIndices = new int[2];
		nodeIndices[0] = reader.ReadInt32();
		nodeIndices[1] = reader.ReadInt32();

		item.nodeIndices = nodeIndices;

		return item; 
    }
}
