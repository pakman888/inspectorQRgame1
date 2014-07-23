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

		/*
		Debug.Log ("printing out deck");
		for(int i = 0; i < deckOfCards.cards.Length; i++)
		{
			Debug.Log (deckOfCards.cards[i].desc + " type- "+ deckOfCards.cards[i].type);
		}
	*/
	}


	//to reuse for int, just replace card with int [] d and deck arr with int [] arr
	void exchange(Deck arr, int i, int j){
		Card d = arr.cards[i];
		arr.cards[i] = arr.cards[j];
		arr.cards[j] = d;
	}

	//int versinn
	/*
	public void shuffleDeck(){
		int j;
		for(int i = 0; i < num.Length; i++){
			j = Random.Range (0,24);
			exchange (num, i, j);
		}
		
		for(int i = 0; i < num.Length; i++){
			j = Random.Range (0,24);
			exchange (num, i, j);
		}

	}
*/
	public void shuffleDeck(){
		int j;
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			j = Random.Range (0,deckOfCards.cards.Length);
			exchange (deckOfCards, i, j);
		}
		
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			j = Random.Range (0,deckOfCards.cards.Length);
			exchange (deckOfCards, i, j);
		}
		
	}

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
		/*
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			for(int j = 0; j < h.cards.Length; j++)
			{

				if( isRumour(deckOfCards.cards[i]) && (deckOfCards.cards[i].desc != h.cards[j].desc) && (h.cards[j].desc!=" ") ){
					h.cards[l] = deckOfCards.cards[i];

					Debug.Log ("current card in deck- "+deckOfCards.cards[i].desc);

					Debug.Log ("adding first card- "+h.cards[l].desc);
					deckOfCards.cards[i] = null;
					deckOfCards.cards[i] = new Card();
					j = h.cards.Length;
					i = deckOfCards.cards.Length;
				}

			}

		}
		

		for(int i = 0; i < deckOfCards.cards.Length; i++){
			if(isRumour(deckOfCards.cards[i])){
				if(h.cards[l] == deckOfCards.cards[i])
				{;
				}
				else
				{
					h.cards[l+1] = deckOfCards.cards[i];
					deckOfCards.cards[i] = null;
					deckOfCards.cards[i] = new Card();
					i = deckOfCards.cards.Length;
				}
			}
		}
		*/
		return h;
		
		
	}


	public void dealCards(){


		Deck d1 = new Deck (6);
		Deck d2 = new Deck (6);

		Deck d3 = new Deck (6);
		Deck d4 = new Deck (6);

		//hacker1 = genFactHand(hacker1);
		//dealHackers(d1, d2);
		//deckOfCards = deckOfCards.resizeDeck(deckOfCards);
		d1 = genFactHand(deckOfCards);

		d1 = getRumourForHacker(d1); //need to do asap. not done properly...

		for(int i = 0; i < 6; i++)
		{
			Debug.Log (d1.cards[i].desc + " type- "+ d1.cards[i].type);
		}
		d2 = genFactHand(d2);
		d2 = getRumourForHacker(d2);


		for(int i = 0; i < 6; i++)
		{
			Debug.Log (d2.cards[i].desc + " type- "+ d1.cards[i].type);
		}

		deckOfCards = deckOfCards.resizeDeck();

		Debug.Log ("new number deck");
		for(int i = 0; i < deckOfCards.cards.Length; i++){
			Debug.Log (deckOfCards.cards[i].desc+ " -"+deckOfCards.cards[i].type);
		}

		Debug.Log("");
		d3 = d3.genRumourHand(deckOfCards);
	Debug.Log ("D3's hand:");
//
		for(int i = 0; i < d3.cards.Length; i++){
			Debug.Log (d3.cards[i].desc);
		}

		Debug.Log ("D4's hand:");

	d4 = d4.genRumourHand (deckOfCards);
		for(int i = 0; i < d4.cards.Length; i++){
				Debug.Log (d4.cards[i].desc);
			}


//		Debug.Log ("new number deck");
//		for(int i = 0; i < deckOfCards.cards.Length; i++){
//			Debug.Log (deckOfCards.cards[i].desc);
//		}
//
		//	num = resizeDeck(num);

		//when d1-d4 were int []
		/*
		Debug.Log ("new number deck");
		for(int i = 0; i < num.Length; i++){
			Debug.Log (num[i]);
		}
		d3 = genRumourHand(num);
		d4 = genRumourHand (num);
		//dealActivists (d3, d4);


		Debug.Log ("hacker1's hand ");
		for(int i = 0; i < d1.Length; i++){
			Debug.Log (d1[i]);
		}
		Debug.Log ("hacker2's hand ");
		for(int i = 0; i < d2.Length; i++){
			Debug.Log (d2[i]);
		}



		Debug.Log ("activist1's hand ");
		for(int i = 0; i < d3.Length; i++){
			Debug.Log (d3[i]);
		}
		Debug.Log ("activist2's hand ");
		for(int i = 0; i < d4.Length; i++){
			Debug.Log (d4[i]);
		}

		Debug.Log ("cards in deck ");
		for(int i = 0; i < num.Length; i++){
			Debug.Log (num[i]);
		}
		*/
	}

	//all that is greyed out is the backbone of playing with the deck
	/*
	public int lastIndex(int []n){
		int i = 0;
		while( (i < n.Length) && (n[i]!= 0) )
		{
			i++;
		}
		Debug.Log("Last index in array: "+i);
		return i;
	}


	public int getElement(int n){
		int j = -1;
		//int [] num = { 1,1,3,3, 5,5,7,7,00,00,      11,11,02,02,22,22, 06,06,66,66,04,       04,44,44};

		for(int i = 0; i < num.Length; i++){
			if(n ==num[i]){
				j = num[i];
				num[i] = -1;
				break;
			}
		}
		return j;
	}

	public int[] genFactHand(int []hacker){
		int []dummy = new int[6];
		bool repeated;
		dummy[0] = getElement (44);
		dummy[1] = getElement (11);
		dummy[2] = getElement(22);
		dummy[3] = getElement (66);

		return dummy;
	}
*/
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

	/*
	public bool isFact(int i){
		switch(i){
		case 11:
		case 22:
		case 44:
		case 66: return true; break;
		}
		return false;
	}

	public bool isRumour(int i){
		switch(i){
		case 0:
		case 1: 
		case 2: 
		case 3:
		case 4:
		case 5:
		case 6: return true; break;
		}
		return false;
	}

	public int[] getRumourForHacker(int []h){
		int l = lastIndex(h);

		int d1 = Random.Range(0,24);
		int dummy = num[d1];
		int d2 = Random.Range(0,24);
		int dummy2 = num[d2];

	
	for(int i = 0; i < num.Length; i++){
		if(isRumour(num[i])){
				h[l] = num[i];
				num[i] = -1;
				i = num.Length;
		}
	}
	

		for(int i = 0; i < num.Length; i++){
		if(isRumour(num[i])){
				if(h[l] == num[i])
				{;
				}
				else
				{
					h[l+1] = num[i];
					num[i] = -1;
					i = num.Length;
				}
		}
	}

		return h;


	}


	public int [] resizeDeck(int []i){ //converted to return deck


		int count = 0;

		for(int j = 0; j < i.Length; j++){
			if(i[j]!=-1){
				count++;
			}
		}

		int [] d = new int[count];
		int index = 0;

		for(int j = 0; j < i.Length; j++){
			if(i[j]!=-1){
				d[index] = i[j];
				index++;
			}
		}

		return d;
	}


	public int[] genRumourHand(int [] h){ // converted in deck file to return Deck
		int dummy;
		int dIndex;
		bool repeated;

	
		int size = (h.Length+1) /2;
		int []arr = new int[size];


		int numIndex = 0;
		int arrIndex = 0;

		while((numIndex < num.Length) && arrIndex < arr.Length )
		{
			if( (num[numIndex] == arr[arrIndex]) || (num[numIndex] == -1) )
			{
				numIndex++;
			}
			else
			if(num[numIndex] != arr[arrIndex])
			{
				dIndex = 0;
				repeated = false;
				while( (dIndex < arr.Length) && (arr[dIndex] != 0) && repeated == false)
				{
					if(arr[dIndex]!= num[numIndex])
					{
						dIndex++;

					}
					else
					if(arr[dIndex]== num[numIndex])
					{
						repeated = true;
						
					}
				}

				if(repeated == false)
				{
					arr[arrIndex] = num[numIndex];
					num[numIndex] = -1;
					arrIndex++;
					numIndex++;
				}
				else
					if(repeated == true)
				{
					numIndex ++;
				}
			}

		}

		return arr;
	}

*/
}
	