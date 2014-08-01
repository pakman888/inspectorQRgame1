using UnityEngine;
using System.Collections;

public class actionCard  {

	public string cName;
	public int suspicionDMG;
	public int cooldwn;
	public int target_type;
	public int dmg_type;
	public int heal_type;

	public actionCard(){
		suspicionDMG = -1;
		cName =" ";
		cooldwn = -1;
		dmg_type = 0; //0 means no dmg to any1
		heal_type = 0;//0 means no heal
		target_type = 0; //0 means self target

	}
	
	public void setActionCard(int dmg, string n, int _cd, int dt, int ht, int tt){
		suspicionDMG = dmg;
		cName = n;
		cooldwn = _cd;
		dmg_type = dt;
		heal_type = ht;
		target_type = tt;
	}

	public bool isSingleTarget(){
		return (target_type ==1);
	}


}
