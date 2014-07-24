using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StampItem : Item {
	public static int BYTE_SIZE = 16;
	public int materialID;
	public int templateID;
	public int posX;
	public int posY;
}