using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuButton : MonoBehaviour {

	public GameObject menu;
	public bool willOpenMenu;
	Tween fade_tween;

	void Start () {

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
		Debug.Log("HOVERING " + gameObject.name + " BUTTON");
		tweenReset(fade_tween);
		GetComponent<CanvasGroup>().DOFade(1f, .25f);
	} 

	public void Exit() {
		tweenReset(fade_tween);
		GetComponent<CanvasGroup>().DOFade(.5f, .25f);		
	}

	public void Pressed() {
		Debug.Log("PRESSING " + gameObject.name + " BUTTON");
		tweenReset(fade_tween);

		menu.GetComponent<MenuManager>().Controller(willOpenMenu); // IS OPENING
	}
}
