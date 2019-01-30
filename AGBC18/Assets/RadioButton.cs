using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RadioButton : MonoBehaviour {

	GameObject selected;

	void Start () {
		selected = transform.GetChild(1).GetChild(0).gameObject;
	}



	public void OnClick() {
		transform.parent.GetComponent<RainSetting>().setToggle(name);
	}

	public void setSelected(bool _isSelected) {
		selected.SetActive(_isSelected);
	}
}
