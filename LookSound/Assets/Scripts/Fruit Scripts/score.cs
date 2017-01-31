using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class score : MonoBehaviour {

    public backing_track bt;
    public Text score_notification;
    public Dictionary<string,AudioSource> notes;
    int on_streak = 0;


	// Use this for initialization
	void Start ()
    {
        notes = new Dictionary<string, AudioSource>();
        var sources = this.GetComponentsInParent<AudioSource>();

        notes.Add("a", sources[1]);
        notes.Add("s", sources[2]);
        notes.Add("d", sources[3]);
        notes.Add("f", sources[4]);
        notes.Add("g", sources[5]);
        notes.Add("h", sources[6]);
        notes.Add("j", sources[7]);
        print(notes);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                notes[c.ToString()].PlayOneShot(notes[c.ToString()].clip, 1);
                beat_check();
            }
        }
        
    }

    void beat_check()
    {
        if (bt.checkOnBeat())
        {
            on_streak++;
            if (on_streak > 3)
            {
                score_notification.text = "On the beat streak! Length " + on_streak.ToString();
                score_notification.enabled = true;
            }
        }
        else
        {
            on_streak = 0;
            score_notification.enabled = false;
        }
    }

}
