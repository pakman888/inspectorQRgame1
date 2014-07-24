using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager> {
	public Camera loadingCamera;
	public Renderer loadingQuad;
	public Camera minimapCamera;
	public Renderer minimapQuad;
	public List<Texture2D> loadingTextures;
	public AudioListener uiAudioListener;
	public UIPanel mainMenu;
	public UIPanel missionSelectMenu;
	public UIPanel hud;
	public UIPanel scoreTally;
	
	void Start() {
		Events.Instance.MainMenuLoaded += (sender, e) => OnMainMenuLoaded();
		Events.Instance.MissionSelectMenuLoaded += (sender, e) => OnMissionSelectMenuLoaded();
		Events.Instance.MissionLoadingStarted += (sender, e) => OnMissionLoadingStarted();
		Events.Instance.MissionEnded += (sender, e) => OnMissionEnded();
		Events.Instance.SwitchedStateContext += (object src, StateContextEventArgs e) => {
			if (e.stateContext == StateContext.Menu) OnMenuContextEntered();
			else OnGameContextEntered();
		};
		
		Events.Instance.OnMainMenuLoaded(this, EventArgs.Empty);
	}
	
	public void DisplayLoadingScreen() {
		var randomIndex = UnityEngine.Random.Range (0, loadingTextures.Count - 1);
		loadingQuad.material.mainTexture = loadingTextures[randomIndex];
		loadingCamera.gameObject.SetActive(true);
		loadingQuad.gameObject.SetActive(true);
	}
	
	public void HideLoadingScreen() {
		loadingCamera.gameObject.SetActive(false);
		loadingQuad.gameObject.SetActive(false);
	}
	
	bool displayingMinimap = false;
	public void DisplayMinimap() {
		displayingMinimap = true;
		minimapCamera.gameObject.SetActive(true);
		minimapQuad.gameObject.SetActive(true);
	}
	
	public void HideMinimap() {
		displayingMinimap = false;
		minimapCamera.gameObject.SetActive(false);
		minimapQuad.gameObject.SetActive(false);
	}
	
	public void ToggleMinimap() {
		// TODO: this is just a temp solution for the minimap build
		if (StateMachine.Instance.IsInGameNotPaused()) {
			if (!displayingMinimap) {
				DisplayMinimap();	
			}
			else {
				HideMinimap();
			}
		}
	}
	
	public void PullDownMap() {
		if (!displayingMinimap) {
			DisplayMinimap();	
		}
	}
	
	public void PushUpMap() {
		if (displayingMinimap) {
			HideMinimap();	
		}
	}
	
	void OnMissionLoadingStarted() {
		missionSelectMenu.gameObject.SetActive(false);
		DisplayLoadingScreen();
	}
	
	void OnGameContextEntered() {
		HideLoadingScreen();
		hud.gameObject.SetActive(true);
	}
	
	void OnMenuContextEntered() {
		PushUpMap();
		hud.gameObject.SetActive(false);
		scoreTally.gameObject.SetActive(false);
	}
	
	void OnMissionSelectMenuLoaded() {
		missionSelectMenu.gameObject.SetActive(true);	
		mainMenu.gameObject.SetActive(false);
	}
	
	void OnMainMenuLoaded() {
		missionSelectMenu.gameObject.SetActive(false);	
		mainMenu.gameObject.SetActive(true);
	}
	
	void OnMissionEnded() {
		hud.gameObject.SetActive(false);
		scoreTally.gameObject.SetActive(true);	
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.M)) {
			ToggleMinimap();
		}
	}
}
