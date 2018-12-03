using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(DirectionalAmbiance))]
public class DirectionalAmbianceInspector : Editor {

	private DirectionalAmbiance _myTarget;

	private SerializedObject _mySerializedObject;
	private SerializedProperty _serializedEventData;

	private void OnEnable () {
		_myTarget = (DirectionalAmbiance)target;

		_mySerializedObject = new SerializedObject( _myTarget );
		_serializedEventData = _mySerializedObject.FindProperty("_fmodEvent");
	}

	private void OnDisable () {

	}

	private void OnDestroy () {

	}

	public override void OnInspectorGUI () {
		DrawEventDataGUI();
		DrawDirectionalDataGUI();
		DrawDecibalDataGUI();

		EditorUtility.SetDirty(_myTarget);
		
	}

    
	private void DrawEventDataGUI()
    {
		EditorGUILayout.LabelField("Event Data", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.PropertyField (_serializedEventData);
		_mySerializedObject.ApplyModifiedProperties();

		_myTarget._fmodParameter = EditorGUILayout.TextField("Parameter Name", _myTarget._fmodParameter);

		EditorGUILayout.EndVertical();
    }

    private void DrawDirectionalDataGUI()
    {
        EditorGUILayout.LabelField("Directional Data", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.LabelField("Orientation: ", _myTarget._orientationAngle.ToString());

		float tempInput;
		tempInput = EditorGUILayout.Slider("Width", _myTarget._widthAngleToAdd, 0f, 180f);
		if(_myTarget._fadeSpreadAngleToAdd + tempInput < 180f) {
			_myTarget._widthAngleToAdd = tempInput;
		} else {
			// Destructively encroaches on Fade
			_myTarget._widthAngleToAdd = tempInput;
			_myTarget._fadeSpreadAngleToAdd = 180f - _myTarget._widthAngleToAdd;

			// Does not encroach on Fade
			//_myTarget._widthAngleToAdd = 180f - _myTarget._fadeSpreadAngleToAdd;
		}

		tempInput = EditorGUILayout.Slider("Fade", _myTarget._fadeSpreadAngleToAdd, 0f, 180f);
		if(_myTarget._widthAngleToAdd + tempInput < 180f) {
			_myTarget._fadeSpreadAngleToAdd = tempInput;
		} else {
			_myTarget._fadeSpreadAngleToAdd = 180f - _myTarget._widthAngleToAdd;
		}

		EditorGUILayout.EndVertical();
    }

	private void DrawDecibalDataGUI()
    {
		EditorGUILayout.LabelField("Levels", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.LabelField("High dB: ", _myTarget._maxDB > -80f ? _myTarget._maxDB.ToString() : "-∞");
        EditorGUILayout.LabelField("Low dB: ", _myTarget._minDB > -80f ? _myTarget._minDB.ToString() : "-∞");

		EditorGUILayout.MinMaxSlider(ref _myTarget._minVolumeForSlider, ref _myTarget._maxVolumeForSlider, 0.0001f, 1f);
		_myTarget._minDB = Mathf.Log10(_myTarget._minVolumeForSlider) * 20f;
		_myTarget._maxDB = Mathf.Log10(_myTarget._maxVolumeForSlider) * 20f;

		EditorGUILayout.EndVertical();
    }

}
