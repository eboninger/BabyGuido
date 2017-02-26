using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmicSequence
{
    private List<char> seq;
    private List<float> durations;
    private float time_in;
    private int current_index;
    private float[] durations_arr;

    public RhythmicSequence(string s)
    {
        seq = new List<char>();
        foreach (char c in s)
        {
            seq.Add(c);
        }
    }

    public void start_time()
    {
        time_in = Time.unscaledTime;
    }

    public bool on_beat()
    {
        float delta = Time.unscaledTime - time_in;
        float current_step = durations_arr[current_index];
        current_index++;

        Debug.Log("DELTA: " + delta + ", CURRENT_STEP: " + current_step);

        if ((delta > (current_step - 0.1)) && (delta < (current_step + 0.1))) {
            return true;
        }

        return false;
    }

    public bool finished()
    {
        return current_index >= durations_arr.Length;
    }

    public void init_durations(float beat_len)
    {
        float tracking = 0.0f;
        durations = new List<float>();
        durations.Add(tracking);

        foreach (char c in seq)
        {
            switch (c)
            {
                case 's':
                    tracking += beat_len / 4.0f;
                    durations.Add(tracking);
                    break;
                case 'e':
                    tracking += beat_len / 2.0f;
                    durations.Add(tracking);
                    break;
                case 'q':
                    tracking += beat_len;
                    durations.Add(tracking);
                    break;
                case 'h':
                    tracking += beat_len * 2.0f;
                    durations.Add(tracking);
                    break;
                case 'w':
                    tracking += beat_len * 4.0f;
                    durations.Add(tracking);
                    break;
                default:
                    Debug.Log("Error in switch in init_durations");
                    break;
            }
        }

        durations_arr = durations.ToArray();
    }

    public List<char> get_seq()
    {
        return seq;
    }

    public List<float> get_dur()
    {
        return durations;
    }
}

public class Rhythm : MonoBehaviour {
    public bool finished_playing, listening;
    public Text countdown_notification, on_rhythm_notification;
    private RhythmicSequence current_rhythm;
    private backing_track bt;
    private float beat, beat_len;
    private bool initialized;
    private Dictionary<char, float> note_divisions;

	// Use this for initialization
	void Start () {
        finished_playing = false;
        initialized = false;
        bt = GameObject.Find("Player").GetComponent<backing_track>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if (listening)
            {
                if (current_rhythm.on_beat())
                {
                    StartCoroutine(notification(on_rhythm_notification, "Good Rhythm!", false, 10));
                } else
                {
                    StartCoroutine(notification(on_rhythm_notification, "Not quite!", false, 0));
                }

                if (current_rhythm.finished())
                {
                    listening = false;
                }
            }
        }
	}

    // if the given notification is not already being displayed (i.e. is_on is false), show it 
    // with the given message and increment the total score by score_increase
    IEnumerator notification(Text notif, string message, bool is_on, int score_increase)
    {   
        if (!is_on)
        {
            notif.text = message;
            notif.enabled = true;
            //is_on = true;
            yield return new WaitForSeconds(0.2f);
            //is_on = false;
            notif.enabled = false;
        }
    }

    // given an offset that represents a beat, as well as the length of a single beat, populate dictionary with proper lengths
    // for beats of different durations (i.e. quarter, half, ...etc.)
    public void set_beat(float b, float b_l)
    {
        note_divisions = new Dictionary<char, float>();
        beat = b;
        beat_len = b_l;

        note_divisions.Add('s', beat_len / 4.0f);
        note_divisions.Add('e', beat_len / 2.0f);
        note_divisions.Add('q', beat_len);
        note_divisions.Add('h', beat_len * 2.0f);
        note_divisions.Add('w', beat_len * 4.0f);

        initialized = true;
    }

    // given a rhythmic sequence and an audio source, play the audio source with the given rhythm, start at the 
    // beginning of musical phrase
    //
    // if listen_to_playback is true, listen and respond to the user's attempt to repeat the rhythm
    public void play_rhythm(RhythmicSequence rs, AudioSource beat_noise, bool listen_to_playback)
    {
        finished_playing = false;

        if (!initialized)
        {
            Debug.Log("No notion of beat yet");
            return;
        }

        rs.init_durations(beat_len);
        current_rhythm = rs;


        // loop until the music has reach an acceptable starting point for the sequence
        while (true)
        {
            if (bt.onStrongBeat())
            {
                break;
            }
        }
        StartCoroutine(play_from_seq(beat_noise, listen_to_playback));

    }


    // private: given a sequence of rhythms and an audio source, play that source in the given rhythm
    private IEnumerator play_from_seq(AudioSource beat_noise, bool listen_to_playback)
    {
        var seq = current_rhythm.get_seq();
        foreach (char single_note in seq)
        {
            var wait_time = note_divisions[single_note];
            beat_noise.Play();
            yield return new WaitForSecondsRealtime(wait_time);

        }

        waitForCountdown();
    }

    private void waitForCountdown()
    {
        while (true)
        {
            if (bt.fourBeatsBefore())
            {
                break;
            }
        }
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()
    {
        for(int i = 4; i > 0; i--)
        {
            countdown_notification.text = i.ToString();
            yield return new WaitForSecondsRealtime(beat_len);
        }
        listening = true;
        current_rhythm.start_time();
        countdown_notification.text = "GO!";
        yield return new WaitForSecondsRealtime(0.4f);
    }
}
