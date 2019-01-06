using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PerspectiveToggle : MonoBehaviour {

	enum BAR_STATES {
		INVALID = -1,
		HOVER,
		EXIT
	}

	enum VIEW_STATES {
		INVALID = -1,
		ORTHOGRAPHIC,
		PERSPECTIVE
	}

	public GameObject perspective, orthographic;
	public float uiH, uiX, perY, orthY = 0;
	public Camera mainCamera;
	Animator anim;

	public string debug_state, debug_cursor = "";
	BAR_STATES current = BAR_STATES.INVALID;
	VIEW_STATES hovered, selected = VIEW_STATES.INVALID;

	public float pixels = 8;

	void Start () {
		mainCamera = Camera.main;
		anim = mainCamera.transform.parent.GetComponent<Animator>();
		selected = VIEW_STATES.ORTHOGRAPHIC;

		uiX = perspective.GetComponent<RectTransform>().anchoredPosition.x;
		uiH = perspective.GetComponent<RectTransform>().sizeDelta.y;

		perY = perspective.GetComponent<RectTransform>().anchoredPosition.y;
		orthY = orthographic.GetComponent<RectTransform>().anchoredPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		assessCursor();
	}

	public void onHover() {
		current = BAR_STATES.HOVER;
		StartCoroutine(drop());
	}

	public void onExit() {
		current = BAR_STATES.EXIT;
		StartCoroutine(raise());
	}

	public void onClick() {
		switch(hovered) {
			case VIEW_STATES.INVALID:
				//Debug.Log("Nada");
				break;
			case VIEW_STATES.ORTHOGRAPHIC:
				orthographicState();
				break;
			case VIEW_STATES.PERSPECTIVE:
				perspectiveState();
				break;
			default:

				break;
		}
	}

	void orthographicState() {
		perspective.transform.GetChild(0).GetComponent<Text>().DOFade(.35f, .5f);
		orthographic.transform.GetChild(0).GetComponent<Text>().DOFade(1f, .5f);
		if(!mainCamera.orthographic)
			cameraShift(true);
	}

	void perspectiveState() {
		orthographic.transform.GetChild(0).GetComponent<Text>().DOFade(.35f, .5f);
		perspective.transform.GetChild(0).GetComponent<Text>().DOFade(1f, .5f);
		if(mainCamera.orthographic)
			cameraShift(false);
	}


	void assessCursor() {
		if(Input.mousePosition.y > (Screen.height - (32 + (2 * uiH))) && Input.mousePosition.y < (Screen.height - (28 + uiH))) {
			debug_cursor = "perspective";
			hovered = VIEW_STATES.PERSPECTIVE;
			debug_state = hovered.ToString();
		}
		else if(Input.mousePosition.y > (Screen.height - (14 + uiH)) && Input.mousePosition.y < (Screen.height - 10)) {
			debug_cursor = "orthographic";		
			hovered = VIEW_STATES.ORTHOGRAPHIC;
			debug_state = hovered.ToString();
		}
		else {
			debug_cursor = "";
			hovered = VIEW_STATES.INVALID;
			debug_state = hovered.ToString();
		}
	}

	void cameraShift(bool _isOrtho) {
        string anim_name;

		if(_isOrtho)
			anim_name = "ToOrtho";
		else
			anim_name = "ToPersp";
		
		anim.Play(anim_name);
	}

	IEnumerator drop() {
		perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, perY), 1, false).SetEase(Ease.OutBack);
		
		yield return new WaitForSeconds(.1f);

		orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, orthY), 1, false).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(1f);
	}

	IEnumerator raise() {
		orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, orthY + 120), 1, false).SetEase(Ease.OutBack);
		
		yield return new WaitForSeconds(.1f);

		perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, perY + 120), 1, false).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(1f);
	}

}
