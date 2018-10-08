using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class CartridgeRotator : MonoBehaviour {


	// ROTATION SETTINGS & VARIABLES

	public Vector3 axisDirection = new Vector3(1, 0, 0);
	public float cycleDuration = 1f;
	public bool playOnStart = true;
	public bool cleanLoop = false;

	Ease mainEase = Ease.InOutQuad;
	Tween rotTween;

	// ROTATION STATE HANDLING 

	enum ROTATION_STATE {
		INVALID = -1,
		IDLE,
		PLAY,
		PAUSE,
		STOP,
		STATE_COUNT
	}

	ROTATION_STATE currRState = ROTATION_STATE.IDLE;
	ROTATION_STATE prevRState = ROTATION_STATE.INVALID;

	public string current = ROTATION_STATE.IDLE.ToString();
	public string previous = ROTATION_STATE.INVALID.ToString();

	// /////////////
	// FUNCTIONS //
	// ///////////

	void SetStates(ROTATION_STATE _prev, ROTATION_STATE _curr) {
		prevRState = _prev;
		previous = _prev.ToString();

		currRState = _curr;
		current = _curr.ToString();
	}

	void Start() {
		for(int i = 0; i < 3; ++i) {
			if(Mathf.Abs(axisDirection[i]) > 1) {
				Debug.Log(i + " is out of range. Direction has been reset.");
				axisDirection[i] = 0;
			}
		}

		if(cleanLoop)
			mainEase = Ease.Linear;

		if(playOnStart)
			Play(cycleDuration, -1);
	}

	// STATE MACHINE FUNCTIONS

	public void Play(float duration, int loops) {
		if(currRState == ROTATION_STATE.IDLE) {
			SetStates(currRState, ROTATION_STATE.PLAY);

			Vector3 _rotation = new Vector3(
			transform.rotation.eulerAngles.x + (axisDirection[0] * 360),
			transform.rotation.eulerAngles.y + (axisDirection[1] * 360),
			transform.rotation.eulerAngles.z + (axisDirection[2] * 360));

			rotTween = gameObject.transform.DORotate(_rotation, duration, RotateMode.FastBeyond360).SetLoops(loops).SetEase(mainEase);
		}
	}

	public void Play() {
		if(currRState == ROTATION_STATE.IDLE) {
			SetStates(currRState, ROTATION_STATE.PLAY);

			Vector3 _rotation = new Vector3(
			transform.rotation.eulerAngles.x + (axisDirection[0] * 360),
			transform.rotation.eulerAngles.y + (axisDirection[1] * 360),
			transform.rotation.eulerAngles.z + (axisDirection[2] * 360));

			rotTween = gameObject.transform.DORotate(_rotation, cycleDuration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(mainEase);
		}		
	}

	public void TogglePause() {
		Debug.Log("Toggling Pause");
		if (currRState == ROTATION_STATE.PAUSE) {
			SetStates(currRState, ROTATION_STATE.PLAY);
			rotTween.Play();
		}
		else {
			SetStates(currRState, ROTATION_STATE.PAUSE);
			rotTween.Pause();
		}
	}

	public void Stop() {
		if(currRState == ROTATION_STATE.PLAY) {
			SetStates(currRState, ROTATION_STATE.STOP);
			if(rotTween.IsInitialized()) {
				if(rotTween.IsPlaying()) {
					StartCoroutine(StopRotation());
				}
			}
		}
	}

	IEnumerator StopRotation() {
		rotTween.SmoothRewind();

		yield return new WaitForSeconds(5f);

		SetStates(currRState, ROTATION_STATE.IDLE);
	}
}
