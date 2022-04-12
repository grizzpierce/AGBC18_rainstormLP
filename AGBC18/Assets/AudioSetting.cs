using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioSetting : MonoBehaviour {
	
	public List<SettingNode> nodes;
	int currentVol, prevVol;
	public string audioType;

	void Start () {
		currentVol = transform.childCount;
		Debug.Log(currentVol);

		for(int i = 0; i < transform.childCount; ++i) {
			nodes.Add(transform.GetChild(i).GetComponent<SettingNode>());
		}
	}
	
	// Update is called once per frame
	public void UpdateSetting(int _num) {
	prevVol = currentVol;
	currentVol = _num;

	switch (assessVolume()) {
		case 1:
			Debug.Log("Is Louder");
			Increase();
			break;
		case 2:
			Debug.Log("Is Quieter");
			Decrease();
			break;
		case 3:
			Debug.Log("It's equal.");
			break;
		default:
			print("Error.");
			break;
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
		for(int i = prevVol; i > currentVol; --i){
			Debug.Log(i);
			nodes[i-1].GetComponent<CanvasGroup>().alpha = .5F;
		}

		// INSERT FMOD VOLUME DECREASE
	}

	int assessVolume() {
		if(currentVol > prevVol) {
			return 1;
		}

		else if (currentVol < prevVol){
			return 2;
		}

		else {
			return 3;
		}

	}
}
