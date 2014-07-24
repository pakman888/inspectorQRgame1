using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionSelectPane : Singleton<MissionSelectPane> {
	int currentTier = 0;
	MissionButton[] missionButtons;
	public MissionDescriptionUI descriptionUI;
	public GameObject missionSelectionGrid;
	public Transform container;

	public TweenAlpha[] routeMapAlphaTweens;
	public TweenAlpha[] mainMapAlphaTweens;

	public UITexture routeMap;
	public UITexture mainMap;
	
	void Start() {
		missionButtons = GetComponentsInChildren<MissionButton>();	

		routeMapAlphaTweens = routeMap.gameObject.GetComponentsInChildren<TweenAlpha>();
		mainMapAlphaTweens = mainMap.gameObject.GetComponentsInChildren<TweenAlpha>();

		Events.Instance.MissionLoadingStarted += (sender, e) => Reset();
	}
	
	public void TierSelected(int tier) {
		if (tier != currentTier) {
			int i = 0;
			foreach (var button in missionButtons) {
				button.tier = tier;
				button.index = i;
				// TODO: replace button graphic
				
				i++;
			}
			currentTier = tier;
			Reset();
		}
	}
	
	public void Reset() {
		//container.localPosition = cachedContainerStartPosition;
		container.GetComponent<TweenPosition>().PlayReverse();
	}
	
	public void MissionSelected(RouteItem mission) {
		descriptionUI.SetMissionDescription(mission.shortDescription);
		var description = MissionDescriptions.Instance.GetDescriptionForTitleKey(mission.shortDescription);
		routeMap.mainTexture = description.map;
		mainMapAlphaTweens[0].Play();
		routeMapAlphaTweens[0].PlayReverse();
	}

	public void BackToSelectGrid() {
		mainMapAlphaTweens[0].PlayReverse();
		routeMapAlphaTweens[0].Play();
	}
}