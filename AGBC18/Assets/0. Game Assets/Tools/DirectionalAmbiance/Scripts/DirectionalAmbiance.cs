using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAmbiance : MonoBehaviour {

	private enum AREA {
		UNDEFINED,
		MIN_AREA,
		FADE_LOWER,
		MAX_AREA,
		FADE_UPPER
	}

	[FMODUnity.EventRef]
	public string _fmodEvent;

	public string _fmodParameter = "Volume0to1";
	
	public float _orientationAngle = 0f;		// From the transform, not editable in inspector 
	public float _widthAngleToAdd = 0f;			// Degrees +/- orientation in which the level is max
	public float _fadeSpreadAngleToAdd = 0f;	// Degrees +/- the extent of width in which the level lerps from 1 to 0

	float _widthUpperAngle;
	float _widthLowerAngle;
	float _spreadUpperAngle;
	float _spreadLowerAngle;
	float _cameraAngle;

	Vector3 _from = Vector3.zero;
	Vector3 _orientationVector = Vector3.forward;
	Vector3 _widthUpperVector;
	Vector3 _widthLowerVector;
	Vector3 _spreadUpperVector;
	Vector3 _spreadLowerVector;
	Vector3 _cameraVector;


	AREA OVERFLOW = AREA.UNDEFINED; 
	

	// Values normalized 0-1, to be sent to FMOD parameter and also control the slider
	public float _maxVolumeForSlider = 1;
	public float _minVolumeForSlider = 1;
	// Values converted to dB
	public float _maxDB = 0;
	public float _minDB = 0;

	FMOD.Studio.EventInstance _thisEvent;
	Camera _mainCamera;

	float _debugValueToParameter = 0f;
	// float _debugCameraAngle = -1f;


	void Awake () {
		_mainCamera = Camera.main;
	}

	void Start () {
		UpdateAngleData();

		_thisEvent = FMODUnity.RuntimeManager.CreateInstance(_fmodEvent);
		_thisEvent.start();
	}
	
	void Update () {
		UpdateCameraAngleData();

		_debugValueToParameter = CalculateVolumeParameter();
		_thisEvent.setParameterValue(_fmodParameter, _debugValueToParameter);
	}



	void UpdateAngleData() {
		_orientationAngle = transform.eulerAngles.y % 360f;

		// If there is no spread angle but there is a width angle, then use the width as a spread
		if ((_widthAngleToAdd != 0f) && (_fadeSpreadAngleToAdd == 0f)) {
			_fadeSpreadAngleToAdd = _widthAngleToAdd;
			_widthAngleToAdd = 0f;
		}

		float tempWidthUpperAngle = Normalize0To360Scale(_orientationAngle + _widthAngleToAdd);
		float tempWidthLowerAngle = Normalize0To360Scale(_orientationAngle - _widthAngleToAdd);

		float tempSpreadUpperAngle = Normalize0To360Scale(tempWidthUpperAngle + _fadeSpreadAngleToAdd);
		float tempSpreadLowerAngle = Normalize0To360Scale(tempWidthLowerAngle - _fadeSpreadAngleToAdd);

		_widthUpperVector = new Vector3( Mathf.Sin(tempWidthUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempWidthUpperAngle * Mathf.Deg2Rad));
		_widthLowerVector = new Vector3( Mathf.Sin(tempWidthLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempWidthLowerAngle * Mathf.Deg2Rad));
		_spreadUpperVector = new Vector3( Mathf.Sin(tempSpreadUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempSpreadUpperAngle * Mathf.Deg2Rad));
		_spreadLowerVector = new Vector3( Mathf.Sin(tempSpreadLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(tempSpreadLowerAngle * Mathf.Deg2Rad));

		_orientationVector = new Vector3( Mathf.Sin(_orientationAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_orientationAngle * Mathf.Deg2Rad));
		_from = transform.position;

		// I can't figure out how to relate my previously calculated angles to angles generated from vectors so screw it they're all from vectors
		_widthUpperAngle = Vector3.SignedAngle(transform.forward, _widthUpperVector, transform.up) + 180f;
		_widthLowerAngle = Vector3.SignedAngle(transform.forward, _widthLowerVector, transform.up) + 180f;
		_spreadUpperAngle = Vector3.SignedAngle(transform.forward, _spreadUpperVector, transform.up) + 180f;
		_spreadLowerAngle = Vector3.SignedAngle(transform.forward, _spreadLowerVector, transform.up) + 180f;

		// UPDATE OVERFLOW DATA
		OVERFLOW = AREA.MIN_AREA;

		// Checks to see whether the width angles straddle 0 degrees. 
		// If so, we'll do some different behaviors when calculating the paramter value
		if (tempWidthLowerAngle > tempWidthUpperAngle) {
			OVERFLOW = AREA.MAX_AREA;
		} else {
			if (_fadeSpreadAngleToAdd != 0f) {
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
		if (_maxVolumeForSlider == _minVolumeForSlider) {
			OVERFLOW = AREA.UNDEFINED;
		}
		// Special Case: if there is no fade and no width to calculate
		if ((_fadeSpreadAngleToAdd == 0f) && (_widthAngleToAdd == 0f)) {
			OVERFLOW = AREA.UNDEFINED;
		}
	}

	void UpdateCameraAngleData() {
		// Necessary for Editor Mode because Awake doesn't get called
		if(_mainCamera == null) _mainCamera = Camera.main;

		Vector3 _tempVector;
		_tempVector = _mainCamera.transform.position - gameObject.transform.position;
		_cameraVector = new Vector3(_tempVector.x, 0f, _tempVector.z);
		_cameraVector.Normalize();

		// Calculates camera angle and represents it in 0 - 360 deg
		_cameraAngle = Vector3.SignedAngle(transform.forward, _cameraVector, transform.up) + 180f;
	}

	float CalculateVolumeParameter() {		
		// If for some reason this hasn't been done yet, do it right now!
		if (_fadeSpreadAngleToAdd == 0f) {
			_fadeSpreadAngleToAdd = _widthAngleToAdd;
			_widthAngleToAdd = 0f;
			UpdateAngleData();
		}

		if (OVERFLOW == AREA.UNDEFINED) {
			Debug.Log("Overflow Undefined");
			return _maxVolumeForSlider;
		}

		// Is the camera angle within the max volume area?
		if (OVERFLOW == AREA.MAX_AREA) {
			Debug.Log("Checking Max Area (Overflow)...");
			if ((_cameraAngle <= _widthLowerAngle) || (_cameraAngle >= _widthUpperAngle)) {
				Debug.Log("Camera in Max Area");
				return _maxVolumeForSlider;
			}
		} else {
			Debug.Log("Checking Max Area...");
			if ((_cameraAngle >= _widthLowerAngle) && (_cameraAngle <= _widthUpperAngle)) {
				Debug.Log("Camera in Max Area");
				return _maxVolumeForSlider; 
			}
		}

		// Is the camera angle within the min volume area?
		if (OVERFLOW == AREA.MIN_AREA) {
			Debug.Log("Checking Min Area (Overflow)...");
			if ((_cameraAngle <= _spreadLowerAngle) || (_cameraAngle >= _spreadUpperAngle)) {
				Debug.Log("Camera in Min Area");
				return _minVolumeForSlider;
			}
		} else {
			Debug.Log("Checking Min Area...");
			if ((_cameraAngle >= _spreadLowerAngle) && (_cameraAngle <= _spreadUpperAngle)) {
				Debug.Log("Camera in Min Area");
				return _minVolumeForSlider;
			}
		}

		float virtualAngle;

		// Is the camera angle within the lower fade area?
		if (OVERFLOW == AREA.FADE_LOWER) {
			Debug.Log("Checking Lower Fade Area (Overflow)...");
			if (_cameraAngle > _spreadLowerAngle) {
				// Not contiguous, overflows at 360
				Debug.Log("Camera in Lower Fade Area, upper section");
				virtualAngle = _spreadLowerAngle + _fadeSpreadAngleToAdd;
				return Mathf.Lerp(_minVolumeForSlider, _maxVolumeForSlider, Ratio(_spreadLowerAngle, virtualAngle, _cameraAngle));

			}
			else if (_cameraAngle < _widthLowerAngle) {
				// Not contiguous, underflows at 0
				Debug.Log("Camera in Lower Fade Area, lower section");
				virtualAngle = _widthLowerAngle - _fadeSpreadAngleToAdd;
				return Mathf.Lerp(_minVolumeForSlider, _maxVolumeForSlider, Ratio(virtualAngle, _widthLowerAngle, _cameraAngle));
			}
		} else {
			Debug.Log("Checking Lower Fade Area...");
			if ((_cameraAngle > _spreadLowerAngle) && (_cameraAngle < _widthLowerAngle)) {
				// Contiguous
				Debug.Log("Camera in Lower Fade Area");
				return Mathf.Lerp(_minVolumeForSlider, _maxVolumeForSlider, Ratio(_spreadLowerAngle, _widthLowerAngle, _cameraAngle));
			}
		}

		// Is the camera angle within the upper fade area?
		if (OVERFLOW == AREA.FADE_UPPER) {
			Debug.Log("Checking Upper Fade Area (Overflow)...");
			if (_cameraAngle > _widthUpperAngle) {
				// Not contiguous, overflows at 360
				Debug.Log("Camera in Upper Fade Area, upper section");
				virtualAngle = _widthUpperAngle + _fadeSpreadAngleToAdd;
				return Mathf.Lerp(_maxVolumeForSlider, _minVolumeForSlider, Ratio(_widthUpperAngle, virtualAngle, _cameraAngle));
			} 
			else if (_cameraAngle < _spreadUpperAngle) {
				// Not contiguous, underflows at 0
				Debug.Log("Camera in Upper Fade Area, lower section");
				virtualAngle = _spreadUpperAngle - _fadeSpreadAngleToAdd;
				return Mathf.Lerp(_maxVolumeForSlider, _minVolumeForSlider, Ratio(virtualAngle, _spreadUpperAngle, _cameraAngle));
			}
		} else {
			Debug.Log("Checking Upper Fade Area...");
			if ((_cameraAngle < _spreadUpperAngle) && (_cameraAngle > _widthUpperAngle)) {
				// Contiguous
				Debug.Log("Camera in Upper Fade Area");
				return Mathf.Lerp(_maxVolumeForSlider, _minVolumeForSlider, Ratio(_widthUpperAngle, _spreadUpperAngle, _cameraAngle));
			}
		}

		Debug.LogWarning("Error: End of calculation section and no area has been choosen!");
		return -1f;
	}






	void OnDrawGizmos () {
		UpdateAngleData();
		UpdateCameraAngleData();

		Color oldColor = Gizmos.color;
		//Matrix4x4 oldMatrix = Gizmos.matrix;
		//Gizmos.matrix = transform.localToWorldMatrix;

		DrawOrientationVectorGizmo();

		//Gizmos.matrix = oldMatrix;

		Gizmos.color = oldColor;
	}

	void OnDrawGizmosSelected () {
		Color oldColor = Gizmos.color;
		//Matrix4x4 oldMatrix = Gizmos.matrix;
		//Gizmos.matrix = transform.localToWorldMatrix;

		DrawAngleVectorsGizmos();
		DrawOrientationVectorGizmo();

		//Gizmos.matrix = oldMatrix;

		DrawCameraVectorGizmo();

		Gizmos.color = oldColor;
	}

	private void DrawOrientationVectorGizmo() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(_from, _orientationVector);
	}

	private void DrawAngleVectorsGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawRay(_from, _widthLowerVector);
		Gizmos.DrawRay(_from, _widthUpperVector);

		Gizmos.color = Color.white;
		Gizmos.DrawRay(_from, _spreadLowerVector);
		Gizmos.DrawRay(_from, _spreadUpperVector);
	}

	private void DrawCameraVectorGizmo() {
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position, _cameraVector);
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