using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public DeckHand hand;
	public string role;
	public int dmg;
	public int suspLvl;
	public int maxSuspLvl;

	public void setRole(string r){
		role = r;
	}

	public void setDmg(int d){
		dmg = d;
	}

	// Use this for initialization
	void Start () {
		suspLvl = 0;
		maxSuspLvl = 10;
		hand = new DeckHand();

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
