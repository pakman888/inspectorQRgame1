using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public DeckHand hand;
	public string role;
	public int dmg;
	public int suspLvl;
	public int receiveUserIndexAttack;
	public int atkUserIndex;
	public int actionCardIndex;
	public bool blaming;

	// Use this for initialization
	public Player() {
		dmg = 0;
		suspLvl = 0;
		blaming = false;
		atkUserIndex = -1;
		actionCardIndex = -1;
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

	public void setAtkUserIndex(int i){
		atkUserIndex = i;
	}

	public void receiveHeal(int h){

		suspLvl -=h;
		if(suspLvl < 0){
			suspLvl = 0;
		}
	}



}
