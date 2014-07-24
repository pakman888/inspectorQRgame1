using UnityEngine;
using System.Collections;

public class MinimapButton : MonoBehaviour {

	public void OnPressed(){
		UIManager.Instance.ToggleMinimap();
	}
}
