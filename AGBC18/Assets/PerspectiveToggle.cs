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
	public Camera mainCamera;

	public string debug_state, debug_cursor = "";
	BAR_STATES current = BAR_STATES.INVALID;
	VIEW_STATES hovered, selected = VIEW_STATES.INVALID;

	void Start () {
		mainCamera = Camera.main;
		selected = VIEW_STATES.ORTHOGRAPHIC;
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
				Debug.Log("Nada");
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
		mainCamera.orthographic = true;		
	}

	void perspectiveState() {
		orthographic.transform.GetChild(0).GetComponent<Text>().DOFade(.35f, .5f);
		perspective.transform.GetChild(0).GetComponent<Text>().DOFade(1f, .5f);
		mainCamera.orthographic = false;		
	}


	void assessCursor() {
		if(Input.mousePosition.y > 570 && Input.mousePosition.y < 600) {
			//debug_cursor = "perspective";
			hovered = VIEW_STATES.PERSPECTIVE;
			debug_state = hovered.ToString();
		}
		else if(Input.mousePosition.y > 603 && Input.mousePosition.y < 633) {
			//debug_cursor = "orthographic";		
			hovered = VIEW_STATES.ORTHOGRAPHIC;
			debug_state = hovered.ToString();
		}
		else {
			debug_cursor = "";
			hovered = VIEW_STATES.INVALID;
			debug_state = hovered.ToString();
		}
	}


	IEnumerator drop() {
		perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(232, 574), 1, false).SetEase(Ease.OutBack);
		
		yield return new WaitForSeconds(.1f);

		orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(232, -8), 1, false).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(1f);
	}

	IEnumerator raise() {
		orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(232, 66), 1, false).SetEase(Ease.OutBack);
		
		yield return new WaitForSeconds(.1f);

		perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(232, 650), 1, false).SetEase(Ease.OutBack);

		yield return new WaitForSeconds(1f);
	}

}
