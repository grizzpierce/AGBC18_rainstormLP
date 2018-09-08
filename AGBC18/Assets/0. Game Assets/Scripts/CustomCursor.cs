using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.GetComponent<RectTransform>().SetPositionAndRotation(Input.mousePosition, Quaternion.Euler(Vector3.zero));

	}
}
