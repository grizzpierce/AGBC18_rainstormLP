using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteBar : MonoBehaviour {

	public GameObject cassetteRevolver;
	int clickAction = -2;
	public string DEBUG_CLICKACTION = "";

	bool isOpen = false;
	Tween raising, lowering;


	void Start() {
		if (cassetteRevolver == null)
			Destroy(this);

		lowering = raising = gameObject.GetComponent<RawImage>().DOFade(0f ,0f);
	}

	void Update()
	{
		if(isOpen) {
			interactionsMode();
		}
	}

    public void RaiseBar() {

		isOpen = true;

		if(lowering.IsInitialized()) {
			if(lowering.IsPlaying()) {
				lowering.Complete();
			}
		}

		/* 
		if(raising.IsInitialized()) {
			if(raising.IsPlaying()) {
				raising.Kill(false);
			}
		}
		*/	
		gameObject.transform.parent.GetComponent<UIModes>().cursorActive(false);
		raising = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -30, 0), 2f).SetEase(Ease.OutElastic);

	}

    public void LowerBar() {
		
		isOpen = false;

		if(raising.IsInitialized()) {
			if(raising.IsPlaying()) {
				raising.Complete();
			}
		}
		gameObject.transform.parent.GetComponent<UIModes>().cursorActive(true);
		lowering = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -100, 0), .5f);

    }

	public void PressBar() {
		switch(clickAction) {
			case -1:
				cassetteRevolver.GetComponent<CassetteSelector>().ShiftLeft();
				break;
			case 0:
				// TOGGLE PLAY/PAUSE
				break;
			case 1:
				cassetteRevolver.GetComponent<CassetteSelector>().ShiftRight();
				break;
			default:
				Debug.Log("STATE MACHINE CODE ERROR");
				break;
		}
	}

	void interactionsMode() {
		//Debug.Log(Input.mousePosition);

		// MORE POLISH FOR SHOWING WHAT CASSETTE IS BEING CURRENTLY HOVERED (COLOR AND MORE MOTION POSSIBLY?)

		if (Input.mousePosition.x < 252) {
			clickAction = -1;
			//DEBUG_CLICKACTION = "LEFT";
		}
		else if (Input.mousePosition.x > 252 && Input.mousePosition.x < 392) {
			clickAction = 0;
			//DEBUG_CLICKACTION = "CENTER";
		}
		else {
			clickAction = 1;
			//DEBUG_CLICKACTION = "RIGHT";
		}
	}
}
