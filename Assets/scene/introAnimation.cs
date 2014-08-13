using UnityEngine;
using System.Collections;

public class introAnimation : MonoBehaviour {
//	public Sprite sprite_intro;
//	public Animator spriteObj;

	public Animator clip;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			goToInstructions();
		}
	}

	public void goToInstructions(){
	
		Application.LoadLevel("overviewPurpose");
	}


}
