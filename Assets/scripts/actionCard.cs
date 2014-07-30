using UnityEngine;
using System.Collections;

public class actionCard  {

	public int suspicionDMG;
	public string cName;
	public int cooldwn;

	public actionCard(){
		suspicionDMG = -1;
		cName =" ";
		cooldwn = -1;
	}

	public actionCard(int dmg, string n, int _cd){
		suspicionDMG = dmg;
		cName = n;
		cooldwn = _cd;
	}


	public void setActionCard(int dmg, string n, int _cd){
		suspicionDMG = dmg;
		cName = n;
		cooldwn = _cd;
	}


}
