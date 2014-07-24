using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class NodeItemParser {
	public static NodeItem Parse(BinaryReader reader) {
        Vector3 position = new Vector3(
            reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle() * -1
        );
		
		Vector3 direction = new Vector3(
			reader.ReadSingle(),
            reader.ReadSingle(),
            reader.ReadSingle() * -1 
		);
        
        var backwardIndex = reader.ReadInt32();
        var forwardIndex = reader.ReadInt32();
        var flags = (int) reader.ReadUInt32();

        var roadNameIndex = reader.ReadByte();

        var node = new NodeItem { 
            position = position, 
            direction = direction,
            backwardIndex = backwardIndex,
            forwardIndex = forwardIndex,
            flags = flags,
            roadNameIndex = roadNameIndex
        };
 
		return node;
	}
}
