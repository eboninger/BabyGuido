using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class score : MonoBehaviour {

    public backing_track bt;
    public Text score_notification;
    public Text chord_notification;
    public Dictionary<string,AudioSource> notes;
    int on_streak = 0;
    int off_streak = 0;
    bool on_chord = false;
    bool first_press = true;


	// Use this for initialization
	void Start ()
    {
        notes = new Dictionary<string, AudioSource>();
        var sources = this.GetComponentsInParent<AudioSource>();
        chord_notification.enabled = false;
        chord_notification.text = "Great Chord!";

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
            if (first_press)
            {
                bt.setBeat();
                first_press = false;
            }
            foreach (char c in Input.inputString)
            {
                notes[c.ToString()].PlayOneShot(notes[c.ToString()].clip, 1);
            }
            beat_check();

            if ((Input.inputString.Length > 1) && (!on_chord))
            {
                StartCoroutine(chord_played());
            }
        }
        
    }

    IEnumerator chord_played()
    {
        chord_notification.enabled = true;
        on_chord = true;
        yield return new WaitForSeconds(2);
        on_chord = false;
        chord_notification.enabled = false;
    }

    void beat_check()
    {
        if (bt.checkOnBeat())
        {
            on_streak++;
            off_streak = 0;
            if (on_streak > 3)
            {
                score_notification.text = "On the beat streak! Length " + on_streak.ToString();
                score_notification.enabled = true;
            }
        }
        else if (bt.checkOffBeat())
        {
            off_streak++;
            on_streak = 0;
            if (off_streak > 3)
            {
                score_notification.text = "Off the beat streak!  Length " + off_streak.ToString();
                score_notification.enabled = true;
            }
        }
        else
        {
            on_streak = 0;
            off_streak = 0;
            score_notification.enabled = false;
        }
    }

}
