using UnityEngine;
using System.Collections;

public class MissionButton : MonoBehaviour {
	public int tier;
	public int index;
	
	public void OnClicked() {
		var mission = MissionRepository.Instance.GetRoute(tier, index);
		MissionSelectPane.Instance.MissionSelected(mission);
		MissionHandler.Instance.SelectMission(mission);
	}
}
