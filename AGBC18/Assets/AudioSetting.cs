using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioSetting : MonoBehaviour {
	
	public List<SettingNode> nodes;
	public int currentVol, prevVol;
	
	void Start () {

		currentVol = transform.childCount;

		for(int i = 0; i < transform.childCount; ++i) {
			nodes.Add(transform.GetChild(i).GetComponent<SettingNode>());
		}
	}
	
	// Update is called once per frame
	public void UpdateSetting(int _num) {
	prevVol = currentVol;
	currentVol = _num;

		if(assessVolume()) {
			Debug.Log("Is Louder");
			Increase();
		}
		else {
			Debug.Log("Is Quieter");
			Decrease();
		}
	}

	// Volume is at 5, you click 10. That means you go 5, 6, 7...
	void Increase() {
		for(int i = prevVol; i <= currentVol; ++i){
			Debug.Log(i);
			nodes[i-1].GetComponent<CanvasGroup>().alpha = 1;
		}

		// INSERT FMOD VOLUME INCREASE
	}

	void Decrease() {
		for(int i = prevVol; i >= currentVol; --i){
			Debug.Log(i);
			nodes[i].GetComponent<CanvasGroup>().alpha = .5F;
		}

		// INSERT FMOD VOLUME DECREASE
	}

	bool assessVolume() {
		if(currentVol > prevVol) {
			return true;
		}

		else {
			return false;
		}

	}
}
