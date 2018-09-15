﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteBar : MonoBehaviour {

	public GameObject cassetteRevolver;
	int clickAction = -2, prevClickAction = -3;
	public string DEBUG_CLICKACTION = "";

	bool isOpen = false;
	Tween raising, lowering;

	int raisedCount = 0;
	float idleTime = 0;
	float timer = 25f;
	bool showcasing = false;

	bool isPlaying = false;

	void resetCursorStatus() {
		clickAction = -2;
		prevClickAction = -3;
		cassetteRevolver.GetComponent<CassetteSelector>().resetHovers();
	}

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
		else {
			if(raisedCount < 5)
				idleTime = idleTime + Time.deltaTime;
		}

		if(raisedCount < 5) {
			if(idleTime >= timer) {
				showcasing = true;
				timer = timer * 1.5f;
				StartCoroutine(showInteraction());

			}
		}
	}

	IEnumerator showInteraction() {

		cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -60, 0), 1f).SetEase(Ease.OutBack);		

		yield return new WaitForSeconds(.9f);

		cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -100, 0), .75f);
		
		idleTime = 0;

		yield return new WaitForSeconds(.5f);

		showcasing = false;
	}

    public void RaiseBar() {
		if(!showcasing) {
			raisedCount++;
			idleTime = 0;
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
			//gameObject.transform.parent.GetComponent<UIModes>().cursorActive(false);
			raising = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -30, 0), 1f).SetEase(Ease.OutBack);
		}
	}

    public void LowerBar() {
		
		isOpen = false;

		if(raising.IsInitialized()) {
			if(raising.IsPlaying()) {
				raising.Complete();
			}
		}
		//gameObject.transform.parent.GetComponent<UIModes>().cursorActive(true);
		lowering = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -100, 0), .5f);
		
		resetCursorStatus();
    }

	public void PressBar() {
		switch(clickAction) {
			case -1:
				cassetteRevolver.GetComponent<CassetteSelector>().ShiftRight();
				resetCursorStatus();
				break;

			case 0:
				centerClick();
				resetCursorStatus();
				break;

			case 1:
				cassetteRevolver.GetComponent<CassetteSelector>().ShiftLeft();
				resetCursorStatus();
				break;

			default:
				Debug.Log("STATE MACHINE CODE ERROR");
				break;
		}
	}

	void interactionsMode() {

		prevClickAction = clickAction;	

		if (Input.mousePosition.x < 252) {
			clickAction = -1;
		}
		else if (Input.mousePosition.x > 252 && Input.mousePosition.x < 392) {
			clickAction = 0;
			//DEBUG_CLICKACTION = "CENTER";
		}
		else {
			clickAction = 1;
			//DEBUG_CLICKACTION = "RIGHT";
		}

		// MORE POLISH FOR SHOWING WHAT CASSETTE IS BEING CURRENTLY HOVERED (COLOR AND MORE MOTION POSSIBLY?)
		if(clickAction != prevClickAction)
			cassetteRevolver.GetComponent<CassetteSelector>().Hover(clickAction);
	}

	void centerClick() {
		if(!isPlaying) {
			isPlaying = true;
			Play();
		}
		else {
			isPlaying = false;
			Stop();
		}
	}

	void Play() {

	}

	void Stop() {
		
	}
}
