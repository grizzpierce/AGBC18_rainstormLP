using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CartridgeData : MonoBehaviour {

    enum CARTRIDGE_STATE
    {
        UNKNOWN,
        KNOWN
    }

    CARTRIDGE_STATE STATE;
    CartridgeDataHolder dataHolder;

    Color unknownColor = new Color32(75, 75, 75, 255);
    public string unknownText = "???";
	public bool startsUnknown = false;

	RawImage ui;
    Text textLabel;


	void Start() {
		ui = gameObject.GetComponent<RawImage>();
                                     
		if(transform.childCount > 0) {
			textLabel = transform.GetChild(0).GetComponent<Text>();
		}

        if(startsUnknown) {
            STATE = CARTRIDGE_STATE.UNKNOWN;
            textLabel.text = unknownText;
            Hide();
        }
	}

    public CartridgeDataHolder GetDataHolder() {
        return dataHolder;
    }

    public bool IsKnown() {
        if(STATE == CARTRIDGE_STATE.KNOWN) {
            return true;
        } else {
            return false;
        }
    }

    // Called from PopupManager
    public void SetKnown(CartridgeDataHolder _dataHolder) {
        if (_dataHolder == null) {
            Debug.Log("No cartridge data supplied");
        } else {
            dataHolder = _dataHolder.GetComponent<CartridgeDataHolder>();
            STATE = CARTRIDGE_STATE.KNOWN;
            StartCoroutine(Reveal());
        }

	}

    void Hide() {
		ui.DOColor(unknownColor, .25f);
		textLabel.DOFade(1f, .25f);
	}

    IEnumerator Reveal() {
        if (dataHolder.revealedText.Equals(null)) {
            textLabel.DOFade(0f, .5f);
            ui.DOColor(dataHolder.color, .5f);
        } else {
            textLabel.DOFade(0f, .5f);
            //yield return new WaitForSeconds(.5f);
            textLabel.text = dataHolder.revealedText;
            ui.DOColor(dataHolder.color, .1f);
            textLabel.DOFade(1f, .5f);
        }
        yield return null;
	}
}
