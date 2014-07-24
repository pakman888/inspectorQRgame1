using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class TouchControl : MonoBehaviour {

	public bool left;
	public bool right;
	
	public bool throttleUp;
	public bool throttleUpBegan;
	public bool throttleDown;
	public bool throttleDownBegan;
	
	public Rect leftButtonRect;
	public Rect rightButtonRect;
	
	public Rect forwardButtonRect;
	public Rect backwardButtonRect;

	public Rect actionButton1Rect;
	public Rect actionButton2Rect;
	public Rect actionButton3Rect;
	
	public Rect actionButton4Rect;
	public Rect actionButton5Rect;
	
	public GUISkin guiSkin;
	private List<ActionRect> actionRects = new List<ActionRect>();
	
	public float buttonWidth;
	public float buttonHeight;
	public void Awake(){
		
		buttonHeight = Screen.height/8.0f;
		buttonWidth = Screen.width/6.0f;
		
		leftButtonRect = new Rect(0,Screen.height- 2 * buttonHeight - 40,buttonWidth,buttonHeight);
		rightButtonRect = new Rect(buttonWidth+20,Screen.height - 2 * buttonHeight - 40,buttonWidth,buttonHeight);
		
		forwardButtonRect = new Rect(Screen.width - buttonWidth,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		backwardButtonRect = new Rect(Screen.width - (buttonWidth*2)-20,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		
		actionButton1Rect = new Rect(Screen.width/2 - buttonWidth/2, Screen.height - buttonHeight, buttonWidth, buttonHeight);
		
		actionButton2Rect = new Rect(Screen.width/2 - buttonWidth/2, 0, buttonWidth, buttonHeight);
		
		actionButton3Rect = new Rect(Screen.width - buttonWidth, Screen.height- 2 * buttonHeight - 40, buttonWidth, buttonHeight);
		
		actionButton4Rect = new Rect(0, Screen.height - buttonHeight , buttonWidth, buttonHeight);
		actionButton5Rect = new Rect(buttonWidth + 20, Screen.height - buttonHeight, buttonWidth, buttonHeight);
		
		ActionRect leftAction = new ActionRect();
		leftAction.rect = leftButtonRect;
		leftAction.act = Left;
		
		ActionRect rightAction = new ActionRect();
		rightAction.rect = rightButtonRect;
		rightAction.act = Right;
		
		ActionRect forwardAction = new ActionRect();
		forwardAction.rect = forwardButtonRect;
		forwardAction.act = Forward;
		
		ActionRect backwardAction = new ActionRect();
		backwardAction.rect = backwardButtonRect;
		backwardAction.act = Backward;

		ActionRect action1 = new ActionRect();
		action1.rect = actionButton1Rect;
		action1.act = Action1;
		
		ActionRect action2 = new ActionRect();
		action2.rect = actionButton2Rect;
		action2.act = Action2;
		
		ActionRect action3 = new ActionRect();
		action3.rect = actionButton3Rect;
		action3.act = Action3;
				
		ActionRect action4 = new ActionRect();
		action4.rect = actionButton4Rect;
		action4.act = Action4;
				
		ActionRect action5 = new ActionRect();
		action5.rect = actionButton5Rect;
		action5.act = Action5;
		
		actionRects.Add(leftAction);
		actionRects.Add(rightAction);
		actionRects.Add(forwardAction);
		actionRects.Add(backwardAction);
		actionRects.Add(action1);
		actionRects.Add(action2);
		actionRects.Add(action3);
		actionRects.Add(action4);
		actionRects.Add(action5);
	}
	
	public void Update(){
		int count = Input.touchCount;
		for (int i = 0;  i < count;  i++){  
		    Touch touch = Input.GetTouch (i);
			Vector2 tp = new Vector2(touch.position.x,Screen.height-touch.position.y);
		    if (touch.phase == TouchPhase.Began){
		       OnTouchBegan(touch.fingerId, tp);
			}else if (touch.phase == TouchPhase.Moved){
		       OnTouchMoved(touch.fingerId, tp);
			}else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended){
		       OnTouchEnded(touch.fingerId,tp);
			}else if (touch.phase == TouchPhase.Stationary){
		       OnTouchStationary(touch.fingerId,tp);
			}
		}	
	}
	
	public void Action5(bool on, bool justTouched) {
		if (justTouched) {
			GetComponent<Bus>().OnTurnSignalButtonPressed(TurnSignal.right);
		}
	}
	
	public void Action4(bool on, bool justTouched) {
		if (justTouched) {
			GetComponent<Bus>().OnTurnSignalButtonPressed(TurnSignal.left);
		}
	}
	
	public void Action3(bool on, bool justTouched) {
		if (justTouched) {
			GetComponent<Bus>().OnDoorsButtonPressed();
		}
	}
	
	public void Action2(bool on, bool justTouched) {
		//GetComponent<CarController>().JumpBusReverse();
	}

	public void Action1(bool on, bool justTouched) {
        //Events.Instance.OnMissionEnded(this, EventArgs.Empty);
		if (justTouched) {
    		UIManager.Instance.ToggleMinimap();
		}
	}
	
	public void OnTouchBegan(int fingerId, Vector2 pos){
		TriggerAction(pos, true, true);
	}
	
	public void OnTouchMoved(int fingerId, Vector2 pos){
		//not used
	}
	
	public void OnTouchEnded(int fingerId, Vector2 pos){
		TriggerAction(pos,false, false);
	}
	
	public void OnTouchStationary(int fingerId, Vector2 pos){
		TriggerAction(pos, true, false);
	} 
	
	public void TriggerAction(Vector2 pos, bool on, bool justTouched){
		actionRects.ForEach(ar => {
			if(ar.rect.Contains(pos)){
				ar.act(on, justTouched);
			}
		});
	}
	
	
	public void Forward(bool on, bool justTouched){
		throttleUp = on;
		throttleUpBegan = justTouched;
	}
	
	public void Backward(bool on, bool justTouched){
		throttleDown = on;
		throttleDownBegan = justTouched;
	}
	
	public void Left(bool on, bool justTouched){
		left = on;
	}
	
	public void Right(bool on, bool justTouched){
		right = on;
	}
	
	public void OnGUI(){
		if (StateMachine.Instance.IsInGameNotPaused()) {
			GUI.skin = guiSkin;	
			//GUILayout.Label(" l " + left + " r " + right + " f " +throttleUp + " b " + throttleDown);
			GUI.Label(leftButtonRect,"Left","test_touch");
			GUI.Label(rightButtonRect,"Right","test_touch");
			GUI.Label(forwardButtonRect,"Forward","test_touch");
			GUI.Label(backwardButtonRect,"Backward","test_touch");
			GUI.Label(actionButton1Rect,"Map","test_touch");
			//GUI.Label(actionButton2Rect,"R Jump","test_touch");
			GUI.Label(actionButton3Rect,"Open Door","test_touch");
			GUI.Label(actionButton4Rect,"LSignal","test_touch");
			GUI.Label(actionButton5Rect,"RSignal","test_touch");
		}
	}
	
	struct ActionRect{
		public Rect rect;
		public Action<bool, bool> act;
	}
}


