using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundMaster : Singleton<SoundMaster> {
	const string mutePrefKey = "Mute";
	bool muted;
	public bool Muted { 
		get { return muted; }
		private set { 
			muted = value;
			AudioListener.volume = muted == false ? 1 : 0;
		}
	}
		
	public AudioListener uiAudioListener;
	public AudioListener gameAudioListener;

	const float SFX_VOLUME_MODIFIER = 0.75f;
	
	public float SFXVolume { get { return sfxVolume; } }
	public float MusicVolume { get { return musicVolume; } }
	float sfxVolume = 1;
	float musicVolume = 1;
	
	public AudioClip[] crash_metal_sounds;
	public AudioClip[] scratch_sounds;
	
    public AudioClip door_open;
    public AudioClip door_close;
	
    public AudioClip minus_bonus;   
    public AudioClip plus_bonus;
	
    public AudioClip bus_stop_sound;
	
    public AudioSource idle;
    public AudioSource tick;
	public AudioSource low_freq;
    public AudioSource high_freq;
	
	public AudioSource sfx;
	
	public AudioSource start;	
    public AudioSource stop;
	
	public AudioSource blinker;   

    public AudioSource air_brake;
    public AudioSource motor_brake;
    public AudioSource horn;
    
    public AudioSource crash_soft;
    public AudioSource crash_scratch;

    public AudioSource door_horn;
    public AudioSource exit_request;
    public AudioSource passenger_scream;
    public AudioSource money;

    public AudioSource passenger_happy;
    public AudioSource passenger_unhappy;
  
    public AudioSource next_stop;
    public AudioSource current_stop;
    
    public AudioSource reverse;
	
	public AudioClip[] big_car_sounds;
	public AudioClip[] small_car_sounds;
	
	public float idle_min_rpm = 100;
	public float idle_max_rpm = 1500;
	public float idle_min_frequency = 0.45f;
	public float idle_max_frequency = 0.65f;
	float idle_volume;
	
	public float tick_min_rpm = 800;
	public float tick_max_rpm = 2800;
	public float tick_min_frequency = 0.5f;
	public float tick_max_frequency = 1.4f;
	float tick_volume;
	
	public float low_min_rpm = 500;
	public float low_max_rpm = 2200;
	public float low_min_frequency = 0.5f;
	public float low_max_frequency = 1.5f;
	float low_volume;
	
	public float high_min_rpm = 600;
	public float high_max_rpm = 3200;
	public float high_min_frequency = 0.5f;
	public float high_max_frequency = 1.8f;
	float high_volume;
	
	public float mbrake_min_rpm = 200;
	public float mbrake_max_rpm = 3500;
	public float mbrake_min_frequency = 0.5f;
	public float mbrake_max_frequency = 1.5f;
	
	CarController controller;
	Drivetrain drivetrain;
	
	float sound_engine_coef = 0.25f;
	
	const float	RPM_COEF = 0.63f;
	const float	SOUND_RPM_CHANGE_SPEED_PLUS	= 800.0f;
	const float	SOUND_RPM_CHANGE_SPEED_MINUS = 400.0f;
	
	const float	SOUND_THROTTLE_CHANGE_SPEED_PLUS	= 1.7f;
	const float	SOUND_THROTTLE_CHANGE_SPEED_MINUS	= 1.8f;
	
	const float SOUND_CRASH_SPEED_LIMIT = 1.0f;
			
	float current_sound_rpm;
	float current_sound_throttle;
	
	bool fading = false;
	AudioSource[] audioSources;
	
	void Start() {
		ApplyStoredMuteSetting();
		StartMusic();
		
		audioSources = GetComponentsInChildren<AudioSource>();
		//Events.Instance.MissionLoaded += new EventHandler(OnMissionLoaded);
		Events.Instance.SwitchedStateContext += (object src, StateContextEventArgs e) => { 
			if (e.stateContext == StateContext.Menu) OnMenuContextEntered();
			else if (e.stateContext == StateContext.Game) OnGameContextEntered();
		};
		Events.Instance.BusInstantiated += OnBusInstantiated;
		Events.Instance.MuteButtonPressed += (sender, e) => ToggleMute();
	}
	
	void ApplyStoredMuteSetting() {
		var isMute = PlayerPrefs.GetInt(mutePrefKey);
		Muted = isMute == 1 ? true : false;
	}
	
	void WriteMuteSetting() {
		PlayerPrefs.SetInt(mutePrefKey, muted ? 1 : 0);
	}
	
	public void ToggleMute() {
		Muted = !Muted;
		WriteMuteSetting();
	}
	
	public void PlayOneShot(AudioClip clip) {
		sfx.PlayOneShot(clip);
	}
	
	public void OnGameContextEntered() {
		gameAudioListener.enabled = true;
		uiAudioListener.enabled = false;
		StartCoroutine(FadeOut(2.0f));	
		StartCoroutine(StartEngineSounds());
	}
	
	public void OnMenuContextEntered() {
		gameAudioListener.enabled = false;
		uiAudioListener.enabled = true;
		StopAllSounds();
		StartMusic();	
	}
	
	void StartMusic() {
		fading = false;
		audio.volume = 1;
		audio.Play();
	}
	
	IEnumerator FadeOut(float fadeTime) {
		fading = true;
		float startVol = audio.volume;
		float time = 0;
		while (time <= fadeTime) {
			if (fading == false) { // the fadeout has been cancelled
				yield break;	
			}
			audio.volume = Mathf.Lerp(startVol, 0, time/fadeTime);
			time += Time.deltaTime;
			yield return null;
		}
		audio.volume = 0;
		audio.Stop();
	}
	
	void OnBusInstantiated(object sender, BusEventArgs e) {
		controller = e.bus.GetComponent<CarController>();
		drivetrain = e.bus.GetComponent<Drivetrain>();
		idle.clip = e.bus.soundData.idle;
		low_freq.clip = e.bus.soundData.low_freq;
		high_freq.clip = e.bus.soundData.high_freq;
		tick.clip = e.bus.soundData.tick;
	}
	
	IEnumerator StartEngineSounds() {
		yield return new WaitForSeconds(0.2f);
		idle.Play();
		low_freq.Play();
		high_freq.Play();
		tick.Play();
	}
	
	void StopAllSounds() {
		foreach (var source in audioSources) {
			source.Stop();	
		}
	}
	
	void Update() {
		if (StateMachine.Instance.IsInContext(StateContext.Game)) {	
			handle_sound_idle();
			handle_sound_low();
			handle_sound_high();
			handle_sound_tick();
			UpdateVolumes();
		}
	}
	
	void UpdateVolumes() {
		foreach (var source in audioSources) {
			// Special cases where the source relies on a specially calculated volume
			if (source == tick) {
				source.volume = tick_volume * sfxVolume * SFX_VOLUME_MODIFIER;	
			}
			else if (source == idle) {
				source.volume = idle_volume * sfxVolume * SFX_VOLUME_MODIFIER;		
			}
			else if (source == low_freq) {
				source.volume = low_volume * sfxVolume * SFX_VOLUME_MODIFIER;		
			}
			else if (source == high_freq) {
				source.volume = high_volume * sfxVolume * SFX_VOLUME_MODIFIER;		
			}
			// regular case
			else {
				source.volume = sfxVolume * SFX_VOLUME_MODIFIER;
			}
		}
	}
	
	// SPAGHETTI NOTE: despite the fact that it returns a value, this is a mutator method
	// for current_sound_rpm
	float get_sound_rpm() {
		var engine_rpm = drivetrain.rpm;
		
		if (current_sound_rpm < engine_rpm) {
			current_sound_rpm += SOUND_RPM_CHANGE_SPEED_PLUS * Time.deltaTime;
			if (current_sound_rpm > engine_rpm) {
				current_sound_rpm = engine_rpm;
			}
		}
		else {
			current_sound_rpm -= SOUND_RPM_CHANGE_SPEED_MINUS * Time.deltaTime;
			if (current_sound_rpm < engine_rpm) {
				current_sound_rpm = engine_rpm;
			}
		}

		return current_sound_rpm * RPM_COEF;
	}
	
	// SPAGHETTI NOTE: analogous to get_sound_rpm
	float get_sound_throttle() {
		float engine_throttle = controller.Throttle;

		if (/*vehicle->get_clutch() > 0.05f || */current_sound_throttle > engine_throttle) {
			current_sound_throttle -= SOUND_THROTTLE_CHANGE_SPEED_MINUS * Time.deltaTime;
			if (current_sound_throttle < engine_throttle) {
				current_sound_throttle = engine_throttle;
			}
		}
		else {
			current_sound_throttle += SOUND_THROTTLE_CHANGE_SPEED_PLUS * Time.deltaTime;
			if (current_sound_throttle > engine_throttle) {
				current_sound_throttle = engine_throttle;
			}
		}
		
		return current_sound_throttle;
	}
	
	void handle_sound_idle() {
		float current_rpm = get_sound_rpm();
		float vol = 1.0f - Mathf.Clamp(current_rpm  / idle_max_rpm, 0.0f, 1.0f);
		idle_volume = vol * sound_engine_coef;
	}	
	
	void handle_sound_low() {
		float current_rpm = get_sound_rpm();
		float freq_rel = Mathf.Clamp((current_rpm - low_min_rpm) / (low_max_rpm - low_min_rpm), 0.0f, 1.0f);
		float freq = (low_max_frequency - low_min_frequency) *  freq_rel + low_min_frequency;
		
		low_volume = sound_engine_coef;
		low_freq.pitch = freq;
	}
	
	void handle_sound_high() {
		float current_rpm = get_sound_rpm();
		float throttle = get_sound_throttle();
	
		float freq_rel = Mathf.Clamp((current_rpm - high_min_rpm) / (high_max_rpm - high_min_rpm), 0.0f, 1.0f);
		float vol = Mathf.Min(throttle * 0.5f + freq_rel * 1.3f, 1.0f);
		float freq = (high_max_frequency - high_min_frequency) *  freq_rel + high_min_frequency;
	
		high_volume = vol * sound_engine_coef;
		high_freq.pitch = freq;
	}
	
	void handle_sound_tick() {
		float current_rpm = get_sound_rpm();
		float throttle = get_sound_throttle();
	
		float freq_rel = Mathf.Clamp((current_rpm - tick_min_rpm) / (tick_max_rpm - tick_min_rpm), 0.0f, 1.0f);
		float vol = Mathf.Min(throttle * 0.3f + freq_rel * 1.0f, 1.0f);
		float freq = (tick_max_frequency - tick_min_frequency) *  freq_rel + tick_min_frequency;
	
		tick_volume = vol * sound_engine_coef;
		tick.pitch = freq;
	}
	
	public void PlayScratch() {
		//PlayOneShot(
	}
	
	public void StopScratch() {
		//crash_scratch.Stop();
	}
	
	public void PlayCrash(float speed) {
		if (speed > SOUND_CRASH_SPEED_LIMIT) {
			PlayOneShot(crash_metal_sounds[UnityEngine.Random.Range(0, crash_metal_sounds.Length)]);
		}
	}
	
	public void PlayDoorOpen() {
		PlayOneShot(door_open);
	}
	
	public void PlayDoorClose() {
		PlayOneShot(door_close);
	}
	
	public void PlayBlinker() {
		blinker.Play();	
	}
	
	public void StopBlinker() {
		blinker.Stop();	
	}
	
	public void PlayReverse() {
		reverse.Play();	
	}
	
	public void StopReverse() {
		reverse.Stop();	
	}
	
	public void PlayBusStopSound() {
		PlayOneShot(bus_stop_sound);
	}
	
	public void PlayPlusBonus() {
		PlayOneShot(plus_bonus);	
	}
	
	public void PlayMinusBonus() {
		PlayOneShot(minus_bonus);
	}
	
	public AudioClip GetRandomBigCarClip() {
		return big_car_sounds[UnityEngine.Random.Range(0, big_car_sounds.Length)];	
	}
	
	public AudioClip GetRandomSmallCarClip() {
		return small_car_sounds[UnityEngine.Random.Range(0, small_car_sounds.Length)];	
	}
}
