using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
THOUGHT PROCESS 2019-01-23 7:45pm Jakey

so im chasing the error in the directional parameter controller, which is right now most pressingly that grass, road, and lamp ambiances
aren't loading anymore, while vendingMachine is, for some reason. Thinking this is to do with loading/call order, but that doesn't make
a whole lot of sense and i've tried to fix it. Right now, the events are only called from SetEnvironmentAmbianceSetting. This is flipped
from what was happening when I started, which is all 3 of those were playing but vendingMachine wasn't. The previous problem is that there
is an error in the ParameterData.GetParameterData call, which causes some (in this case, the lamp, but previously also the drain) to throw
a calculation error on certain sections of their orbit.

this came up during work in the interactable script, which needs functionality for pre and post dialog audio and delay time.
 */
 
public class AudioManager : MonoBehaviour {

    public enum MUSIC_STATE {
        INTRO,
        PLAYING,
        IDLE
    }

    // public enum ENVIRO_SETTING {
    //     LIGHTWEIGHT,
    //     ROBUST,
    //     UNDEFINED
    // }


    [Space]
    [Header("Banks")]
    [FMODUnity.BankRef]
    public string musicBank;
    [FMODUnity.BankRef]
    public string mainBank;
    // [FMODUnity.BankRef]
    // public string robustAmbiances;
    // [FMODUnity.BankRef]
    // public string lightweightAmbiances;


    // [Space]
    // [Header("VCAs")]
    // public string masterVolume;
    // public string musicVolume;
    // public string uiVolume;
    // public string enviroVolume;

    [Space]
    [Header("Busses")]
    public string masterVolumeBusRef = "bus:/SFX_Master";
    public string ambientVolumeBusRef = "bus:/SFX_Master/SFX_Environment/SFX_Enviro_Ambiance";


    [Space]
    [Header("Common Audio Events")]
    [FMODUnity.EventRef]
    public string cartridgeLoad;
    [FMODUnity.EventRef]
    public string cartridgePlay;
    [FMODUnity.EventRef]
    public string cartridgeStop;
    [FMODUnity.EventRef]
    public string cartridgeFinishPlaying;
    [FMODUnity.EventRef]
    public string cartridgeRattle;
    [FMODUnity.EventRef]
    public string findTapeInteract;
    [FMODUnity.EventRef]
    public string dudInteract;
    [FMODUnity.EventRef]
    public string badInteract;
    [FMODUnity.EventRef]
    public string textScroll;
    [FMODUnity.EventRef]
    public string rainAmbiance;


    Camera _cameraRig;
    Transform _cameraRigTransform;
    FMOD.Studio.EventInstance _rainAmbianceInstance;

    MUSIC_STATE _currentState = MUSIC_STATE.INTRO;
    // ENVIRO_SETTING _environmentAudioSetting = ENVIRO_SETTING.UNDEFINED;

    // GameObject[] _directionalAmbianceObjects;

    // FMOD.Studio.VCA _masterVolumeVCA;
    // FMOD.Studio.VCA _musicVolumeVCA;
    // FMOD.Studio.VCA _uiVolumeVCA;
    // FMOD.Studio.VCA _enviroVolumeVCA;

    FMOD.Studio.Bus _masterVolumeBus;
    FMOD.Studio.Bus _ambientVolumeBus;

    // float _currentMasterVolumePercent = -1.0f;
    // float _currentMusicVolumePercent = -1.0f;
    // float _currentUIVolumePercent = -1.0f;
    // float _currentEnviroVolumePercent = -1.0f;


    // constants for volume slider implementation
    static float dynamic_range = 40.0f;
    static float power = Mathf.Pow(10.0f, dynamic_range / 20.0f);
    static float a = 1.0f / power;
    static float b = Mathf.Log(power);

    

    void Awake () {
        if (_cameraRig == null) {
            _cameraRig = Camera.main;
        }

        // if (_environmentAudioSetting ==  ENVIRO_SETTING.UNDEFINED) {
        //     _environmentAudioSetting = ENVIRO_SETTING.ROBUST;
        // }

        if (mainBank != "") {
            FMODUnity.RuntimeManager.LoadBank(mainBank, true);
        } else {
            Debug.LogWarning("Main bank not defined!");
        }
        if (musicBank != "") {
            FMODUnity.RuntimeManager.LoadBank(musicBank, false);
        } else {
            Debug.LogWarning("Music bank not defined!");
        }

        // if (_environmentAudioSetting == ENVIRO_SETTING.LIGHTWEIGHT) {
        //     FMODUnity.RuntimeManager.LoadBank(lightweightAmbiances, true);
        // } else if (_environmentAudioSetting == ENVIRO_SETTING.ROBUST) {
        //     FMODUnity.RuntimeManager.LoadBank(robustAmbiances, true);
        // }
    }

	void Start () {

        _cameraRigTransform = _cameraRig.GetComponent<Transform>();

        _masterVolumeBus = FMODUnity.RuntimeManager.GetBus(masterVolumeBusRef);
        _ambientVolumeBus = FMODUnity.RuntimeManager.GetBus(ambientVolumeBusRef);

        SetMasterBusVolume(0.0f);

        // _directionalAmbianceObjects = GameObject.FindGameObjectsWithTag("DirectionalAmbiance");
        

        // Only plays single background rain from start in lightweight mode
        // if (_environmentAudioSetting == ENVIRO_SETTING.LIGHTWEIGHT) {
        //     foreach(GameObject directionalAmbianceObject in _directionalAmbianceObjects) {
        //         directionalAmbianceObject.GetComponent<DirectionalParameterController.DirectionalParameterController>().SetEnvironmentAmbianceSetting(ENVIRO_SETTING.LIGHTWEIGHT);
        //     }

            _rainAmbianceInstance = FMODUnity.RuntimeManager.CreateInstance(rainAmbiance);
            _rainAmbianceInstance.start();
        // } else {
        //     foreach(GameObject directionalAmbianceObject in _directionalAmbianceObjects) {
        //         directionalAmbianceObject.GetComponent<DirectionalParameterController.DirectionalParameterController>().SetEnvironmentAmbianceSetting(ENVIRO_SETTING.ROBUST);
        //     }
        // }

        // _masterVolumeVCA = FMODUnity.RuntimeManager.GetVCA(masterVolume);
        // _musicVolumeVCA = FMODUnity.RuntimeManager.GetVCA(musicVolume);
        // _uiVolumeVCA = FMODUnity.RuntimeManager.GetVCA(uiVolume);
        // _enviroVolumeVCA = FMODUnity.RuntimeManager.GetVCA(enviroVolume);
	}

    public void playOneShot(string _toPlay) {
        FMODUnity.RuntimeManager.PlayOneShot(_toPlay);
    }

    void Update() {
        // if(_environmentAudioSetting == ENVIRO_SETTING.LIGHTWEIGHT) {
            _rainAmbianceInstance.setParameterValue("Elevation_User", _cameraRigTransform.position.y);
        // }
    }
	
    void OnDestroy()
    {
        _rainAmbianceInstance.release();
    }

    public bool isMusicPlaying() {
        if (_currentState == MUSIC_STATE.PLAYING) {
            return true;
        } else {
            return false;
        }
    }

    public void setMusicPlaying() {
        _currentState = MUSIC_STATE.PLAYING;
    }

    public void setMusicIdle() {
        _currentState = MUSIC_STATE.IDLE;
    }

    
    // BUS AND VOLUME SETTINGS
    public void SetMasterBusVolume(float _controlPercent) {
        float _volumePercent;
        if (_controlPercent == 0.0f) {
            _volumePercent = 0.0f;
        } else {
            _volumePercent = a * Mathf.Exp(_controlPercent * b);
        }
        _masterVolumeBus.setVolume(_volumePercent);
    }

    public float GetMasterBusVolume() {
        float _lastVolumeSent;
        float _actualVolume;
        float _normalizedToSend;
        _masterVolumeBus.getVolume(out _lastVolumeSent, out _actualVolume);
        if(_lastVolumeSent == 0.0f) {
            _normalizedToSend = 0.0f;
        } else {
            _normalizedToSend = Mathf.Log(_lastVolumeSent / a) / b;
        }
        return _normalizedToSend;
    }

    public void SetAmbianceBusVolume(float _controlPercent) {
        float _volumePercent;
        if (_controlPercent == 0.0f) {
            _volumePercent = 0.0f;
        } else {
            _volumePercent = a * Mathf.Exp(_controlPercent * b);
        }
        _ambientVolumeBus.setVolume(_volumePercent);
    }

    public float GetAmbianceBusVolume() {
        float _lastVolumeSent;
        float _actualVolume;
        float _normalizedToSend;
        _ambientVolumeBus.getVolume(out _lastVolumeSent, out _actualVolume); 
        if(_lastVolumeSent == 0.0f) {
            _normalizedToSend = 0.0f;
        } else {
            _normalizedToSend = Mathf.Log(_lastVolumeSent / a) / b;
        }
        return _normalizedToSend;
    }

    public void SetIntroRainVolume(float _controlPercent) {
        if (_rainAmbianceInstance.isValid()) {
            float _volumePercent;
            if (_controlPercent == 0.0f) {
                _volumePercent = 0.0f;
            } else {
                _volumePercent = a * Mathf.Exp(_controlPercent * b);
            }
            _rainAmbianceInstance.setVolume(_volumePercent);
        }
    }

    public float GetIntroRainVolume() {
        if (_rainAmbianceInstance.isValid()) {
            float _lastVolumeSent;
            float _actualVolume;
            float _normalizedToSend;

            _rainAmbianceInstance.getVolume(out _lastVolumeSent, out _actualVolume);
            if(_lastVolumeSent == 0.0f) {
                _normalizedToSend = 0.0f;
            } else {
                _normalizedToSend = Mathf.Log(_lastVolumeSent / a) / b;
            }
            return _normalizedToSend;
        } else {
            return 0.0f;
        }
    }

    public void DestroyIntroRain() {
        _rainAmbianceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _rainAmbianceInstance.release();
    }


    // MENU INTERFACE

    

    // public void SetMasterVolume(float controlPercentage) {
    //     if (controlPercentage != _currentMasterVolumePercent) {
    //         float volumePercent;
    //         if (controlPercentage == 0.0f) {
    //             volumePercent = 0.0f;
    //         } else {
    //             volumePercent = a * Mathf.Exp(controlPercentage * b);
    //         }
    //         _masterVolumeVCA.setVolume(volumePercent);
    //         _currentMasterVolumePercent = controlPercentage;
    //     }
    // }

    // public void SetMusicVolume(float controlPercentage) {
    //     if (controlPercentage != _currentMusicVolumePercent) {
    //         float volumePercent;
    //         if (controlPercentage == 0.0f) {
    //             volumePercent = 0.0f;
    //         } else {
    //             volumePercent = a * Mathf.Exp(controlPercentage * b);
    //         }
    //         _musicVolumeVCA.setVolume(volumePercent);
    //         _currentMusicVolumePercent = controlPercentage;
    //     }
    // }

    // public void SetUIVolume(float controlPercentage) {
    //     if (controlPercentage != _currentMusicVolumePercent) {
    //         float volumePercent;
    //         if (controlPercentage == 0.0f) {
    //             volumePercent = 0.0f;
    //         } else {
    //             volumePercent = a * Mathf.Exp(controlPercentage * b);
    //         }
    //         _uiVolumeVCA.setVolume(volumePercent);
    //         _currentUIVolumePercent = controlPercentage;
    //     }
    // }

    // public void SetEnviroVolume(float controlPercentage) {
    //     if (controlPercentage != _currentEnviroVolumePercent) {
    //         float volumePercent;
    //         if (controlPercentage == 0.0f) {
    //             volumePercent = 0.0f;
    //         } else {
    //             volumePercent = a * Mathf.Exp(controlPercentage * b);
    //         }
    //         _enviroVolumeVCA.setVolume(volumePercent);
    //         _currentEnviroVolumePercent = controlPercentage;
    //     }
    // }

    // public void SetEnvironmentAmbianceSetting(ENVIRO_SETTING newEnviroSetting) {
    //     if (_environmentAudioSetting != newEnviroSetting) {
    //         switch (newEnviroSetting) {
    //             case ENVIRO_SETTING.LIGHTWEIGHT:
    //                 // Going to lightweight drops the robust audio first, then loads the new audio, to save on processing/memory
    //                 foreach(GameObject directionalAmbianceObject in _directionalAmbianceObjects) {
    //                     directionalAmbianceObject.GetComponent<DirectionalParameterController.DirectionalParameterController>().SetEnvironmentAmbianceSetting(ENVIRO_SETTING.LIGHTWEIGHT);
    //                 }
    //                 FMODUnity.RuntimeManager.UnloadBank(robustAmbiances);
    //                 if (lightweightAmbiances != "") {
    //                     FMODUnity.RuntimeManager.LoadBank(lightweightAmbiances, true);
    //                 } else {
    //                     Debug.LogWarning("Lightweight environments bank not defined!");
    //                 }

    //                 _rainAmbianceInstance = FMODUnity.RuntimeManager.CreateInstance(rainAmbiance);
    //                 _rainAmbianceInstance.start();

    //                 break;
    //             case ENVIRO_SETTING.ROBUST:
    //                 // Going to robust starts new audio, then unloads the lightweight audio, to make the transition cleaner without a gap
    //                 if (lightweightAmbiances != "") {
    //                     FMODUnity.RuntimeManager.LoadBank(robustAmbiances, true);
    //                 } else {
    //                     Debug.LogWarning("Robust environments bank not defined!");
    //                 }
    //                 foreach(GameObject directionalAmbianceObject in _directionalAmbianceObjects) {
    //                     directionalAmbianceObject.GetComponent<DirectionalParameterController.DirectionalParameterController>().SetEnvironmentAmbianceSetting(ENVIRO_SETTING.ROBUST);
    //                 }
    //                 _rainAmbianceInstance.release();
    //                 FMODUnity.RuntimeManager.UnloadBank(lightweightAmbiances);
    //                 break;
    //         }
    //     }
    // }
}

