using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class fadeIn : MonoBehaviour {

	Tween fade;

	void Start () {
		fade = GetComponent<Image>().DOFade(0f, 5f).SetAutoKill(false);
	}
	
	void Update () {
		if(fade.IsComplete()) {
			Destroy(gameObject);
		}
		else {

		}
	}
}
