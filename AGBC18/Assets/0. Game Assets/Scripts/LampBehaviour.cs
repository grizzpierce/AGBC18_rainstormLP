using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LampBehaviour : MonoBehaviour {

	public GameObject spotLight, areaLight;
	public bool isLit = true;
	public bool isChanging = false;

	public float transitionTime = 0.5f;

	[FMODUnity.EventRef]
	public string lampInteractAudio;


	void OnMouseDown()
	{	
		if(isChanging == false) {
			isChanging = true;

			FMODUnity.RuntimeManager.PlayOneShot(lampInteractAudio);
			if (isLit) {
				this.GetComponent<DirectionalParameterControllerNameSpace.DirectionalParameterController>().SetEventPaused(true);
			} else {
				this.GetComponent<DirectionalParameterControllerNameSpace.DirectionalParameterController>().SetEventPaused(false);
			}

			StartCoroutine(toggleLight());
		}
	}

	IEnumerator toggleLight() {
		if(isLit) {
			spotLight.GetComponent<Light>().DOIntensity(0, transitionTime);
			areaLight.GetComponent<Light>().DOIntensity(0, transitionTime);
			isLit = false;
		}

		else {
			spotLight.GetComponent<Light>().DOIntensity(6, transitionTime);
			areaLight.GetComponent<Light>().DOIntensity(12, transitionTime);
			isLit = true;
		}

		yield return new WaitForSeconds(transitionTime);
		isChanging = false; 
	}
}
