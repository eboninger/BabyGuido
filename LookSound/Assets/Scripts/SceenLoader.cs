using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceenLoader : MonoBehaviour {

	public void loadBeachScene(){
		Application.LoadLevel (1);
	}

	public void loadFruitScene(){
		Application.LoadLevel (2);
	}

	public void loadRhythmScene(){
		Application.LoadLevel (3);
	}
}
