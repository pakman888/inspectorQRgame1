using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

	// Use this for initialization
	Card []cards;

	public Deck(){
		cards = new Card[24];
		for(int i = 0; i < cards.Length;i++){
			cards[i] = new Card();
		}
	}

	public void playerDeck(){
		cards = null;
		cards = new Card[6];
		for(int i = 0; i < cards.Length;i++){
			cards[i] = new Card();
		}
	}
	public int lastCardIndex(){
		int i = 0;
		while( (i < cards.Length) && (cards[i].desc!= " ") )
		{
			i++;
		}
		return i;
	}


	public void addCard(Card c){
		cards[lastCardIndex()] = c;
	}

	public void addCard(Card[] c){

		//Debug.Log (lastCardIndex());
		cards[lastCardIndex()] = c[0];
		cards[lastCardIndex()] = c[1];
	}

	public Card getCard(int index){
		return cards[index];
	}
}
