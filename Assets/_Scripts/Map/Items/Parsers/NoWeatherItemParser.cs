using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class NoWeatherItemParser {
	public static NoWeatherItem Parse(BinaryReader reader) {
        var item = new NoWeatherItem();
        
        reader.ReadBytes(40);
        item.flags = reader.ReadInt32();
        item.distType = reader.ReadByte();
		
		item.width = reader.ReadSingle();
        item.height = reader.ReadSingle();
        item.nodeIndex = reader.ReadInt32();

		return item; 
    }
}
