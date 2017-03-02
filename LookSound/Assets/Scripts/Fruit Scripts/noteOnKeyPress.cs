using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class noteOnKeyPress : MonoBehaviour {

	public GameObject soundSource;
	public Dictionary<string, Note> notes;
	int on_streak = 0, off_streak = 0, prev_note = -10, scale_up = 0, scale_down = 0, user_score = 0;
	bool on_chord = false, first_press = true, on_scale = false;
	public Note input_note;


	// Use this for initialization
	void Start (){
		notes = new Dictionary<string, Note>();
		var sources = soundSource.GetComponents<AudioSource>();

		notes.Add("a", new Note(sources[1], 0));
		notes.Add("s", new Note(sources[2], 1));
		notes.Add("d", new Note(sources[3], 2));
		notes.Add("f", new Note(sources[4], 3));
		notes.Add("g", new Note(sources[5], 4));
		notes.Add("h", new Note(sources[6], 5));
		notes.Add("j", new Note(sources[7], 6));
		notes.Add("k", new Note(sources[8], 7));
	}

	// Update is called once per frame
	void Update (){
		if (Input.anyKeyDown){
			// to calibrate the rhythm
			if (first_press){
				first_press = false;
			}

			// play each note corresponding to buttons pressed
			foreach (char c in Input.inputString){
				try {
					input_note = notes[c.ToString()];
					input_note.sample.Play();
				} catch (Exception e) {
					print("Error: no fruit associated with " + c.ToString());
				} 

			}
		}


	}

}
