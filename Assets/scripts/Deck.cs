using UnityEngine;
using System.Collections;

public class Deck : MonoBehaviour {

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
		while( (i < cards.Length) && (cards[i].desc!= " ") )
		{
			i++;
		}
		return i;
	}
	


	public Deck resizeDeck(Deck i){
		
		
		int count = 0;
		
		for(int j = 0; j < i.cards.Length; j++){
			if(i.cards[j]!= null){
				count++;
			}
		}
		
		Deck d = new Deck(count);
		int index = 0;
		
		for(int j = 0; j < i.cards.Length; j++){
			if(i.cards[j] != null){
				d.cards[index] = i.cards[j];
				index++;
			}
		}
		
		return d;
	}

	public void addCard(Card c){
		cards[lastCardIndex()] = c;
	}

	public void rmvCard(int i){
		cards[i] = null;
	}

	public void addCard(Card[] c){
		cards[lastCardIndex()] = c[0];
		cards[lastCardIndex()] = c[1];
	}

	public Card getCard(Deck d, string desc){
//		Card c = d.cards[index]; //not done.
//		cards[index] = null;
		Card c = new Card();
		return c;
	}






	public Deck genRumourHand( Deck mainDeck){
		int dummy;
		int dIndex;
		bool repeated;

		
		int size = (mainDeck.cards.Length+1) /2;
		Deck arr = new Deck(size);
		
		
		int mainDeckIndex = 0;
		int arrIndex = 0;
		
		while((mainDeckIndex < mainDeck.cards.Length) && arrIndex < arr.cards.Length )
		{
			if( (mainDeck.cards[mainDeckIndex] == arr.cards[arrIndex]) || (mainDeck.cards[mainDeckIndex]!= null) )
			{
				mainDeckIndex++;
			}
			else
				if(mainDeck.cards[mainDeckIndex] != arr.cards[arrIndex])
			{
				dIndex = 0;
				repeated = false;
				while( (dIndex < arr.cards.Length) && (arr.cards[dIndex] != null) && repeated == false)
				{
					if(arr.cards[dIndex]!= mainDeck.cards[mainDeckIndex])
					{
						dIndex++;
						
					}
					else
						if(arr.cards[dIndex]== mainDeck.cards[mainDeckIndex])
					{
						repeated = true;
						
					}
				}
				
				if(repeated == false)
				{
					arr.cards[arrIndex] = mainDeck.cards[mainDeckIndex];
					mainDeck.cards[mainDeckIndex] = null;
					arrIndex++;
					mainDeckIndex++;
				}
				else
					if(repeated == true)
				{
					mainDeckIndex ++;
				}
			}
			
		}
		
		return arr;
	}


}
