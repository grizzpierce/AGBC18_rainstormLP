using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using DG.Tweening;

public class IntroCassette : MonoBehaviour {

	public GameObject cartridge, cameraRig, canvas, cassetteManager, environment;
	public AudioManager audioManager;

	[FMODUnity.EventRef]
	public string cassetteInteractAudio;

	[FMODUnity.EventRef]
	public string cassetteStartAudio;


	bool isLaunched = false;

	void Awake() {
		//environment.SetActive(false);
	}

	void Start() {

		if(cartridge == null)
			Destroy(this);

	}

    void OnMouseDown()
    {
        if(!isLaunched) {
			Destroy(gameObject.GetComponent<BoxCollider>());
			cartridge.transform.parent = null;

			if(cassetteInteractAudio != "") {
				FMODUnity.RuntimeManager.PlayOneShot(cassetteInteractAudio);
			} else if (audioManager.audioBin.cartridgeRattle != "") {
				FMODUnity.RuntimeManager.PlayOneShot(audioManager.audioBin.cartridgeRattle);
			} else {
				Debug.Log("No FMOD Event data for intro cassette interact");
			}

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

		yield return new WaitForSeconds(1.5f);

		if(cassetteStartAudio != "") {
			FMODUnity.RuntimeManager.PlayOneShot(cassetteStartAudio);
		} else if (audioManager.audioBin.cartridgeLoad != "") {
			FMODUnity.RuntimeManager.PlayOneShot(audioManager.audioBin.cartridgeLoad);
		} else {
			Debug.Log("No FMOD Event data for intro cassette load");
		}

		yield return new WaitForSeconds(0.5f);

		Destroy(cartridge);				
		//environment.SetActive(true);

		cameraRig.transform.DORotate(new Vector3(0, 30, 0), 3f);
		cameraRig.transform.DOMove(new Vector3(0, -2, 0), 3f);
		
		yield return new WaitForSeconds(3f);
		
		cameraRig.GetComponent<MapRotator>().enabled = true;
		canvas.GetComponent<UIModes>().LaunchMain();
		cassetteManager.GetComponent<CassetteManagement>().Launch();

	}
}
