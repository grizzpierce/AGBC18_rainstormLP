using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KioskManager : MonoBehaviour {

	public bool isKioskModeOn = false;

	public void resetGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
