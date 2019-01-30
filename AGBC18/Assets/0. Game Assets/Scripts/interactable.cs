using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactable : MonoBehaviour {

    public PopupManager manager;
    public AudioManager audioManager;
    public CartridgeDataHolder cassetteFound;
    Color cassetteColor;

    public bool cassetteOverride = false;
    public string preDialog;
    public string postDialog; 
    public bool interactedWith = false;

    float pressTimer = 0;
    bool recentlyPressed = false;
    float timerLimit = 240;

    Animator anim;
    UIModes ui;

    void Start() {
        anim = this.GetComponent<Animator>();
        ui = GameObject.Find("Game Canvas").GetComponent<UIModes>();

        if(!cassetteOverride) {
            if(cassetteFound == null) {
                cassetteColor = new Color(0, 0, 0, 0);
            }
            else {
                cassetteColor = new Color(cassetteFound.color.r, cassetteFound.color.g, cassetteFound.color.b, 1f);
            }
        }
    }

    void FixedUpdate() {
        if(!cassetteOverride) {
            if (recentlyPressed) {
                pressTimer += Time.deltaTime;

                if (pressTimer >= timerLimit) {
                    pressTimer = 0;
                    recentlyPressed = false;
                }
            }
        }
    }

     void OnMouseDown()
     {
         if(!ui.isMenuOpen) {         
            if(!cassetteOverride) {
                if(!recentlyPressed) {
                    if(manager.GetIfAvailable()) {

                        // LAUNCH CARTRIDGE DIALOG IF UNPRESSED BEFORE
                        if(!interactedWith) {
                            manager.Pop(preDialog, cassetteColor, cassetteFound);
                            
                            if (audioManager != null) {
                                audioManager.playOneShot(audioManager.findTapeInteract);                        
                            } 
                            
                            else {
                                Debug.Log("Set the Audio Manager in Interactable!");
                            }

                            interactedWith = true;
                        }

                        // LAUNCH SECONDARY DIALOG IF PRESSED
                        else {
                            manager.Pop(postDialog, new Color(0, 0, 0, 0));               
                        }
                    }
                }
            }

            if(interactedWith)
                recentlyPressed = true;
                
            anim.Play("Pressed");
            pressTimer = 0;
        }
     }


    //  void OnDrawGizmos() {
    //      Gizmos.DrawIcon(transform.position, "test_icon2.png", true);
    //  }
}
