using UnityEngine;
using System.Collections;

public class Fact : Card {
	//this contains information from Card. Do not delete! Fact not equal to Rumour
	public bool truth;
	public bool lie;
	public Fact(){
		truth = true;
		lie = true;
	}


}
