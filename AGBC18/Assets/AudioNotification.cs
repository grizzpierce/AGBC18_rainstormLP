using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioNotification : MonoBehaviour {

	public CartridgeRotator cassette;
	public RawImage ui;
	bool DEBUG_TEST = false;
	Color stopColor = new Color(0, 0, 0, .1f);


	void Start () {
		ui = transform.GetChild(0).gameObject.GetComponent<RawImage>();

	}
	
	public void Launch() {
		ui.DOFade(.25f, 2f).SetEase(Ease.InQuad);
	}

	public void Play(Color colr) {
		Color alphaColor = new Color(colr.r, colr.g, colr.b, .25f);
		cassette.playGeneric();
		ui.DOColor(alphaColor, .5f);
	}

	public void Stop() {
		cassette.Stop();
		ui.DOColor(stopColor, .2f);
	}
}
