using UnityEngine;
using System.Collections;

public class playStaffSounds : MonoBehaviour {
	public GameObject staff;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(){
		print("in play");
		foreach(Transform child in staff.transform){
			print("play " + child.name + "s sound");
			AudioSource audio = child.GetComponent<AudioSource>();
			if(audio){
				audio.Play();
			}
		}


	}
}
