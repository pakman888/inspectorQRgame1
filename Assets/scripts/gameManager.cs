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
	//counter to make sure btnstart can be enabled
	int count;
	public string p1;
	public string p2;
	public string p3;
	public string p4;
	public string p5;
	int [,]ppl;//used to assign single/multiple targets. clears when new round happens.
	//int [] ppl;

	Color dummy;

	//manages suspicion chart widget
	public UIWidget scw; //contains user info with hp bar
	public UIWidget uhw;
	public UIButton btnP1;
	public UIButton btnP2;
	public UIButton btnP3;
	public UIButton btnP4;
	public UIButton btnP5;

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

	// Use this for initialization
	void Start () {
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
	
	}
	
	// Update is called once per frame
	void Update () {

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

		if(roundBegins == true)
		{

				if(round < maxRound){
				 GameObject.Find("roundIndicator").GetComponent<UILabel>().text = "Round "+ round;
				if(turn <5){

					if(turn == 0){
						GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 1's turn";
						disableButton(btnP1);

					}

					if(turn == 1){
						GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 2's turn";
						enableButton(btnP1);
						disableButton(btnP2);
					}

					if(turn ==2){
						GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 3's turn";

						enableButton(btnP2);
						disableButton(btnP3);
					}
					
					if(turn ==3){
						GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 4's turn";

						enableButton(btnP3);
						disableButton(btnP4);
					}

					if(turn ==4){
						GameObject.Find("turnIndicator").GetComponent<UILabel>().text = "Player 5's turn";

						enableButton(btnP4);
						disableButton(btnP5);
					}
				}
				else
					if(turn ==5){
					enableButton(btnP5);
					turn = 0;
					combatPhase();
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
			Debug.Log("end of game");
		}
	}

	public void disableButton(UIButton btn)
	{

		dummy = btn.defaultColor;
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
				Debug.Log ("snitching everyone");
			}

			if(users[turn].hand.acArr[actIndex].isBlock == true)
			{
				users[turn].isDefending = true;
				Debug.Log ("snitching everyone");
			}

			if(users[turn].hand.acArr[actIndex].isSnitch() == true)
			{
				Debug.Log ("snitching everyone");
			}
			
		}

		resetTargetColor();
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
		GameObject.Find ("btn0").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("btn10").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("btn20").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("btn30").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("btn40").GetComponent<UIButton>().defaultColor=  Color.white;
		GameObject.Find ("btn50").GetComponent<UIButton>().defaultColor=  Color.white;
	}

	void resetTargetColor(){
		Debug.Log("reset target colour");

			if(turn == 0){
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= GameObject.Find ("playerSelect1").GetComponent<UIButton>().disabledColor;
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);	

			GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);

			}
			
			if(turn == 1){
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= GameObject.Find ("playerSelect2").GetComponent<UIButton>().disabledColor;
			GameObject.Find ("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);	

				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);

			}
			
			if(turn == 2){
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= GameObject.Find ("playerSelect3").GetComponent<UIButton>().disabledColor;
			GameObject.Find ("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);	
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor	= Color.white;
			GameObject.Find ("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);

			}
			
			if(turn == 3){
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= GameObject.Find ("playerSelect4").GetComponent<UIButton>().disabledColor;
			GameObject.Find ("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);	

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);

			}
			if(turn == 4){
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.white;
			GameObject.Find ("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= GameObject.Find ("playerSelect5").GetComponent<UIButton>().disabledColor;
			GameObject.Find ("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);	
			}
		

	
	}


	public void selectSingleUser(){
	
		singleTarget = true;
		if(users[turn].hand.acArr[actIndex].isAllegation())
		{
			Debug.Log("Allegation targetting");
			resetTargetColor();
			if(turn == 0){

					
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;

				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;

				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;

			}

			if(turn == 1){
					
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = Color.red;
					GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}

			if(turn == 2){
					
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}

			if(turn == 3){
					
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){
					
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = Color.red;
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}

		if(users[turn].hand.acArr[actIndex].isTaunt())
		{
			resetTargetColor();
			if(turn == 0){

				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 1){

				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 2){
			
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 3){

				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){

				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255F, 255F, 0.0F, 255F);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}

		if(users[turn].hand.acArr[actIndex].isBlame())
		{
			resetTargetColor();
			if(turn == 0){
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover =new Color(255,129,0,255);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 1){
				
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 2){
				
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			
			if(turn == 3){
				
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().pressed= Color.red;
				
			}
			if(turn == 4){
				
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().pressed= Color.red;
				
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().hover = new Color(255,129,0,255);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().pressed= Color.red;
				
			}
		}

	}


	public void snitchAll(){

		singleTarget= false;
			resetTargetColor();
			if(turn == 0){
				Debug.Log("p2,p3,p4,p5 will be damaged from snitch");
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
			}

			if(turn ==1)
				Debug.Log("p1,p3,p4,p5 will be damaged from snitch");
			{
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
			}

			if(turn == 2){
				Debug.Log("p1,p2,p4,p5 will be damaged from snitch");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
			}

			if(turn == 3){
				Debug.Log("p1,p2,p3,p5 will be damaged from snitch");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.red;
			}

			if(turn == 4){Debug.Log("p1,p2,p3,p4 will be damaged from snitch");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.red;
					GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
			}

	}

	public void blameTarget(){
		singleTarget = true;

	}

	public void healUser(){
		singleTarget = false;

			resetTargetColor();
		if(users[turn].hand.acArr[actIndex].isHeal == true)
		{	if(turn == 0){
				Debug.Log("may heal p1");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
				GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.black;
				GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);

				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);

			GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
			}
			
			else
			if(turn ==1)
				Debug.Log("may heal p2");
			{
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
						GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
		}/*
		else
			if(turn == 2){
				Debug.Log("may heal p3");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(false,true);
						GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
			GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(false,true);
			}
		else
			if(turn == 3){
				Debug.Log("may heal p4");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(false,true);

				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(false,true);
			}
			else
			if(turn == 4){Debug.Log("may heal p5");
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(false,true);
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.black;
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(false,true);
					GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= new Color(62,255,0,255);
			GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(false,true);
			}*/

		}


	}

	public void blockUser(){
		singleTarget = false;
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
	
		if(turn!= 0)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 0;
				resetTargetColor();
				users[turn].atkUserIndex = int.Parse (GameObject.Find("playerSelect1").GetComponentInChildren<UILabel>().text);
				Debug.Log("Player "+ turn+ "will strike player : "+int.Parse (GameObject.Find("playerSelect1").GetComponentInChildren<UILabel>().text));
				GameObject.Find ("playerSelect1").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("playerSelect1").GetComponent<UIButton>().UpdateColor(true,true);
			}


		}
	}

	public void clickTarget1(){
		
		if(turn != 1)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 1;
				resetTargetColor();
				users[turn].atkUserIndex = int.Parse (GameObject.Find("playerSelect2").GetComponentInChildren<UILabel>().text);
				Debug.Log("Player "+ turn+ "will strike player : "+int.Parse (GameObject.Find("playerSelect2").GetComponentInChildren<UILabel>().text));
				GameObject.Find ("playerSelect2").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("playerSelect2").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}

	public void clickTarget2(){

		if(turn != 2)
		{
		if(singleTarget== true)
		{
				singleTargetVictim = 2;
				resetTargetColor();
				users[turn].atkUserIndex = int.Parse (GameObject.Find("playerSelect3").GetComponentInChildren<UILabel>().text);
				Debug.Log("Player "+ turn+ "will strike player : "+int.Parse (GameObject.Find("playerSelect3").GetComponentInChildren<UILabel>().text));
				GameObject.Find ("playerSelect3").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("playerSelect3").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}

	public void clickTarget3(){
		if(turn!=3)
		{
		if(singleTarget== true)
		{
				singleTargetVictim = 3;
				resetTargetColor();
			users[turn].atkUserIndex = int.Parse (GameObject.Find("playerSelect4").GetComponentInChildren<UILabel>().text);
				Debug.Log("Player "+ turn+ "will strike player : "+int.Parse (GameObject.Find("playerSelect4").GetComponentInChildren<UILabel>().text));
				GameObject.Find ("playerSelect4").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("playerSelect4").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}

	public void clickTarget4(){
		if(turn != 4)
		{
			if(singleTarget== true)
			{
				singleTargetVictim = 4;
				resetTargetColor();
				users[turn].atkUserIndex = int.Parse (GameObject.Find("playerSelect5").GetComponentInChildren<UILabel>().text);
				Debug.Log("Player "+ turn+ "will strike player : "+int.Parse (GameObject.Find("playerSelect5").GetComponentInChildren<UILabel>().text));
				GameObject.Find ("playerSelect5").GetComponent<UIButton>().defaultColor= Color.red;
				GameObject.Find("playerSelect5").GetComponent<UIButton>().UpdateColor(true,true);
			}
		}
	}






	public void pickAction0(){//allegation
	
			actIndex = int.Parse(GameObject.Find ("btn0").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
			users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}

	public void pickAction1(){//taunt

			actIndex = int.Parse(GameObject.Find ("btn10").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
		
		users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
		
	
	}

	public void pickAction2(){//snitch
		
			actIndex = int.Parse(GameObject.Find ("btn20").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
			users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}
	
	public void pickAction3(){//blame
		
			actIndex = int.Parse(GameObject.Find ("btn30").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
			

			users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}

	public void pickAction4(){//heal

		
			actIndex = int.Parse(GameObject.Find ("btn40").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
			users[turn].hand.acArr[actIndex].showCard();
		checkTargetUsers(actIndex);
	}

	public void pickAction5(){//defend

			actIndex = int.Parse(GameObject.Find ("btn50").GetComponentInChildren<UILabel>().text);
		Debug.Log(actIndex);
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
			Debug.Log("player index"+i+"'s action index- "+ppl[i,0]);
		}

		int highestPriorityUser = -1;
		int secondPriorityUser = -1;
		int thirdPriorityUser = -1;
		int forthPriorityUser = -1;
		int lastPriorityUser = -1;

		for(int i = 0; i < ppl.Length; i++){
			Debug.Log ("checking user index "+i+" to see their priority");

			if(i ==0)
			{
				highestPriorityUser = i;

			}
			else
			if(i==1)
			{
				if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 1)
				{
					secondPriorityUser = highestPriorityUser;
					highestPriorityUser = i;

				}
				else
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == -1
					   || comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 0)
				{
					secondPriorityUser = i;
					highestPriorityUser = highestPriorityUser;
				}
			
			}
			else
			if(i==2)
			{ // i > top
				if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 1)
				{
					thirdPriorityUser = secondPriorityUser;
					secondPriorityUser = highestPriorityUser;
					highestPriorityUser = i;
					
				}// i < top or i == top
				else
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == -1 ||
					   comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 0   )
				{
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 1 )
					{//if i > 2nd
						thirdPriorityUser = secondPriorityUser;
						secondPriorityUser = i;
					}
					else
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == -1
						   || comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 0)
					{
						//if i < 2nd or i == 2nd
						thirdPriorityUser = i;
					}

				}

			}

			if(i==3)
			{//i > top
				if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 1)
				{
					forthPriorityUser = thirdPriorityUser;
					thirdPriorityUser = secondPriorityUser;
					secondPriorityUser = highestPriorityUser;
					highestPriorityUser = i;
					
				}
				else//i< top or i== top
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == -1 ||
					   comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 0   )
				{
					//if i> 2nd
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 1)
					{
						forthPriorityUser = thirdPriorityUser;
						thirdPriorityUser = secondPriorityUser;
						secondPriorityUser = i;
						
					}
					else//if i< 2nd or i == 2nd
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == -1 ||
						   comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 0   )
					{//if i > 3rd
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == 1 )
						{
							forthPriorityUser = thirdPriorityUser;
							thirdPriorityUser = i;
						}//if i <=3rd 
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == -1
						   || comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == 0)
						{
							
							forthPriorityUser = i;
						}
						
					}
					
				}
			
			}

			if(i == 4){
			
			
				if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 1)
				{
					lastPriorityUser = forthPriorityUser;
					forthPriorityUser = thirdPriorityUser;
					thirdPriorityUser = secondPriorityUser;
					secondPriorityUser = highestPriorityUser;
					highestPriorityUser = i;
					
				}
				else//i< top or i== top
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == -1 ||
					   comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[highestPriorityUser].hand.acArr[ ppl[highestPriorityUser,0] ].priority) == 0   )
				{
					//if i> 2nd
					if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 1)
					{
						forthPriorityUser = thirdPriorityUser;
						thirdPriorityUser = secondPriorityUser;
						secondPriorityUser = i;
						
					}
					else//if i< 2nd or i == 2nd
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == -1 ||
						   comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[secondPriorityUser].hand.acArr[ ppl[secondPriorityUser,0] ].priority) == 0   )
					{//if i > 3rd
						if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == 1 )
						{
							forthPriorityUser = thirdPriorityUser;
							thirdPriorityUser = i;
						}
						else//if i <=3rd 
							if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == -1
							   || comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[thirdPriorityUser].hand.acArr[ ppl[thirdPriorityUser,0] ].priority) == 0)
						{

							if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[forthPriorityUser].hand.acArr[ ppl[forthPriorityUser,0] ].priority) == 1 )
							{
								lastPriorityUser = forthPriorityUser;
								forthPriorityUser = i;
							}
							else//if i <=3rd 
								if(comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[forthPriorityUser].hand.acArr[ ppl[forthPriorityUser,0] ].priority) == -1
								   || comparePriorities(users[i].hand.acArr[ ppl[i,0] ].priority, users[forthPriorityUser].hand.acArr[ ppl[forthPriorityUser,0] ].priority) == 0)
							{

								lastPriorityUser = i;
							}

						}
						
					}
					
				}

			
			}
 }
		//priority lists which attacks go first. ppl[,] contains the index for each action card. priority contains index of action card being used on player(s)
		//or users.
	for(int u = 0; u < users.Length; u++)//check to see if they are defending against anyone
			{
				for(int j = 0; j < users.Length; j++)//check to see if anyone is striking anyone
				{
					if(u!=j)
					{
						if( (users[j].atkUserIndex == u) && (users[j].hand.acArr[ppl[j,0] ].isAllegation()) ) //is user j attacking user u?
						{//allegation fight

							Debug.Log ("Player "+j+" wants to allegate player "+u);
							if(users[u].isDefending == true && users[u].usedAction == false)
							{
								users[u].usedAction = true;
								users[j].usedAction = true;
							
							Debug.Log ("Player "+u+" defended against allegation from player "+j);
							}
							else
							if(users[u].isDefending == true && users[u].usedAction == true)
							{	
							if(users[u].taunted == false)
							{
								Debug.Log ("Player "+u+" got allegated by player "+j);
								users[u].receiveDmg(1);
							}
							else
								if(users[u].taunted == true)
							{
								Debug.Log ("Player "+u+" got allegated by player "+j+ " and received taunt bonus damage");
								users[u].receiveDmg(2);
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
								Debug.Log ("Allegation damage is 1");
								users[u].receiveDmg(1);
							}
							else
								if(users[u].taunted == true)
							{
								Debug.Log ("Allegation damage is 2");
								users[u].taunted = false;
								users[u].receiveDmg(2);

							}
							
							users[j].usedAction = true;

							}
						}
						
						if( (users[j].atkUserIndex == u) && (users[j].hand.acArr[ppl[j,0] ].isTaunt()) ) //is user j attacking user u?
						{
						Debug.Log ("Player "+j+" wants to taunt player "+u);
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
								Debug.Log ("Player "+u+" got taunted without defending by player "+j);
								users[u].taunted = true;
								users[j].usedAction = true;
							}
						}


					}
				}

			}

		for(int i = 0; i < users.Length; i++){
			users[i].newRoundStatReset();
		}
	}

	public void calcNewSuspicionPt(Player p, int index){ //reduce points
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
