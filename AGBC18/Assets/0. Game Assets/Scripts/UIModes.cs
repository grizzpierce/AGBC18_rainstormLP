using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIModes : MonoBehaviour {

	public GameObject cursorUI, timeUI, cassetteUI, dialogUI, cassetteBar, prespectiveUI;

	void Start() {
		if(cursorUI == null || timeUI == null || cassetteBar == null) {
			Destroy(this);
		}
		activateTimeUI(false);
	}

	public void LaunchMain() {
		activateTimeUI(true);
		//cassetteUI.GetComponent<AudioNotification>().Launch();
		dialogUI.SetActive(true);
		cassetteBar.SetActive(true);
		prespectiveUI.SetActive(true);
	}

	public void cursorActive(bool isActive) {
		cursorUI.GetComponent<Image>().enabled = isActive;
	}

	void activateTimeUI(bool isActivating) {
		if(isActivating) {
			timeUI.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, -40), 1f);
		}
		else {
			timeUI.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 30), 0f);
		}
	}
}
