using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour {

	Animator anim;
	public GameObject canvas;

	void Start () {
		anim = GetComponent<Animator>();
	}

	void Update () {
		
	}

	public void Hover() {
		anim.Play("Hover");
	} 

	public void Pressed() {
		canvas.GetComponent<UIModes>().MenuController(false);
	}
}
