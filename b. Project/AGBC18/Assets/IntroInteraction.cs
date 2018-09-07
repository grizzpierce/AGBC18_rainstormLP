using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroInteraction : MonoBehaviour {

	public MapRotator rotator;

	// Use this for initialization
	void Start () {
		if(rotator == null) 
			Destroy(this);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
