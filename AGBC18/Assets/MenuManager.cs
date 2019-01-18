using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour {

	public GameObject closeButton;

	public void Controller(bool _isOpening) {
		float boolVal = (float)(_isOpening ? 1 : 0);
		
		GetComponent<CanvasGroup>().interactable = _isOpening;
		GetComponent<CanvasGroup>().blocksRaycasts = _isOpening;
		GetComponent<CanvasGroup>().DOFade(boolVal, .5f);


		closeButton.GetComponent<CanvasGroup>().DOFade((float)boolVal, .5f).SetDelay(boolVal*.25f);
	}
}
