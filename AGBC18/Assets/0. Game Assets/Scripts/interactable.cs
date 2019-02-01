using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
TODO finish implementing pre and post dialog functionality
 */

public class interactable : MonoBehaviour {

    public CartridgeDataHolder cassetteFound;

    public bool cassetteOverride = false;
    public bool _interactedWith = false;


    [Header("First Interaction")]
    public string preDialog;
    [FMODUnity.EventRef]
    public string preDialogAudio;
    public float cartridgeDiscoverAudioDelay = 0.0f;

    [Space]
    [Header("Subsequent Interactions")]
    public string postDialog; 
    [FMODUnity.EventRef]
    public string postDialogAudio;

    float _pressTimer = 0f;
    bool _recentlyPressed = false;
    float _timerLimit = 240f;

    Animator _anim;
    Color _cassetteColor;
    UIModes _ui;


    PopupManager _popupManager;
    AudioManager _audioManager;

    void Start() {
        _anim = this.GetComponent<Animator>();

        if (_audioManager == null) {
            _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        }
        if (_popupManager == null) {
            _popupManager = GameObject.FindGameObjectWithTag("PopupManager").GetComponent<PopupManager>();
        }
        _ui = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<UIModes>();

        if(!cassetteOverride) {
            if(cassetteFound == null) {
                _cassetteColor = new Color(0, 0, 0, 0);
            }
            else {
                _cassetteColor = new Color(cassetteFound.color.r, cassetteFound.color.g, cassetteFound.color.b, 1f);
            }
        }
    }

    void FixedUpdate() {
        if(!cassetteOverride) {
            if (_recentlyPressed) {
                _pressTimer += Time.deltaTime;

                if (_pressTimer >= _timerLimit) {
                    _pressTimer = 0;
                    _recentlyPressed = false;
                }
            }
        }
    }

    void OnMouseDown()
    {
        // If menu or popup is open, accept no input
        if(!(_ui.isMenuOpen || _popupManager.IsDisplaying())) {
            // Is this a cassette container?
            if(!cassetteOverride) {
                if (!_interactedWith) {
                    StartCoroutine(TapeDiscoveryAudio());
                } else {
                    FMODUnity.RuntimeManager.PlayOneShot(postDialogAudio);
                }

                // Only display the popup if it has not been recently interacted with
                if(!_recentlyPressed) {
                    if(_popupManager.GetIfAvailable()) {

                        // LAUNCH CARTRIDGE DIALOG IF UNPRESSED BEFORE
                        if(!_interactedWith) {
                            _popupManager.Pop(preDialog, _cassetteColor, cassetteFound);
                            _interactedWith = true;
                            _recentlyPressed = true;

                        }

                        // LAUNCH SECONDARY DIALOG IF PRESSED
                        else {
                            _popupManager.Pop(postDialog, new Color(0, 0, 0, 0));
                        }
                    }
                }
            // This is a micro interaction!
            } else {
                FMODUnity.RuntimeManager.PlayOneShot(preDialogAudio);
                if (!_recentlyPressed) {
                    if (_popupManager.GetIfAvailable()) {
                        _popupManager.Pop(preDialog, Color.clear);
                    }
                }
            }
            
            _anim.Play("Pressed");
            _pressTimer = 0;
        }
    }

    IEnumerator TapeDiscoveryAudio() {
        FMODUnity.RuntimeManager.PlayOneShot(preDialogAudio);
        if(!cassetteOverride) {
            yield return new WaitForSeconds(cartridgeDiscoverAudioDelay);
            FMODUnity.RuntimeManager.PlayOneShot(_audioManager.findTapeInteract);
        }
    }
}
