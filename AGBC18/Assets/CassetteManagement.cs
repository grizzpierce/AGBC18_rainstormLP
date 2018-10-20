using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteManagement : MonoBehaviour {

	public GameObject activeSlot, first;

	public GameObject playing;

	public AudioNotification notifier;
	public Camera mainCam;
	public Vector3 camPos;

	Tween shake;

    FMOD.Studio.EventInstance playingTrack;

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

        if (playingTrack.isValid()) {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            playingTrack.getPlaybackState(out playbackState);
            if(playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
                playingTrack.release();
                playing = null;
                notifier.Stop();            
            }
        }
	}

	public void assess() {
		GameObject pressed = activeSlot.transform.GetChild(0).gameObject;

		if(!pressed.GetComponent<CartridgeData>().isUnknown) {
            if (pressed == playing) {
                playingTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
			else {
                if (playing != null){
                    playingTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
                var audioEvent = pressed.GetComponent<CartridgeData>().trackAudioEvent;
                if (audioEvent == null) {
                    Debug.Log("No Track Event Data Found");
                }
                else {
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

    //private void StopPlaying() {
    //    if (playingTrack.isValid()) {
    //        FMOD.Studio.PLAYBACK_STATE playbackState;
    //        playingTrack.getPlaybackState(out playbackState);

    //        if (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED) {
    //            playingTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //        }
    //        else {
    //            playingTrack.release();
    //            playing = null;
    //            notifier.Stop();
    //        }

    //    }
    //}
}
