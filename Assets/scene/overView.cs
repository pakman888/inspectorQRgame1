using UnityEngine;
using System.Collections;

public class overView : MonoBehaviour {
	
	public Animator ovAnimation;
	int count;

	Sprite [] scenes;
	// Use this for initialization
	void Start () {
	//	animation[ovAnimation.animation].wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel("main tuuli changes");
		}
	}
}
