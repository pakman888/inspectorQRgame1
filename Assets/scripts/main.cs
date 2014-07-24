using UnityEngine;
using System.IO;
public class main: MonoBehaviour {

	// Use this for file reading
	public FileInfo fs = null;
	public StreamReader r = null;
	public string text = "";
	public string flag;

	//file information array after reading
	public string []when;
	public string []where;
	public string []what;
	public string []who;


	//in game Crime Information
	public int i_whenC; //i_wXXXXXX is an index corresponding to the information array. used to make sure extra "cards" will not have extra duplicates of (fact & rumours of crime)
	public string whenC;

	public int i_whereC;
	public string whereC;
	
	public int i_whoC;
	public string whoC;

	public int i_whatC;
	public string whatC;

	public int x_whenC;
	public string whenX;

	public int x_whereC;
	public string whereX;

	public int x_whoC;
	public string whoX;

	public int x_whatC;
	public string whatX;

	//deck setup
	Deck deckOfCards;
	Card [] whereFcard;
	Card [] whereRcard ;

	Card [] whoFcard ;
	Card [] whoRcard;
	
	Card [] whatFcard ;
	Card [] whatRcard;
	
	Card [] whenFcard ;
	Card [] whenRcard;
	
	Card[] xtraWhen;
	Card[] xtraWhere;
	Card[] xtraWhat ;
	Card[] xtraWho;
	//player's hand
	Deck activist1;
	Deck activist2;
	Deck hacker1;
	Deck hacker2;
	//test array
	int [] num = { 1,1,3,3, 5,5,7,7, 8,8, 11,11, 02,02,22,22, 06,06,66,66, 04,04,44,44};


	void Start () {
		activist1 = new Deck();
		activist1.playerDeck();
		activist2 = new Deck();
		activist2.playerDeck();
		hacker1 = new Deck();
		hacker1.playerDeck();
		hacker2 = new Deck();
		hacker2.playerDeck();

		Random.seed = (int)System.DateTime.Now.Ticks;
		deckOfCards = new Deck();
		fs = new FileInfo("assets/gameinfo/gameInfo.txt");
		r = fs.OpenText();
		flag = " ";
		initializeArrays();

		while( (text = r.ReadLine () )!= null ){
			
			if(  (flag == null) ||  ((flag != text ) && (text.Length == 1))    )
			{
				flag = text;
			}
			else
			assignInfo(flag, text);
		}

		r.Close ();

		int valWhere = Random.Range(0,7);
		int valWhen = Random.Range(0,7);
		int valWhat = Random.Range(0,7);
		int valWho = Random.Range(0,7);

		int xWhere = checkNotDuplicate(valWhere);
		int xWhen = checkNotDuplicate(valWhen);
		int xWhat = checkNotDuplicate(valWhat);
		int xWho = checkNotDuplicate(valWho);


	//	(int whenIndex,int whereIndex,int whoIndex,int whatIndex )
		setGameInfo(valWhen,valWhere,valWho,valWhat );
		//int whenIndex,int whereIndex,int whoIndex,int whatIndex 
		setXtraInfo(xWhen,xWhere,xWho,xWhat );
		makeDeck();
		shuffleDeck();
		dealCards();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public int checkNotDuplicate(int num){
		int j = Random.Range(0,7);
		while(j== num){
			j = Random.Range(0,7);
		}
		Debug.Log("real rumour- "+num);
		Debug.Log("fake rumour- "+j);
		return j;
	}


	public void assignInfo(string f, string t){
		switch(f)
		{
		case "*": addWhen(t);break;
		case "@": addWho(t);break;
		case "&": addWhere(t);break;
		case "$": addWhat(t);break;
		defualt: ;break;
		}
	}

	void initializeArrays(){
		when = new string[7];
		what = new string[7];
		where = new string[7];
		who = new string[7];

		initWArray(when);
		initWArray(what);
		initWArray(who);
		initWArray(where);

		activist1 = new Deck();
		activist2 = new Deck();
		hacker1 = new Deck();
		hacker2 = new Deck();

		activist1.playerDeck();
		activist2.playerDeck();
		hacker1.playerDeck();
		hacker2.playerDeck();
	}

	void initWArray(string [] arr){
		for(int i = 0; i < arr.Length; i++){
			arr[i] = ";"; 
		}
	}

	void addWhen(string t){
		if(when[0] == ";")		{
			when[0] = t;
		}
		else
		if(when[1] == ";")		{
			when[1] = t;
		}
		else
		if(when[2] == ";")		{
			when[2] = t;
		}
		else
		if(when[3] == ";")		{
			when[3] = t;
		}
		else
		if(when[4] == ";")		{
			when[4] = t;
		}
		else
		if(when[5] == ";")		{
			when[5] = t;
		}
		else
		if(when[6] == ";")		{
			when[6] = t;
		}
	}

	void addWhat(string t){
		if(what[0] == ";")		{
			what[0] = t;
		}
		else
		if(what[1] == ";")		{
			what[1] = t;
		}else
		if(what[2] == ";")		{
			what[2] = t;
		}else
		if(what[3] == ";")		{
			what[3] = t;
		}else
		if(what[4] == ";")		{
			what[4] = t;
		}else
		if(what[5] == ";")		{
			what[5] = t;
		}else
		if(what[6] == ";")		{
			what[6] = t;
		}
	}

	void addWho(string t){
		if(who[0] == ";")
		{
			who[0] = t;
		}
		else
		if(who[1] == ";")
		{
			who[1] = t;
		}
		else
		if(who[2] == ";")
		{
			who[2] = t;
		}
		else
		if(who[3] == ";")
		{
			who[3] = t;
		}
		else
		if(who[4] == ";")
		{
			who[4] = t;
		}
		else
		if(who[5] == ";")
		{
			who[5] = t;
		}
		else
		if(who[6] == ";")
		{
			who[6] = t;
		}
	}

	void addWhere(string t){
		if(where[0] == ";")
		{
			where[0] = t;
		}else
		if(where[1] == ";")
		{
			where[1] = t;
		}else
		if(where[2] == ";")
		{
			where[2] = t;
		}else
		if(where[3] == ";")
		{
			where[3] = t;
		}else
		if(where[4] == ";")
		{
			where[4] = t;
		}else
		if(where[5] == ";")
		{
			where[5] = t;
		}else
		if(where[6] == ";")
		{
			where[6] = t;
		}
	}

	void setGameInfo(int whenIndex,int whereIndex,int whoIndex,int whatIndex ){
		i_whereC = whereIndex;
		i_whenC = whenIndex;
		i_whoC = whoIndex;
		i_whatC = whatIndex;

		whereC = where[whereIndex];
		whenC = when[whenIndex];
		whoC = who[whoIndex];
		whatC = what[whatIndex];

	}


	void setXtraInfo(int whenIndex,int whereIndex,int whoIndex,int whatIndex ){
		x_whatC = whatIndex;
		x_whenC = whenIndex;
		x_whereC = whereIndex;
		x_whoC = whoIndex;

		whereX = where[x_whereC];
		whenX = when[x_whenC];
		whoX = who[x_whoC];
		whatX = what[x_whatC];
	}

	public Card[] initFacts(Card[] c, string str)
	{
		c = new Fact[2];

		for(int i = 0; i < 2; i++){
			c[i] = new Fact();
			c[i].desc = str;
		}
		return c;
	}

	public Card[] initRumours(Card[] c ,string str)
	{
		c = new Rumour[2];
		for(int i = 0; i < 2; i++){
			c[i] = new Rumour();
			c[i].desc = str;
		}
		return c;
	}

	public void makeDeck(){

		whereFcard = initFacts(whereFcard, whereC);
		whereRcard = initRumours(whereRcard, whereC);
		whoFcard = initFacts(whoFcard, whoC);
		whoRcard= initRumours(whoRcard, whoC);
		whatFcard=	initFacts(whatFcard, whatC);
		whatRcard= initRumours(whatRcard, whatC);
		whenFcard= initFacts(whenFcard, whenC);
		whenRcard= initRumours(whenRcard, whenC);
		Debug.Log("Facts : ");
		Debug.Log(whoFcard[0].desc);
		Debug.Log(whatFcard[0].desc);
		Debug.Log(whereFcard[0].desc);
		Debug.Log(whenFcard[0].desc);


		Debug.Log("");
		Debug.Log("Fact rumours: ");
		Debug.Log(whoRcard[0].desc);
		Debug.Log(whatRcard[0].desc);
		Debug.Log(whereRcard[0].desc);
		Debug.Log(whenRcard[0].desc);

		xtraWhen= initRumours(xtraWhen, whenX);
		xtraWhere= initRumours(xtraWhere, whereX);
		xtraWhat= initRumours(xtraWhat, whatX);
		xtraWho= initRumours(xtraWho, whoX);
		Debug.Log("Fake rumours: ");
		Debug.Log(xtraWho[0].desc);
		Debug.Log(xtraWhat[0].desc);
		Debug.Log(xtraWhere[0].desc);
		Debug.Log(xtraWhen[0].desc);
	

		//24 cards in total
		deckOfCards.addCard (whoFcard);
		deckOfCards.addCard (whereFcard);//add array
		deckOfCards.addCard (whenFcard);
		deckOfCards.addCard (whatFcard);
	

		deckOfCards.addCard (whoRcard);	
		deckOfCards.addCard (whatRcard);
		deckOfCards.addCard (whenRcard);
		deckOfCards.addCard (whereRcard);
	
	

		deckOfCards.addCard (xtraWhen); //16
		deckOfCards.addCard (xtraWhat);
	

		deckOfCards.addCard (xtraWhen);
		deckOfCards.addCard (xtraWho);

	}


	//to reuse for int, just replace card with int [] d and deck arr with int [] arr
	void exchange(Deck arr, int i, int j){
		Card d = arr.cards[i];
		arr.cards[i] = arr.cards[j];
		arr.cards[j] = d;
	}

	public void shuffleDeck(){
		int j;

		for(int l = 0; l < 4; l++)
		{
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			j = Random.Range (0,deckOfCards.cards.Length);
			exchange (deckOfCards, i, j);
		}
		
		}


		
	}
	//check point that works
	/*
	public Deck getRumourForHacker(Deck h){
		int l = h.lastCardIndex();
		int addLimit = 0;
		int totalAddLimit = 2;
		bool repeated;
		//Debug.Log ("inside getRumour. Last index in the deck- "+l);

		for(int dIndex = 0; dIndex < deckOfCards.cards.Length; dIndex++){
			repeated = false;
			if(isRumour(deckOfCards.cards[dIndex]))
			{	
			//	Debug.Log (deckOfCards.cards[dIndex].desc + " is a rumour!");
				for(int hIndex = 0; hIndex < h.cards.Length; hIndex++){
//					Debug.Log ("index - "+ hIndex);
//					Debug.Log ("dcard ("+deckOfCards.cards[dIndex].desc+")");
//					Debug.Log ("hcard ("+h.cards[hIndex].desc+")");
					if( (deckOfCards.cards[dIndex].desc == h.cards[hIndex].desc)&& h.cards[hIndex].desc != null)
					{
								repeated = true;
		//						Debug.Log ("Repeated card.");
								hIndex = h.cards.Length;
					}
				}
			
				if((repeated == false)){
					//	if( (deckOfCards.cards[dIndex].desc != h.cards[hIndex].desc))
					//	{
					
		//			Debug.Log ("Unique card.");
					
					h.cards[(l+addLimit)] = deckOfCards.cards[dIndex];
					//Debug.Log ("added card.");
		//			deckOfCards.cards[dIndex] = null;
					deckOfCards.cards[dIndex] = new Card();						
					addLimit+=1;
					
					if(addLimit >= totalAddLimit)
					{
						dIndex = deckOfCards.cards.Length;
					}

			}

				

			//}
			
		}
		}
		
			return h;
		
		
	}
*/

	public Deck getRumourForHacker(Deck h){
		int l = h.lastCardIndex();
		int addLimit = 0;
		int totalAddLimit = 2;
		bool repeated;
	
		for(int dIndex = 0; dIndex < deckOfCards.cards.Length; dIndex++){
			repeated = false;
			if(isRumour(deckOfCards.cards[dIndex]))
			{	
				for(int hIndex = 0; hIndex < h.cards.Length; hIndex++){
					/*if( (deckOfCards.cards[dIndex].desc == h.cards[hIndex].desc)&& h.cards[hIndex].desc != null)
					{
						repeated = true;
						hIndex = h.cards.Length;
					}*/
					if( ( (deckOfCards.cards[dIndex].desc == h.cards[hIndex].desc)&& (deckOfCards.cards[dIndex].type == h.cards[hIndex].type))
					   					&& h.cards[hIndex].desc != null)
					{
						repeated = true;
						hIndex = h.cards.Length;
					}
				}
				
				if((repeated == false)){
					h.cards[(l+addLimit)] = deckOfCards.cards[dIndex];
					deckOfCards.cards[dIndex] = new Card();						
					addLimit+=1;
					
					if(addLimit >= totalAddLimit)
					{
						dIndex = deckOfCards.cards.Length;
					}
					
				}

				
			}
		}
		
		return h;
		
		
	}


	public void dealCards(){


		Deck d1 = new Deck (6);
		Deck d2 = new Deck (6);

		//Deck d3 = new Deck (6);
		Deck d3 = new Deck(6);
		Deck d4 = new Deck (6);

		//hacker1 = genFactHand(hacker1);
		//dealHackers(d1, d2);
		//deckOfCards = deckOfCards.resizeDeck(deckOfCards);

		Debug.Log(" ");
		d3.genRumourHand(deckOfCards);
		Debug.Log ("D3's hand:");
		//
		for(int i = 0; i < d3.cards.Length; i++){
			Debug.Log (d3.cards[i].desc);
		}

		Debug.Log ("D4's hand:");
		
		d4.genRumourHand (deckOfCards); //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
		for(int i = 0; i < d4.cards.Length; i++){
			Debug.Log (d4.cards[i].desc);
		}
		deckOfCards = deckOfCards.resizeDeck();


		d1 = genFactHand(deckOfCards);

		d1 = getRumourForHacker(d1); //need to do asap. not done properly...
		Debug.Log ("hand1");
		for(int i = 0; i < 6; i++)
		{
			Debug.Log (d1.cards[i].desc + " type- "+ d1.cards[i].type);
		}
		d2 = genFactHand(d2);
		d2 = getRumourForHacker(d2);
		Debug.Log ("hand2");

		for(int i = 0; i < 6; i++)
		{
			Debug.Log (d2.cards[i].desc + " type- "+ d1.cards[i].type);
		}



		/*
		Debug.Log ("new number deck");
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			Debug.Log (deckOfCards.cards[i].desc+ " -"+deckOfCards.cards[i].type);
		}*/
/*
		Debug.Log("");
		d3 = d3.genRumourHand(deckOfCards);
	Debug.Log ("D3's hand:");
//
		for(int i = 0; i < d3.cards.Length; i++){
			Debug.Log (d3.cards[i].desc);
		}

		Debug.Log ("D4's hand:");

	d4 = d4.genRumourHand (deckOfCards); //$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
		for(int i = 0; i < d4.cards.Length; i++){
				Debug.Log (d4.cards[i].desc);
			}
*/
	}


	public Deck genFactHand(Deck md){
		Deck dummy = new Deck(6);

		dummy.cards[0]= md.getCard(deckOfCards, whereC, whereFcard[0].type);
		dummy.cards[1]= md.getCard(deckOfCards, whoC, whoFcard[0].type);
		dummy.cards[2]= md.getCard(deckOfCards, whenC, whenFcard[0].type);
		dummy.cards[3]= md.getCard(deckOfCards, whatC, whatFcard[0].type);
		return dummy;
	}

	public bool isFact(Card i){
		if(i.type == 'f')
			return true;
		else
		return false;
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

}
	