using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class fadeIn : MonoBehaviour {

	public float waitTime, fadeDuration;
	Tween fade;

	void Start () {
		StartCoroutine(fadeProcess());

	}
	
	void Update () {
		if(fade != null) {
			if(fade.IsComplete()) {
				Destroy(gameObject);
			}
		}
	}

	IEnumerator fadeProcess() {

		yield return new WaitForSeconds(waitTime);
		fade = GetComponent<Image>().DOFade(0f, fadeDuration).SetAutoKill(false);
	}
}
