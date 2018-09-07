using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIModes : MonoBehaviour {

	public GameObject timeUI, cassetteUI, dialogUI;

	public void LaunchMain() {
		timeUI.SetActive(true);
		cassetteUI.SetActive(true);
		dialogUI.SetActive(false);
	}

}
