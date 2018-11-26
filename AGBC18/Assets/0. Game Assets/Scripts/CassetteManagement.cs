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

    [Range(0f,10f)]
    public float fadeInTimeOnStart = 1;
    [Range(0f,10f)]
    public float fadeOutTimeOnStop = 1;

	void Start() {
		camPos = mainCam.transform.localPosition;
		shake = mainCam.DOShakePosition(0, 0, 0, 0, true);
	}

	public void Launch() {
        CartridgeData cartridgeData = firstCassetteObject.GetComponent<CartridgeData>();
        cartridgeData.SetKnown(firstData);

        playing = firstCassetteObject;
        StartCoroutine(TapeStart(playing));
        
        // var audioEvent = cartridgeData.GetDataHolder().trackAudioEvent;
        // if (audioEvent == null)
        // {
        //     Debug.Log("No Track Event Data Found");
        // }
        // else
        // {
        //     playingTrack = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
        //     playingTrack.start();
        //     //audioManager.setMusicPlaying();

        //     notifier.Play(firstCassetteObject.GetComponent<RawImage>().color);
        // }
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
                //audioManager.setMusicIdle();
                IsStopping = false;
                //TODO play cartridge finish playing
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
                StartCoroutine(TapeStop(playing));
            } else {
                // Check to see if there is a currently playing cartridge at all; if so, stops it.
                if (playing != null)
                {
                    StartCoroutine(TapeChange(playing, pressed));
                }
                else
                {
                    StartCoroutine(TapeStart(pressed));
                }

			}
		} else {
            // unknown cassette
            shake = mainCam.DOShakePosition(1f, .2f, 10, 90, true);
            //Debug.Log("DEBUG: unknown cassette.");
		}
	}

    IEnumerator TapeChange(GameObject _playing, GameObject _pressed) {

        yield return StartCoroutine(TapeStop(_playing));
        // TODO: Cassette Shift
        yield return StartCoroutine(TapeStart(_pressed));
        yield return null;
    }

    IEnumerator TapeStop(GameObject _playing) {
        IsStopping = true;
        // fades out for time defined per track 
        //playingTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //yield return new WaitForSeconds(_playing.GetComponent<CartridgeData>().GetDataHolder().fadeOutTime);
        
        float t = 0.0f;
        float value;
        while (t < fadeOutTimeOnStop) {
            t += Time.deltaTime;
            value = Mathf.Lerp(1, 0, t/fadeOutTimeOnStop);
            playingTrack.setParameterValue("TapeStart", value);
            yield return null;
        }
        playingTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
        IsStopping = false;
        yield return null;
    }

    IEnumerator TapeStart(GameObject _pressed) {
        // TODO play cartridge start
        // TODO wait for reel up

        var audioEvent = _pressed.GetComponent<CartridgeData>().GetDataHolder().trackAudioEvent;
        // only proceed to new track if there is a new track event available
        if (audioEvent == null)
        {
            Debug.Log("No Track Event Data for selected track!");
        }
        else
        {
            if (playingTrack.isValid()) playingTrack.release();
            playingTrack = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
            playingTrack.start();

            notifier.Play(_pressed.GetComponent<RawImage>().color);
            playing = _pressed;

            float t = 0.0f;
            float value;
            while (t < fadeInTimeOnStart) {
                t += Time.deltaTime;
                value = Mathf.Lerp(0, 1, t/fadeInTimeOnStart);
                //Debug.Log("Lerp Value: " + value);
                playingTrack.setParameterValue("TapeStart", value);
                yield return null;
            }
        }

        yield return null;
    }
}
