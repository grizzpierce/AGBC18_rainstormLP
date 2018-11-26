using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAmbiance : MonoBehaviour {

	public AudioManager _audioManager;

	[FMODUnity.EventRef]
	public string _eventReference;
	FMOD.Studio.EventInstance _thisEvent;

	void Start () {
		//_thisEvent = FMODUnity.RuntimeManager.CreateInstance(_eventReference);
	}
	
	void Update () {
		
	}
}
