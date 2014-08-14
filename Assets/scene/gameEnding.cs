using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;


public class gameEnding : MonoBehaviour {
	string filePath;
	string line;
	int winner;
	int []scores;
	bool canExit;
	int index;
	// Use this for initialization
	void Start () {
		index = 0;
		canExit = false;
		filePath = Application.dataPath+"/saveWinnerIndex.txt";
		scores = new int[5];
		try{
			StreamReader theReader = new StreamReader(filePath, Encoding.Default); 

			using (theReader)
			{
				// While there's lines left in the text file, do this:
				do
				{
					line = theReader.ReadLine();
					if(line!= null)
					{scores[index] = Convert.ToInt32 (line) ;
						index++;}
				}
				while (line != null);
				
				// Done reading, close the reader and return true to broadcast success
				theReader.Close();
			}


		}
		catch (Exception e)
		{
			Console.WriteLine("Cannot find final result file", e.Message);
			Application.Quit();
		}
		for(int i = 0; i < scores.Length; i++)
		{
			Debug.Log(scores[i]);
		}
		//Debug.Log ("In new scene. Player "+line+" is the winner!");
		/*
		switch(winner)
		{
		case 0: GameObject.Find ("innocent_icon").GetComponent<UISprite>().spriteName = 
						GameObject.Find ("guilty1_icon").GetComponent<UISprite>().spriteName; 
			GameObject.Find ("guilty1_icon").GetComponent<UISprite>().depth = -13; break;
			GameObject.Find ("guilty1_suspicionLevel").GetComponent<UILabel>().depth = -13; break;

		
		case 1:GameObject.Find ("innocent_icon").GetComponent<UISprite>().spriteName = 
			GameObject.Find ("guilty2_icon").GetComponent<UISprite>().spriteName; 
			GameObject.Find ("guilty2_icon").GetComponent<UISprite>().depth = -13; break;
			GameObject.Find ("guilty2_suspicionLevel").GetComponent<UILabel>().depth = -13; break;
		
		case 2:GameObject.Find ("innocent_icon").GetComponent<UISprite>().spriteName = 
			GameObject.Find ("guilty3_icon").GetComponent<UISprite>().spriteName; 
			GameObject.Find ("guilty3_icon").GetComponent<UISprite>().depth = -13; break;
			GameObject.Find ("guilty3_suspicionLevel").GetComponent<UILabel>().depth = -13; break;

		case 3:GameObject.Find ("innocent_icon").GetComponent<UISprite>().spriteName = 
			GameObject.Find ("guilty4_icon").GetComponent<UISprite>().spriteName; 
			GameObject.Find ("guilty4_icon").GetComponent<UISprite>().depth = -13; break;
			GameObject.Find ("guilty4_suspicionLevel").GetComponent<UILabel>().depth = -13; break;

		case 4:GameObject.Find ("innocent_icon").GetComponent<UISprite>().spriteName = 
			GameObject.Find ("guilty5_icon").GetComponent<UISprite>().spriteName; 
			GameObject.Find ("guilty5_icon").GetComponent<UISprite>().depth = -13; break;
			GameObject.Find ("guilty5_suspicionLevel").GetComponent<UILabel>().depth = -13; break;

		default: ;break;
		}*/
	
	}
	
	// Update is called once per frame
	void Update () {
		if(canExit!=false)
		{
			if(Input.GetMouseButtonDown(0)){
				GameObject.Find ("2_0 credits").GetComponent<UISprite>().depth = 13;
				GameObject.Find ("1_4 ending").GetComponent<UISprite>().depth = 0;
				
			}
			if(Input.GetButtonDown("Jump") == true)
			{
				Application.Quit();
			}
		}
	}

	public void endScreen(){
		canExit = true;
		GameObject.Find ("1_4 ending").GetComponent<UISprite>().depth = 13;

	}



	}



