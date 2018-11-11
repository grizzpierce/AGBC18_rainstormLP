using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToggleAnimation : MonoBehaviour {

	enum TOGGLE_STATES {
		INVALID = -1,
		PERSPECTIVE,
		ORTHOGRAPHIC,
		TWEENING
	}

	public Camera renderCamera;

	// ALL LOCAL POSITIONS/ROTATIONS
	Vector3 perspCamPos = new Vector3(2.5f, 2.2f, 2);
	Vector3 perspCamRot = new Vector3(30, 0, 0);

	Vector3 orthCamPos = new Vector3(2.5f, -0.89f, 2);
	Vector3 orthCamRot = Vector3.zero;

	TOGGLE_STATES current = TOGGLE_STATES.INVALID;

	void Start () {
		current = TOGGLE_STATES.PERSPECTIVE;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)) {
			if(current == TOGGLE_STATES.PERSPECTIVE) {
				StartCoroutine(camAnim(orthCamRot, orthCamPos, TOGGLE_STATES.ORTHOGRAPHIC));
			}
			if(current == TOGGLE_STATES.ORTHOGRAPHIC) {
				StartCoroutine(camAnim(perspCamRot, perspCamPos, TOGGLE_STATES.PERSPECTIVE));
			}
		}
	}

	IEnumerator camAnim(Vector3 _rot, Vector3 _pos, TOGGLE_STATES _state) {

		current = TOGGLE_STATES.TWEENING;

		renderCamera.transform.DOLocalRotate(_rot, 1f);
		renderCamera.transform.DOLocalMove(_pos, 1f);
        yield return new WaitForSeconds(1f);

		current = _state;
	}
}
