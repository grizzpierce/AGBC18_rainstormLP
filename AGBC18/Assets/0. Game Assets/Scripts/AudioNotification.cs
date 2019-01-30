using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioNotification : MonoBehaviour {

	public CartridgeRotator cassette;
	public RawImage ui;
	Text trackTitle;
	// bool DEBUG_TEST = false;
	Color stopColor = new Color(0, 0, 0, .1f);


	void Start () {
		ui = transform.GetChild(0).gameObject.GetComponent<RawImage>();
		trackTitle = transform.GetChild(1).GetComponent<Text>();
		trackTitle.color = new Color(255, 255, 255, 0);
	}
	
	public void Launch() {
		ui.DOFade(.25f, 2f).SetEase(Ease.InQuad);
	}

	public void Play(Color colr, string track) {
		Color alphaColor = new Color(colr.r, colr.g, colr.b, .25f);
		cassette.playGeneric();
		ui.DOColor(alphaColor, .5f);

		StartCoroutine(resetText(track));
	}

	IEnumerator resetText(string track) {
		trackTitle.DOFade(0, 1f);

		yield return new WaitForSeconds(1f);
		trackTitle.text = track;
		trackTitle.DOFade(1, 1f);
	}

	public void Stop() {
		cassette.Stop();
		ui.DOColor(stopColor, .2f);
		trackTitle.DOFade(0, .2f);
	}
}
