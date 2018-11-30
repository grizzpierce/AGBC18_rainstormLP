using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum MUSIC_STATE {
        INTRO,
        PLAYING,
        IDLE
    }

    public AudioBin audioBin;
    public Camera cameraRig;
    private Transform cameraRigTransform;
    private FMOD.Studio.EventInstance rainAmbiance;

    MUSIC_STATE currentState = MUSIC_STATE.INTRO;

    void Awake () {
        if (cameraRig == null) {
            cameraRig = Camera.main;
        }
    }

	void Start () {
        if(audioBin == null) {
            audioBin = GetComponentInParent<AudioBin>();
        }

        cameraRigTransform = cameraRig.GetComponent<Transform>();
        var audioString = audioBin.rainAmbiance;
        if(audioString == "") {
            Debug.Log("No FMOD Event Data for Rain Ambiance");
        }
        else 
        {
            rainAmbiance = FMODUnity.RuntimeManager.CreateInstance(audioBin.rainAmbiance);
            rainAmbiance.start();
            // FMODUnity.RuntimeManager.AttachInstanceToGameObject(rainAmbiance, cameraRig.GetComponent<Transform>(), cameraRig.GetComponentInChildren<Rigidbody>());
        }

	}

    void Update() {
        //  rainAmbiance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(cameraRigTransform));
        rainAmbiance.setParameterValue("Elevation_User", cameraRigTransform.position.y);
    }
	
    void OnDestroy()
    {
        rainAmbiance.release();
    }

    public bool isMusicPlaying() {
        if (currentState == MUSIC_STATE.PLAYING) {
            return true;
        } else {
            return false;
        }
    }

    public void setMusicPlaying() {
        currentState = MUSIC_STATE.PLAYING;
    }

    public void setMusicIdle() {
        currentState = MUSIC_STATE.IDLE;
    }
}
