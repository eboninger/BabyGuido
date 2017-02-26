using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Note
{
    public AudioSource sample;
    public int num;

    public Note(AudioSource a, int n)
    {
        sample = a;
        num = n;
    }
}

public class score : MonoBehaviour {

    public backing_track bt;
    public Text rhythm_notification, chord_notification, scale_notification, score_display;
    public Dictionary<string, Note> notes;
    int on_streak = 0, off_streak = 0, prev_note = -10, scale_up = 0, scale_down = 0, user_score = 0;
    bool on_chord = false, first_press = true, on_scale = false;
    public Note input_note;


	// Use this for initialization
	void Start ()
    {
        notes = new Dictionary<string, Note>();
        var sources = this.GetComponentsInParent<AudioSource>();
        chord_notification.enabled = false;
        chord_notification.text = "Great Chord!";

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
	void Update ()
    {
        if (Input.anyKeyDown)
        {
            // to calibrate the rhythm
            if (first_press)
            {
                bt.setBeat(false);
                first_press = false;
            }

            // play each note corresponding to buttons pressed
            foreach (char c in Input.inputString)
            {
                input_note = notes[c.ToString()];
                input_note.sample.Play();
            }
            give_points();
        }
        

    }

    private void give_points()
    {
        beat_check();

        // if a chord is played
        if ((Input.inputString.Length > 1))
        {
            StartCoroutine(notification(chord_notification, "Great Chord!! +5", on_chord, 5));
            scale_up = 0;
            scale_down = 0;
        }
        // if the note played is one above the previous note
        else if (input_note.num == (prev_note + 1))
        {
            scale_up++;
        }
        // if the note played is one below the previous note
        else if (input_note.num == (prev_note - 1))
        {
            scale_down++;
        }
        // nothing special happens (i.e. both scales end)
        else
        {
            scale_down = 0;
            scale_up = 0;
        }

        prev_note = input_note.num;

        if (scale_up > 3)
        {
            StartCoroutine(notification(scale_notification, "Great Scale Up!! +10", on_scale, 10));
            scale_up = 0;
        }
        else if (scale_down > 3)
        {
            StartCoroutine(notification(scale_notification, "Great Scale Down!! +10", on_scale, 10));
            scale_down = 0;
        }
    }

    // if the given notification is not already being displayed (i.e. is_on is false), show it 
    // with the given message and increment the total score by score_increase
    IEnumerator notification(Text notif, string message, bool is_on, int score_increase)
    {
        user_score += score_increase;
        score_display.text = "Score: " + user_score;

        if (!is_on)
        {
            notif.text = message;
            notif.enabled = true;
            is_on = true;
            yield return new WaitForSeconds(2);
            is_on = false;
            notif.enabled = false;
        }
    }

    void beat_check()
    {
        if (bt.checkOnBeat())
        {
            if (on_streak == 0)
            {
                rhythm_notification.enabled = false;
            }
            on_streak++;
            off_streak = 0;
            if (on_streak > 3)
            {
                rhythm_notification.text = "On the beat streak! Length " + on_streak.ToString();
                rhythm_notification.enabled = true;
                rhythm_score_increase(2);
            }
        }
        else if (bt.checkOffBeat())
        {
            if (off_streak == 0)
            {
                rhythm_notification.enabled = false;
            }
            off_streak++;
            on_streak = 0;
            if (off_streak > 3)
            {
                rhythm_notification.text = "Off the beat streak!  Length " + off_streak.ToString();
                rhythm_notification.enabled = true;
                rhythm_score_increase(3);
            }
        }
        else
        {
            on_streak = 0;
            off_streak = 0;
            rhythm_notification.enabled = false;
        }
    }

    void rhythm_score_increase(int increase_by)
    {
        user_score += increase_by;
        score_display.text = "Score: " + user_score;
    }

}
