using UnityEngine;
using System;
using System.Collections;

public class StateMachine : Singleton<StateMachine> {
	[SerializeField] private GameState gameState;
	[SerializeField] private MenuState menuState;
	[SerializeField] private StateContext context;
	
	public StateContext Context { 
		get { return context; }
		private set { 
			context = value;	
		}
	}
	
    protected override void Awake() {
        SingletonSetup(); 
		
		Events.Instance.MainMenuLoaded += (src, e) => SetState(MenuState.Main);
		Events.Instance.SettingsMenuLoaded += (src, e) => SetState(MenuState.Settings);
		Events.Instance.MissionSelectMenuLoaded += (src, e) => SetState(MenuState.MissionSelect);
		Events.Instance.MissionLoadingStarted += (src, e) => SetState(MenuState.Loading);
		
		Events.Instance.MissionLoaded += (src, e) => SetState(GameState.Normal);	
		Events.Instance.PauseButtonPressed += (src, e) => SetState(GameState.Pause);
		Events.Instance.ResumeButtonPressed += (src, e) => SetState(GameState.Normal);
		Events.Instance.StoppedAtBusStop += (src, e) => SetState(GameState.BusStop);	
		Events.Instance.BusStopCompleted += (src, e) => SetState(GameState.Normal);	
		Events.Instance.MissionEnded += (src, e) => SetState(GameState.ScoreTally);	
		Events.Instance.ScoreTallyEnded += (src, e) => SetState(GameState.End);
    }
	
	void SetState(GameState state) {
		gameState = state;
		if (Context != StateContext.Game) {
			Events.Instance.OnSwitchedStateContext(this, new StateContextEventArgs(StateContext.Game));
			Context = StateContext.Game;	
		}
	}
	
	void SetState(MenuState state) {
		menuState = state;
		if (Context != StateContext.Menu) {
			Events.Instance.OnSwitchedStateContext(this, new StateContextEventArgs(StateContext.Menu));
			Context = StateContext.Menu;
		}
	}
	
	public bool IsInContext(StateContext context) {
		return Context == context;	
	}
	
	public bool IsInState(GameState state) {
		return Context == StateContext.Game && gameState == state;
	}
	
	public bool IsInState(MenuState state) {
		return Context == StateContext.Menu && menuState == state;
	}
	
	public bool IsInGameNotPaused() {
		return IsInContext(StateContext.Game) && !IsInState(GameState.Pause);	
	}
}

public enum StateContext { 
	Game,
	Menu,
	None
}

public enum GameState {
	Normal,
	BusStop,
	Pause,
	ScoreTally,
	End,
	Map
}

public enum MenuState {
	Main,
	MissionSelect,
	Settings,
	Loading
}