using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalAmbiance : MonoBehaviour {

	[FMODUnity.EventRef]
	public string _fmodEvent;
	
	[Range(-180,180)]
	public float _orientation = 0;
	[Range(0,90)]
	public float _spread = 0;
	[Range(0,90)]
	public float _width = 0;

	[Range(-75,0)]
	public float _maxVolume = 0;
	[Range(-75,0)]
	public float _minVolume = 0;

	FMOD.Studio.EventInstance _thisEvent;

	Camera _mainCamera;

	void Awake () {
		_mainCamera = gameObject.GetComponentInParent<Camera>();
	}

	void Start () {
		_thisEvent = FMODUnity.RuntimeManager.CreateInstance(_fmodEvent);
		_thisEvent.start();
	}
	
	void Update () {
		
	}

	Vector3 from = Vector3.zero;
	Vector3 direction = Vector3.forward;
	void OnDrawGizmos () {
		Color oldColor = Gizmos.color;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		OrientationVectorGizmo();

		Gizmos.matrix = oldMatrix;
		Gizmos.color = oldColor;
	}

	void OnDrawGizmosSelected () {
		Color oldColor = Gizmos.color;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		OrientationVectorGizmo();

		Gizmos.matrix = oldMatrix;
		Gizmos.color = oldColor;
	}

	private void OrientationVectorGizmo() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(from, direction);
	}
}
