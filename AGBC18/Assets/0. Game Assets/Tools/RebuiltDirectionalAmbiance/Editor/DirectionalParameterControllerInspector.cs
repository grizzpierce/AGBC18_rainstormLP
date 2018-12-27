using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DirectionalParameterController {
	[CustomEditor(typeof(DirectionalParameterController))]
	public class DirectionalParameterControllerInspector : Editor {
		
		private DirectionalParameterController _myTarget;
		private SerializedObject _mySerializedObject;
		private SerializedProperty _eventProperty;

		private bool SHOW_ADVANCED;

		private void OnEnable() {
			_myTarget = (DirectionalParameterController)target;

			_mySerializedObject = new SerializedObject(_myTarget);
			_eventProperty = _mySerializedObject.FindProperty("_fmodEvent");
		}

		public override void OnInspectorGUI() {
			_mySerializedObject.Update();
			DrawEventDataHeaderGUI();
			DrawListGUI();
			_mySerializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(_myTarget);
		}

		private void DrawEventDataHeaderGUI() {

			EditorGUILayout.LabelField("Event Data", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("box");

			EditorGUILayout.PropertyField(_eventProperty);
			EditorGUILayout.LabelField("Orientation: ", _myTarget._orientationAngle.ToString());

			EditorGUI.indentLevel++;

			SHOW_ADVANCED = EditorGUILayout.Foldout(SHOW_ADVANCED, "Advanced Settings");
			if (SHOW_ADVANCED) {
				_myTarget.SHOW_ON_UNSELECTED = EditorGUILayout.ToggleLeft("Show gizmos while not selected", _myTarget.SHOW_ON_UNSELECTED);
			}

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
		}

		private void DrawListGUI() {
			EditorGUILayout.LabelField("Parameter Data", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("box");
			EditorGUI.indentLevel++;

			foreach(ParameterData item in _myTarget._parameters) {

				bool tempBool = item.DROPDOWN_OPEN;
				
				item.DROPDOWN_OPEN = EditorGUILayout.Foldout(
					item.DROPDOWN_OPEN, 
					String.IsNullOrEmpty(item._fmodParameter) ? "Empty Parameter" : String.Format("Parameter: {0}", item._fmodParameter)
				);

				if (item.DROPDOWN_OPEN == false) {
					item.IN_FOCUS = false;
				}

				if (tempBool != item.DROPDOWN_OPEN) {
					if (item.DROPDOWN_OPEN) {
						foreach(ParameterData subItem in _myTarget._parameters) {
							subItem.IN_FOCUS = false;
						}
						item.IN_FOCUS = true;
					}
				}
				if (item.DROPDOWN_OPEN) {
					EditorGUILayout.BeginVertical("box");

					EditorGUILayout.Separator();

					item._fmodParameter = EditorGUILayout.TextField("Parameter Name", item._fmodParameter);

					EditorGUILayout.Separator();

					item._orientationOffset = EditorGUILayout.Slider("Offset Angle", item._orientationOffset, -180f, 180f);
					
					float tempInput;
					tempInput = EditorGUILayout.Slider("Width", item._widthAngleToAdd, 0f, 180f);
					if(item._fadeAngleToAdd + tempInput < 180f) {
						item._widthAngleToAdd = tempInput;
					} else {
						// Destructively encroaches on Fade
						item._widthAngleToAdd = tempInput;
						item._fadeAngleToAdd = 180f - item._widthAngleToAdd;

						// Does not encroach on Fade
						//_myTarget._widthAngleToAdd = 180f - _myTarget._fadeSpreadAngleToAdd;
					}

					tempInput = EditorGUILayout.Slider("Fade", item._fadeAngleToAdd, 0f, 180f);
					if(item._widthAngleToAdd + tempInput < 180f) {
						item._fadeAngleToAdd = tempInput;
					} else {
						item._fadeAngleToAdd = 180f - item._widthAngleToAdd;
					}

					EditorGUILayout.Separator();

					switch (item.DISPLAY) {
						case ParameterData.DISPLAY_TYPE.UNDEFINED:
							break;
						case ParameterData.DISPLAY_TYPE.NORMALIZED:
							EditorGUILayout.LabelField("Maximum Value: ", item._maxNormalizedValue.ToString());
							EditorGUILayout.LabelField("Minimum Value: ", item._minNormalizedValue.ToString());
							break;
						case ParameterData.DISPLAY_TYPE.DECIBEL:
							item._maxValueToDisplay = Mathf.Log10(item._maxNormalizedValue) * 20f;
							item._minValueToDisplay = Mathf.Log10(item._minNormalizedValue) * 20f;

							EditorGUILayout.LabelField("Maximum dB: ", item._maxValueToDisplay > -80f ? item._maxValueToDisplay.ToString() : "-∞");
							EditorGUILayout.LabelField("Minimum dB: ", item._minValueToDisplay > -80f ? item._minValueToDisplay.ToString() : "-∞");
							break;
						case ParameterData.DISPLAY_TYPE.PERCENTAGE:
							item._maxValueToDisplay = item._maxNormalizedValue * 100f;
							item._minValueToDisplay = item._minNormalizedValue * 100f;

							EditorGUILayout.LabelField("Maximum Percentage: ", String.Format("{0}%", item._maxValueToDisplay.ToString()));
							EditorGUILayout.LabelField("Minimum Percentage: ", String.Format("{0}%", item._minValueToDisplay.ToString()));
							break;
					}

					EditorGUILayout.MinMaxSlider(ref item._minNormalizedValue, ref item._maxNormalizedValue, item.DISPLAY == ParameterData.DISPLAY_TYPE.DECIBEL ? 0.0001f : 0f, 1f);

					EditorGUILayout.Separator();

					EditorGUI.indentLevel++;
					item.SHOW_ADVANCED = EditorGUILayout.Foldout(item.SHOW_ADVANCED, "Advanced Settings");
					if (item.SHOW_ADVANCED) {
						item.DISPLAY = (ParameterData.DISPLAY_TYPE) EditorGUILayout.EnumPopup("Level Display Type", item.DISPLAY);
						item.SHOW_ON_UNSELECTED = EditorGUILayout.ToggleLeft("Show Gizmos while unfocused", item.SHOW_ON_UNSELECTED);
						
						EditorGUILayout.BeginHorizontal();
						GUILayout.Space(20f);
						bool removeThis = GUILayout.Button("Delete");
						if (removeThis) {
							if (EditorUtility.DisplayDialog(
								"Deleting Parameter",
								"Are you sure you want to delete this parameter? \n This action cannot be undone!",
								"Delete",
								"Cancel"
							)) {
								_myTarget._parameters.Remove(item);
							}
						}

						bool copyThis = GUILayout.Button("Copy");
						if (copyThis) {
							_myTarget._parameters.Add(new ParameterData(
								item._fmodParameter,
								item._orientationOffset,
								item._widthAngleToAdd,
								item._fadeAngleToAdd,
								item.DISPLAY,
								item._maxNormalizedValue,
								item._minNormalizedValue,
								_myTarget._orientationAngle,
								_myTarget.transform
							));
						}
						EditorGUILayout.EndHorizontal();
					}

					
					EditorGUI.indentLevel--;

					EditorGUILayout.EndVertical();
				}
			}

			if (GUILayout.Button("+", GUILayout.Width(30))) {
				_myTarget._parameters.Add(new ParameterData());
			}

			EditorGUI.indentLevel--;
			EditorGUILayout.EndVertical();
		}
	}
}
