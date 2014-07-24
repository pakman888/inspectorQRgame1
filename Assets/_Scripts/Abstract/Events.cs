using System;
using UnityEngine;
using System.Collections;

public class Events : Singleton<Events> {
	public event EventHandler MainMenuLoaded;
	public event EventHandler MissionSelectMenuLoaded;
	public event EventHandler SettingsMenuLoaded;
	public event MissionEventHandler MissionLoadingStarted;
	
	public event BusEventHandler BusInstantiated;
	
	public event EventHandler MissionLoaded;
	public event BusStopEventHandler StoppedAtBusStop;
	public event BusStopEventHandler BusStopCompleted;
	public event EventHandler MissionEnded;
	public event EventHandler ScoreTallyEnded;
		
	public event EventHandler PauseButtonPressed;
	public event EventHandler ResumeButtonPressed;
	public event EventHandler MuteButtonPressed;
	
	public event StateContextEventHandler SwitchedStateContext;

    public event BusEventHandler Overbraking;
    public event BusEventHandler BlinkerChange;
	
	public void OnMainMenuLoaded(object sender, EventArgs e) {
		MainMenuLoaded(sender, e);
	}
	
	public void OnMissionSelectMenuLoaded(object sender, EventArgs e) {
		MissionSelectMenuLoaded(sender, e);
	}
	
	public void OnSettingsMenuLoaded(object sender, EventArgs e) {
		SettingsMenuLoaded(sender, e);	
	}
	
	public void OnMissionLoadingStarted(object sender, MissionEventArgs e) {
		MissionLoadingStarted(sender, e);	
	}
	
	public void OnMissionLoaded(object sender, EventArgs e) {
		MissionLoaded(sender, e);	
	}
	
	public void OnPauseButtonPressed(object sender, EventArgs e) {
		PauseButtonPressed(sender, e);	
	}
	
	public void OnResumeButtonPressed(object sender, EventArgs e) {
		ResumeButtonPressed(sender, e);	
	}
	
	public void OnMuteButtonPressed(object sender, EventArgs e) {
		MuteButtonPressed(sender, e);	
	}
	
	public void OnMissionEnded(object sender, EventArgs e) {
		MissionEnded(sender, e);	
	}
	
	public void OnBusInstantiated(object sender, BusEventArgs e) {
		BusInstantiated(sender, e);	
	}
	
	public void OnStoppedAtBusStop(object sender, BusStopEventArgs e) {
		StoppedAtBusStop(sender, e);	
	}
	
	public void OnBusStopCompleted(object sender, BusStopEventArgs e) {
		BusStopCompleted(sender, e);	
	}
	
	public void OnScoreTallyEnded(object sender, EventArgs e) {
		ScoreTallyEnded(sender, e);	
	}
	
	public void OnSwitchedStateContext(object sender, StateContextEventArgs e) {
		SwitchedStateContext(sender, e);	
	}

    public void OnOverBraking(object sender, BusEventArgs e) {
        Overbraking(sender, e);
    }

    public void OnBlinkreChange(object sender, BusEventArgs e) {
        BlinkerChange(sender, e);
    }

}

public delegate void MissionEventHandler(object sender, MissionEventArgs e);
public delegate void BusStopEventHandler(object sender, BusStopEventArgs e);
public delegate void BusEventHandler(object sender, BusEventArgs e);
public delegate void StateContextEventHandler(object sender, StateContextEventArgs e);

public class BusStopEventArgs : EventArgs {
	public BusStopScript busStop;
	
	public BusStopEventArgs(BusStopScript busStop) {
		this.busStop = busStop;	
	}
}

public class MissionEventArgs : EventArgs {
	public RouteItem mission;
	
	public MissionEventArgs(RouteItem mission) {
		this.mission = mission;	
	}
}

public class BusEventArgs : EventArgs {
	public Bus bus;
	
	public BusEventArgs(Bus bus) {
		this.bus = bus;
	}
}

public class StateContextEventArgs : EventArgs {
	public StateContext stateContext;
	
	public StateContextEventArgs(StateContext stateContext) {
		this.stateContext = stateContext;	
	}
}