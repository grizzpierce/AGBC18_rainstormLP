using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSizeController : MonoBehaviour {

	public WindowManager winManager;
	public List<RadioButton> toggles;
	
	void Start () {
		for(int i = 0; i < transform.childCount; ++i) {
			toggles.Add(transform.GetChild(i).GetComponent<RadioButton>());
			toggles[i].setSelected(false);
		}

		toggles[toggles.Count-1].setSelected(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void rbPressed(int winSize) {
		winManager.setSize(winSize);
		RadioButton temp = null;

		for(int i = 0; i < transform.childCount; ++i) {
			toggles[i].GetComponent<RadioButton>().setSelected(false);
			if(winSize == int.Parse(toggles[i].name)) {
				temp = toggles[i];
			}
		}	

		if(temp != null)
			temp.setSelected(true);		
	}
}
