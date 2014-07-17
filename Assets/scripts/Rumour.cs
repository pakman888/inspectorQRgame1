using UnityEngine;
using System.Collections;

public class Rumour : Card {
	//this contains information from Card. Do not delete! Fact not equal to Rumour
	public bool truth;
	public bool lie;
	public Rumour(){
		truth = true;
		lie = true;
	}


}
