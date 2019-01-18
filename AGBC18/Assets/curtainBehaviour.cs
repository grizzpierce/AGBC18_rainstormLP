using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class curtainBehaviour : MonoBehaviour {

	public float waitTime, fadeDuration;
	Tween inital_fade;

	void Start () {
		GetComponent<Image>().DOFade(1f, 0);
		StartCoroutine(startFade());
	}
	
	void Update () {
		if(inital_fade != null) {
			if(inital_fade.IsComplete()) {
				transform.SetSiblingIndex(transform.GetSiblingIndex() - 2);
				gameObject.SetActive(false);
				inital_fade.Kill();
			}
		}
	}

	IEnumerator startFade() {

		yield return new WaitForSeconds(waitTime);
		inital_fade = GetComponent<Image>().DOFade(0f, fadeDuration).SetAutoKill(false);
	}

	public void setFade(bool _turningOn) {
		if(_turningOn) {
			GetComponent<Image>().DOFade(.95f, .5f);
		}
		else {
			GetComponent<Image>().DOFade(0f, .5f);			
		}

	}
 
}
