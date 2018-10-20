using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupManager : MonoBehaviour {

	enum POPUP_STATE {
		INVALID = -1,
		INACTIVE,
		LOADING,
		IDLE,
		CLOSING
	}

	public Text textUI;
	public Image textContainer;
	public RawImage cassetteUI;
	public GameObject readyUI;

	POPUP_STATE current = POPUP_STATE.INVALID;
	public string DEBUG_STATE = "INVALID";

	string dialog = "No text was entered. Please use Pop(string text);";
	Color cassetteColor;
	public CartridgeData cassette;

	void SetState(POPUP_STATE state) {
		current = state;
		DEBUG_STATE = state.ToString();
	}
	public bool GetIfAvailable() {
		if (current == POPUP_STATE.INACTIVE)
			return true;

		return false;
	}

	void Start() {
		textUI.text = "";
		SetState(POPUP_STATE.INACTIVE);
	}

	public void OnClick() {
		if(current == POPUP_STATE.IDLE) {
			readyUI.SetActive(false);
			cassette = null;
			SetState(POPUP_STATE.CLOSING);
			StartCoroutine(Popdown());
		}

	}

	// LEGACY -> FOR TESTING ONLY
	void Pop() {
		if(current == POPUP_STATE.INACTIVE) {
			textUI.text = "";
			SetState(POPUP_STATE.LOADING);
			StartCoroutine(Popup());
		}
	}

	public void Pop(string text, Color color) {
		dialog = text;
		cassetteColor = color;

		if(current == POPUP_STATE.INACTIVE) {
			textUI.text = "";
			SetState(POPUP_STATE.LOADING);
			StartCoroutine(Popup());
		}
	}

	public void Pop(string text, Color color, CartridgeData cartridge) {
		dialog = text;
		cassetteColor = color;
		cassette = cartridge;

		if(current == POPUP_STATE.INACTIVE) {
			textUI.text = "";
			SetState(POPUP_STATE.LOADING);
			StartCoroutine(Popup());
		}
	}

	IEnumerator Popup() {
		transform.DOMove(new Vector3(Screen.width/2, Screen.height/2, 0), 1f, false);
		textContainer.DOFade(.3f, 1f);
		textUI.DOFade(1f, 0f);

		yield return new WaitForSeconds(1f);

		char[] temp = dialog.ToCharArray();
		for (int i = 0; i < dialog.Length; ++i) {
			textUI.text = textUI.text + temp[i];
			yield return new WaitForSeconds(.05f);
		}

		cassetteUI.transform.DOMove(new Vector3(Screen.width/2, Screen.height/2 + 75, 0), 1f, false);
		cassetteUI.DOColor(cassetteColor, 1f);
		yield return new WaitForSeconds(1f);

		if(cassette != null)
			cassette.setUnknown(false);

		readyUI.SetActive(true);
		SetState(POPUP_STATE.IDLE);
	}

	IEnumerator Popdown() {

		textUI.DOFade(0f, .25f);

		cassetteUI.transform.DOMove(new Vector3(Screen.width/2, Screen.height/2 + 70, 0), .5f, false);
		cassetteUI.DOColor(new Color(0 , 0, 0, 0), .5f);

		yield return new WaitForSeconds(.5f);

		transform.DOMove(new Vector3(Screen.width/2, Screen.height/2 - 30, 0), 1f, false);
		textContainer.DOFade(0f, 1f);

		yield return new WaitForSeconds(1f);

		SetState(POPUP_STATE.INACTIVE);
	}
}
