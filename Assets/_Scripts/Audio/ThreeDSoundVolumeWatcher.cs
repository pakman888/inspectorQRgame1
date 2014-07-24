using UnityEngine;
using System.Collections;

public class ThreeDSoundVolumeWatcher : MonoBehaviour {
	void Update () {
		audio.volume = SoundMaster.Instance.SFXVolume;
	}
}
