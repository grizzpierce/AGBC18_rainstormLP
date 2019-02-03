using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

	public int canvasSize = 960;
	public GameObject main_camera;

	// Use this for initialization
	void Start () {
		Screen.SetResolution(canvasSize, canvasSize, FullScreenMode.Windowed);		
	}
	
	public void setSize(int size) {
		canvasSize = size;

		Screen.SetResolution(canvasSize, canvasSize, FullScreenMode.Windowed);	

		if(canvasSize >= 960) {
			//main_camera.GetComponent<Renderscale>().m_RenderScale = 8;
			Debug.Log("Render Scale set to 8");
		}

		else if(canvasSize >= 640) {
			//main_camera.GetComponent<Renderscale>().m_RenderScale = 4;
			Debug.Log("Render Scale set to 4");
		}

		else {
			//main_camera.GetComponent<Renderscale>().m_RenderScale = 2;
			Debug.Log("Render Scale set to 2");
		}

	}

	void FixedUpdate () {
		if(Screen.width != canvasSize || Screen.height != canvasSize) {
			Screen.SetResolution(canvasSize, canvasSize, FullScreenMode.Windowed);			
		}
	}
}
