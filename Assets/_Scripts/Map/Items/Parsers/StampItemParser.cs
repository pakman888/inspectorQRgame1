using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class StampItemParser {
	public static StampItem Parse(byte[] sourceBytes) {
		var item = new StampItem();
		item.materialID = BitConverter.ToInt32(sourceBytes, 0);
		item.templateID = BitConverter.ToInt32(sourceBytes, 4);
		item.posX = BitConverter.ToInt32(sourceBytes, 8);
		item.posY = BitConverter.ToInt32(sourceBytes, 12);
		return item;
	}
}
