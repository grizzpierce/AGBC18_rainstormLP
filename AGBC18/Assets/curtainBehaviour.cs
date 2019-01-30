using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class curtainBehaviour : MonoBehaviour {

	public float waitTime, fadeDuration;
	Tween inital_fade;

	void Start () {
		GetComponent<CanvasGroup>().DOFade(1f, 0);
		StartCoroutine(startFade());
	}
	
	void Update () {
		if(inital_fade != null) {
			if(inital_fade.IsComplete()) {

				transform.parent = GameObject.Find("Menu").transform;
				GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0, false);
				transform.SetAsFirstSibling();


				setCanvasGroupActive(true);
				inital_fade.Kill();
				setFade(true);
			}
		}
	}

	public void setCanvasGroupActive(bool _cond) {
		GetComponent<CanvasGroup>().interactable = _cond;
		GetComponent<CanvasGroup>().blocksRaycasts = _cond;
	}

	IEnumerator startFade() {

		yield return new WaitForSeconds(waitTime);
		inital_fade = GetComponent<CanvasGroup>().DOFade(0f, fadeDuration).SetAutoKill(false);
	}

	public void setFade(bool _turningOn) {
		if(_turningOn) {
			GetComponent<CanvasGroup>().DOFade(.95f, .5f);
		}
		else {
			GetComponent<CanvasGroup>().DOFade(0f, .5f);			
		}

	}
 
}
