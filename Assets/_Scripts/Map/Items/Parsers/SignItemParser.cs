using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SignItemParser {
	public static SignItem Parse(byte[] sourceBytes) {
		var item = new SignItem();
		item.signModelID = BitConverter.ToInt32(sourceBytes, 0);
		item.sideOffset = BitConverter.ToSingle(sourceBytes, 4);
		item.flags = BitConverter.ToInt32(sourceBytes, 8);
		item.coef = BitConverter.ToSingle(sourceBytes, 12);
		item.heightOff = BitConverter.ToSingle(sourceBytes, 16);
		item.yRot = BitConverter.ToSingle(sourceBytes, 20);
		var signBoards = new SignBoard[3];
		for (int i = 0; i < 3; i++) {
			var signBoard = new SignBoard();
			signBoard.roadName = BitConverter.ToUInt64(sourceBytes, 24 + i * 24);
			signBoard.cityName1 = BitConverter.ToUInt64(sourceBytes, 32 + i * 24);
			signBoard.cityName2 = BitConverter.ToUInt64(sourceBytes, 40 + i * 24);
			signBoards[i] = signBoard;
		}
		item.signBoards = signBoards;
		return item;
	}
}