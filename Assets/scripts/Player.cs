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
	public int multiplier;
	public bool isHealing;
	public bool isDefending;
	public bool taunted;
	public bool usedAction;


	// Use this for initialization
	public Player() {
		taunted = false;
		usedAction = false;
		dmg = 0;
		suspLvl = 0;
		blaming = false;
		atkUserIndex = -1;
		actionCardIndex = -1;
		multiplier = 0;
		hand = new DeckHand();
		isHealing = false;
		isDefending = false;
	}


	public void newRoundStatReset(){
		blaming = false;
		atkUserIndex = -1;
		actionCardIndex = -1;
		multiplier = 0;
		hand = new DeckHand();
		isHealing = false;
		isDefending = false;
		taunted = false;
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
