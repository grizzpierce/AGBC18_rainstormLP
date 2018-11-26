using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioBin : MonoBehaviour
{
    [Header("Cartridge Audio Events")]
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

    [Space]
    [Header("Ambiance Events")]
    [FMODUnity.EventRef]
    public string rainAmbiance;

    [FMODUnity.EventRef]
    public string lampAmbiance;

    [FMODUnity.EventRef]
    public string vendingMachineAmbiance;

    [FMODUnity.EventRef]
    public string vendingMachineFlicker;

    [FMODUnity.EventRef]
    public string grassAmbiance;

    [Space]
    [Header("Interaction Audio Events")]
    [FMODUnity.EventRef]
    public string vendingMachineInteract;

    [FMODUnity.EventRef]
    public string lampInteract;

    [FMODUnity.EventRef]
    public string binInteract;

    [FMODUnity.EventRef]
    public string pylonInteract;

    [FMODUnity.EventRef]
    public string bikeRackInteract;

    [FMODUnity.EventRef]
    public string cupInteract;

    [FMODUnity.EventRef]
    public string grassInteract;

    [FMODUnity.EventRef]
    public string flyerInteract;

    [FMODUnity.EventRef]
    public string graffitiInteract;

    [FMODUnity.EventRef]
    public string dudInteract;

    [Space]
    [Header("UI Audio Events")]
    [FMODUnity.EventRef]
    public string textScroll;
}