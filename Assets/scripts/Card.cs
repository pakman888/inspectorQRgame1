﻿using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {

	//can be a player card, fact or rumour.
	public char type;
	public string desc;
	public Card(){
		desc = " ";
		type = 'c';
	}

}
