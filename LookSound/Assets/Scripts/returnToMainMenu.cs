using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnToMainMenu : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.B)){
			Application.LoadLevel(0);
		}
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
		
	}
}
