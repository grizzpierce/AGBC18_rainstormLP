using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RadioButton : MonoBehaviour {

	GameObject bubble;
	public bool isSelected = false;

	void Awake () {
		bubble = transform.GetChild(1).GetChild(0).gameObject;
	}

	public void setSelected(bool _isSelected) {
		bubble.SetActive(_isSelected);
		isSelected = _isSelected;
	}
}
