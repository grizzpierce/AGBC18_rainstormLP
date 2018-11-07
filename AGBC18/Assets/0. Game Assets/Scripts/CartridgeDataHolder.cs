using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartridgeDataHolder : MonoBehaviour {

    public Color color;

    public string unknownText = "???";
    public string revealedText;

    public int fadeOutTime;

    [FMODUnity.EventRef]
    [SerializeField] public string trackAudioEvent;

	void Start () {
        if (fadeOutTime.Equals(0)) {
            fadeOutTime = 1;
        }
	}
	
	void Update () {
		
	}
}
