using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour{

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
	public UIWidget pcw;
	//pickCharacter menu buttons
	public UIButton btnTopL;
	public UIButton btnTopR;
	public UIButton btnBotL;
	public UIButton btnBotR;
	public UIButton btnMid;
	public UIButton btnStart;


	bool btnTopLPicked;
	bool btnTopRPicked;
	bool btnBotLPicked;
	bool btnBotRPicked;
	bool btnMidPicked;
	bool btnStartPicked;

	bool uiwIsInvis;
	public UILabel lblRound;
	//counter to make sure btnstart can be enabled
	int count;
	public string p1;
	public string p2;
	public string p3;
	public string p4;
	public string p5;
	int [,]ppl;//used to assign single/multiple targets. clears when new round happens.
	//int [] ppl;



	//manages suspicion chart widget
	public UIWidget scw; //contains user info with hp bar
	public UIWidget uhw;
	public UIButton btnP1;
	public UIButton btnP2;
	public UIButton btnP3;
	public UIButton btnP4;
	public UIButton btnP5;
	public UISprite test;


	public UI2DSprite p1pt1;
	public UI2DSprite p1pt2;
	public UI2DSprite p1pt3;
	public UI2DSprite p1pt4;
	public UI2DSprite p1pt5;
	public UI2DSprite p1pt6;
	public UI2DSprite p1pt7;
	public UI2DSprite p1pt8;
	public UI2DSprite p1pt9;
	public UI2DSprite p1pt10;
	public UI2DSprite p1pt11;
	public UI2DSprite p1pt12;
	public UI2DSprite p1pt13;
	public UI2DSprite p1pt14;
	public UI2DSprite p1pt15;
	public UI2DSprite p1pt16;
	public UI2DSprite p1pt17;
	public UI2DSprite p1pt18;
	public UI2DSprite p1pt19;
	public UI2DSprite p1pt20;
	public UI2DSprite p1pt21;
	public UI2DSprite p1pt22;
	public UI2DSprite p1pt23;
	public UI2DSprite p1pt24;
	public UI2DSprite p1pt25;
	public UI2DSprite p1pt26;
	public UI2DSprite p1pt27;
	public UI2DSprite p1pt28;
	public UI2DSprite p1pt29;
	public UI2DSprite p1pt30;
	public UI2DSprite p1pt31;
	public UI2DSprite p1pt32;

	public UI2DSprite p2pt1 = new UI2DSprite();
	public UI2DSprite p2pt2;
	public UI2DSprite p2pt3;
	public UI2DSprite p2pt4;
	public UI2DSprite p2pt5;
	public UI2DSprite p2pt6;
	public UI2DSprite p2pt7;
	public UI2DSprite p2pt8;
	public UI2DSprite p2pt9;
	public UI2DSprite p2pt10;
	public UI2DSprite p2pt11;
	public UI2DSprite p2pt12;
	public UI2DSprite p2pt13;
	public UI2DSprite p2pt14;
	public UI2DSprite p2pt15;
	public UI2DSprite p2pt16;
	public UI2DSprite p2pt17;
	public UI2DSprite p2pt18;
	public UI2DSprite p2pt19;
	public UI2DSprite p2pt20;
	public UI2DSprite p2pt21;
	public UI2DSprite p2pt22;
	public UI2DSprite p2pt23;
	public UI2DSprite p2pt24;
	public UI2DSprite p2pt25;
	public UI2DSprite p2pt26;
	public UI2DSprite p2pt27;
	public UI2DSprite p2pt28;
	public UI2DSprite p2pt29;
	public UI2DSprite p2pt30;
	public UI2DSprite p2pt31;
	public UI2DSprite p2pt32;

	//game round values
	public bool pickedAction;
	public int actIndex; 
	public UISprite cardInfo_sprite;
	public UIButton btnEndTurn;
	public UIButton btnResetChoice;
	public UIButton ac0;
	public UIButton ac1;
	public UIButton ac2;
	public UIButton ac3;
	public UIButton ac4;
	public UIButton ac5;

	public bool singleTarget;
	public int singleTargetVictim;

	public bool stackDmg;
	public bool dmgAll;

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
	// Use this for initialization
	void Start () {
		//p1SuspicionPoints = new UI2DSprite[32];
		//p1SuspicionPoints = new UI2DSprite[32];//GameObject.Find ("pointWidget_suspect1").GetComponentsInParent<UI2DSprite>();
//		p1pts = new UISprite[2];
		//asshole = new UISprite[1];


	//	p2pt1.sprite2d.name = GameObject.Find ("s2p1").GetComponent<UI2DSprite>().sprite2D.name ;
		singleTargetVictim = -1;
		singleTarget = false;
		stackDmg = false;
		dmgAll = false;

		actIndex = -1;
		round = 0;
		maxRound = 10;
		turn = 0;
		users = new Player[5];
		ppl = new int[users.Length,1];
		//ppl = new int[users.Length];
		for(int i = 0; i < users.Length; i++){
			users[i] = new Player();
		}
		charSelect = true;//ppl pick their characters 

		//this will be false when character select works
		//roundBegins = false; //game loop
	
		showResult = false; //time to display game result

		uiwIsInvis = false;
	//	btnBotLPicked = false;
	//	btnTopLPicked = false;
	//	btnTopRPicked = false;
	//	btnBotRPicked = false;
	//	btnStart.enabled = false;
	//	btnStartPicked = false;

	
		btnStart.defaultColor = btnStart.disabledColor;
		scw.alpha = 0.0f;
		scw.enabled = false;
		uhw.alpha = 0.0f;
		uhw.enabled = false;

		pickedAction = false;
		disableScwBtns();
		p1 = " ";
		p2 = " ";
		p3 = " ";
		p4 = " ";
		p5 = " ";
		count = 0;

		ac0picked = false;
		ac1picked = false;
		ac2picked = false;
		ac3picked = false;
		ac4picked = false;
		ac5picked = false;
	



		//reSetP1SuspicionPoints();
	}
	
	// Update is called once per frame
	void Update () {

		//this only applies when character selection works
		/*
		if(charSelect == true)
		{
			if(count< 5)
			{
				btnStart.defaultColor = btnStart.disabledColor;
			}
			else
			if(count>= 5)
			{
				btnStart.defaultColor = dummy;
				btnStart.enabled = true;
			}
			

			Debug.Log ("btnstartpicked is: "+btnStartPicked );
			
			if( (btnStartPicked == true) ) {
				if(scw.alpha < 1 ){	
					Debug.Log ("true");
					pcw.alpha-=0.01f;

				}

				if(pcw.alpha <= 0.01f && scw.alpha < 1 && uhw.alpha < 1)
				{
					scw.alpha +=0.01f;
					uhw.alpha+= 0.01f;
					if(scw.alpha> 1 && uhw.alpha > 1)
					{
						roundBegins = true;
						charSelect = false;
						enableScwBtns();
						scw.enabled = true;
						uhw.enabled = true;
						btnStart.enabled = false;
					}
				}
			}
		}
*/

		if(roundBegins == true)
		{
			if(actIndex ==-1)
			{
				btnEndTurn.enabled = false;
				btnEndTurn.defaultColor = btnEndTurn.disabledColor;
			}
			else
				if(actIndex >-1)
			{
				btnEndTurn.enabled = true;
				btnEndTurn.defaultColor = new Color(136,218,69,255);
			}
				//GameObject.Find("rounds").GetComponent<UILabel>().text = "Round "+round.ToString();
				if(round < maxRound){
				 GameObject.Find("rounds").GetComponent<UILabel>().text = "Round "+ round;
				if(turn <5){

					if(turn == 0){
					//	GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 1's turn";
						disableButton(btnP1);

					}

					if(turn == 1){
					//	GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 2's turn";
						enableButton(btnP1);
						disableButton(btnP2);
					}

					if(turn ==2){
					//	GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 3's turn";

						enableButton(btnP2);
						disableButton(btnP3);
					}
					
					if(turn ==3){
					//	GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 4's turn";

						enableButton(btnP3);
						disableButton(btnP4);
					}

					if(turn ==4){
					//	GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 5's turn";

						enableButton(btnP4);
						disableButton(btnP5);
					}
				}
				else
					if(turn ==5){
					Debug.Log ("over 5 turns.");
					enableButton(btnP5);
					combatPhase();
					revealacArr();
					round++;
				}
			}
			else
				if(round>= maxRound){
				roundBegins = false;
			}
		}
		else
		if(roundBegins == false)
		{
			//Debug.Log("end of game");
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

	public void disableScwBtns(){
	
		ac0.enabled = false;
		ac1.enabled = false;
		ac2.enabled = false;
		ac3.enabled = false;
		ac4.enabled = false;
		ac5.enabled = false;

	}


	public void enableScwBtns(){
		
		ac0.enabled = true;
		ac1.enabled = true;
		ac2.enabled = true;
		ac3.enabled = true;
		ac4.enabled = true;
		ac5.enabled = true;
	
	}

	public void checkacArrelectUserDisable(){

	}

	public void showCardAction(){

	}

	void reSetP1SuspicionPoints()
	{
		//GameObject.Find("pointWidget_suspect1").GetComponentsInParents<UI2DSprite>();
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
				Debug.Log ("single heal");
			}

			if(users[turn].hand.acArr[actIndex].isBlock == true)
			{
				users[turn].isDefending = true;
				Debug.Log ("single block ");
			}

			if(users[turn].hand.acArr[actIndex].isSnitch() == true)
			{
				Debug.Log ("snitch everyone");
			}
			
		}

		Debug.Log ("Turn- "+turn);
		resetTargetColor();
		resetActionCardColor();
		turn++;

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
		turn = 0;
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
							for(int x = 0; x < users.Length; x++)
							{
								if(x!= j && users[x].hand.acArr[ppl[x,0]].isSnitch() == true){
									Debug.Log ("user index "+x+" is damaged from snitch.");
									users[x].receiveDmg(3);
								}
							}

						}
						users[j].usedAction = true;
						// end of j snitches on everyone 
					}

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

		//	setCooldownOnCard(i,ppl[i,0])
		}





	//	GameObject.Find("skill_descr_suspect1").GetComponent<UILabel>().text = users[0].suspLvl.ToString();
	//	GameObject.Find("skill_descr_suspect2").GetComponent<UILabel>().text = users[1].suspLvl.ToString();
	//	GameObject.Find("skill_descr_suspect3").GetComponent<UILabel>().text = users[2].suspLvl.ToString();
	//	GameObject.Find("skill_descr_suspect4").GetComponent<UILabel>().text = users[3].suspLvl.ToString();
	//	GameObject.Find("skill_descr_suspect5").GetComponent<UILabel>().text = users[4].suspLvl.ToString();

//		GameObject.Find ("pointWidget_suspect1").GetComponentInParent<UI2DSprite>().GetComponent("1").get = "None";




	}

	//public void setCooldownOnCard


	public void setPlayerPoints(int userIndex){

		Debug.Log("here");
		switch(userIndex){
		case 0: 	Debug.Log("uerIndex = 0");
				for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
				{	Debug.Log("gain dept");
				asshole[i].depth= 2;
				}
				
				for(int j = users[userIndex].suspLvl-1; j < asshole.Length;j++)//lose
			{
				Debug.Log (userIndex+" is adjusting health loss index->" +j);
				asshole[j].depth= -3;
				}
				; break;

		case 1: Debug.Log("uerIndex = 1");
				for(int i = 0; i < users[userIndex].suspLvl-1; i++)//gain
				{
				asshole2[i].depth= 2;
				}
				
			for(int j = users[userIndex].suspLvl-1; j < asshole2.Length;j++)//lose
			{Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole2[j].depth= -3;
				}
				; break;

		case 2: Debug.Log("uerIndex = 2");
				for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{Debug.Log("gain suspicion index"+ i);
				asshole2[i].depth= 2;
				}
				
				for(int j = users[userIndex].suspLvl-1; j < asshole2.Length;j++)//lose
			{Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole2[j].depth= -3;
				}
				; break;

		case 3: Debug.Log("uerIndex = 3");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{
				asshole3[i].depth= 2;
			}
			
			for(int j = users[userIndex].suspLvl-1; j < asshole3.Length;j++)//lose
			{Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole3[j].depth= -3;
			}
			; break;

		case 4: Debug.Log("uerIndex = 4");
			for(int i = 0; i < users[userIndex].suspLvl; i++)//gain
			{
				asshole4[i].depth= 2;
			}
			
			for(int j = users[userIndex].suspLvl-1; j < asshole4.Length;j++)//lose
			{Debug.Log (userIndex+" is adjusting health index->" +j);
				asshole4[j].depth= -3;
			}
			; break;
		default: ;break;
		}
	}




	public int blameDmg(int userIndex, int m)
	{
		Debug.Log("blame dmg for user index - "+userIndex+" multiplier #"+m);

			if(m == 2)
		{Debug.Log("return 2");
				return 2;
		}
		else
			if(m == 3)
		{	Debug.Log("return 3");	return 4;
		}else 
			if(m==4)
		{Debug.Log("return 7");
			return 7;
		}
		else
			return 0;
	}

	
	public void clickStart(){
		Debug.Log ("starting game.");
		btnStartPicked = true;
		
		users[0].setRole(p1);
		users[1].setRole (p2);
		users[2].setRole(p3);
		users[3].setRole (p4);
		users[4].setRole (p5);
		for(int i = 0; i < users.Length; i++){
			Debug.Log (users[i].role);
		}
		
		count = 0;
		pcw.enabled = false;
		
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
			case 4: 
				p5 = GameObject.Find ("name").GetComponentInParent<UILabel>().text;
				btnTopLPicked = true;
				Debug.Log("player5 is: "+p5);
				users[4].setRole(p4);
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
			case 4: 
				p5 = GameObject.Find ("name2").GetComponentInParent<UILabel>().text;
				btnBotLPicked = true;
				Debug.Log("player5 is: "+p5);
				users[4].setRole(p5);
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
			case 4: 
				p5 = GameObject.Find ("name3").GetComponentInParent<UILabel>().text;
				btnBotLPicked = true;
				Debug.Log("player5 is: "+p5);
				users[4].setRole(p5);
				break;
		}
			btnBotL.defaultColor = btnBotL.disabledColor;
			count++;
		}
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
			case 4: 
				p5 = GameObject.Find ("name4").GetComponentInParent<UILabel>().text;
				btnBotRPicked = true;
				Debug.Log("player5 is: "+p5);
				users[4].setRole(p5);
				break;
			}
			btnBotR.defaultColor = btnBotR.disabledColor;
			count++;
		}
	}
		public void ClickProfile5(){
			
		if(btnMidPicked == false)
			{
				switch(count){
				case 0: 
					p1 = GameObject.Find ("name5").GetComponentInParent<UILabel>().text;
				btnMidPicked = true;
					Debug.Log("player1 is: "+p1);
					users[0].setRole(p1);
					break;
					
				case 1: 
					p2 = GameObject.Find ("name5").GetComponentInParent<UILabel>().text;
				btnMidPicked = true;
					Debug.Log("player2 is: "+p2);
					users[1].setRole(p2);
					break;  
				case 2: 
					
					p3 = GameObject.Find ("name5").GetComponentInParent<UILabel>().text;
				btnMidPicked = true;
					Debug.Log("player3 is: "+p3);
					users[2].setRole(p3);
					break;
				case 3: 
					p4 = GameObject.Find ("name5").GetComponentInParent<UILabel>().text;
				btnMidPicked = true;
					Debug.Log("player4 is: "+p4);
					users[3].setRole(p4);
					break;
				case 4: 
					p5 = GameObject.Find ("name5").GetComponentInParent<UILabel>().text;
				btnMidPicked = true;
					Debug.Log("player5 is: "+p5);
					users[4].setRole(p5);
					break;
				}
				btnMid.defaultColor = btnMid.disabledColor;
				count++;
			}

	}


}
