using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour {

	public GameObject curtain, menuButton, closeButton;

	private GameObject LAST_PRESSED, NOW_AVAILABLE;

	public void Controller(bool _isOpening) {
		if(_isOpening) {
			LAST_PRESSED = menuButton;
			NOW_AVAILABLE = closeButton;
		}
		else {
			LAST_PRESSED = closeButton;
			NOW_AVAILABLE = menuButton;			
		}

		StartCoroutine(controllerSequence(_isOpening));
	}

		/* STEPS FOR MENU /*
		
		1. Turn off [LAST PRESSED] button interactivity
		2. Fade out [LAST PRESSED] button canvas group
		3. Transition in curtain + menu content
		4. Fade in [NOW AVAILABLE] button canvas group
		5. Turn on [NOW AVAILABLE button canvas group]

		*/


	IEnumerator controllerSequence(bool _isOpening) {
		float _isOpeningVal = (float)(_isOpening ? 1 : 0);

	// PHASE 1: Turn off [LAST PRESSED] button interactivity
		LAST_PRESSED.GetComponent<CanvasGroup>().interactable = false;
		LAST_PRESSED.GetComponent<CanvasGroup>().blocksRaycasts = false;

	// PHASE 2: Fade out [LAST PRESSED] button canvas group
		LAST_PRESSED.GetComponent<CanvasGroup>().DOFade(0, .5f);

	// PHASE 3: Transition in curtain + menu content
		curtain.GetComponent<curtainBehaviour>().setCanvasGroupActive(_isOpening);
		curtain.GetComponent<curtainBehaviour>().setFade(_isOpening);
		GetComponent<CanvasGroup>().DOFade(_isOpeningVal, .5f);
		yield return new WaitForSeconds(.5f);

	// PHASE 4: Fade in [NOW AVAILABLE] button canvas group
		NOW_AVAILABLE.GetComponent<CanvasGroup>().DOFade(.5f, .25f);
		yield return new WaitForSeconds(.25f);

	// PHASE 5: Turn on [NOW AVAILABLE button canvas group]
		NOW_AVAILABLE.GetComponent<CanvasGroup>().interactable = true;
		NOW_AVAILABLE.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

}
