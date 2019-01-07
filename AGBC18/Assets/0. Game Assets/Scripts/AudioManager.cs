using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public enum MUSIC_STATE {
        INTRO,
        PLAYING,
        IDLE
    }

    private Camera cameraRig;
    private Transform cameraRigTransform;
    private FMOD.Studio.EventInstance rainAmbianceInstance;


    [Space]
    [Header("Audio Events")]
    [FMODUnity.EventRef]
    public string cartridgeLoad;
    [FMODUnity.EventRef]
    public string cartridgePlay;
    [FMODUnity.EventRef]
    public string cartridgeStop;
    [FMODUnity.EventRef]
    public string cartridgeFinishPlaying;
    [FMODUnity.EventRef]
    public string cartridgeRattle;
    [FMODUnity.EventRef]
    public string cartridgeScrollLeft;
    [FMODUnity.EventRef]
    public string cartridgeScrollRight;
    [FMODUnity.EventRef]
    public string findTapeInteract;
    [FMODUnity.EventRef]
    public string dudInteract;
    [FMODUnity.EventRef]
    public string textScroll;

    [FMODUnity.EventRef]
    public string rainAmbiance;

    MUSIC_STATE currentState = MUSIC_STATE.INTRO;

    void Awake () {
        if (cameraRig == null) {
            cameraRig = Camera.main;
        }
    }

	void Start () {

        cameraRigTransform = cameraRig.GetComponent<Transform>();
        var audioString = rainAmbiance;
        if(audioString == "") {
            Debug.Log("No FMOD Event Data for Rain Ambiance");
        }
        else 
        {
            rainAmbianceInstance = FMODUnity.RuntimeManager.CreateInstance(rainAmbiance);
            rainAmbianceInstance.start();
            // FMODUnity.RuntimeManager.AttachInstanceToGameObject(rainAmbianceInstance, cameraRig.GetComponent<Transform>(), cameraRig.GetComponentInChildren<Rigidbody>());
        }

	}

    public void playOneShot(string _toPlay) {
        FMODUnity.RuntimeManager.PlayOneShot(_toPlay);
    }

    void Update() {
        //  rainAmbianceInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(cameraRigTransform));
        rainAmbianceInstance.setParameterValue("Elevation_User", cameraRigTransform.position.y);
    }
	
    void OnDestroy()
    {
        rainAmbianceInstance.release();
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
