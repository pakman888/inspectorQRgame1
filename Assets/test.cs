using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public UISlider _slider;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void LoseValue(){
		this._slider.value = this._slider.value - 0.1f;
	}

	public void AddValue(){
		this._slider.value = this._slider.value + 0.05f;
	}



}
