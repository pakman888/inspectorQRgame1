using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SignItem : Item {
	public static int BYTE_SIZE = 96;
	public int signModelID;
	public float sideOffset; 
	public float coef;
	public float heightOff;
	public float yRot;
	public SignBoard[] signBoards;
}

[System.Serializable]
public class SignBoard {
	public UInt64 roadName;
	public UInt64 cityName1;
	public UInt64 cityName2;
}