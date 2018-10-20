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

	void Start() {
		camPos = mainCam.transform.localPosition;
		shake = mainCam.DOShakePosition(0, 0, 0, 0, true);
	}

	public void Launch() {
		playing = first;
	}

	void Update() {
		if(shake != null) {
			if(!shake.IsPlaying()) {
				if(mainCam.transform.localPosition != camPos) {
					mainCam.transform.localPosition = camPos;
				}
			}
		}
	}

	public void assess() {
		GameObject pressed = activeSlot.transform.GetChild(0).gameObject;

		if(!pressed.GetComponent<CartridgeData>().isUnknown) {
			if(pressed == playing) {
				notifier.Stop();
				playing = null;
			}
			else {
				notifier.Play(pressed.GetComponent<RawImage>().color);
				playing = pressed;
			}
		}

		else {
			shake = mainCam.DOShakePosition(1f, .2f, 10, 90, true);
		}
	}
}
