using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CustomCursor : MonoBehaviour {

	AudioManager _audioManager;

	RectTransform mainCursor, fxCursor;

	bool isPressed, isPlaying = false;
	Tween fxGrow, fxFade;


	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		mainCursor = gameObject.GetComponent<RectTransform>();
		fxCursor = transform.GetChild(0).GetComponent<RectTransform>();

		_audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () {

		mainCursor.SetPositionAndRotation(Input.mousePosition, Quaternion.Euler(Vector3.zero));

		if(Input.GetMouseButton(0)) {
			if(!isPressed) {
				isPressed = true;
				// TODO: Make system to consume mouse inputs and decide what cursor noise to make
				// FMODUnity.RuntimeManager.PlayOneShot(_audioManager.badInteract);

				if(!isPlaying) {
					isPlaying = true;
					StartCoroutine(cursorDown());
				}
			}
		}

		if(Input.GetMouseButtonUp(0)) {
			isPressed = false;
		}

		if(Input.mousePresent) {
			if(Cursor.visible == true) {
				Cursor.visible = false;
			}
		}
	}
	
	IEnumerator cursorDown() {
		mainCursor.DOSizeDelta(new Vector2(4, 4), .1f);

		yield return new WaitForSeconds(.1f);

		fxGrow = fxCursor.DOSizeDelta(new Vector2(36, 36), .4f);
		fxFade = fxCursor.gameObject.GetComponent<Image>().DOFade(0, .4f).SetEase(Ease.OutQuart);
		gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(6, 6), .4f);

		yield return new WaitForSeconds(.4f);
		

		fxCursor.DOSizeDelta(new Vector2(6, 6), 0f);
		fxCursor.gameObject.GetComponent<Image>().DOFade(.85f, 0f);	

		isPlaying = false;
	}

	IEnumerator cursorUp() {
		gameObject.GetComponent<RectTransform>().DOSizeDelta(new Vector2(6, 6), .2f);

		yield return new WaitForSeconds(.2f);

		if(fxGrow.IsPlaying()) {
			fxGrow.Kill();
		}
		if(fxFade.IsPlaying()) {
			fxFade.Kill();
		}
		
		fxCursor.DOSizeDelta(new Vector2(6, 6), 0f).From();
		fxCursor.gameObject.GetComponent<Image>().DOFade(.85f, .1f).From();		
	}
}
