using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorActions : MonoBehaviour {

	public GameObject cassetteRevolver;

	// Use this for initialization
	void Start () {
		
		if(cassetteRevolver == null) {
			Destroy(this);
		}

	}

	public void Right () {
		Debug.Log("CLICK");
		cassetteRevolver.GetComponent<CassetteSelector>().ShiftRight();
	}
}
