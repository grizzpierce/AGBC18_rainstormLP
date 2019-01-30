using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class SettingNode : MonoBehaviour {

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick() {
		int _var = Int32.Parse(gameObject.name);
		//Debug.Log(_var);
		transform.parent.GetComponent<AudioSetting>().UpdateSetting(_var);
	}


}
