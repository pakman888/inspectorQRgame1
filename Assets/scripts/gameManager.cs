using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

	//general game players
	public Player [] users;
	// show gamestate
	public bool charSelect;//ppl pick their characters 
	public bool roundBegins; //game loop
	public bool showResult;//
	//round number
	int round;
	int maxRound;


	//the container to hold character select widget
	public UIWidget uiw;
	//pickCharacter menu buttons
	public UIButton btnTopL;
	public UIButton btnTopR;
	public UIButton btnBotL;
	public UIButton btnBotR;
	public UIButton btnStart;


	bool btnTopLPicked;
	bool btnTopRPicked;
	bool btnBotLPicked;
	bool btnBotRPicked;
	bool btnStartPicked;
	bool uiwIsInvis;
	//counter to make sure btnstart can be enabled
	int count;
	public string p1;
	public string p2;
	public string p3;
	public string p4;
	int []ppl;//used to assign single/multiple targets. clears when new round happens.
	Color dummy;

	//manages suspicionchart widget
	public UIWidget scw; //contains user info with hp bar
	public UIWidget uhw;
	//game round values
	public bool pickedAction;
	public int actIndex; 
	public UIButton btnEndTurn;
	public UIButton btnResetChoice;
	public UIButton ac0;
	public UIButton ac1;
	public UIButton ac2;
	public UIButton ac3;
	public UIButton ac4;
	public UIButton ac5;
	public UIButton ac6;
	public UIButton ac7;
	public UIButton ac8;
	public UIButton ac9;

	public UIButton ac10;
	public UIButton ac11;
	public UIButton ac12;
	public UIButton ac13;
	public UIButton ac14;
	public UIButton ac15;
	public UIButton ac16;
	public UIButton ac17;
	public UIButton ac18;
	public UIButton ac19;


	// Use this for initialization
	void Start () {
		ppl = new int[users.Length];

		actIndex = -1;
		round = 0;
		maxRound = 10;
		users = new Player[4];
		for(int i = 0; i < users.Length; i++){
			users[i] = new Player();
		}
		charSelect = true;//ppl pick their characters 
		roundBegins = false; //game loop
		showResult = false; //time to display game result

		uiwIsInvis = false;
		btnBotLPicked = false;
		btnTopLPicked = false;
		btnTopRPicked = false;
		btnBotRPicked = false;
		btnStart.enabled = false;
		btnStartPicked = false;

		dummy = btnStart.defaultColor;
		btnStart.defaultColor = btnStart.disabledColor;
		scw.alpha = 0.0f;
		scw.enabled = false;

		pickedAction = false;
		disableScwBtns();
		p1 = " ";
		p2 = " ";
		p3 = " ";
		p4 = " ";
		count = 0;
	//	_sprite = _buttonStart.GetComponentInChildren();
	//	_sprite.enabled = !_spire.enabled;
	}
	
	// Update is called once per frame
	void Update () {

		if(charSelect == true)
		{
			if(count< 4)
			{
				btnStart.defaultColor = btnStart.disabledColor;
			}
			else
			if(count>= 4)
			{
				btnStart.defaultColor = dummy;
				btnStart.enabled = true;
			}
			

			if( (btnStartPicked == true) ) {
				if(scw.alpha < 1 && uiw.alpha > 0.01f){	
						uiw.alpha-=0.01f;
				}
				if(uiw.alpha < 0.01f && scw.alpha < 1 )
				{
					scw.alpha +=0.01f;

					if(scw.alpha> 1)
					{
						roundBegins = true;
						charSelect = false;
					}
				}
			}
		}

		if(roundBegins == true)
		{
			enableScwBtns();
			scw.enabled = true;
			if(count == 0){
				Debug.Log ("Player 1's turn");
			}

			if(count == 1){
				Debug.Log ("Player 2's turn");
			}

			if(count ==2){
				Debug.Log ("Player 3's turn");
			}
			
			if(count ==3){
				Debug.Log ("Player 4's turn");
			}

		}
	}


	public void disableScwBtns(){
	
		ac0.enabled = false;
		ac1.enabled = false;
		ac2.enabled = false;
		ac3.enabled = false;
		ac4.enabled = false;
		ac5.enabled = false;
		ac6.enabled = false;
		ac7.enabled = false;
		ac8.enabled = false;
		ac9.enabled = false;
		ac10.enabled = false;
		ac10.enabled = false;
		ac12.enabled = false;
		ac13.enabled = false;
		ac14.enabled = false;
		ac15.enabled = false;
		ac16.enabled = false;
		ac17.enabled = false;
		ac18.enabled = false;
		ac19.enabled = false;
	}

	public void enableScwBtns(){
		
		ac0.enabled = true;
		ac1.enabled = true;
		ac2.enabled = true;
		ac3.enabled = true;
		ac4.enabled = true;
		ac5.enabled = true;
		ac6.enabled = true;
		ac7.enabled = true;
		ac8.enabled = true;
		ac9.enabled = true;
		ac10.enabled = true;
		ac10.enabled = true;
		ac12.enabled = true;
		ac13.enabled = true;
		ac14.enabled = true;
		ac15.enabled = true;
		ac16.enabled = true;
		ac17.enabled = true;
		ac18.enabled = true;
		ac19.enabled = true;
	}

	//functions for the action card buttons
	//each action should reveal if it targets other people or self target
	//each action should reveal if it has multiple target effect. 
	//if target 1 person, pick 1 person and frame indicting default person is either player 2 or 1. cannot self inflict.
	//if target 2 persons, pick 1 and then select 2nd person. if click again, border reassigns from 1st to 2nd person. 
	//default persons is player 1 & 2, 1 & 3, and 2 & 3. 
	//if target everyone, every1 except user gets a border.

	public void clickConfirm(){


	}


	public void pickAction0(){
		if(pickedAction==false)
		{

			actIndex = int.Parse(GameObject.Find ("btn0").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	public void pickAction1(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn10").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	public void pickAction2(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn20").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
		}
	}
	
	public void pickAction3(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn30").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	public void pickAction4(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn40").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction5(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn50").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction6(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn60").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction7(){
		if(pickedAction==false)
		{actIndex = int.Parse(GameObject.Find ("btn70").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	public void pickAction8(){
		if(pickedAction==false)
			actIndex = int.Parse(GameObject.Find ("btn80").GetComponentInChildren<UILabel>().text);
		Debug.Log (actIndex);
		pickedAction = true;
	}
	
	public void pickAction9(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn90").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction10(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn100").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction11(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn110").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction12(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn120").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction13(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn130").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction14(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn140").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction15(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn150").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	public void pickAction16(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn160").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction17(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn170").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction18(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn180").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}
	
	public void pickAction19(){
		if(pickedAction==false)
		{
			actIndex = int.Parse(GameObject.Find ("btn190").GetComponentInChildren<UILabel>().text);
			Debug.Log (actIndex);
			pickedAction = true;
		}
	}

	//--------------------------------------------------------------------------------------


	public void pickTarget(Player p, int index){
	
	}

	public void revealCards(){
	}

	public void combatPhase(){
	}

	public void calcNewSuspicionPt(Player p, int index){ //reduce points
	}

	public void ClickProfile1(){

		if( btnTopLPicked == false)
		{

		switch(count){
		case 0: 
			p1 = GameObject.Find ("name").GetComponentInParent<UILabel>().text;
			btnTopLPicked = true;
			Debug.Log("player1 is: "+p1);
			users[0].setRole(p1);
			break;
			
		case 1: 
			p2 = GameObject.Find ("name").GetComponentInParent<UILabel>().text;
			btnTopLPicked = true;
			Debug.Log("player2 is: "+p2);
			users[1].setRole(p2);
			break;  
		case 2: 
			
			p3 = GameObject.Find ("name").GetComponentInParent<UILabel>().text;
			btnTopLPicked = true;
			Debug.Log("player3 is: "+p3);
			users[2].setRole(p3);
			break;
		case 3: 
			p4 = GameObject.Find ("name").GetComponentInParent<UILabel>().text;
			btnTopLPicked = true;
			Debug.Log("player4 is: "+p4);
			users[3].setRole(p4);
			break;
		}
			btnTopL.defaultColor = btnTopL.disabledColor;

			count++;
		}
	}


	public void ClickProfile2(){

		if(btnTopRPicked == false)
		{
			switch(count){
			case 0: 
				p1 = GameObject.Find ("name2").GetComponentInParent<UILabel>().text;
				btnTopRPicked = true;
				Debug.Log("player1 is: "+p1);
				users[0].setRole(p1);
				break;
				
			case 1: 
				p2 = GameObject.Find ("name2").GetComponentInParent<UILabel>().text;
				btnTopRPicked = true;
				Debug.Log("player2 is: "+p2);
				users[1].setRole(p2);
				break;  
			case 2: 
				
				p3 = GameObject.Find ("name2").GetComponentInParent<UILabel>().text;
				btnTopRPicked = true;
				Debug.Log("player3 is: "+p3);
				users[2].setRole(p3);
				break;
			case 3: 
				p4 = GameObject.Find ("name2").GetComponentInParent<UILabel>().text;
				btnTopRPicked = true;
				Debug.Log("player4 is: "+p4);
				users[3].setRole(p4);
				break;
			}
			btnTopR.defaultColor = btnTopR.disabledColor;
			count++;
		}
				
	}

	public void ClickProfile3(){
		if(btnBotLPicked == false)
		{
			switch(count){
			case 0: 
				p1 = GameObject.Find ("name3").GetComponentInParent<UILabel>().text;
				
				btnBotLPicked = true;
				Debug.Log("player1 is: "+p1);
				users[0].setRole(p1);
				break;
				
			case 1: 
				p2 = GameObject.Find ("name3").GetComponentInParent<UILabel>().text;
				btnBotLPicked = true;
				Debug.Log("player2 is: "+p2);
				users[1].setRole(p2);
				break;  
			case 2: 
				
				p3 = GameObject.Find ("name3").GetComponentInParent<UILabel>().text;
				btnBotLPicked = true;
				Debug.Log("player3 is: "+p3);
				users[2].setRole(p3);
				break;
			case 3: 
				p4 = GameObject.Find ("name3").GetComponentInParent<UILabel>().text;
				btnBotLPicked = true;
				Debug.Log("player4 is: "+p4);
				users[3].setRole(p4);
				break;
		}
			btnBotL.defaultColor = btnBotL.disabledColor;
			count++;
		}
	}


	public void clickStart(){
		Debug.Log ("starting game.");
		btnStartPicked = true;

		users[0].setRole(p1);
		users[1].setRole (p2);
		users[2].setRole(p3);
		users[3].setRole (p4);

		for(int i = 0; i < users.Length; i++){
			Debug.Log (users[i].role);
		}

		count = 0;
		uiw.enabled = false;

	}

	public void ClickProfile4(){

		if(btnBotRPicked == false)
		{
			switch(count){
			case 0: 
				p1 = GameObject.Find ("name4").GetComponentInParent<UILabel>().text;
				btnBotRPicked = true;
				Debug.Log("player1 is: "+p1);
				users[0].setRole(p1);
				break;
				
			case 1: 
				p2 = GameObject.Find ("name4").GetComponentInParent<UILabel>().text;
				btnBotRPicked = true;
				Debug.Log("player2 is: "+p2);
				users[1].setRole(p2);
				break;  
			case 2: 
				
				p3 = GameObject.Find ("name4").GetComponentInParent<UILabel>().text;
				btnBotRPicked = true;
				Debug.Log("player3 is: "+p3);
				users[2].setRole(p3);
				break;
			case 3: 
				p4 = GameObject.Find ("name4").GetComponentInParent<UILabel>().text;
				btnBotRPicked = true;
				Debug.Log("player4 is: "+p4);
				users[3].setRole(p4);
				break;
			}
			btnBotR.defaultColor = btnBotR.disabledColor;
			count++;
		}


	}


}
