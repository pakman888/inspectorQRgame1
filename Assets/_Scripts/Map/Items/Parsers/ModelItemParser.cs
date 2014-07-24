using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ModelItemParser {
	public static ModelItem Parse(BinaryReader reader) {
		var item = new ModelItem();

        reader.ReadBytes(40);
		item.flags = reader.ReadInt32();
        item.distType = reader.ReadByte();

        item.modelIndex = reader.ReadInt32();
        item.modelLookIndex = reader.ReadInt32();
        item.nodeIndex = reader.ReadInt32();
	    // so this is weird, they say in there notes that it's in the order of yaw pitch roll, 
	    // but this looks like it might actually just be a rotation array, in the opposite handedness, 
        // which is why the 360 - x is required.
        // wacky.

        // Two Hacks don't make a right.
		var eulerAngles = new Vector3(
			reader.ReadSingle(), // yaw
			180 - reader.ReadSingle(), //
			reader.ReadSingle()  // roll
		);

        //prism euler yaw, pitch roll
        //unity euler roll, pitch, yaw
        var ConvertertedEulars = new Vector3(
            eulerAngles.z,
            eulerAngles.y,
            eulerAngles.x
        );

        item.eulerAngles = ConvertertedEulars;

		item.scale = new Vector3(
			reader.ReadSingle(),
			reader.ReadSingle(),
			reader.ReadSingle()
		);
		
		return item;
    }
}
