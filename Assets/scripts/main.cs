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
	Deck deckOfCards;



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
		makeDeck();


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

	}

	void initWArray(string [] arr){
		for(int i = 0; i < arr.Length; i++){
			arr[i] = ";"; 
		}
	}

	void addWhen(string t){
		if(when[0] == ";")
		{
			when[0] = t;
		}
		else
		if(when[1] == ";")
		{
			when[1] = t;
		}
		else
		if(when[2] == ";")
		{
			when[2] = t;
		}
		else
		if(when[3] == ";")
		{
			when[3] = t;
		}
		else
		if(when[4] == ";")
		{
			when[4] = t;
		}
		else
		if(when[5] == ";")
		{
			when[5] = t;
		}
		else
		if(when[6] == ";")
		{
			when[6] = t;
		}
	}

	void addWhat(string t){
		if(what[0] == ";")
		{
			what[0] = t;
		}
		else
		if(what[1] == ";")
		{
			what[1] = t;
		}else
		if(what[2] == ";")
		{
			what[2] = t;
		}else
		if(what[3] == ";")
		{
			what[3] = t;
		}else
		if(what[4] == ";")
		{
			what[4] = t;
		}else
		if(what[5] == ";")
		{
			what[5] = t;
		}else
		if(what[6] == ";")
		{
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

		whereC = where[i_whereC];
		whenC = when[i_whenC];
		whoC = who[i_whoC];
		whatC = what[i_whatC];


	}

	public void makeDeck(){
		Card [] whereFcard = new Fact[2];
		Card [] whereRcard = new Rumour[2];

	}

}
