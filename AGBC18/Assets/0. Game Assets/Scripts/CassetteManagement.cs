using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteManagement : MonoBehaviour {

	public GameObject activeSlot, first;

	public GameObject playing;
    public bool isPlaying { get; private set; }

	public AudioNotification notifier;
	public Camera mainCam;
	public Vector3 camPos;

	Tween shake;

    FMOD.Studio.EventInstance playingTrack;
    public AudioManager audioManager;

	void Start() {
		camPos = mainCam.transform.localPosition;
		shake = mainCam.DOShakePosition(0, 0, 0, 0, true);
	}

	public void Launch() {
		playing = first;
        var audioEvent = first.GetComponent<CartridgeData>().trackAudioEvent;
        if (audioEvent == null)
        {
            Debug.Log("No Track Event Data Found");
        }
        else
        {
            playingTrack = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            playingTrack.start();

            notifier.Play(first.GetComponent<RawImage>().color);
        }
	}

	void Update() {
		if(shake != null) {
			if(!shake.IsPlaying()) {
				if(mainCam.transform.localPosition != camPos) {
					mainCam.transform.localPosition = camPos;
				}
			}
		}

        // Checks playing track to see if it is finished playing
        if (playingTrack.isValid()) {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            playingTrack.getPlaybackState(out playbackState);
            if(playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
                playingTrack.release();
                //TODO play cartridge finish playing
                playing = null;
                notifier.Stop();
            }
        }
	}

    public IEnumerator Assess() {
		GameObject pressed = activeSlot.transform.GetChild(0).gameObject;

        // Check if cartridge is discovered
		if(!pressed.GetComponent<CartridgeData>().isUnknown) {
            // Check to see if the pressed cartridge is currently playing; if so, stops it.
            if (pressed == playing) {
                playingTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
			else {
                // Check to see if there is a currently playing cartridge at all; if so, stops it.
                if (playing != null){
                    yield return StartCoroutine(TapeStopAudioCoroutine());
                    playingTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
                var audioEvent = pressed.GetComponent<CartridgeData>().trackAudioEvent;
                if (audioEvent == null) {
                    Debug.Log("No Track Event Data Found");
                }
                else {
                    if(playingTrack.isValid()) {
                        playingTrack.release();
                    }
                    yield return StartCoroutine(TapeStartAudioCoroutine());
                    playingTrack = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
                    playingTrack.start();

                    notifier.Play(pressed.GetComponent<RawImage>().color);
                    playing = pressed;
                }

			}
		}
        // unknown cassette
		else {
            shake = mainCam.DOShakePosition(1f, .2f, 10, 90, true);
		}
	}

    private IEnumerator TapeStopAudioCoroutine() {
        // TODO play cartridge stop 
        // TODO wait for track fadeout
        // TODO pitch shift track down over fadeout?
        yield return null;
    }

    private IEnumerator TapeStartAudioCoroutine() {
        // TODO play cartridge start
        // TODO wait for reel up
        // TODO play track
        yield return null;
    }
}
