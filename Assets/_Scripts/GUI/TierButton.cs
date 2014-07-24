using UnityEngine;
using System.Collections;

public class TierButton : MonoBehaviour {
	public int tier;
	
	public void OnPressed() {
		MissionSelectPane.Instance.TierSelected(tier);	
	}
}