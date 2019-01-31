using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DirectionalParameterControllerNameSpace {
	[Serializable]
	public class ParameterData {
		public string _fmodParameter;
		public float _orientationOffset = 0f;
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
		private enum AREA {
			UNDEFINED,
			MIN_AREA,
			FADE_LOWER,
			MAX_AREA,
			FADE_UPPER
		}
		AREA OVERFLOW = AREA.UNDEFINED; 

		public enum DISPLAY_TYPE {
			UNDEFINED = 0,
			NORMALIZED,
			DECIBEL,
			PERCENTAGE
		}
		public DISPLAY_TYPE DISPLAY = DISPLAY_TYPE.NORMALIZED;
		public float _maxNormalizedValue = 1f;
		public float _minNormalizedValue = 1f;
		public float _maxValueToDisplay;
		public float _minValueToDisplay;

		public bool SHOW_ON_UNSELECTED;
		enum GIZMO_DRAW_STATE {
			UNDEFINED,
			SELECTED_IN_FOCUS,
			SELECTED_OUT_OF_FOCUS,
			UNSELECTED_DRAW_GIZMO,
			HIDE_GIZMO
		}
		GIZMO_DRAW_STATE DRAW_STATE = GIZMO_DRAW_STATE.UNDEFINED;
		public bool DROPDOWN_OPEN;
		public bool IN_FOCUS;
		public bool SHOW_ADVANCED;

		// Constructor for initializing empty
		public ParameterData () {

		}
		
		// Constructor for copying
		public ParameterData (
			string fmodParameter,
			float offset,
			float widthToAdd,
			float fadeToAdd,
			DISPLAY_TYPE display,
			float maxNormalized,
			float minNormalized,
			float orientation,
			Transform transform
		) {
			_fmodParameter = fmodParameter;
			_orientationOffset = offset;
			_widthAngleToAdd = widthToAdd;
			_fadeAngleToAdd = fadeToAdd;
			DISPLAY = display;
			_maxNormalizedValue = maxNormalized;
			_minNormalizedValue = minNormalized;

			UpdateAngleData(orientation, transform);
		}
 

		public void DrawAngleVectorsGizmos(Vector3 from, bool SELECTED, bool GAMEOBJECT_SHOW_ON_UNSELECTED) {
			if(SELECTED) {
				if (IN_FOCUS) {
					DRAW_STATE = GIZMO_DRAW_STATE.SELECTED_IN_FOCUS;
				} else {
					DRAW_STATE = GIZMO_DRAW_STATE.SELECTED_OUT_OF_FOCUS;
				}
			} else {
				if (GAMEOBJECT_SHOW_ON_UNSELECTED) {
					if (SHOW_ON_UNSELECTED) {
						DRAW_STATE = GIZMO_DRAW_STATE.UNSELECTED_DRAW_GIZMO;
					} else {
						DRAW_STATE = GIZMO_DRAW_STATE.HIDE_GIZMO;
					}
				} else {
					DRAW_STATE = GIZMO_DRAW_STATE.HIDE_GIZMO;
				}
 			}

			Color newGizmoColor;

			switch (DRAW_STATE) {
				case GIZMO_DRAW_STATE.UNDEFINED:
					break;

				case GIZMO_DRAW_STATE.SELECTED_IN_FOCUS:
					newGizmoColor = Color.yellow;
					Gizmos.color = newGizmoColor;
					Gizmos.DrawRay(from, _widthLowerVector);
					Gizmos.DrawRay(from, _widthUpperVector);

					newGizmoColor = Color.white;
					Gizmos.color = newGizmoColor;
					Gizmos.DrawRay(from, _fadeLowerVector);
					Gizmos.DrawRay(from, _fadeUpperVector);
					break;

				case GIZMO_DRAW_STATE.SELECTED_OUT_OF_FOCUS:
					newGizmoColor = new Color(0.75f, 0.75f, 0.75f, 1);
					Gizmos.color = newGizmoColor;
					Gizmos.DrawRay(from, _widthLowerVector);
					Gizmos.DrawRay(from, _widthUpperVector);

					newGizmoColor = new Color(0.5f, 0.5f, 0.5f, 1);
					Gizmos.color = newGizmoColor;
					Gizmos.DrawRay(from, _fadeLowerVector);
					Gizmos.DrawRay(from, _fadeUpperVector);
					break;

				case GIZMO_DRAW_STATE.UNSELECTED_DRAW_GIZMO:
					newGizmoColor = new Color(1, 1, 1, 0.75f);
					Gizmos.color = Color.white * newGizmoColor;
					Gizmos.DrawRay(from, _widthLowerVector);
					Gizmos.DrawRay(from, _widthUpperVector);

					newGizmoColor = new Color(1, 1, 1, .5f);
					Gizmos.color = Color.yellow * newGizmoColor;
					Gizmos.DrawRay(from, _fadeLowerVector);
					Gizmos.DrawRay(from, _fadeUpperVector);
					break;

				case GIZMO_DRAW_STATE.HIDE_GIZMO:
					break;

				default:
					Debug.LogWarning("Draw Angle Vector switch default state!");
					break;
			}
		}



		public void UpdateAngleData(float orientationAngle, Transform transform) {
			float tempOrientationAngle = orientationAngle + _orientationOffset;

			if ((_widthAngleToAdd != 0f) && (_fadeAngleToAdd == 0f)) {
				_fadeAngleToAdd = _widthAngleToAdd;
				_widthAngleToAdd = 0f;
			}

			float tempWidthUpperAngle = Normalize0To360Scale(tempOrientationAngle + _widthAngleToAdd);
			float tempWidthLowerAngle = Normalize0To360Scale(tempOrientationAngle - _widthAngleToAdd);

			float tempSpreadUpperAngle = Normalize0To360Scale(tempWidthUpperAngle + _fadeAngleToAdd);
			float tempSpreadLowerAngle = Normalize0To360Scale(tempWidthLowerAngle - _fadeAngleToAdd);

			_widthUpperVector = new Vector3( Mathf.Sin(tempWidthUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempWidthUpperAngle * Mathf.Deg2Rad));
			_widthLowerVector = new Vector3( Mathf.Sin(tempWidthLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempWidthLowerAngle * Mathf.Deg2Rad));
			_fadeUpperVector = new Vector3( Mathf.Sin(tempSpreadUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempSpreadUpperAngle * Mathf.Deg2Rad));
			_fadeLowerVector = new Vector3( Mathf.Sin(tempSpreadLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempSpreadLowerAngle * Mathf.Deg2Rad));


			_widthUpperAngle = Vector3.SignedAngle(transform.forward, _widthUpperVector, transform.up) + 180f;
			_widthLowerAngle = Vector3.SignedAngle(transform.forward, _widthLowerVector, transform.up) + 180f;
			_fadeUpperAngle = Vector3.SignedAngle(transform.forward, _fadeUpperVector, transform.up) + 180f;
			_fadeLowerAngle = Vector3.SignedAngle(transform.forward, _fadeLowerVector, transform.up) + 180f;


			OVERFLOW = AREA.MIN_AREA;
			// Checks to see whether the width angles straddle 0 degrees. 
			// If so, we'll do some different behaviors when calculating the paramter value
			if (tempWidthLowerAngle > tempWidthUpperAngle) {
				OVERFLOW = AREA.MAX_AREA;
			} else {
				if (_fadeAngleToAdd != 0f) {
					// Checks to see whehter the fade angles straddle 0 degrees, then checks whether its the upper or lower fade.
					// Nested whithin the width check because if width straddles 0, then both fade regions will always be contiguous
					if (tempSpreadLowerAngle > tempSpreadUpperAngle) {
						if (tempSpreadLowerAngle > tempWidthLowerAngle) {
							OVERFLOW = AREA.FADE_LOWER;
						}
						if (tempWidthUpperAngle > tempSpreadUpperAngle) {
							OVERFLOW = AREA.FADE_UPPER;
						}
					}
				}
			}
			
			// Special Case: if there is no difference to calculate
			if (_maxNormalizedValue == _minNormalizedValue) {
				OVERFLOW = AREA.UNDEFINED;
			}
			// Special Case: if there is no fade and no width to calculate
			if ((_fadeAngleToAdd == 0f) && (_widthAngleToAdd == 0f)) {
				OVERFLOW = AREA.UNDEFINED;
			}
		}

		public float GetParameterValue(float cameraAngle) {
			if (OVERFLOW == AREA.UNDEFINED) {
				return _maxNormalizedValue;
			}

			// Is the camera angle within the max volume area?
			if (OVERFLOW == AREA.MAX_AREA) {
				if ((cameraAngle <= _widthLowerAngle) || (cameraAngle >= _widthUpperAngle)) {
					return _maxNormalizedValue;
				}
			} else {
				if ((cameraAngle >= _widthLowerAngle) && (cameraAngle <= _widthUpperAngle)) {
					return _maxNormalizedValue; 
				}
			}

			// Is the camera angle within the min volume area?
			if (OVERFLOW == AREA.MIN_AREA) {
				if ((cameraAngle <= _fadeLowerAngle) || (cameraAngle >= _fadeUpperAngle)) {
					return _minNormalizedValue;
				}
			} else {
				if ((cameraAngle >= _fadeLowerAngle) && (cameraAngle <= _fadeUpperAngle)) {
					return _minNormalizedValue;
				}
			}

			float virtualAngle;

			// Is the camera angle within the lower fade area?
			if (OVERFLOW == AREA.FADE_LOWER) {
				if (cameraAngle > _fadeLowerAngle) {
					// Not contiguous, overflows at 360
					virtualAngle = _fadeLowerAngle + _fadeAngleToAdd;
					return Mathf.Lerp(_minNormalizedValue, _maxNormalizedValue, Ratio(_fadeLowerAngle, virtualAngle, cameraAngle));

				}
				else if (cameraAngle < _widthLowerAngle) {
					// Not contiguous, underflows at 0
					virtualAngle = _widthLowerAngle - _fadeAngleToAdd;
					return Mathf.Lerp(_minNormalizedValue, _maxNormalizedValue, Ratio(virtualAngle, _widthLowerAngle, cameraAngle));
				}
			} else {
				if ((cameraAngle > _fadeLowerAngle) && (cameraAngle < _widthLowerAngle)) {
					// Contiguous
					return Mathf.Lerp(_minNormalizedValue, _maxNormalizedValue, Ratio(_fadeLowerAngle, _widthLowerAngle, cameraAngle));
				}
			}

			// Is the camera angle within the upper fade area?
			if (OVERFLOW == AREA.FADE_UPPER) {
				if (cameraAngle > _widthUpperAngle) {
					// Not contiguous, overflows at 360
					virtualAngle = _widthUpperAngle + _fadeAngleToAdd;
					return Mathf.Lerp(_maxNormalizedValue, _minNormalizedValue, Ratio(_widthUpperAngle, virtualAngle, cameraAngle));
				} 
				else if (cameraAngle < _fadeUpperAngle) {
					// Not contiguous, underflows at 0
					virtualAngle = _fadeUpperAngle - _fadeAngleToAdd;
					return Mathf.Lerp(_maxNormalizedValue, _minNormalizedValue, Ratio(virtualAngle, _fadeUpperAngle, cameraAngle));
				}
			} else {
				if ((cameraAngle < _fadeUpperAngle) && (cameraAngle > _widthUpperAngle)) {
					// Contiguous
					return Mathf.Lerp(_maxNormalizedValue, _minNormalizedValue, Ratio(_widthUpperAngle, _fadeUpperAngle, cameraAngle));
				}
			}

			Debug.LogWarning("Error: End of calculation section and no area has been choosen!");
			return -1f;
		}

		float Normalize0To360Scale(float i) {
			if (i >= 360f) {
				i = i % 360f;
			}
			else if (i < 0f) {
				i = i + 360f;
			}
			return i;
		}

		float Ratio(float min, float max, float value) {
			value = value - min;
			max = max - min;
			return value / max;
		}
	}
}
