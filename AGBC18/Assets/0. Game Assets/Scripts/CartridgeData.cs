using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CartridgeData : MonoBehaviour {

	public Color color;
	Color unkownColor = new Color32(75, 75, 75, 255);
	public bool isUnknown = false;

	RawImage ui;
	public Text text;

    [FMODUnity.EventRef]
    [SerializeField] public string trackAudioEvent;


	void Start() {
		ui = gameObject.GetComponent<RawImage>();
		if(transform.childCount > 0) {
			text = transform.GetChild(0).GetComponent<Text>();
		}

		SetUnknown(isUnknown);
	}

	public void SetUnknown(bool cond) {
		if(cond) {
			isUnknown = true;
			Hide();
		}
		else {
			isUnknown = false;
			Reveal();
		}
	}

    void Hide() {
		ui.DOColor(unkownColor, .25f);
		text.DOFade(1f, .25f);
	}

    void Reveal() {
		ui.DOColor(color, .5f);
		text.DOFade(0f, .5f);
	}
}
