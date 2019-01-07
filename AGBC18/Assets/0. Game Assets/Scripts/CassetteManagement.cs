using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteManagement : MonoBehaviour {
    
    public GameObject activeSlot, firstCassetteObject;
    public CartridgeDataHolder firstData;

	public GameObject playing;
    public bool IsPlaying { get; private set; }
    private bool IsStopping = false;
    private bool IsStarting = false;

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

    // Called from Intro Interaction animation
	public void Launch() {
        CartridgeData cartridgeData = firstCassetteObject.GetComponent<CartridgeData>();
        cartridgeData.SetKnown(firstData);

        playing = firstCassetteObject;
        StartCoroutine(TapeStart(playing));
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
                //Debug.Log("DEBUG: Releasing stopped track.");
                playingTrack.release();



                playing = null;
                notifier.Stop();
            }
        } else {
            IsStopping = false;
        }
	}

    public void Assess() {
		GameObject pressed = activeSlot.transform.GetChild(0).gameObject;

        // Check if cartridge is discovered
		if(pressed.GetComponent<CartridgeData>().IsKnown()) {
            // Check to see if the pressed cartridge is currently playing; if so, stops it.
            if (pressed == playing) {
                StartCoroutine(TapeStop());
            } else {
                // Check to see if there is a currently playing cartridge at all; if so, stops it.
                if (!IsStopping || !IsStarting) {
                    if (playing != null) {
                        StartCoroutine(TapeChange(pressed));
                    } else {
                        StartCoroutine(TapeStart(pressed));
                    }
                }
			}
		} else {
            // unknown cassette
            shake = mainCam.DOShakePosition(1f, .2f, 10, 90, true);
            //Debug.Log("DEBUG: unknown cassette.");
		}
	}

    IEnumerator TapeChange(GameObject _pressed) {

        yield return StartCoroutine(TapeStop());
        yield return StartCoroutine(TapeStart(_pressed));
        yield return null;
    }

    IEnumerator TapeStop() {
        IsStopping = true;
        
        FMODUnity.RuntimeManager.PlayOneShot(audioManager.cartridgeStop);

        float t = 0.0f;
        float targetT = playing.GetComponent<CartridgeData>().GetDataHolder().fadeOutTimeOnStop;
        float value;

        // if we're dividing by zero, skip the lerp and the math error
        if (targetT != t) {
            while (t < targetT) {
                t += Time.deltaTime;
                value = Mathf.Lerp(1, 0, t/targetT);
                playingTrack.setParameterValue("TapeStart", value);
                yield return null;
            }
        }
        playingTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        IsStopping = false;
        yield return null;
    }

    IEnumerator TapeFinish() {
        IsStopping = true;
        yield return StartCoroutine(PlayOneShot(audioManager.cartridgeFinishPlaying));
        IsStopping = false;
        yield return null;
    }

    IEnumerator TapeStart(GameObject _pressed) {
        // TODO play cartridge start
        // TODO wait for reel up
        IsStarting = true;
        yield return StartCoroutine(PlayOneShot(audioManager.cartridgeLoad));

        var audioEvent = _pressed.GetComponent<CartridgeData>().GetDataHolder().trackAudioEvent;

        
        // only proceed to new track if there is a new track event available
        if (audioEvent == null)
        {
            yield return StartCoroutine(PlayOneShot(audioManager.cartridgePlay));
            Debug.Log("No Track Event Data for selected track!");
            yield return StartCoroutine(PlayOneShot(audioManager.cartridgeFinishPlaying));
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(audioManager.cartridgePlay);
            if (playingTrack.isValid()) playingTrack.release();
            playingTrack = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            playingTrack.start();

            notifier.Play(_pressed.GetComponent<RawImage>().color, _pressed.GetComponent<CartridgeData>().GetDataHolder().revealedText);
            playing = _pressed;

            float t = 0.0f;
            float targetT = _pressed.GetComponent<CartridgeData>().GetDataHolder().fadeInTimeOnStart;
            float value;
            
            // if we're dividing by zero, skip the lerp and the math error
            if (targetT != t) {
                while (t < targetT) {
                    t += Time.deltaTime;
                    value = Mathf.Lerp(0, 1, t/targetT);
                    //Debug.Log("Lerp Value: " + value);
                    playingTrack.setParameterValue("TapeStart", value);
                    yield return null;
                }
            }
        }
        IsStarting = false;
        yield return null;
    }
    
    FMOD.Studio.EventInstance playingOneShot;

    IEnumerator PlayOneShot(string audioEvent) {
        if (audioEvent != "") {
            playingOneShot = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            playingOneShot.start();

            FMOD.Studio.PLAYBACK_STATE playbackState;
            playingOneShot.getPlaybackState(out playbackState);
            if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED) {
                playingOneShot.release();
                playingOneShot.clearHandle();
                yield return null;
            }
        } else {
            Debug.Log("No Event Data for Selected OneShot");
        }
    }
}
