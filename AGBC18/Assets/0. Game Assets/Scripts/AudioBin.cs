using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBin : MonoBehaviour {

    [Header("Tracks")]
    [FMODUnity.EventRef]
    public string SFX_track1;

    [Header("Cartridge Audio Events")]
    [FMODUnity.EventRef]
    public string SFX_cartridgeLoad;

    [FMODUnity.EventRef]
    public string SFX_cartridgePlay;

    [FMODUnity.EventRef]
    public string SFX_cartridgeStop;

    [FMODUnity.EventRef]
    public string SFX_cartridgeFinishPlaying;

    [FMODUnity.EventRef]
    public string SFX_cartridgeRattle;

    [Space]
    [Header("Ambiance Events")]
    [FMODUnity.EventRef]
    public string SFX_rainAmbiance;

    [FMODUnity.EventRef]
    public string SFX_lampAmbiance;

    [FMODUnity.EventRef]
    public string SFX_vendingMachineAmbiance;

    [FMODUnity.EventRef]
    public string SFX_vendingMachineFlicker;

    [FMODUnity.EventRef]
    public string SFX_grassAmbiance;

    [Space]
    [Header("Interaction Audio Events")]
    [FMODUnity.EventRef]
    public string SFX_vendingMachineInteract;

    [FMODUnity.EventRef]
    public string SFX_lampInteract;

    [FMODUnity.EventRef]
    public string SFX_binInteract;

    [FMODUnity.EventRef]
    public string SFX_pylonInteract;

    [FMODUnity.EventRef]
    public string SFX_bikeRackInteract;

    [FMODUnity.EventRef]
    public string SFX_cupInteract;

    [FMODUnity.EventRef]
    public string SFX_grassInteract;

    [FMODUnity.EventRef]
    public string SFX_flyerInteract;

    [FMODUnity.EventRef]
    public string SFX_graffitiInteract;

    [FMODUnity.EventRef]
    public string SFX_dudInteract;

    [Space]
    [Header("UI Audio Events")]
    [FMODUnity.EventRef]
    public string SFX_textScroll;


	void Start () {
		
	}
	void Update () {
		
	}
}
