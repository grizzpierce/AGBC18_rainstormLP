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
    public bool interactedWith = false;


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
    UIModes ui;


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
        ui = GameObject.Find("Game Canvas").GetComponent<UIModes>();

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
        if(!ui.isMenuOpen) {         
            if(!cassetteOverride) {
                if(!_recentlyPressed) {
                    if(_popupManager.GetIfAvailable()) {

                        // LAUNCH CARTRIDGE DIALOG IF UNPRESSED BEFORE
                        if(!interactedWith) {
                            _popupManager.Pop(preDialog, _cassetteColor, cassetteFound);
                            
                            if (_audioManager != null) {
                                _audioManager.playOneShot(_audioManager.findTapeInteract);                        
                            } 
                            
                            else {
                                Debug.Log("Set the Audio Manager in Interactable!");
                            }

                            interactedWith = true;
                        }

                        // LAUNCH SECONDARY DIALOG IF PRESSED
                        else {
                            _popupManager.Pop(postDialog, new Color(0, 0, 0, 0));               

                            // LAUNCH SECONDARY DIALOG IF PRESSED
                            else {
                                manager.Pop(postDialog, new Color(0, 0, 0, 0));               
                            }
                        }
                    }
                }

                if(interactedWith)
                    _recentlyPressed = true;
                    
                _anim.Play("Pressed");
                _pressTimer = 0;
            }
        }
     }
}
