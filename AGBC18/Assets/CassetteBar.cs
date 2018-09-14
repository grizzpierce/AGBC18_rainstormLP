using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CassetteBar : MonoBehaviour {

	public GameObject cassetteRevolver;

	Tween raising, lowering;


	void Start() {
		if (cassetteRevolver == null)
			Destroy(this);

		lowering = raising = gameObject.GetComponent<RawImage>().DOFade(0f ,0f);
	}


    public void RaiseBar() {

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

		raising = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -30, 0), 2f).SetEase(Ease.OutElastic);

	}

    public void LowerBar() {
		
		if(raising.IsInitialized()) {
			if(raising.IsPlaying()) {
				raising.Complete();
			}
		}

		lowering = cassetteRevolver.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -100, 0), .5f);

    }	
}
