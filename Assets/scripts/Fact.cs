using UnityEngine;
using System.Collections;

public class Fact : Card {
	//this contains information from Card. Do not delete! Fact not equal to Rumour
	public bool truth;
	public bool lie;

	public Fact(){
		truth = true;
		lie = false;
		type = 'f';
	}

	public void fcopy(Fact f){
		truth = f.truth;
		lie = f.lie;
		type = f.type;
	}
}
