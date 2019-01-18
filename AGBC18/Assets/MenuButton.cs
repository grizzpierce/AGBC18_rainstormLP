using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuButton : MonoBehaviour {

	Animator anim;
	public GameObject canvas;

	Tween fade_tween;

	void Start () {
		anim = GetComponent<Animator>();
	}

	void Update () {
		
	}

	void tweenReset(Tween _tween) {
		if(_tween != null) {
			if(_tween.IsComplete()){
				_tween.Kill();
			}
		}
	}

	public void Hover() {
		tweenReset(fade_tween);
		GetComponent<CanvasGroup>().DOFade(.75f, .25f);
	} 

	public void Exit() {
		if(GetComponent<CanvasGroup>().interactable == true) {
			tweenReset(fade_tween);
			GetComponent<CanvasGroup>().DOFade(1f, .25f);		
		}
	}

	public void Pressed() {
		tweenReset(fade_tween);
		GetComponent<CanvasGroup>().DOFade(0, .25f);

		GetComponent<CanvasGroup>().interactable = false;
		canvas.GetComponent<UIModes>().MenuController(true);
	}
}
