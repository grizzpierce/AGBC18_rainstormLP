using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;

public class IntroCassette : MonoBehaviour {

	public GameObject cartridge, cameraRig, canvas, manager;

	bool isLaunched = false;

	void Start() {
		if(cartridge == null)
			Destroy(this);

	}

    void OnMouseDown()
    {
        if(!isLaunched) {
			Destroy(gameObject.GetComponent<BoxCollider>());
			cartridge.transform.parent = null;

			StartCoroutine(IntroAnimation());
			isLaunched = true;
		}
		

	    // UNPARENT THE CARTRIDGE MESH, TRIGGER A COROUTINE TO SEND IT DOWN, AND THEN ONCE DELETED ROTATE THE SCREEN IN PLACE.
	    // ALSO TRY AS HARD AS POSSIBLE TO NOT USE ROTATEAROUNDAXIS, AS I HAVE NO WAY OF MANIPULATING THE OBJECT.
    }

	IEnumerator IntroAnimation() {

		cartridge.transform.DORotate(new Vector3(-90, 0, 0), 5f);
		cartridge.transform.DOMove(new Vector3(0, 27.5f, 0), 5f);

        yield return new WaitForSeconds(7f);

		cartridge.transform.DOMove(Vector3.zero, 2f).SetEase(Ease.InBack);

		yield return new WaitForSeconds(2f);

		Destroy(cartridge);
		cameraRig.transform.DORotate(new Vector3(0, 30, 0), 3f);
		cameraRig.transform.DOMove(Vector3.zero, 3f);

		yield return new WaitForSeconds(3f);

		cameraRig.GetComponent<MapRotator>().enabled = true;
		canvas.GetComponent<UIModes>().LaunchMain();
		manager.GetComponent<CassetteManagement>().Launch();
	}
}
