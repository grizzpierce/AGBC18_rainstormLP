using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;
using DG.Tweening;

public class IntroCassette : MonoBehaviour {

	public GameObject cartridge, cameraRig, mainCamera, canvas, cassetteManager, environment, title;
	public AudioManager audioManager;

	public float cassetteLoadDelay = 1.0f;
	public float masterFadeinTargetVolume = 1.0f;
	public float masterFadeinTime = 2.0f;
	public float enviroCrossfadeDelay = 0.0f;
	public float enviroFadeinTargetVolume = 1.0f;
	public float enviroCrossfadeTime = 4.0f;
	public float introRainTargetVolume = 1.0f;


	bool isLaunched = false;

	void Awake() {
		//environment.SetActive(false);
	}

	void Start() {
		if(cartridge == null)
			Destroy(this);

		StartCoroutine(TitleSplash());
		StartCoroutine(TitleVolume());

	}

    void OnMouseDown()
    {
        if(!isLaunched) {
			Destroy(gameObject.GetComponent<BoxCollider>());
			cartridge.transform.parent = null;

			
			FMODUnity.RuntimeManager.PlayOneShot(audioManager.dudInteract);

			StartCoroutine(IntroAnimation());
			StartCoroutine(IntroAudioCues());
			StartCoroutine(IntroBusLerp());
			isLaunched = true;
		}
		

	    // UNPARENT THE CARTRIDGE MESH, TRIGGER A COROUTINE TO SEND IT DOWN, AND THEN ONCE DELETED ROTATE THE SCREEN IN PLACE.
	    // ALSO TRY AS HARD AS POSSIBLE TO NOT USE ROTATEAROUNDAXIS, AS I HAVE NO WAY OF MANIPULATING THE OBJECT.
    }

	IEnumerator TitleSplash() {

		title.GetComponent<Image>().DOFade(.9f, 4f).SetDelay(.25f);
		title.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 0), 4f).SetEase(Ease.OutBack).SetDelay(.25f);

		yield return new WaitForSeconds(4.5f);

		//title.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 520), 4f);
		title.GetComponent<Image>().DOFade(0f, 2f);
		mainCamera.transform.DOLocalRotate(new Vector3(30, 0, 0), 5f).SetEase(Ease.InBack);
	}
	
	IEnumerator TitleVolume() {
		audioManager.SetMasterBusVolume(0.0f);
		audioManager.SetAmbianceBusVolume(0.0f);
		audioManager.SetIntroRainVolume(introRainTargetVolume);
		
		yield return new WaitForSeconds(4.5f);

		while (audioManager.GetMasterBusVolume() < (masterFadeinTargetVolume - 0.01f)) {
			audioManager.SetMasterBusVolume(Mathf.Lerp(audioManager.GetMasterBusVolume(), masterFadeinTargetVolume, masterFadeinTime * Time.deltaTime));
		}
		if (audioManager.GetMasterBusVolume() >= (masterFadeinTargetVolume - 0.01f)) {
			audioManager.SetMasterBusVolume(masterFadeinTargetVolume);
		}
	}

	IEnumerator IntroAnimation() {

		//title.GetComponent<Image>().DOFade(0, 1.5f);
		//title.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 290), 1.5f);		

		cartridge.transform.DORotate(new Vector3(-90, 0, 0), 5f);
		cartridge.transform.DOMove(new Vector3(0, 27.5f, 0), 5f);

        yield return new WaitForSeconds(5f);
		Destroy(title);

		cartridge.transform.DOMove(Vector3.zero, 2f).SetEase(Ease.InBack);

		yield return new WaitForSeconds(2.0f);

		Destroy(cartridge);	
		//environment.SetActive(true);

		cameraRig.transform.DORotate(new Vector3(0, 30, 0), 3f);
		cameraRig.transform.DOMove(new Vector3(0, -2, 0), 3f);
		
		yield return new WaitForSeconds(3f);
		

	}

	IEnumerator IntroAudioCues() {
		yield return new WaitForSeconds(cassetteLoadDelay);

		cameraRig.GetComponent<MapRotator>().enabled = true;
		canvas.GetComponent<UIModes>().LaunchMain();
		cassetteManager.GetComponent<CassetteManagement>().Launch();
	}

	IEnumerator IntroBusLerp() {
		yield return new WaitForSeconds(enviroCrossfadeDelay);

		while ((audioManager.GetAmbianceBusVolume() < (enviroFadeinTargetVolume - 0.01f)) && (audioManager.GetIntroRainVolume() > 0.01f)) {
			audioManager.SetAmbianceBusVolume(Mathf.Lerp(audioManager.GetAmbianceBusVolume(), enviroFadeinTargetVolume, enviroCrossfadeTime * Time.deltaTime));
			audioManager.SetIntroRainVolume(Mathf.Lerp(audioManager.GetIntroRainVolume(), 0.0f, enviroCrossfadeTime * Time.deltaTime));
		}
		if (audioManager.GetAmbianceBusVolume() >= (enviroFadeinTargetVolume - 0.01f)) {
			audioManager.SetAmbianceBusVolume(enviroFadeinTargetVolume);
		}
		if (audioManager.GetIntroRainVolume() <= 0.01f) {
			audioManager.DestroyIntroRain();
		}
	}
}
