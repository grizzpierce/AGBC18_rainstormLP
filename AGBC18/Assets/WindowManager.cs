using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.SetResolution(640, 640, FullScreenMode.Windowed);		
	}
	
	void FixedUpdate () {
		if(Screen.width != 640 || Screen.height != 640) {
			Screen.SetResolution(640, 640, FullScreenMode.Windowed);			
		}
	}
}
