using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAmbiance : MonoBehaviour {

	[FMODUnity.EventRef]
	public string _fmodEvent;
	
	public float _orientationAngle = 0f;		// From the transform, not editable in inspector 
	public float _widthAngleToAdd = 0f;			// Degrees +/- orientation in which the level is max
	public float _fadeSpreadAngleToAdd = 0f;	// Degrees +/- the extent of width in which the level lerps from 1 to 0

	float _widthUpperAngle;
	float _widthLowerAngle;
	float _spreadUpperAngle;
	float _spreadLowerAngle;
	float _cameraAngleLocalSpace;
	float _cameraAngleWorldSpace;

	Vector3 _from = Vector3.zero;
	Vector3 _orientationVector = Vector3.forward;
	Vector3 _widthUpperVector;
	Vector3 _widthLowerVector;
	Vector3 _spreadUpperVector;
	Vector3 _spreadLowerVector;
	Vector3 _cameraVector;
	

	// Values normalized 0-1, to be sent to FMOD parameter and also control the slider
	public float _maxVolumeForSlider = 1;
	public float _minVolumeForSlider = 1;
	// Values converted to dB
	public float _maxDB = 0;
	public float _minDB = 0;

	FMOD.Studio.EventInstance _thisEvent;
	Camera _mainCamera;


	void Awake () {
		_mainCamera = Camera.main;
	}

	void Start () {
		_thisEvent = FMODUnity.RuntimeManager.CreateInstance(_fmodEvent);
		_thisEvent.start();
	}
	
	void Update () {
		UpdateAngleData();
	}

	/*
	 *  Get the angle of orientation - already have that as _orientation, vector as _direction
	 * 	Add and subtract _width from _orientation angle to get _widthUpperAngle and _widthLowerAngle
	 *	Add _fadeSpread to _widthUpperAngle to get _spreadUpperAngle, subtract _fadeSpread from _widthLowerAngle to get _spreadLowerAngle
	 *	Check for overflow, % 360
	 *	Perform CreateNormalizedVector3 to _widthUpperAngle, _widthLowerAngle, _spreadUpperAngle, _spreadLowerAngle
	 *	DrawGizmos 
	 *	Get Vector3 to camera, discard y component, normalize
	 *	Calculate the normalized value for that vector in regard to orientation, width, and spread
	 *	Set parameter for fmodEvent
	 */

	void UpdateAngleData() {
		_orientationAngle = transform.eulerAngles.y % 360f;

		_widthUpperAngle = (_orientationAngle + _widthAngleToAdd) % 360f;
		_widthLowerAngle = (_orientationAngle - _widthAngleToAdd) % 360f;

		_spreadUpperAngle = (_widthUpperAngle + _fadeSpreadAngleToAdd) % 360f;
		_spreadLowerAngle = (_widthLowerAngle - _fadeSpreadAngleToAdd) % 360f;

		_widthUpperVector = new Vector3( Mathf.Sin(_widthUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_widthUpperAngle * Mathf.Deg2Rad));
		_widthLowerVector = new Vector3( Mathf.Sin(_widthLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_widthLowerAngle * Mathf.Deg2Rad));
		_spreadUpperVector = new Vector3( Mathf.Sin(_spreadUpperAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_spreadUpperAngle * Mathf.Deg2Rad));
		_spreadLowerVector = new Vector3( Mathf.Sin(_spreadLowerAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_spreadLowerAngle * Mathf.Deg2Rad));

		// Necessary for Editor Mode because Awake doesn't get called
		if(_mainCamera == null) _mainCamera = Camera.main;

		Vector3 _tempVector;
		_tempVector = _mainCamera.transform.position - gameObject.transform.position;
		_cameraVector = new Vector3(_tempVector.x, 0f, _tempVector.z);
		_cameraVector.Normalize();

		// Now that we have a nice vector, we have to rebuild the entire thing with the local rotation subtracted
		//_cameraAngleLocalSpace = Mathf.Asin(_cameraVector.x) * Mathf.Rad2Deg;
		//_cameraAngleWorldSpace = (_cameraAngleLocalSpace - _orientationAngle) % 360f;
		//_cameraVector = new Vector3( Mathf.Sin(_cameraAngleWorldSpace * Mathf.Deg2Rad), 0f, Mathf.Cos(_cameraAngleWorldSpace * Mathf.Deg2Rad));
	}

	void OnDrawGizmos () {
		UpdateAngleData();

		Color oldColor = Gizmos.color;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		DrawOrientationVectorGizmo();

		Gizmos.matrix = oldMatrix;

		Gizmos.color = oldColor;
	}

	void OnDrawGizmosSelected () {
		Color oldColor = Gizmos.color;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		DrawAngleVectorsGizmos();
		DrawOrientationVectorGizmo();

		Gizmos.matrix = oldMatrix;

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
}
