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
		EditorGUILayout.PropertyField (_serializedEventData);
		_mySerializedObject.ApplyModifiedProperties();
    }

    private void DrawDirectionalDataGUI()
    {
        EditorGUILayout.LabelField("Directional Data", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");

		_myTarget._orientation = EditorGUILayout.Slider("Orientation", _myTarget._orientation, -180, 180);
		_myTarget._spread = EditorGUILayout.Slider("Spread", _myTarget._spread, 0, 90);
		_myTarget._width = EditorGUILayout.Slider("Width", _myTarget._width, 0, 90);

		EditorGUILayout.EndVertical();
    }

	private void DrawDecibalDataGUI()
    {
		EditorGUILayout.LabelField("Levels", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical("box");

		EditorGUILayout.LabelField("High dB: ", _myTarget._maxVolume.ToString());
        EditorGUILayout.LabelField("Low dB: ", _myTarget._minVolume.ToString());

		EditorGUILayout.MinMaxSlider(ref _myTarget._minVolume, ref _myTarget._maxVolume, -75, 0);

		EditorGUILayout.EndVertical();
    }

}
