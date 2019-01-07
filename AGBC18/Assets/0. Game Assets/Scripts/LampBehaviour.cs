using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LampBehaviour : MonoBehaviour {

	public GameObject spotLight, areaLight;
	public bool isLit = true;
	public bool isChanging = false;


	void OnMouseDown()
	{	
		if(isChanging == false) {
			isChanging = true;
			StartCoroutine(toggleLight());
		}
	}

	IEnumerator toggleLight() {
		if(isLit) {
			spotLight.GetComponent<Light>().DOIntensity(0, .5f);
			areaLight.GetComponent<Light>().DOIntensity(0, .5f);
			isLit = false;
		}

		else {
			spotLight.GetComponent<Light>().DOIntensity(6, .5f);
			areaLight.GetComponent<Light>().DOIntensity(12, .5f);
			isLit = true;
		}

		yield return new WaitForSeconds(.5f);
		isChanging = false; 
	}
}
