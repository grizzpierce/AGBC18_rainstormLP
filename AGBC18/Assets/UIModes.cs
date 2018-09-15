using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIModes : MonoBehaviour {

	public GameObject cursorUI, timeUI, cassetteUI, dialogUI, cassetteBar;

	void Start() {
		if(cursorUI == null || timeUI == null || cassetteBar == null) {
			Destroy(this);
		}
		activateTimeUI(false);
	}

	public void LaunchMain() {
		activateTimeUI(true);
		//cassetteUI.SetActive(true);
		dialogUI.SetActive(false);
		cassetteBar.SetActive(true);
	}

	public void cursorActive(bool isActive) {
		cursorUI.GetComponent<Image>().enabled = isActive;
	}

	void activateTimeUI(bool isActivating) {
		if(isActivating) {
			timeUI.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, 300, 0), 1f);
		}
		else {
			timeUI.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, 340, 0), 0f);
		}
	}
}
