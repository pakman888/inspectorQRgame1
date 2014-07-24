using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MobileFlyCam : MonoBehaviour {
	float cameraSensitivity = 90;
	float climbSpeed = 30;
	float normalMoveSpeed = 50;
 
	private float rotationY = 0.0f;
	
	public bool left;
	public bool right;
	
	public bool throttleUp;
	public bool throttleDown;
	
	public bool raise;
	public bool lower;
	
	public Rect leftButtonRect;
	public Rect rightButtonRect;
	
	public Rect forwardButtonRect;
	public Rect backwardButtonRect;

	public Rect actionButton1Rect;
	public Rect actionButton2Rect;
	
	public GUISkin guiSkin;
	private List<ActionRect> actionRects = new List<ActionRect>();
	
	public float buttonWidth;
	public float buttonHeight;
	public void Awake(){
		
		buttonHeight = Screen.height/8.0f;
		buttonWidth = Screen.width/6.0f;
		
		leftButtonRect = new Rect(0,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		rightButtonRect = new Rect(buttonWidth+20,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		
		forwardButtonRect = new Rect(Screen.width - buttonWidth,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		backwardButtonRect = new Rect(Screen.width - (buttonWidth*2)-20,Screen.height-buttonHeight,buttonWidth,buttonHeight);
		
		actionButton1Rect = new Rect(Screen.width/2 - buttonWidth/2, Screen.height - buttonHeight, buttonWidth, buttonHeight);
		
		actionButton2Rect = new Rect(Screen.width/2 - buttonWidth/2, 0, buttonWidth, buttonHeight);
		
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
		
		actionRects.Add(leftAction);
		actionRects.Add(rightAction);
		actionRects.Add(forwardAction);
		actionRects.Add(backwardAction);
		actionRects.Add(action1);
		actionRects.Add(action2);
	}
	
	public void Update(){
		HandleTouches();
		
		if (left) {
			rotationY -= cameraSensitivity * Time.deltaTime;
		}
		if (right) {
			rotationY += cameraSensitivity * Time.deltaTime;
		}
 
		transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.up);
		
		if (throttleUp) {
			transform.position += transform.forward * normalMoveSpeed * Time.deltaTime;
		}
		if (throttleDown) {
			transform.position -= transform.forward * normalMoveSpeed * Time.deltaTime;
		}
 
		if (raise) {transform.position += transform.up * climbSpeed * Time.deltaTime;}
		if (lower) {transform.position -= transform.up * climbSpeed * Time.deltaTime;}
	}
	
	void HandleTouches(){ 
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
	
	public void Action2(bool on) {
		//GetComponent<CarController>().JumpBusReverse();
		lower = on;
	}

	public void Action1(bool on) {
        //Application.LoadLevel(0);
		//GetComponent<CarController>().JumpBus();
		raise = on;
    }
	
	public void Forward(bool on){
		throttleUp = on;
	}
	
	public void Backward(bool on){
		throttleDown = on;
	}
	
	public void Left(bool on){
		left = on;
	}
	
	public void Right(bool on){
		right = on;
	}
	
	public void OnTouchBegan(int fingerId, Vector2 pos){
		TriggerAction(pos,true);
	}
	
	public void OnTouchMoved(int fingerId, Vector2 pos){
		//not used
	}
	
	public void OnTouchEnded(int fingerId, Vector2 pos){
		TriggerAction(pos,false);
	}
	
	public void OnTouchStationary(int fingerId, Vector2 pos){
		TriggerAction(pos,true);
	}
	
	public void TriggerAction(Vector2 pos, bool on){
		actionRects.ForEach(ar => {
			if(ar.rect.Contains(pos)){
				ar.act(on);
			}
		});
	}
	
	

	
	public void OnGUI(){
		GUI.skin = guiSkin;	
		//GUILayout.Label(" l " + left + " r " + right + " f " +throttleUp + " b " + throttleDown);
		GUI.Label(leftButtonRect,"Rotate Left","test_touch");
		GUI.Label(rightButtonRect,"Rotate Rite","test_touch");
		GUI.Label(forwardButtonRect,"Forward","test_touch");
		GUI.Label(backwardButtonRect,"Backward","test_touch");
		GUI.Label(actionButton1Rect,"Raise","test_touch");
		GUI.Label(actionButton2Rect,"Lower","test_touch");
	}
	
	struct ActionRect{
		public Rect rect;
		public Action<bool> act;
	}
}

