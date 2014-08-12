using UnityEngine;
using System.Collections;

public class actionCard  {
	/*
	public string cName;
	public int suspicionDMG;
	public int cooldwn;

	public int target_num;//0 means none, 1 means 1, 2 means 1+
	public int dmg_type;
	public int heal_type;
	public bool teamupReq;
	public bool teamConseq;
*/
	// new values assign
	//applies for attack
	public bool isAttack;
	public bool isDef;
	public bool repAtkConseq;
	public bool isDmgDealer;
	public bool singlePlayerDmg;

	//applies for defend
	public bool isHeal;
	public bool isBlock;

	//general action info
	public bool actionSuccessful;

	public int suspicionDmg; //>0 means atk, <0 means heal, 0 means ineffective
	public int cooldown;
	public int cdRemain;
	public int limit;

	public int priority;



	public actionCard(){
		isAttack = false;
		isDef = false;
		repAtkConseq = false;
		isDmgDealer = false;
		singlePlayerDmg = false;
		isHeal = false;
		isBlock = false;
		actionSuccessful = false;
		suspicionDmg = 0;
		cdRemain = 0;
	}
	
	public void setActionCard(bool isA, bool isD, bool isRAC,bool isDD, bool isSPD, bool isH, bool isB, bool isAs, int sD,int cd, int limit, int pri ){
		isAttack = isA;
		isDef = isD;
		repAtkConseq = isRAC;
		isDmgDealer = isDD;
		singlePlayerDmg = isSPD;
		isHeal = isH;
		isBlock = isB;
		actionSuccessful = isAs;
		suspicionDmg = sD;
		cooldown = cd;
		this.limit = limit;
		priority = pri;
		cdRemain = 0;
	}

	public bool isAllegation(){
		return ( (isAttack == true) && ( isDef == false) && (repAtkConseq == false)     
		        && (isDmgDealer == true) && (singlePlayerDmg == true) );
	}

	public bool isTaunt(){
		return ( (isAttack == true)&& ( isDef == false)  && (repAtkConseq == false)     
		        && (isDmgDealer == false) && (singlePlayerDmg == true) );
	}

	public bool isSnitch(){
		return ( (isAttack == true)&& ( isDef == false)  && (repAtkConseq == true)     
		        && (isDmgDealer == true) && (singlePlayerDmg == false) );
	}

	public bool isBlame(){
		return ( (isAttack == true)&& ( isDef == false)  && (repAtkConseq == false)     
		        && (isDmgDealer == true) && (singlePlayerDmg == false) );
	}

	public void showCard()
	{
		if(isAllegation()==true)
		{
			Debug.Log("Allegation card");
		}
		if(isTaunt ()== true)
		{
			Debug.Log("Taunt");
		}
		if(isSnitch() == true)
		{
			Debug.Log("Snitch");
		}
		if(isBlame () == true)
		{
			Debug.Log("Blame");
		}
		if(isHeal == true)
		{	
			Debug.Log("Heal");
		}
		if(isBlock == true){
			Debug.Log("Block");
		}
	}

}
