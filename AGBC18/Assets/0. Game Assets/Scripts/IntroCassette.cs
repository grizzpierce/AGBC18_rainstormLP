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
	public float masterFadeinDelay = 3.0f;
	public float masterFadeinTargetVolume = 1.0f;
	public float masterFadeinTime = 2.0f;
	public float enviroFadeinDelay = 0.0f;
	public float enviroFadeinTargetVolume = 1.0f;
	public float enviroFadeinTime = 4.0f;
	public float introRainFadeoutDelay = 1.0f;
	public float introRainTargetVolume = 1.0f;
	public float introRainFadeoutTime = 5.0f;

	float _calculatedMasterVolume = 1.0f;
	float _calculatedEnviroVolume = 1.0f;
	float _calculatedIntroRainVolume = 1.0f;


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
			StartCoroutine(CasseteLoadTimer());
			StartCoroutine(RainFadeout());
			StartCoroutine(EnviroFadein());

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
		_calculatedEnviroVolume = 0.0f;
		_calculatedMasterVolume = 0.0f;
		_calculatedIntroRainVolume = introRainTargetVolume;

		audioManager.SetMasterBusVolume(_calculatedMasterVolume);
		audioManager.SetAmbianceBusVolume(_calculatedEnviroVolume);
		audioManager.SetIntroRainVolume(_calculatedIntroRainVolume);

		float _elapsedTime = 0.0f;
		
		yield return new WaitForSeconds(masterFadeinDelay);

		while (_calculatedMasterVolume < (masterFadeinTargetVolume - 0.01f)) {
			_calculatedMasterVolume = Mathf.Lerp(audioManager.GetMasterBusVolume(), masterFadeinTargetVolume, (_elapsedTime / masterFadeinTime));
			audioManager.SetMasterBusVolume(_calculatedMasterVolume);
			_elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		if (_calculatedMasterVolume >= (masterFadeinTargetVolume - 0.01f)) {
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

	IEnumerator CasseteLoadTimer() {
		yield return new WaitForSeconds(cassetteLoadDelay);

		cameraRig.GetComponent<MapRotator>().enabled = true;
		canvas.GetComponent<UIModes>().LaunchMain();
		cassetteManager.GetComponent<CassetteManagement>().Launch();
	}

	IEnumerator RainFadeout() {
		yield return new WaitForSeconds(enviroFadeinDelay + introRainFadeoutDelay);

		float _elapsedTime = 0.0f;

		while (_calculatedIntroRainVolume > 0.01f) {
			_calculatedIntroRainVolume = Mathf.Lerp(audioManager.GetIntroRainVolume(), 0.0f, (_elapsedTime / introRainFadeoutTime));
			audioManager.SetIntroRainVolume(_calculatedIntroRainVolume);
			_elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		audioManager.DestroyIntroRain();
	}

	IEnumerator EnviroFadein() {
		yield return new WaitForSeconds(enviroFadeinDelay);

		float _elapsedTime = 0.0f;

		while (_calculatedEnviroVolume < (enviroFadeinTargetVolume - 0.01f)) {
			_calculatedEnviroVolume = Mathf.Lerp(audioManager.GetAmbianceBusVolume(), enviroFadeinTargetVolume, (_elapsedTime / enviroFadeinTime));
			audioManager.SetAmbianceBusVolume(_calculatedEnviroVolume);
			_elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		if (audioManager.GetAmbianceBusVolume() < enviroFadeinTargetVolume) {
			audioManager.SetAmbianceBusVolume(enviroFadeinTargetVolume);
		}
	}
}
