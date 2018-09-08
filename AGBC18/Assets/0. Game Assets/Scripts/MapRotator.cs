using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Rewired;

public class MapRotator : MonoBehaviour {

	public int playerID = 0;
	public float ghostRotator = 0;

	private Player player; 
	private Tween rotationTween;

	void Start () {
        player = ReInput.players.GetPlayer(playerID);
		rotationTween = DOTween.To(()=> ghostRotator, x=> ghostRotator = x, 0, 0);
	}
	
	void Update () {
		if ((player.GetButton("Rotator") == true) || player.GetNegativeButton("Rotator") == true) {
			if(player.GetButton("Rotator")) {
				if(rotationTween.IsPlaying()) {
					rotationTween.Kill();
				}

				rotationTween = DOTween.To(()=> ghostRotator, x=> ghostRotator = x, 1, 1);
			}
			else {
				if(rotationTween.IsPlaying()) {
					rotationTween.Kill();
				}


				rotationTween = DOTween.To(()=> ghostRotator, x=> ghostRotator = x, -1, 1);				
			}
		}
		else
		{
			if(rotationTween.IsPlaying())
				rotationTween.Kill();

			rotationTween = DOTween.To(()=> ghostRotator, x=> ghostRotator = x, 0, 10);
		}

		if(player.GetButton("Brake")) {
			rotationTween.Kill();
			ghostRotator = 0;
		}

		transform.Rotate(0, player.GetAxis("Rotator") + ghostRotator, 0, Space.Self);
	}
}
