using UnityEngine;
using System.Collections;

public class Spy : MonoBehaviour {

	public int spy_when;
	public int spy_who;
	public int spy_what;
	public int spy_where;
	public bool suspect_answer;
	public bool returnAnswer;

	public void setFacts(string who, string when, string where, string what){
		setWho(who);
		setWhat(what);
		setWhen(when);
		setWhere(where);
	}

	public void setWho(string w){
		switch(w){
		case "person1": spy_who = 1; break;
		case "person2": spy_who = 2; break;
		case "person3": spy_who = 3; break;
		case "person4": spy_who = 4; break;
		case "person5": spy_who = 5; break;
		case "person6": spy_who = 6; break;
		case "person7": spy_who = 7; break;
		default: ;break;
		}
	
	}

	public void setWhat(string w){
		switch(w){
		case "action1": spy_what = 1; break;
		case "action2": spy_what = 2; break;
		case "action3": spy_what = 3; break;
		case "action4": spy_what = 4; break;
		case "action5": spy_what = 5; break;
		case "action6": spy_what = 6; break;
		case "action7": spy_what = 7; break;
		default: break;
		}
	}

	public void setWhere(string w){
		switch(w){
		case "place1": spy_where = 1; break;
		case "place2": spy_where = 2; break;
		case "place3": spy_where = 3; break;
		case "place4": spy_where = 4; break;
		case "place5": spy_where = 5; break;
		case "place6": spy_where = 6; break;
		case "place7": spy_where = 7; break;
		default:;break;
		}
	}

	public void setWhen(string w){
		switch(w){
		case "Monday": spy_when = 1; break;	
		case "Tuesday": spy_when = 2; break;
		case "Wednesday": spy_when = 3; break;
		case "Thursday": spy_when = 4; break;
		case "Friday": spy_when = 5; break;
		case "Saturday": spy_when = 6; break;
		case "Sunday": spy_when = 7; break;
		default: ;break;
		}
	}

	public int switchWhen(string w){
		int dummy;
		switch(w){
		case "Monday": dummy = 1; break;	
		case "Tuesday": dummy = 2; break;
		case "Wednesday": dummy= 3; break;
		case "Thursday": dummy = 4; break;
		case "Friday": dummy = 5; break;
		case "Saturday": dummy= 6; break;
		case "Sunday": dummy = 7; break;
		default: dummy = -1;break;
		}
		return dummy;

	}


	public int switchWho(string w){

		int dummy;
		switch(w){
		case "person1": dummy = 1; break;
		case "person2": dummy = 2; break;
		case "person3": dummy = 3; break;
		case "person4": dummy = 4; break;
		case "person5": dummy = 5; break;
		case "person6": dummy = 6; break;
		case "person7": dummy = 7; break;
		default: dummy = -1;break;
		}
		return dummy;
	}
	
	public int switchWhat(string w){
		int dummy;
		switch(w){
		case "action1": dummy = 1; break;
		case "action2": dummy = 2; break;
		case "action3": dummy = 3; break;
		case "action4": dummy = 4; break;
		case "action5": dummy = 5; break;
		case "action6": dummy = 6; break;
		case "action7": dummy = 7; break;
		default: dummy = -1;break;
		}

		return dummy;
	}
	
	public int switchWhere(string w){
		int dummy = -1;
		switch(w){
		case "place1": dummy = 1; break;
		case "place2": dummy = 2; break;
		case "place3": dummy = 3; break;
		case "place4": dummy = 4; break;
		case "place5": dummy = 5; break;
		case "place6": dummy = 6; break;
		case "place7": dummy = 7; break;
		default:;break;
		}
		return dummy;
	}


	
	public bool isWhat(string w){

		switch(w){
		case "action1": 
		case "action2": 
		case "action3": 
		case "action4": 
		case "action5": 
		case "action6": 
		case "action7": return true; break;
		default: return false;break;
		}
	}
	
	public bool isWhere(string w){
		switch(w){
		case "place1": 
		case "place2": 
		case "place3": 
		case "place4": 
		case "place5": 
		case "place6": 
		case "place7": return true; break;
		default:return false;break;
		}
	}

	
	public bool isWhen(string w){
		switch(w){
		case "Monday": 	
		case "Tuesday": 
		case "Wednesday": 
		case "Thursday": 
		case "Friday": 
		case "Saturday": 
		case "Sunday": return true; break;
		default: return false;break;
		}
		
	}

	public bool isWho(string w){
		
		switch(w){
		case "person1": 
		case "person2": 
		case "person3": 
		case "person4": 
		case "person5": 
		case "person6": 
		case "person7": return true; break;
		default: return false ;break;
		}
	}

	bool verify(string subj, bool ans){
		int topic = -1;
		bool dAnswer = false;
		if(isWhere (subj)){
			if( (switchWhere(subj) == spy_where )&& (ans == true)){
				dAnswer = true;
			}
		}
		if(isWhat (subj)){
			if( (switchWhat(subj) == spy_what )&& (ans == true)){
				dAnswer = true;
			}
		}
		if(isWhen (subj)){
			if( (switchWhen(subj) == spy_when )&& (ans == true)){
				dAnswer = true;
			}
		}
		if(isWho (subj)){
			if( (switchWho(subj) == spy_who )&& (ans == true)){
				dAnswer = true;
			}
		}

		return dAnswer;

	}

	// Use this for initialization
	void Start () {
		spy_when = -1;
		spy_who = -1;
		spy_where = -1;
		spy_what = -1;	
		suspect_answer = false;
		returnAnswer = false;
	}



	// Update is called once per frame
	void Update () {
	
	}
}
