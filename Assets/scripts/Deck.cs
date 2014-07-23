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



		//almost!!! it gets blown from null values...
		/*
		for(int i = 0; i < d.cards.Length;i++)
		{
			if( d.cards[i].desc != f[0].desc || d.cards[i].desc == null)//so far works
			{
				//Debug.Log (d.cards[i].desc);
				index+=1;
			}

			if(d.cards[i].desc == f[0].desc)//so far works
			{
			//	Debug.Log ("good so far");
				if(d.cards[i].type == f[0].type)
				{
			//		Debug.Log ("yeaaah");
					i = d.cards.Length;
				}
				else
				if(d.cards[i].type != f[0].type)
				{
				//	Debug.Log (d.cards[i].type);
					index+=1;
				}
			}
		}*/
		//Debug.Log ("index : "+index); //searching needs to check for null cards. rest works

	//	Debug.Log ("adding : "+d.cards[index].desc+" at index "+index);

	//	c = d.cards[index];

		
	//	Debug.Log ("check removed card : "+d.cards[index].desc+" at index "+index);
	//	d.cards[index] = null;
	

	//	Debug.Log (d.cards[index]);

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

	public Deck genRumourHand( Deck mainDeck){
		bool repeated;
		int size = (mainDeck.cards.Length+1) /2;
		Deck arr = new Deck(size);
		
		
		int mainDeckIndex = 0;
		int arrIndex = 0;

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
					for(arrIndex = 0; arrIndex < arr.cards.Length; arrIndex++) // go through player's hand
					{

						if(arr.cards[arrIndex].desc!= " ")
						{
						//	Debug.Log("arr- "+arrIndex+" not empty.");
							if( arr.cards[arrIndex].desc == mainDeck.cards[mainDeckIndex].desc)
							{
								repeated = true;
							}
						}
						else
						if(arr.cards[arrIndex].desc==" ")
						{
						//	Debug.Log("arr- "+arrIndex+" is empty.");

							if(repeated == true) // get out and move to next card
							{
								Debug.Log("mainDeck- "+mainDeck.cards[mainDeckIndex].desc+" is repeated ");
								arrIndex = arr.cards.Length;
							}
							else
							if(repeated == false) // get out and move to next card
							{
						//		Debug.Log("adding :  "+mainDeck.cards[mainDeckIndex].desc);

								arr.cards[arrIndex] = mainDeck.cards[mainDeckIndex];
								mainDeck.cards[mainDeckIndex] = null;
								mainDeck.cards[mainDeckIndex] = new Card(); //removing card from main deck
								arrIndex = arr.cards.Length;
							}
						}
					}
				}
			}

		}





///1 or two element is left in deck... SO CLOSE
		 /*
		while((mainDeckIndex < mainDeck.cards.Length) && (arrIndex < arr.cards.Length) )
		{
			if( (mainDeck.cards[mainDeckIndex].desc == arr.cards[arrIndex].desc) )
			{
				mainDeckIndex++;
			}
			else
				if(mainDeck.cards[mainDeckIndex].desc != arr.cards[arrIndex].desc)
			{
				dIndex = 0;
				repeated = false;
				while( (dIndex < arr.cards.Length) && (arr.cards[dIndex].desc != " ") && repeated == false)
				{
					if(arr.cards[dIndex].desc!= mainDeck.cards[mainDeckIndex].desc)
					{
						dIndex++;

					}
					else
						if(arr.cards[dIndex].desc== mainDeck.cards[mainDeckIndex].desc)
					{
						repeated = true;

					}
				}

				if(repeated == false && mainDeck.cards[mainDeckIndex].desc != " " )
				{
					arr.cards[arrIndex] = mainDeck.cards[mainDeckIndex];
					mainDeck.cards[mainDeckIndex] = null;
					mainDeck.cards[mainDeckIndex] = new Card();
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
		*/

		return arr;
	}

/*
 * 		
 * */
}
