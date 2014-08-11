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



	}
	void setacArr(int index, actionCard ac){
		switch(index)
		{
			//setActionCard(bool isAttack, bool isDef, bool repAtkConseq,bool isDmgDealer, bool singlePlayerDmg, bool isHeal,
			// bool isBlock, bool actionSuccessful, int suspicionDmg, int cd, int limit )
			//default, actions are not successful (false)
		case 0: ac.setActionCard(true, false, false ,true,true , false, false, false, 1, 1, 10, 1); break; //allegation
		case 1: ac.setActionCard(true, false, false, false,true,false, false, false, 0, 1, 10, 5);break;//taunt
		case 2: ac.setActionCard(true, false, true, true, false, false, false, false, 3, 2, 2, 2);break; //snitch
		case 3: ac.setActionCard(true, false, false, true, false, false, false, false, 0, 1, 10, 3);break; //blame. depends how many people use it at the same time
		case 4: ac.setActionCard(false, true, false, false, false, true, false, false, -2, 1, 10, 4);break;//reason. +2 if user is blamed or allegated
		case 5: ac.setActionCard(false, true, false, false, false, false, true, false, 0, 1, 10, 6);break;//defend
	
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
