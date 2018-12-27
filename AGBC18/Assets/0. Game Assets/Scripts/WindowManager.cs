using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

	int canvasSize = 960;

	// Use this for initialization
	void Start () {
		Screen.SetResolution(canvasSize, canvasSize, FullScreenMode.Windowed);		
	}
	
	void FixedUpdate () {
		if(Screen.width != canvasSize || Screen.height != canvasSize) {
			Screen.SetResolution(canvasSize, canvasSize, FullScreenMode.Windowed);			
		}
	}
}
