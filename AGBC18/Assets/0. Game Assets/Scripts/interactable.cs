using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactable : MonoBehaviour {

    public PopupManager manager;
    public AudioManager audioManager;
    public CartridgeDataHolder cassetteFound;
    Color cassetteColor;

    public string preDialog;
    public string postDialog; 
    public bool interactedWith = false;

    public float pressTimer = 0;
    public bool recentlyPressed = false;
    public float timerLimit = 240;

    Animator anim;

    void Start() {
        anim = this.GetComponent<Animator>();

        if(cassetteFound == null) {
            cassetteColor = new Color(0, 0, 0, 0);
        }
        else {
            cassetteColor = new Color(cassetteFound.color.r, cassetteFound.color.g, cassetteFound.color.b, 1f);
        }
    }

    void FixedUpdate() {
        if (recentlyPressed) {
            pressTimer += Time.deltaTime;

            if (pressTimer >= timerLimit) {
                pressTimer = 0;
                recentlyPressed = false;
            }
        }
    }

     void OnMouseDown()
     {
         if(!recentlyPressed) {
            if(manager.GetIfAvailable()) {

                // LAUNCH CARTRIDGE DIALOG IF UNPRESSED BEFORE
                if(!interactedWith) {
                    manager.Pop(preDialog, cassetteColor, cassetteFound);
                    
                    if (audioManager != null) {
                        FMODUnity.RuntimeManager.PlayOneShot(audioManager.audioBin.findTapeInteract);
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

        recentlyPressed = true;
        anim.Play("Pressed");
        pressTimer = 0;
     }


    //  void OnDrawGizmos() {
    //      Gizmos.DrawIcon(transform.position, "test_icon2.png", true);
    //  }
}
