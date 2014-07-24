using UnityEngine;
using System.Collections;

public class UIHoldButton : UIButton {
	public bool BeingHeld { get; private set; }
	public bool JustPressed { get; private set; }
	int justPressedCounter = 0;
	
	protected override void OnPress(bool isPressed) {
		if (isPressed) {
			BeingHeld = true;
			JustPressed = true;
			justPressedCounter = 0;
		}
		else {
			BeingHeld = false;	
		}
	}
	
	void Update() {
		if (JustPressed) {
			justPressedCounter++;
			if (justPressedCounter > 1) JustPressed = false;
		}
	}
}
