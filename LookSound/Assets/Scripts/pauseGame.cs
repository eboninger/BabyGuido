using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseGame : MonoBehaviour {
	private bool paused;
	public AudioSource audio1;
	public GameObject pauseImage;

	// Use this for initialization
	void Start () {
		paused = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P)){
			if(paused){
				audio1.UnPause();
				pauseImage.SetActive(false);
				Time.timeScale = 1.0f;
				paused = false;
			} else{
				audio1.Pause();
				pauseImage.SetActive(true);
				Time.timeScale = 0.0f;
				paused = true;
			}
		}
	}
}
