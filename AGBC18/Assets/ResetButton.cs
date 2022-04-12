using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	public GameObject ksManager;

	void Start (){
		if(!ksManager.GetComponent<KioskManager>().isKioskModeOn) {
			Destroy(this.gameObject);
		}
	}
	
	public void OnClick() {
		ksManager.GetComponent<KioskManager>().resetGame();
	}
	
}
