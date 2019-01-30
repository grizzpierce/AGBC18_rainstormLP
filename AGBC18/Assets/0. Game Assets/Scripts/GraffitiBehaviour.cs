using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GraffitiBehaviour : MonoBehaviour {


	[FMODUnity.EventRef]
	public string graffitiSFX;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		FMODUnity.RuntimeManager.PlayOneShot(graffitiSFX);
		Color _color = Random.ColorHSV(0, 1, .6f, .6f, .4f, .4f, .25f, 1);
		GetComponent<Renderer>().material.DOColor(_color, 1f);
	}
}
