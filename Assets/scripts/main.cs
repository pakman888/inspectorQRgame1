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

	void Start () {
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

		setGameInfo(Random.Range(0,7),Random.Range(0,7),Random.Range(0,7),Random.Range(0,7) );
		setXtraInfo(Random.Range(0,7),Random.Range(0,7),Random.Range(0,7),Random.Range(0,7) );
		makeDeck();
		shuffleDeck();
		dealCards();
	}
	
	// Update is called once per frame
	void Update () {

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

		xtraWhen= initRumours(xtraWhen, whenX);
		xtraWhere= initRumours(xtraWhere, whereX);
		xtraWhat= initRumours(xtraWhat, whatX);
		xtraWho= initRumours(xtraWho, whoX);

		//setting up the cards
		//where
//		whereFcard[0].desc = whereC; 
//		whereFcard[1].desc = whereC; 

//		whereRcard[0].desc = whereC;
//		whereRcard[1].desc = whereC;

		//when
		//for(int i = 0; i < whenFcard.Length; i++){
//		whenFcard[0].desc = whenC; 
//		whenFcard[1].desc = whenC; 
		//}
		//for(int i = 0; i < whenRcard.Length; i++){
//		whenRcard[0].desc = whenC;
//		whenRcard[1].desc = whenC;
		//}
		//who
		//for(int i = 0; i < whoFcard.Length; i++){
//			whoFcard[0].desc = whoC; 
//		whoFcard[1].desc = whoC; 
		//}
//		for(int i = 0; i < whoRcard.Length; i++){
//		whoRcard[0].desc = whoC;
//		whoRcard[1].desc = whoC;
//		}
		//what
//		for(int i = 0; i < whatFcard.Length; i++){
//		whatFcard[0].desc = whatC;
//		whatFcard[1].desc = whatC; 
//		}
//		for(int i = 0; i < whatRcard.Length; i++){
//			whatRcard[0].desc = whatC;
//		whatRcard[1].desc = whatC;

	//	Debug.Log (whereFcard[0].desc);
	//	Debug.Log (whereFcard[1].desc);

		//24 cards in total
		deckOfCards.addCard (whoFcard);
		deckOfCards.addCard (whereFcard);//add array
		deckOfCards.addCard (whenFcard);
		deckOfCards.addCard (whatFcard);
		//Debug.Log (deckOfCards.lastCardIndex());

		//deckOfCards.addCard (whereRcard[0]);

		deckOfCards.addCard (whoRcard);	
		deckOfCards.addCard (whatRcard);
		deckOfCards.addCard (whenRcard);
		deckOfCards.addCard (whereRcard);
	
		//Debug.Log (deckOfCards.lastCardIndex());

		deckOfCards.addCard (xtraWhen); //16
		deckOfCards.addCard (xtraWhat);
		//Debug.Log (deckOfCards.lastCardIndex());

		deckOfCards.addCard (xtraWhen);
		deckOfCards.addCard (xtraWho);
	
		//Debug.Log (deckOfCards.lastCardIndex());


		//		} 
	}

	void exchange(int [] arr, int i, int j){
		int d = arr[i];
		arr[i] = arr[j];
		arr[j] = d;
	}


	public void shuffleDeck(){
		int [] num = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24};
		int j;
		for(int i = 0; i < num.Length; i++){
			j = Random.Range (0,24);
			exchange (num, i, j);
		}
		
		for(int i = 0; i < num.Length; i++){
			j = Random.Range (0,24);
			exchange (num, i, j);
		}


		for(int i = 0; i < num.Length; i++){
			Debug.Log (num[i]);
		}
	}

	public void dealCards(){
	
	
	}
}