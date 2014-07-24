using UnityEngine;
using System.Collections;

public class Deck {

	// Use this for initialization
	public Card []cards;

	public Deck(){
		cards = new Card[24];
		for(int i = 0; i < cards.Length;i++){
			cards[i] = new Card();
		}
	}

	public Deck( int num){

		cards = new Card[num];
	
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
		while( (i < cards.Length) &&  (cards[i].desc != " "))
		{
			i++;
		}
		return i;
	}

	public int lastCardIndex(Deck d){
		int i = 0;
		while( (i < d.cards.Length) &&  (d.cards[i].desc != " "))
		{
			i++;
		}
		return i;
	}
	
	public void rmvCard(int i){
		cards[i] = null;
	}

	public void addCard(Card[] c){
		cards[lastCardIndex()] = c[0];
		cards[lastCardIndex()] = c[1];
	}

	public Deck resizeDeck(){ //converted to return deck
		
		
		int count = 0;
		//Debug.Log ("resizing deck");
		for(int j = 0; j < cards.Length; j++){
			if(cards[j].desc!=" "){
		//		Debug.Log(cards[j].desc);
				count++;
			}
		}
		
		Deck d = new Deck(count);
		int index = 0;
		
		for(int j = 0; j < cards.Length; j++){
			if(cards[j].desc!=" "){
				d.cards[index] = cards[j];
				index++;
			}
		}
		
		return d;
	}



	public Card getCard(Deck d, string f, char t){
//		Card c = d.cards[index]; //not done.
//		cards[index] = null;
		Card c;
		int index = 0;

		for(int i = 0; i < d.cards.Length;i++)
		{
			if( d.cards[i].desc != f || d.cards[i].desc == null)//so far works
			{
				//Debug.Log (d.cards[i].desc);
				index+=1;
			}
			
			if(d.cards[i].desc == f)//so far works
			{
				//	Debug.Log ("good so far");
				if(d.cards[i].type == t)
				{
					//		Debug.Log ("yeaaah");
					i = d.cards.Length;
				}
				else
					if(d.cards[i].type !=t)
				{
					//	Debug.Log (d.cards[i].type);
					index+=1;
				}
			}
		}
		if(t == 'f'){
			c = new Fact();

		}

		if(t == 'r'){
			c = new Rumour();
		}

		c = d.cards[index];
		d.cards[index] = new Card();
		return c;
	}




	public bool isRumour(Card i){
		bool rumour = false;
		
		if(i.type == 'r')
			rumour = true;
		else
			if(i.type == null)
		{
			;
		}
		
		return rumour;
	}

	public bool checkHandIsSet(Card[] d){
		int notNull = 0;

		for(int i = 0; i < d.Length; i++){
			if(d[i].desc!= " "){
				notNull +=1;
			}
		}

		return (notNull == d.Length);
	
	}

	//check point.

	public void genRumourHand( Deck mainDeck){
		bool repeated;
		
		int mainDeckIndex = 0;
//		int arrIndex = 0;

		for(mainDeckIndex = 0; mainDeckIndex < mainDeck.cards.Length; mainDeckIndex++){
		
			if(isRumour(mainDeck.cards[mainDeckIndex]))
			{
				if(mainDeck.cards[mainDeckIndex].desc == " ")
				{
					//Debug.Log(mainDeckIndex+ " Empty ++");
				}
				else
				if(isRumour(mainDeck.cards[mainDeckIndex]))
				{
					repeated = false;
					//Debug.Log(mainDeckIndex+ " !Empty");
					for(int arrIndex = 0; arrIndex < cards.Length; arrIndex++) // go through player's hand
					{

						if(cards[arrIndex].desc!= " ")
						{
						//	Debug.Log("arr- "+arrIndex+" not empty.");
							if( cards[arrIndex].desc == mainDeck.cards[mainDeckIndex].desc)
							{
								repeated = true;
							}
						}
						else
						if(cards[arrIndex].desc==" ")
						{
						//	Debug.Log("arr- "+arrIndex+" is empty.");

							if(repeated == true) // get out and move to next card
							{
							//	Debug.Log("mainDeck- "+mainDeck.cards[mainDeckIndex].desc+" is repeated ");
								arrIndex = cards.Length;
							}
							else
								if(repeated == false) // get out and move to next card
							{
		//						Debug.Log("adding :  "+mainDeck.cards[mainDeckIndex].desc);

								cards[arrIndex] = mainDeck.cards[mainDeckIndex];
								mainDeck.cards[mainDeckIndex] = null;
								mainDeck.cards[mainDeckIndex] = new Card(); //removing card from main deck
								arrIndex = cards.Length;
							}
						}
						if(checkHandIsSet(cards) == true)
						{
							mainDeckIndex = mainDeck.cards.Length;
						}
					}


				}
			}

		}
	//return cards;
	}

/*
 * 		
 * */
}
