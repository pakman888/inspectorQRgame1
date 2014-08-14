using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;
using System;

public class gameManager : MonoBehaviour{
	string path;
	string fileName;
	
	//general game players
	public Player [] users;
	// show gamestate
	public bool charSelect;//ppl pick their characters 
	public bool roundBegins = true; //game loop
	public bool showResult;//
	//round number
	int round;
	int maxRound;
	int turn;
	
	//the container to hold character select widget
	public UIWidget intro_wid;
	public UIWidget turn_wid;
	//pickCharacter menu buttons
	
	public UIButton btnStart;
	
	public UIButton btnNextRound;
	
	public UISprite []revealP1Targets;
	public UISprite []revealP2Targets;
	public UISprite []revealP3Targets;
	public UISprite []revealP4Targets;
	public UISprite []revealP5Targets;
	
	
	
	
	
	bool btnStartPicked;
	
	bool uiwIsInvis;
	public UILabel lblRound;
	//counter to make sure btnstart can be enabled
	int count;
	int [,]ppl;//used to assign single/multiple targets. clears when new round happens.
	//int [] ppl;
	
	
	
	//manages suspicion chart widget
	public UIWidget msw; //contains user info with hp bar
	public UIWidget mrw;
	public UIWidget end;
	
	//used to inidcate next user's turn
	public UIWidget turnWid;
	public UISprite []nextUserTurn;
	public UIButton btnNxtTrn;
	public bool nextTurn;
	
	public UIButton btnP1;
	public UIButton btnP2;
	public UIButton btnP3;
	public UIButton btnP4;
	public UIButton btnP5;
	public UISprite test;
	
	//game round values
	public bool pickedAction;
	public int actIndex; 
	public UISprite cardInfo_sprite;
	public UIButton btnEndTurn;
	public UIButton ac0;
	public UIButton ac1;
	public UIButton ac2;
	public UIButton ac3;
	public UIButton ac4;
	public UIButton ac5;
	public bool movSelWid;
	
	
	
	public bool singleTarget;
	public int singleTargetVictim;
	
	public bool stackDmg;
	public bool dmgAll;
	public bool showCards;
	public bool ac0picked;
	public bool ac1picked;
	public bool ac2picked;
	public bool ac3picked;
	public bool ac4picked;
	public bool ac5picked;
	
	public UISprite []asshole;
	public UISprite []asshole2;
	public UISprite []asshole3;
	public UISprite []asshole4;
	public UISprite []asshole5;
	
	public UISprite []assholeCpy;
	public UISprite []assholeCpy2;
	public UISprite []assholeCpy3;
	public UISprite []assholeCpy4;
	public UISprite []assholeCpy5;
	
	public UISprite card_player1Choice;
	public UISprite card_player2Choice;
	public UISprite card_player3Choice;
	public UISprite card_player4Choice;
	public UISprite card_player5Choice;
	public bool cardsOnCD;
	public bool movRevWid;
	
	
	public bool isCombat;
	// Use this for initialization
	void Start () {
		revealP1Targets = new UISprite[4];
		revealP2Targets = new UISprite[4];
		revealP3Targets = new UISprite[4];
		revealP4Targets = new UISprite[4];
		revealP5Targets = new UISprite[4];
		
		
		
		fileName = "/saveWinnerIndex.txt";
		
		
		movRevWid = false;
		nextTurn = true;
		cardsOnCD = false;
		isCombat = false;
		showCards = false;
		singleTargetVictim = -1;
		singleTarget = false;
		stackDmg = false;
		dmgAll = false;
		btnNextRound.enabled = false;
		actIndex = -1;
		movSelWid = false;
		round = 0;
		maxRound = 2;
		turn = 0;
		users = new Player[5];
		ppl = new int[users.Length,1];
		for(int i = 0; i < users.Length; i++){
			users[i] = new Player();
		}
		cardInfo_sprite.alpha = 0f;
		charSelect = true;//ppl pick their characters 
		
		//this will be false when character select works
		//roundBegins = false; //game loop
		
		showResult = false; //time to display game result
		
		uiwIsInvis = false;
		
		btnStart.defaultColor = btnStart.disabledColor;
		msw.alpha = 0.0f;
		msw.enabled = false;
		mrw.alpha = 0.0f;
		mrw.enabled = false;
		
		
		pickedAction = false;
		disablemswBtns();
		
		count = 0;
		
		ac0picked = false;
		ac1picked = false;
		ac2picked = false;
		ac3picked = false;
		ac4picked = false;
		ac5picked = false;
	}
	
	
	
	void showTurnWidget(){
		turn_wid.enabled = true;
		turn_wid.alpha = 1.0f;
		btnNxtTrn.enabled = true;
		
		if(turn== 0)
			nextUserTurn[turn].depth = 10;
		else
			if(turn >0)
		{
			nextUserTurn[turn-1].depth = -3;
			nextUserTurn[turn].depth = 10;
		}
	}
	
	
	void hideTurnWidget(){
		turn_wid.enabled = false;
		turn_wid.alpha = 0f;
		btnNxtTrn.enabled = false;
		movSelWid = true;
	}
	
	void showMovSelWid(){
		Debug.Log ("show move select widget");
		msw.enabled = true;
		msw.alpha = 1f;
	}
	
	void hideMovSelWid(){
		Debug.Log ("hiding move select widget");
		msw.enabled = false;
		msw.alpha = 0.0f;
	}
	
	void hideMovRevWid(){
		mrw.alpha = 0.0f;
		mrw.enabled = false;
		btnNextRound.enabled = false;
	}
	
	void showMovRevWid(){
		mrw.enabled = true;
		mrw.alpha = 1.0f;
		btnNextRound.enabled = true;
		
	}
	// Update is called once per frame
	void Update () {
		
		if(roundBegins == true){
			GameObject.Find("rounds").GetComponent<UILabel>().text = "Round "+ (round+1);
			if(round < maxRound)
			{
				if(turn <5){
					
					if(nextTurn == true)
					{
						Debug.Log ("showing next turn");
						hideMovSelWid();
						showTurnWidget();
					}
					
					if(movSelWid== true)
					{
						Debug.Log ("showing move select widget");
						hideTurnWidget();
						showMovSelWid();
						ckeckUserBtnDisable();
					}
				}
				else
				if(turn ==5){
					movRevWid = true;
						hideTurnWidget();
						hideMovSelWid();
				}
				
				if(movRevWid == true)
				{
					Debug.Log("MovRevWid = true");
						showMovRevWid();
						movRevWid = false;
						revealacArr();
						showCards = true;
						isCombat = true;
						movRevWid = false;
				}
				if(showCards == true)
				{
					//	Debug.Log ("show cards is true");
							if(isCombat == true)
									combatPhase();	
					//if(cardsOnCD == true)
				//{
			//		reduceCardCoolDown();
			//	}*/
				}
				
			}
			else
			if(round>= maxRound){
				roundBegins = false;
			}
		}
		else
		if(roundBegins == false){
			//Debug.Log("end of game");
			hideMovSelWid();
			hideMovSelWid();
			hideTurnWidget();
			
			string results = " ";
			for(int i = 0; i < users.Length ;i++)
			{
				results += users[i].suspLvl.ToString() +"\n";
			}
			
			path = Application.dataPath+ fileName;
			
			System.IO.File.WriteAllText(path,results);
			Application.LoadLevel("ending");
			
			
		}

		
	}

	/*
	public void reduceCardCoolDown(){
		for(int i = 0; i < users.Length; i++)
		{
			for(int cards = 0; cards < 6; cards++)
			{
				if(users[i].hand.acArr[ppl[cards,0]].cdRemain > 0)
				{
					users[i].hand.acArr[ppl[cards,0]].cdRemain-=1;
				}
			}
		}

*/

	
	public void btnNextTurn()
	{
		movSelWid = true;
	}
	
	public void nextRound(){
		round++;
		
		Debug.Log ("next round. Hiding moveRev && movSel");
		hideMovRevWid();
		hideMovSelWid();
		showCards = false;
		movRevWid = false;
		movSelWid = false;
		nextTurn= true;
	//	resetPlayerTargetReveal();
		
		turn = 0;
		//Debug.Log ("next round clicked");
	}
	
	
	void playerTargetReveal(int userIndex){
		
		if(users[userIndex].hand.acArr[ppl[userIndex,0]].isSnitch() == true){
			//snitching everyone
			for(int i = 0; i < 4; i++)
			{
				if(userIndex == 0)
				{
					revealP1Targets[i].depth = 14;
				}
				
				if(userIndex == 1)
				{
					revealP2Targets[i].depth = 14;
				}
				if(userIndex == 2)
				{
					revealP3Targets[i].depth = 14;
				}
				if(userIndex == 3)
				{
					revealP4Targets[i].depth = 14;
				}
				if(userIndex == 4)
				{
					revealP5Targets[i].depth = 14;
				}
				
			}
		}
		
		if(users[userIndex].hand.acArr[ppl[userIndex,0]].isBlame() == true || users[userIndex].hand.acArr[ppl[userIndex,0]].isAllegation() == true
		   || users[userIndex].hand.acArr[ppl[userIndex,0]].isTaunt() == true)
		{
			if(userIndex == 0)
			{
				switch(users[userIndex].atkUserIndex){
				case 1:  revealP1Targets[1].depth = 14;break;
				case 2:  revealP1Targets[0].depth = 14;break;
				case 3:  revealP1Targets[2].depth = 14;break;
				case 4:  revealP1Targets[3].depth = 14;break;
				default: ;break;
				}

				//revealP1Targets[users[userIndex].atkUserIndex].depth = 14;
			}
			
			if(userIndex == 1)
			{
				switch(users[userIndex].atkUserIndex){
				case 0:  revealP2Targets[1].depth = 14;break;
				case 2:  revealP2Targets[2].depth = 14;break;
				case 3:  revealP2Targets[2].depth = 14;break;
				case 4:  revealP2Targets[3].depth = 14;break;
				default: ;break;
				}
				//revealP2Targets[users[userIndex].atkUserIndex].depth = 14;
			}
			if(userIndex == 2)
			{
				revealP3Targets[users[userIndex].atkUserIndex].depth = 14;
			}
			if(userIndex == 3)
			{
				revealP4Targets[users[userIndex].atkUserIndex].depth = 14;
			}
			if(userIndex == 4)
			{
				revealP5Targets[users[userIndex].atkUserIndex].depth = 14;
			}
			
		}
	}
	
	
	
	void resetPlayerTargetReveal()
	{
		for(int i = 0; i < users.Length; i++)
		{
			for(int j = 0; j < users.Length; j++)
			{
				switch(i){
				case 0: revealP1Targets[j].depth = -3; break;
				case 1: revealP2Targets[j].depth = -3;break;
				case 2: revealP3Targets[j].depth = -3;break;
				case 3: revealP4Targets[j].depth = -3;break;
				case 4: revealP5Targets[j].depth = -3;break;
				default: ;break;
				}
			}
		}
	}
	
	
	
	
	
	void cardChoiceReveal(int userIndex){
		if(userIndex ==0){
			switch(ppl[userIndex,0]  ){
			case 0: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;break;
			case 1: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;break;
			case 2: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;break;
			case 3: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;break;
			case 4: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;break;
			case 5: GameObject.Find ("move_suspect1").GetComponent<UISprite>().spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;break;
			default: Debug.Log ("user 1 didn't pick a proper action");break;
			}
		}
		if(userIndex ==1){
			switch(ppl[userIndex,0] ){
			case 0: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;break;
			case 1: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;break;
			case 2: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;break;
			case 3: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;break;
			case 4: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;break;
			case 5: GameObject.Find ("move_suspect2").GetComponent<UISprite>().spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;break;
				
				default: Debug.Log ("user 2 didn't pick a proper action");break;			}
		}
		if(userIndex ==2){
			switch(ppl[userIndex,0]  ){
			case 0: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;break;
			case 1: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;break;
			case 2: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;break;
			case 3: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;break;
			case 4: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;break;
			case 5: GameObject.Find ("move_suspect3").GetComponent<UISprite>().spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;break;
				
			default: Debug.Log ("user 3 didn't pick a proper action");break;
			}
		}
		if(userIndex ==3){
			switch(ppl[userIndex,0] ){
			case 0: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;break;
			case 1: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;break;
			case 2: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;break;
			case 3: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;break;
			case 4: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;break;
			case 5: GameObject.Find ("move_suspect4").GetComponent<UISprite>().spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;break;
			default: Debug.Log ("user 4 didn't pick a proper action");break;
			}
		}
		if(userIndex ==4){
			switch(ppl[userIndex,0] ){
			case 0: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;break;
			case 1: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;break;
			case 2: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;break;
			case 3: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;break;
			case 4: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;break;
			case 5: GameObject.Find ("move_suspect5").GetComponent<UISprite>().spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;break;
				
			default: Debug.Log ("user 5 didn't pick a proper action");break;
			}
		}
		
	}
	
	
	void ckeckUserBtnDisable(){
		if(turn == 0){
			disableButton(btnP1);
		}
		
		if(turn == 1){
			enableButton(btnP1);
			disableButton(btnP2);
		}
		
		if(turn ==2){
			enableButton(btnP2);
			disableButton(btnP3);
		}
		
		if(turn ==3){
			
			enableButton(btnP3);
			disableButton(btnP4);
		}
		
		if(turn ==4){
			enableButton(btnP4);
			disableButton(btnP5);
		}
	}
	
	
	
	public void disableButton(UIButton btn)
	{
		btn.defaultColor = btn.disabledColor;
		btn.enabled = false;
		
	}
	
	public void enableButton(UIButton btn){
		btn.defaultColor = Color.white;
		btn.enabled = true;
		
	}
	
	public void disablemswBtns(){
		
		ac0.enabled = false;
		ac1.enabled = false;
		ac2.enabled = false;
		ac3.enabled = false;
		ac4.enabled = false;
		ac5.enabled = false;
		
	}
	
	
	public void enablemswBtns(){
		
		ac0.enabled = true;
		ac1.enabled = true;
		ac2.enabled = true;
		ac3.enabled = true;
		ac4.enabled = true;
		ac5.enabled = true;
		
	}
	
	//functions for the action card buttons
	//each action should reveal if it targets other people or self target
	//each action should reveal if it has multiple target effect. 
	//if target 1 person, pick 1 person and frame indicting default person is either player 2 or 1. cannot self inflict.
	//if target 2 persons, pick 1 and then select 2nd person. if click again, border reassigns from 1st to 2nd person. 
	//default persons is player 1 & 2, 1 & 3, and 2 & 3. 
	//if target everyone, every1 except user gets a border.
	
	public void clickConfirm(){
		//users[count].atkUserIndex = 
		Debug.Log("player " + turn+" is locking in action - "+actIndex);
		ppl[turn,0] = actIndex;
		
		if(singleTarget == true)
		{
			if(users[turn].hand.acArr[actIndex].isBlame())
			{
				Debug.Log ("blame target user is: "+singleTargetVictim);
				users[turn].atkUserIndex = singleTargetVictim;
				users[singleTargetVictim].multiplier+=1;
				Debug.Log ("current multiplier for user "+singleTargetVictim+": "+users[singleTargetVictim].multiplier);
			}
			else
				if(users[turn].hand.acArr[actIndex].isAllegation() || users[turn].hand.acArr[actIndex].isTaunt())
			{
				Debug.Log ("single target user is: "+singleTargetVictim);
				users[turn].atkUserIndex = singleTargetVictim;
				users[singleTargetVictim].receiveUserIndexAttack = turn;
			}
			
		}
		else
			if(singleTarget == false)
		{
			if(users[turn].hand.acArr[actIndex].isHeal == true)
			{
				users[turn].isHealing = true;
				users[turn].usedHealSnitchDef = false;
				users[turn].hand.acArr[ppl[turn,0]].limit -=1;
				users[turn].atkUserIndex = turn;

				Debug.Log ("single heal");
			}
			
			if(users[turn].hand.acArr[actIndex].isBlock == true)
			{
				users[turn].isDefending = true;
				users[turn].atkUserIndex = turn;
				Debug.Log ("single block ");
			}
			
			if(users[turn].hand.acArr[actIndex].isSnitch() == true)
			{
				Debug.Log ("snitch everyone");
			}
			
		}
		
		Debug.Log ("Turn- "+turn);
		cardInfo_sprite.alpha = 0f;
		turn++;
		resetTargetColor();
		resetActionCardColor();//colour reset is working-ish, but it does not reset after turn or round is over.
		
		movSelWid = false;
		nextTurn = true;
	}
	
	void resetActionCardIndex(){
		
		ac0picked = false;
		ac1picked = false;
		ac2picked = false;
		ac3picked = false;
		ac4picked = false;
		ac5picked = false;
	}
	
	void resetActionCardColor(){
		Debug.Log("reset colour");
		
		/*
		for(int i = 0; i < 6; i++)
		{
			if(users[turn].hand.acArr[i].cdRemain > 0)
			{
				disableActionCard(i);
			}
		}*/
		//original shit
		
		GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
		
		GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
		
		GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
		
		GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
		
		GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
		
		GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);	
		
	}
	
	void disableActionCard(int cardIndex){
		switch(cardIndex){
		case 0: 
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().enabled = false;
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);
			break;
		case 1: 
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_taunt").GetComponent<UIButton>().enabled = false;
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);break;
		case 2:
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().enabled = false;	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);break;
		case 3:
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_blame").GetComponent<UIButton>().enabled = false;	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);break;
		case 4: 
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_reason").GetComponent<UIButton>().enabled = false;	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);break;
		case 5:
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.gray;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);break;
			GameObject.Find ("card_defend").GetComponent<UIButton>().enabled = false;	
		default:
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().enabled = true;
			
			GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_taunt").GetComponent<UIButton>().enabled = true;
			
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_snitchAll").GetComponent<UIButton>().enabled = true;
			
			GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_blame").GetComponent<UIButton>().enabled = true;
			
			GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);	
			GameObject.Find ("card_reason").GetComponent<UIButton>().enabled = true;
			
			GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor=  Color.white;
			GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("card_defend").GetComponent<UIButton>().enabled = true;
			break;
		}
	}
	
	
	void resetTargetColor(){
		Debug.Log("reset target colour");
		
		if(turn == 0){
			disableButton(btnP1);
			//GameObject.Find ("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);	
			
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().UpdateColor(true,true);
			
		}
		
		if(turn == 1){
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			
			disableButton(btnP2);
			
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().UpdateColor(true,true);
			
		}
		
		if(turn == 2){
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			
			disableButton(btnP3);
			
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor	= Color.white;
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().UpdateColor(true,true);
			
		}
		
		if(turn == 3){
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			
			disableButton(btnP4);
			
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().UpdateColor(true,true);
			
		}
		if(turn == 4){
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
			
			disableButton(btnP5);
			
		}
		
		
		
	}
	
	
	public void selectSingleUser(){
		
		singleTarget = true;
		if(users[turn].hand.acArr[actIndex].isAllegation())
		{
			Debug.Log("Allegation targetting");
			//resetTargetColor();
			if(turn == 0){
				
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 1){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 2){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 3){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}
		
		if(users[turn].hand.acArr[actIndex].isTaunt())
		{
			resetTargetColor();
			if(turn == 0){
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 1){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 2){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 3){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}
		
		if(users[turn].hand.acArr[actIndex].isBlame())
		{
			resetTargetColor();
			if(turn == 0){
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover =new Color(255,129,0,255);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 1){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 2){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 3){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){
				
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}
		
	}
	
	
	public void snitchAll(){
		
		singleTarget= false;
		//resetTargetColor();
		if(turn == 0){
			Debug.Log("p2,p3,p4,p5 will be damaged from snitch");
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
		}
		
		if(turn ==1)
			Debug.Log("p1,p3,p4,p5 will be damaged from snitch");
		{
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
		}
		
		if(turn == 2){
			Debug.Log("p1,p2,p4,p5 will be damaged from snitch");
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
		}
		
		if(turn == 3){
			Debug.Log("p1,p2,p3,p5 will be damaged from snitch");
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.red;
		}
		
		if(turn == 4){Debug.Log("p1,p2,p3,p4 will be damaged from snitch");
			GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.red;
			GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
		}
		
	}
	
	public void blameTarget(){
		singleTarget = true;
		
	}
	
	public void healUser(){
		singleTarget = false;
		
		//resetTargetColor();
		
		if(users[turn].hand.acArr[actIndex].isHeal == true)
		{	if(turn == 0){
				Debug.Log("may heal p1");
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().disabledColor= new Color(62f,255f,0f,255f);
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(true,false);
				
				//GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.black;
				//	GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
				
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(true,false);
				
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(true,false);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(true,false);
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(true,false);
			}
			
			else
				if(turn ==1)
					
			{
				Debug.Log("may heal p2");
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
			}
			else
			if(turn == 2){
				Debug.Log("may heal p3");
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
			}
			else
			if(turn == 3){
				Debug.Log("may heal p4");
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
				
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(false,true);
			}
			else
			if(turn == 4){
				Debug.Log("may heal p5");
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(false,true);
			}
			singleTargetVictim = turn;
		}
		
		
	}
	
	public void blockUser(){
		singleTarget = false;
		singleTargetVictim = turn;
	}
	
	public void checkTargetUsers(int act){
		
		if(users[turn].hand.acArr[act].isAllegation() == true)
		{
			selectSingleUser();
		}
		
		if(users[turn].hand.acArr[act].isTaunt ()== true)
		{
			Debug.Log("Taunt");
			selectSingleUser();
		}
		
		if(users[turn].hand.acArr[act].isSnitch() == true)
		{
			Debug.Log("Snitch");
			snitchAll();
		}
		
		if(users[turn].hand.acArr[act].isBlame () == true)
		{
			Debug.Log("Blame");
			blameTarget();
		}
		
		if(users[turn].hand.acArr[act].isHeal == true)
		{	
			Debug.Log("Heal");
			healUser();
		}
		if(users[turn].hand.acArr[act].isBlock == true){
			Debug.Log("Block");
			blockUser();
		}
		
		
	}
	
	
	public void clickTarget0(){
		//	resetTargetColor();
		if(turn!= 0 && pickedAction == true)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 0;
				resetTargetColor();
				users[turn].atkUserIndex = 0;
				users[0].receiveUserIndexAttack = turn;
				
				Debug.Log("Player "+ turn+ "will strike player : "+singleTargetVictim);
				GameObject.Find ("button_suspect1").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("button_suspect1").GetComponent<UIButton>().UpdateColor(true,true);
				
				
			}
			
			
		}
	}
	
	public void clickTarget1(){
		//	resetTargetColor();
		if(turn != 1 && pickedAction == true)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 1;
				resetTargetColor();
				users[turn].atkUserIndex =1;
				users[1].receiveUserIndexAttack = turn;
				Debug.Log("Player "+ turn+ "will strike player : "+singleTargetVictim);
				GameObject.Find ("button_suspect2").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("button_suspect2").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}
	
	public void clickTarget2(){
		//resetTargetColor();
		if(turn != 2 && pickedAction == true)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 2;
				resetTargetColor();
				users[turn].atkUserIndex = 2;
				users[2].receiveUserIndexAttack = turn;
				
				Debug.Log("Player "+ turn+ "will strike player : "+singleTargetVictim);
				GameObject.Find ("button_suspect3").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("button_suspect3").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}
	
	public void clickTarget3(){
		if(turn!=3 && pickedAction == true)
		{resetTargetColor();
			if(singleTarget== true)
			{
				singleTargetVictim = 3;
				resetTargetColor();
				users[turn].atkUserIndex = 3;
				users[3].receiveUserIndexAttack = turn;
				
				Debug.Log("Player "+ turn+ "will strike player : "+ singleTargetVictim);
				GameObject.Find ("button_suspect4").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("button_suspect4").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}
	
	public void clickTarget4(){
		if(turn != 4 && pickedAction == true)
		{//resetTargetColor();
			if(singleTarget== true)
			{
				singleTargetVictim = 4;
				resetTargetColor();
				users[4].receiveUserIndexAttack = turn;
				users[turn].atkUserIndex = 4;
				
				Debug.Log("Player "+ turn+ "will strike player : "+singleTargetVictim);
				GameObject.Find ("button_suspect5").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("button_suspect5").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}
	
	
	public void pickAction0(){//allegation
		resetActionCardColor();
		resetActionCardIndex();
		cardInfo_sprite.alpha = 1f;
		ac0picked = true;
		pickedAction = true;
		
		GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_makeAllegation").GetComponent<UIButton>().UpdateColor(true,true);
		
		actIndex = 0;
		Debug.Log(actIndex);
		//cardInfo_sprite = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;
		cardInfo_sprite.spriteName = GameObject.Find ("card_makeAllegation").GetComponent<UISprite>().spriteName;
		actIndex = 0;
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	public void pickAction1(){//taunt
		ac1picked = true;
		resetActionCardColor();
		pickedAction = true;
		cardInfo_sprite.alpha = 1f;
		GameObject.Find ("card_taunt").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_taunt").GetComponent<UIButton>().UpdateColor(true,true);
		actIndex = 1;
		Debug.Log(actIndex);
		cardInfo_sprite.spriteName = GameObject.Find ("card_taunt").GetComponent<UISprite>().spriteName;
		
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
		
		
	}
	
	public void pickAction2(){//snitch
		ac2picked = true;
		pickedAction = false;
		resetActionCardColor();
		cardInfo_sprite.alpha = 1f;
		GameObject.Find ("card_snitchAll").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_snitchAll").GetComponent<UIButton>().UpdateColor(true,true);
		
		actIndex = 2;
		Debug.Log(actIndex);
		cardInfo_sprite.spriteName = GameObject.Find ("card_snitchAll").GetComponent<UISprite>().spriteName;
		
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	public void pickAction3(){//blame
		ac3picked = true;
		resetActionCardColor();
		pickedAction = true;
		cardInfo_sprite.alpha = 1f;
		GameObject.Find ("card_blame").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_blame").GetComponent<UIButton>().UpdateColor(true,true);
		
		actIndex = 3;
		Debug.Log(actIndex);
		cardInfo_sprite.spriteName = GameObject.Find ("card_blame").GetComponent<UISprite>().spriteName;
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	public void pickAction4(){//heal
		ac4picked = true;
		pickedAction = false;
		resetActionCardColor();
		cardInfo_sprite.alpha = 1f;
		GameObject.Find ("card_reason").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_reason").GetComponent<UIButton>().UpdateColor(true,true);
		
		actIndex = 4;
		Debug.Log(actIndex);
		cardInfo_sprite.spriteName = GameObject.Find ("card_reason").GetComponent<UISprite>().spriteName;
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	public void pickAction5(){//defend
		ac5picked = true;
		pickedAction = false;
		resetActionCardIndex();
		resetActionCardColor();
		cardInfo_sprite.alpha = 1f;
		GameObject.Find ("card_defend").GetComponent<UIButton>().defaultColor = Color.yellow;
		GameObject.Find ("card_defend").GetComponent<UIButton>().UpdateColor(true,true);
		actIndex = 5;
		Debug.Log(actIndex);
		cardInfo_sprite.spriteName = GameObject.Find ("card_defend").GetComponent<UISprite>().spriteName;
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	//--------------------------------------------------------------------------------------
	
	public void revealacArr(){
		Debug.Log ("reveal cards");
		
		for(int i = 0; i < 5; i++)
		{
			cardChoiceReveal(i);
			playerTargetReveal(i);
			
		}Debug.Log ("next round btn enabled");
		
		
	}
	
	public int comparePriorities(int i, int j)
	{
		if(i > j)
			return 1;
		else
			if(i < j)
				return -1;
		else
			return 0;
		
	}
	
	
	
	public void combatPhase(){
		for(int i = 0; i < ppl.Length; i++){
			Debug.Log("player index "+i+"'s action index- "+ppl[i,0]+" targetting user index "+users[i].atkUserIndex);
		}
		
		for(int u = 0; u < users.Length; u++)//check to see if they are defending against anyone
		{//allegation and taunt should work.
			for(int j = 0; j < users.Length; j++)//check to see if anyone is striking anyone
			{
				if(j!=u)
				{
					
					if( (users[j].atkUserIndex == u) && (users[j].hand.acArr[ppl[j,0] ].isAllegation()) ) //is user j allegating user u?
					{//allegation fight
						
						//			Debug.Log ("Player "+j+" wants to allegate player "+u);
						if(thirdPartyTaunt(j) == true)
						{
							users[j].usedAction = true;
						}
						else
							if((users[u].isDefending == true && users[u].usedAction == false) )
						{
							users[u].usedAction = true;
							users[j].usedAction = true;
							Debug.Log ("Player "+u+" defended against allegation from player "+j);
						}
						else
							if( (users[u].isDefending == true && users[u].usedAction == true) && thirdPartyTaunt(j) == false)
						{	
							if(users[u].taunted == false)
							{
								Debug.Log ("Player "+u+" got allegated by player "+j);
								users[u].receiveDmg(1);
							}
							else
								if(users[u].taunted == true)
							{
								Debug.Log ("Player "+u+" got allegated by player "+j+ " and received 4 damage");
								users[u].receiveDmg(4);
								users[u].taunted = false;
							}
							
							users[j].usedAction = true;
						}
						else
							if(users[u].isDefending == false)
						{
							Debug.Log ("Player "+u+" got allegated without defending by player "+j);
							
							if(users[u].taunted == false)
							{
								//Debug.Log ("Allegation damage is 1");
								users[u].receiveDmg(1);
							}
							else
								if(users[u].taunted == true)
							{
								//					Debug.Log ("Allegation damage is 4");
								users[u].taunted = false;
								users[u].receiveDmg(4);
								
							}
							
							if(users[u].isHealing == true && users[u].usedAction == false){
								Debug.Log ("allegating user index "+u+" for healing and they got damaged for it");
								users[u].receiveDmg(2);
								users[u].usedAction = true;
								
							}
							else
							if(users[u].isHealing == true && users[u].usedAction == true){
								Debug.Log ("allegating user index "+u+" for attempted healing");
								users[u].usedAction = true;
								users[u].receiveDmg(1);
							}
							users[j].usedAction = true;
							
						}
						
						
						
					}
					
					if( (users[j].atkUserIndex == u) && (users[j].hand.acArr[ppl[j,0] ].isTaunt() &&  users[j].usedAction == false) ) 
					{//is user U being taunted?thirdPartyTaunt(j)
						//			Debug.Log ("Player "+j+" wants to taunt player "+u);
						if(users[u].isDefending == true && users[u].usedAction == false)
						{
							Debug.Log ("Player "+u+" defended against taunt from player "+j);
							users[u].usedAction = true;
							users[j].usedAction = true;
						}
						else
							if(users[u].isDefending == true && users[u].usedAction == true)
						{	
							Debug.Log ("Player "+u+" got taunted by player "+j);
							users[u].taunted = true;
							users[j].usedAction = true;
						}
						else
							if(users[u].isDefending == false)
						{	
							
							if(users[u].isHealing == true && users[u].usedAction == false)
							{
								Debug.Log ("negated u's heal with a taunt");
								users[u].usedAction = true;
							}
							else
								if(users[u].isHealing == true && users[u].usedAction == true)
							{
								Debug.Log ("taunted u who tried to.");
								users[u].taunted = true;
								users[j].usedAction = true;
							}
							else{
								Debug.Log ("Player "+u+" got taunted without defending by player "+j);
								users[u].taunted = true;
								users[j].usedAction = true;}
						}
					}
					
					if(users[j].hand.acArr[ppl[j,0] ].isBlame() && users[j].atkUserIndex == u && users[j].usedAction == false)//is user j blaming user u?
					{
						Debug.Log ("Player index "+j+" wants to blame player index "+u);
						Debug.Log("");
						if( thirdPartyTaunt(j) == true ) 
						{//blame fight reduction
							
							Debug.Log ("Player index "+j+" got taunted and player index "+u+" loses 1 multiplier");
							users[users[j].atkUserIndex].multiplier-=1;
							//Debug.Log ("player index "+u+"'s current multiplier: "+users[users[j].atkUserIndex].multiplier);
						}
						else
							if( thirdPartyTaunt(j) == false ) 
						{//blame fight reduction
							
							Debug.Log ("Player index "+j+" blamed player index "+u+" and u +1 multiplier");
							
							
						}
						Debug.Log ("player index "+u+"'s current multiplier: "+users[users[j].atkUserIndex].multiplier);
						users[j].usedAction = true;
					}
					//Debug.Log (users[u].multiplier +" people blammed user index "+u);
					if(users[j].hand.acArr[ppl[j,0] ].isSnitch() && users[j].usedAction == false)
					{																			// j snitches on everyone 
						if(multipleSnitch() == false)
						{
							Debug.Log ("only user index "+j+" is snitching.");
							for(int x = 0; x < users.Length; x++)
							{
								if(x!= j){
									
									if(users[x].isDefending == false ){
										
										
										//healing is true
										if(users[x].isHealing == true )
										{
											if(users[x].usedHealSnitchDef == true)
											{
												Debug.Log ("user index "+x+" was taunted and damaged from snitch.");
												users[x].receiveDmg(3);
											}
											else
												if(users[x].usedHealSnitchDef ==false && thirdPartyTaunt(x)==false )
											{
												Debug.Log ("user index "+x+" used their defend and isn't damaged from snitch.");
												users[x].usedHealSnitchDef = true;
											}
											else
												if(users[x].usedHealSnitchDef ==false && thirdPartyTaunt(x)==true )
											{
												Debug.Log ("user index "+x+" was taunted and was damaged from snitch.");
												users[x].receiveDmg(3);
											}
											
										}
										else
										if(users[x].isHealing == false){
											Debug.Log ("user index "+x+" isn't healing and is damaged from snitch.");
											users[x].receiveDmg(3);
										}
									}
									else
									if(users[x].isDefending == true){
										if(users[x].usedAction == false && thirdPartyTaunt(x) == false)
										{
											Debug.Log ("user index "+x+" defended from snitch.");
											users[x].usedAction = true;
										}
										else
											if(users[x].usedAction == false && thirdPartyTaunt(x) == true)
										{
											Debug.Log ("user index "+x+" was taunted and damaged from snitch.");
											users[x].usedAction = true;
										}
										else
											if(users[x].usedAction ==true )
										{
											Debug.Log ("user index "+x+" used their defend and is damaged from snitch.");
											users[x].receiveDmg(3);
										}
										
									}
								}
							}
						}
						else
							if(multipleSnitch()==true)
						{
							Debug.Log ("user index "+j+" isn't the only one snitching.");	
							//for(int x = 0; x < users.Length; x++)
							//{
							//if(x!= j && users[x].hand.acArr[ppl[x,0]].isSnitch() == true){
							Debug.Log ("user index "+j+" is damaged from snitch.");
							users[j].receiveDmg(3);
							//	}
							//	}
							
						}
						users[j].usedAction = true;
						// end of j snitches on everyone 
					}
					
					if(users[u].isHealing == true && negateHeal(u) == false)
					{
						Debug.Log ("heal success!");
						users[u].receiveHeal(2);
					}
					//end of the big if
				}
				
			}
			
			//need to inflict blame multiplier damage to everyone.
			
		}
		
		for(int i = 0; i < users.Length; i++){
			Debug.Log("User "+i+" final multiplier- "+users[i].multiplier);
			users[i].receiveDmg(blameDmg(i,users[i].multiplier));
			Debug.Log("User "+i+" final damage- "+users[i].suspLvl);
			setPlayerPoints(i);
			users[i].newRoundStatReset();
			
			//setCooldownOnCard(i);
		}
		
		
		
		
		
		//	GameObject.Find("skill_descr_suspect1").GetComponent<UILabel>().text = users[0].suspLvl.ToString();
		//	GameObject.Find("skill_descr_suspect2").GetComponent<UILabel>().text = users[1].suspLvl.ToString();
		//	GameObject.Find("skill_descr_suspect3").GetComponent<UILabel>().text = users[2].suspLvl.ToString();
		//	GameObject.Find("skill_descr_suspect4").GetComponent<UILabel>().text = users[3].suspLvl.ToString();
		//	GameObject.Find("skill_descr_suspect5").GetComponent<UILabel>().text = users[4].suspLvl.ToString();
		
		//		GameObject.Find ("pointWidget_suspect1").GetComponentInParent<UI2DSprite>().GetComponent("1").get = "None";
		isCombat = false;
		
		
		
	}
	
	public void setCooldownOnCard(int userIndex){
		
		users[userIndex].hand.acArr[ppl[userIndex,0]].cdRemain = users[userIndex].hand.acArr[ppl[userIndex,0]].cooldown;
	}
	
	
	public void setPlayerPoints(int userIndex){
		
		Debug.Log("here");
		switch(userIndex){
		case 0: 	Debug.Log("userIndex = 0");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{	//Debug.Log("gain dept");
				asshole[i].depth= 4;
				assholeCpy[i].depth = 4;
			}
			
			for(int j = users[userIndex].suspLvl; j < asshole.Length;j++)//lose
			{
				//Debug.Log (userIndex+" is adjusting health loss index->" +j);
				asshole[j].depth= -3;
				assholeCpy[j].depth = -3;
			}
			; break;
			
		case 1: Debug.Log("userIndex = 1");
			for(int i = 0; i < users[userIndex].suspLvl-1; i++)//gain
			{
				asshole2[i].depth= 4;
				assholeCpy2[i].depth = 4;
			}
			
			for(int j = users[userIndex].suspLvl; j < asshole2.Length;j++)//lose
			{//Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole2[j].depth= -3;
				assholeCpy2[j].depth = -3;
			}
			; break;
			
		case 2: Debug.Log("userIndex = 2");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{//Debug.Log("gain suspicion index"+ i);
				asshole3[i].depth= 4;
				assholeCpy3[i].depth = 4;
			}
			
			for(int j = users[userIndex].suspLvl; j < asshole2.Length;j++)//lose
			{//Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole3[j].depth= -3;
				assholeCpy3[j].depth = -3;
			}
			; break;
			
		case 3: Debug.Log("userIndex = 3");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{
				asshole4[i].depth= 4;
				assholeCpy4[i].depth = 4;
			}
			
			for(int j = users[userIndex].suspLvl; j < asshole3.Length;j++)//lose
			{//Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole4[j].depth= -3;
				assholeCpy4[j].depth = -3;
			}
			; break;
			
		case 4: Debug.Log("userIndex = 4");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{
				asshole5[i].depth= 4;
				assholeCpy5[i].depth = 4;
			}
			
			for(int j = users[userIndex].suspLvl; j < asshole4.Length;j++)//lose
			{//Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole4[j].depth= -3;
				assholeCpy5[j].depth = -3;
				
			}
			; break;
		default: ;break;
		}
	}
	
	
	
	
	public int blameDmg(int userIndex, int m)
	{
		Debug.Log("blame dmg for user index - "+userIndex+" multiplier #"+m);
		
		if(m == 2)
		{Debug.Log("return 3");
			return 3;
		}
		else
			if(m == 3)
		{	Debug.Log("return 5");	return 5;
		}else 
			if(m==4)
		{Debug.Log("return 8");
			return 8;
		}
		else
			return 0;
	}
	
	
	public void clickStart(){
		Debug.Log ("starting game.");
		btnStartPicked = true;
		
		
		count = 0;
		intro_wid.enabled = false;
		
	}
	
	public bool thirdPartyTaunt(int userIndex){
		bool foo = false;
		for(int i = 0; i < users.Length;i++){
			if(i!= userIndex){
				if( users[i].hand.acArr[ppl[i,0]].isTaunt() && users[i].atkUserIndex == userIndex ) //checking if attacker is being taunted
				{
					Debug.Log ("User index "+i+" prevented player "+userIndex+ " from attacking player "+users[userIndex].atkUserIndex);
					users[i].usedAction = true;
					foo= true;
					
				}
			}
			
			if(foo == true)
				break;
		}
		return foo;
	}
	
	
	public bool negateHeal(int userIndex){
		bool foo = false;
		for(int i = 0; i < users.Length;i++){
			if(i!= userIndex){
				if((users[userIndex].hand.acArr[ppl[userIndex,0]].isHeal == true) &&  (users[i].atkUserIndex == userIndex) )
					//checking if attacker is being taunted
				{
					Debug.Log ("user index "+i+" is targetting player index "+userIndex);
					if(users[i].hand.acArr[ppl[i,0]].isBlame() == true){
						Debug.Log ("User index "+i+" prevented player "+userIndex+ " from healing with a blame");
						users[i].usedAction = true;
						users[userIndex].usedAction = true;
						return true;
					}
					
					if(users[i].hand.acArr[ppl[i,0]].isAllegation() == true){
						Debug.Log ("User index "+i+" prevented player "+userIndex+ " from healing with an allegation");
						users[i].usedAction = true;
						users[userIndex].usedAction = true;
						return true;
					}
					
					if(users[i].hand.acArr[ppl[i,0]].isTaunt() == true)
						Debug.Log ("User index "+i+" prevented player "+userIndex+ " from healing with a taunt");
					users[i].usedAction = true;
					users[userIndex].usedAction = true;
					return true;
				}
			}
			
			
		}
		return foo;
	}
	
	
	public bool multipleSnitch() // if more than 1 person snitches, then everyone gains 3 suspicion pts
	{
		int count = 0;
		for(int i = 0; i < users.Length; i++)
		{
			if(users[i].hand.acArr[ppl[i,0]].isSnitch())
			{
				count++;
			}
		}
		
		return (count>1);
	}
	
	
	public bool singleSnitch(){//this is to let ppl who try to heal block a snitch attack
		int count = 0;
		for(int i = 0; i < users.Length; i++)
		{
			if(users[i].hand.acArr[ppl[i,0]].isSnitch())
			{
				count++;
			}
		}
		
		return (count==1);
	}
	
	
	
	
	
	
}
