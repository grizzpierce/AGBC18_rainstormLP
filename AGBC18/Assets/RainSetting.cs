using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSetting : MonoBehaviour {

	enum TOGGLE_OPTIONS {
		INVALID = -1,
		LIGHTWEIGHT,
		ROBUST,
		OPTION_COUNT
	}

	public GameObject lightweighToggle, robustToggle;
	TOGGLE_OPTIONS currentOp, prevOp;

	void Start () {
		currentOp = TOGGLE_OPTIONS.ROBUST;
	}

	public void setToggle(string _name){
		Debug.Log(_name);
		convertName(_name);

		if(currentOp != prevOp) {
			switch(currentOp) {
				case TOGGLE_OPTIONS.LIGHTWEIGHT:
				lightweight();
				break;

				case TOGGLE_OPTIONS.ROBUST:
				robust();
				break;
			}
		}
	}

	void convertName(string _name) {
		prevOp = currentOp;

		if(_name == "LIGHTWEIGHT")
			currentOp = TOGGLE_OPTIONS.LIGHTWEIGHT;

		if(_name == "ROBUST")
			currentOp = TOGGLE_OPTIONS.ROBUST;			

		Debug.Log(currentOp.ToString());
	}

	void lightweight() {
		robustToggle.GetComponent<RadioButton>().setSelected(false);
		lightweighToggle.GetComponent<RadioButton>().setSelected(true);

	}

	void robust() {
		lightweighToggle.GetComponent<RadioButton>().setSelected(false);
		robustToggle.GetComponent<RadioButton>().setSelected(true);

	}
}
