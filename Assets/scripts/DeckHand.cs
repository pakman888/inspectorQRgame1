using UnityEngine;
using System.Collections;

public class DeckHand : MonoBehaviour {

	public actionCard [] acArr;

	// Use this for initialization
	public DeckHand () {
	
		acArr = new actionCard[20];
		for(int i = 0; i < acArr.Length; i++){
			acArr[i] = new actionCard();
			setacArr (i,acArr[i]);
		}



	}/*
	public void setActionCard(int dmg, string n, int _cd, int dt, int ht, int tt){
		suspicionDMG = dmg;
		cName = n;
		cooldwn = _cd;
		dmg_type = dt;
		heal_type = ht;
		target_type = tt;
	}*/
	void setacArr(int index, actionCard ac){
		switch(index)
		{
			//damage, name, cooldown, damage type, heal type, target type
		case 0: ac.setActionCard(1, "0", 1, 1, 0, 1); break;
		case 1: ac.setActionCard(2, "10", 1,0, 1, 0);break;
		case 2: ac.setActionCard(3, "20", 1, 2, 0, 3);break;
		case 3: ac.setActionCard(1, "30", 2, 0, 0, 0);break;
		case 4: ac.setActionCard(5, "40", 1, 1, 0, 1);break;
		case 5: ac.setActionCard(6, "50", 8, 1, 0, 1);break;
		case 6: ac.setActionCard(7, "60", 14, 1, 0, 1);break;
		case 7: ac.setActionCard(8, "70", 17, 1, 0, 1);break;
		case 8: ac.setActionCard(9, "80", 15, 1, 0, 1);break;
		case 9: ac.setActionCard(10, "90", 12, 1, 0, 1);break;
		case 10: ac.setActionCard(11, "100", 18, 1, 0, 1);break;
		case 11: ac.setActionCard(12, "110", 0, 1, 0, 1);break;

			
		case 12: ac.setActionCard(13, "120", 4, 1, 0, 1);break;
		case 13: ac.setActionCard(14, "130", 5, 1, 0, 1);break;
		case 14: ac.setActionCard(15, "140", 9, 1, 0, 1);break;
		case 15: ac.setActionCard(16, "150", 10, 1, 0, 1);break;
		case 16: ac.setActionCard(17, "160", 11, 1, 0, 1);break;
		case 17: ac.setActionCard(18, "170", 13, 1, 0, 1);break;
		case 18: ac.setActionCard(19, "180", 16, 1, 0, 1);break;
		case 19: ac.setActionCard(20, "190", 19, 1, 0, 1);break;
		default: ;break;
		}
	}

	public int getDeckSize(){
		int u = 0;
		for(int i = 0; i < acArr.Length; i++){
			u+=1;
		}
		return u;
	}



}
