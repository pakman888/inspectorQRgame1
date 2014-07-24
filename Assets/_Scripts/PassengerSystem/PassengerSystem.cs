using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PassengerSystem : Singleton<PassengerSystem> {

    const float IMPROVE_MOOD_SPEED = 0.002f;
    const float MOOD_THRESHOLD = 0.2f; 

    List<float> passengers;
    public int Passengers { get { return passengers.Count; } }
    public int Happy {
        get {
            return (from float passenger in passengers
             where passenger >= 0.5f
             select passengers).Count();
        }
    }
    public int UnHappy {
        get {
            return (from float passenger in passengers
                    where passenger < 0.5f
                    select passengers).Count();
        }
    }

    public void PassengerEnters(float mood) {
        passengers.Add(mood);
    }

    public void PassengerLeaves() {
        passengers.RemoveAt(Random.Range(0, passengers.Count));
    }

    public void UpdateMood(float changes) {
        for ( int i = 0; i < passengers.Count; i++ ) {
            passengers[i] *= changes;
            passengers[i] = Mathf.Clamp01(passengers[i]);
        }
    }

    public void Reset() {
        passengers = new List<float>();
    }

    void Start() {
        Events.Instance.MissionLoaded += (src,e) => Reset();
    }

    void Update() {
		if(StateMachine.Instance.IsInGameNotPaused()){
			for ( int i = 0; i < passengers.Count; i++ ) {
	            passengers[i] = Mathf.Clamp01(passengers[i] + IMPROVE_MOOD_SPEED * Time.deltaTime);
	        }
			HUD.Instance.happyPassengerDisplay.text = Happy.ToString();
			HUD.Instance.unhappyPassengerDisplay.text = UnHappy.ToString();
		}
    }
}
