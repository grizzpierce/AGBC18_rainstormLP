using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace DirectionalAmbiance {
	[Serializable]
	public class ParameterData {

		public string _fmodParameter;
		public float _widthAngleToAdd = 0f;
		public float _fadeAngleToAdd = 0f;

		public float _widthUpperAngle;
		public float _widthLowerAngle;
		public float _fadeUpperAngle;
		public float _fadeLowerAngle;

		public Vector3 _widthUpperVector;
		public Vector3 _widthLowerVector;
		public Vector3 _fadeUpperVector;
		public Vector3 _fadeLowerVector;

		public enum DISPLAY_TYPE {
			UNKNOWN,
			ZERO_TO_ONE,
			DECIBEL,
			ZERO_TO_ONE_HUNDRED
		}
		public float _maxValueForSlider = 1f;
		public float _minValueForSlider = 1f;
		public DISPLAY_TYPE DISPLAY = DISPLAY_TYPE.UNKNOWN;
		public float _maxValueToDisplay = 0f;
		public float _minValueToDisplay = 0f;
	}
}
