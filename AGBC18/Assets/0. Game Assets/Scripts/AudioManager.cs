using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioBin audioBin;
    private FMOD.Studio.EventInstance rainAmbiance;

    public 

	void Start () {
        if(audioBin == null) {
            audioBin = GetComponentInParent<AudioBin>();
        }

        var audioString = audioBin.SFX_rainAmbiance;
        if(audioString == "") {
            Debug.Log("No FMOD Event Data for Rain Ambiance");
        }
        else 
        {
            rainAmbiance = FMODUnity.RuntimeManager.CreateInstance(audioBin.SFX_rainAmbiance);
            rainAmbiance.start();
        }

	}
	
	void Update () {
		
	}

    private void OnDestroy()
    {
        rainAmbiance.release();
    }
}
