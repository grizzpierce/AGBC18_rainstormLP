using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartridgeDataHolder : MonoBehaviour {

    public Color color;

    public string unknownText = "???";
    public string revealedText;

    [Range(0, 10)]
    public float fadeInTimeOnStart = 1.0f;
    [Range(0, 10)]
    public float fadeOutTimeOnStop = 1.0f;

    [FMODUnity.EventRef]
    [SerializeField] public string trackAudioEvent;

	void Start () {
        
	}
	
	void Update () {
		
	}
}
