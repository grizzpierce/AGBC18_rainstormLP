using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCapture : MonoBehaviour {

	Text timeText;

	void Start () {
		timeText = GetComponent<Text>();
		TimeCheck();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		TimeCheck();
	}

	void TimeCheck() {

		int hour = System.DateTime.Now.Hour % 12;
		if (hour == 0) {
			hour = 12;
		}

		string checkedTime = hour.ToString("00") + ":" + System.DateTime.Now.Minute.ToString("00");

		if(checkedTime != timeText.text) {
			timeText.text = checkedTime;
		}
	}
}
