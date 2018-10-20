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

	void Start() {
		ui = gameObject.GetComponent<RawImage>();
		if(transform.childCount > 0) {
			text = transform.GetChild(0).GetComponent<Text>();
		}

		setUnknown(isUnknown);
	}

	public void setUnknown(bool cond) {
		if(cond) {
			isUnknown = true;
			hide();
		}
		else {
			isUnknown = false;
			reveal();
		}
	}

	void hide() {
		ui.DOColor(unkownColor, .25f);
		text.DOFade(1f, .25f);
	}

	void reveal() {
		ui.DOColor(color, .5f);
		text.DOFade(0f, .5f);
	}
}
