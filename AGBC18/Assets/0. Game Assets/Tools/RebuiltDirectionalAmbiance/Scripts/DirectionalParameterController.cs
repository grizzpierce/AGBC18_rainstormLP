using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DirectionalParameterControllerNameSpace {
	public class DirectionalParameterController : MonoBehaviour {

		[FMODUnity.EventRef]
		public string _fmodEvent;

		public float _orientationAngle = 0f;
		float _cameraAngle;

		Vector3 _from = Vector3.zero;
		Vector3 _orientationVector = Vector3.forward;
		Vector3 _cameraVector;

		
		public bool SHOW_ON_UNSELECTED;


		public List<ParameterData> _parameters;

		FMOD.Studio.EventInstance _thisEvent;
		Camera _mainCamera;


		void OnEnable () {
			if (_parameters == null) {
				_parameters = new List<ParameterData>();
			}
		}

		void Awake () {
			_mainCamera = Camera.main;

			if (_parameters == null) {
				_parameters = new List<ParameterData>();
			}
		}

		// Use this for initialization
		void Start () {
			UpdateAngleData();

			_thisEvent = FMODUnity.RuntimeManager.CreateInstance(_fmodEvent);
			_thisEvent.start();
		}
		
		// Update is called once per frame
		void Update () {
			UpdateCameraData();

			foreach(ParameterData parameterDataInstance in _parameters) {
				_thisEvent.setParameterValue(parameterDataInstance._fmodParameter, parameterDataInstance.GetParameterValue(_cameraAngle));
			}
		}

/* 
		void OnDrawGizmos() {
			UpdateAngleData();
			UpdateCameraData();
			if (SHOW_ON_UNSELECTED) {

				Color oldColor = Gizmos.color;

				foreach(ParameterData parameterDataInstance in _parameters) {
					parameterDataInstance.DrawAngleVectorsGizmos(_from, Selection.Contains(this.gameObject), SHOW_ON_UNSELECTED);
				}

				DrawOrientationVectorGizmo(Selection.Contains(this.gameObject));

				Gizmos.color = oldColor;
			}
		}

		void OnDrawGizmosSelected() {
			Color oldColor = Gizmos.color;

			foreach(ParameterData parameterDataInstance in _parameters) {
				parameterDataInstance.DrawAngleVectorsGizmos(_from, Selection.Contains(this.gameObject), SHOW_ON_UNSELECTED);
			}
			DrawCameraVectorGizmo();

			Gizmos.color = oldColor;
		}

		private void DrawOrientationVectorGizmo(bool SELECTED) {
			if (SELECTED) {
				Gizmos.color = Color.red;
			} else if (SHOW_ON_UNSELECTED) {
				Gizmos.color = new Color (1f, 0f, 0f, 0.5f);
			} else {
				Gizmos.color = Color.clear;
			}
			Gizmos.DrawRay(_from, _orientationVector);
		}

		private void DrawCameraVectorGizmo() {
			Gizmos.color = Color.cyan;
			Gizmos.DrawRay(transform.position, _cameraVector);
		}

*/
		void UpdateAngleData() {
			_orientationAngle = transform.eulerAngles.y % 360f;

			_orientationVector = new Vector3( Mathf.Sin(_orientationAngle * Mathf.Deg2Rad), 0f, Mathf.Cos(_orientationAngle * Mathf.Deg2Rad));
			_from = transform.position;

			foreach(ParameterData parameterDataInstance in _parameters) {
				parameterDataInstance.UpdateAngleData(_orientationAngle, transform);
			}
		}

		void UpdateCameraData() {
			if(_mainCamera == null) _mainCamera = Camera.main;

			Vector3 _tempVector;
			_tempVector = _mainCamera.transform.position - gameObject.transform.position;
			_cameraVector = new Vector3(_tempVector.x, 0f, _tempVector.z);
			_cameraVector.Normalize();

			// Calculates camera angle and represents it in 0 - 360 deg
			_cameraAngle = Vector3.SignedAngle(transform.forward, _cameraVector, transform.up) + 180f;
		}

		public FMOD.Studio.EventInstance GetThisEventInstance() {
			return _thisEvent;
		}

		public void SetEventPaused(bool _pausedValue) {
			if (_pausedValue) {
				_thisEvent.setPaused(true);
			} else {
				_thisEvent.setPaused(false);
			}
		}
	}
}
