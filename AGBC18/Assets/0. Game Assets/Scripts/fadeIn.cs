using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class fadeIn : MonoBehaviour {

	public float waitTime, fadeDuration;
	Tween fade;

	void Start () {
		GetComponent<Image>().DOFade(1f, 0);
		StartCoroutine(fadeProcess());

	}
	
	void Update () {
		if(fade != null) {
			if(fade.IsComplete()) {
				transform.SetSiblingIndex(transform.GetSiblingIndex() - 2);
				gameObject.SetActive(false);
				Destroy(this);
			}
		}
	}

	IEnumerator fadeProcess() {

		yield return new WaitForSeconds(waitTime);
		fade = GetComponent<Image>().DOFade(0f, fadeDuration).SetAutoKill(false);
	}
}
