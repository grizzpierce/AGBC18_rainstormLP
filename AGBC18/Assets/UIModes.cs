using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIModes : MonoBehaviour {

	public GameObject cursorUI, timeUI, cassetteUI, dialogUI, cassetteBar;

	public void LaunchMain() {
		timeUI.SetActive(true);
		cassetteUI.SetActive(true);
		dialogUI.SetActive(false);
		cassetteBar.SetActive(true);
	}

	public void cursorActive(bool isActive) {
		cursorUI.GetComponent<Image>().enabled = isActive;
	}

}
