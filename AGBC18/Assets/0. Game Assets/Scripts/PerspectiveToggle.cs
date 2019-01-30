using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PerspectiveToggle : MonoBehaviour {

	enum BAR_STATES {
		INVALID = -1,
		HOVER,
		EXIT,
		OFF
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
	Tween persp_tween, orth_tween; 

	float exitedTimer = 0;

	public float pixels = 8;

	void Start () {
		mainCamera = Camera.main;
		anim = mainCamera.transform.parent.GetComponent<Animator>();
		selected = VIEW_STATES.ORTHOGRAPHIC;

		uiX = perspective.GetComponent<RectTransform>().anchoredPosition.x;
		uiH = perspective.GetComponent<RectTransform>().sizeDelta.y;

		perY = perspective.GetComponent<RectTransform>().anchoredPosition.y;
		orthY = orthographic.GetComponent<RectTransform>().anchoredPosition.y;

		orth_tween = orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, orthY + 120), 0, false);
		persp_tween = perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, perY + 120), 0, false);

	}
	
	// Update is called once per frame
	void Update () {

		if(current == BAR_STATES.HOVER)
			assessCursor();
		else if(current == BAR_STATES.EXIT) {

			exitedTimer += Time.deltaTime;

			if(exitedTimer > .5f) {

				orth_tween = orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, orthY + 120), .75f, false).SetEase(Ease.OutBack).SetAutoKill(false);
				persp_tween = perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, perY + 120), .75f, false).SetEase(Ease.OutBack).SetDelay(.1f).SetAutoKill(false);

				current = BAR_STATES.OFF;
			}
		}
		

	}

	public void onHover() {
		current = BAR_STATES.HOVER;

		killTweens();

		persp_tween = perspective.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, perY), .5f, false).SetEase(Ease.OutBack).SetAutoKill(false);
		orth_tween = orthographic.GetComponent<RectTransform>().DOAnchorPos(new Vector2(uiX, orthY), .5f, false).SetEase(Ease.OutBack).SetDelay(.1f).SetAutoKill(false);
	}

	public void onExit() {
		current = BAR_STATES.EXIT;
		exitedTimer = 0f;
	}

	public void onClick() {
		switch(hovered) {
			case VIEW_STATES.INVALID:
				//Debug.Log("Nada");
				break;
			case VIEW_STATES.ORTHOGRAPHIC:
				selected = VIEW_STATES.ORTHOGRAPHIC;
				orthographicState();
				break;
			case VIEW_STATES.PERSPECTIVE:
				selected = VIEW_STATES.PERSPECTIVE;
				perspectiveState();
				break;
			default:

				break;
		}
	}

	void orthographicState() {
		perspective.GetComponent<CanvasGroup>().DOFade(.5f, .25f);

		perspective.transform.GetChild(0).GetComponent<Text>().DOFade(.35f, .5f);
		orthographic.transform.GetChild(0).GetComponent<Text>().DOFade(1f, .5f);
		if(!mainCamera.orthographic)
			cameraShift(true);
	}

	void perspectiveState() {
		orthographic.GetComponent<CanvasGroup>().DOFade(.5f, .25f);

		orthographic.transform.GetChild(0).GetComponent<Text>().DOFade(.35f, .5f);
		perspective.transform.GetChild(0).GetComponent<Text>().DOFade(1f, .5f);
		if(mainCamera.orthographic)
			cameraShift(false);
	}


	void assessCursor() {
		if(Input.mousePosition.y > (Screen.height - (32 + (2 * uiH))) && Input.mousePosition.y < (Screen.height - (28 + uiH))) {
			debug_cursor = "perspective";
			hovered = VIEW_STATES.PERSPECTIVE;
			perspective.GetComponent<CanvasGroup>().DOFade(1f, .25f);
			debug_state = hovered.ToString();
		}
		else if(Input.mousePosition.y > (Screen.height - (14 + uiH)) && Input.mousePosition.y < (Screen.height - 10)) {
			debug_cursor = "orthographic";		
			hovered = VIEW_STATES.ORTHOGRAPHIC;
			orthographic.GetComponent<CanvasGroup>().DOFade(1f, .25f);
			debug_state = hovered.ToString();
		}
		else {
			debug_cursor = "";
			hovered = VIEW_STATES.INVALID;

			if(selected == VIEW_STATES.ORTHOGRAPHIC) {
				perspective.GetComponent<CanvasGroup>().DOFade(.5f, .25f);
				orthographic.GetComponent<CanvasGroup>().DOFade(1f, .25f);
			}
			if(selected == VIEW_STATES.PERSPECTIVE) {
				perspective.GetComponent<CanvasGroup>().DOFade(1f, .25f);
				orthographic.GetComponent<CanvasGroup>().DOFade(.5f, .25f);				
			}


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

	void killTweens() {
		orth_tween.Kill();
		persp_tween.Kill();
	}

}
