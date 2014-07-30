using UnityEngine;
using System.Collections;

public class DeckHand : MonoBehaviour {

	public actionCard [] hand;

	// Use this for initialization
	void Start () {
	
		hand = new actionCard[20];
		for(int i = 0; i < hand.Length; i++){
			setHand (i,hand[i]);
		}



	}

	void setHand(int index, actionCard ac){
		switch(index)
		{
		case 0: ac.setActionCard(1, "0", 7); break;
		case 1: ac.setActionCard(2, "10", 6);break;
		case 2: ac.setActionCard(3, "20", 3);break;
		case 3: ac.setActionCard(4, "30", 2);break;
		case 4: ac.setActionCard(5, "40", 1);break;
		case 5: ac.setActionCard(6, "50", 8);break;
		case 6: ac.setActionCard(7, "60", 14);break;
		case 7: ac.setActionCard(8, "70", 17);break;
		case 8: ac.setActionCard(9, "80", 15);break;
		case 9: ac.setActionCard(10, "90", 12);break;
		case 10: ac.setActionCard(11, "100", 18);break;
		case 11: ac.setActionCard(12, "110", 0);break;

			
		case 12: ac.setActionCard(13, "120", 4);break;
		case 13: ac.setActionCard(14, "130", 5);break;
		case 14: ac.setActionCard(15, "140", 9);break;
		case 15: ac.setActionCard(16, "150", 10);break;
		case 16: ac.setActionCard(17, "160", 11);break;
		case 17: ac.setActionCard(18, "170", 13);break;
		case 18: ac.setActionCard(19, "180", 16);break;
		case 19: ac.setActionCard(20, "190", 19);break;
		default: ;break;
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
