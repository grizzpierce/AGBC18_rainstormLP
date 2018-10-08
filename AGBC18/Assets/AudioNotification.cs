using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioNotification : MonoBehaviour {

	public CartridgeRotator cassette;
	public RawImage ui;
	bool DEBUG_TEST = false;
	Color stopColor = new Color(0, 0, 0, .25f);

	// Use this for initialization
	void Start () {
		ui = transform.GetChild(0).gameObject.GetComponent<RawImage>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.I)) {
			if(!DEBUG_TEST) {
				DEBUG_TEST = true;
				Stop();
			}
		}
		if(Input.GetKeyDown(KeyCode.O)) {
			if(DEBUG_TEST) {
				DEBUG_TEST = false;
				Play(new Color(1, 0, 0, 1));
			}
		}

	}

	void Play(Color colr) {
		cassette.Play();
		ui.DOColor(colr, .5f);
	}

	void Stop() {
		cassette.Stop();
		ui.DOColor(stopColor, .5f);
	}
}
