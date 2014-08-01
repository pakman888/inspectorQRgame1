using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public DeckHand hand;
	public string role;
	public int dmg;
	public int suspLvl;


	// Use this for initialization
	public Player() {
		dmg = 0;
		suspLvl = 0;
		hand = new DeckHand();
	}

	public void setRole(string r){
		role = r;
	}

	public void setDmg(int d){
		dmg = d;
	}

	public void receiveDmg(int d){
		suspLvl+= d;
	}

	public void receiveHeal(int h){

		suspLvl -=h;
		if(suspLvl < 0){
			suspLvl = 0;
		}
	}

}
