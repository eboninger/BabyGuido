using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmicSequence
{
    private List<char> seq;
    private List<float> durations;
    private float time_in, total_length;
    private int current_index, total_notes, num_correct, num_wrong;
    private float[] durations_arr;
    public const float MARGIN_OF_ERROR = 0.1f;

    public RhythmicSequence(string s)
    {
        seq = new List<char>();
        total_length = 0.0f;
        time_in = -1.0f;

        foreach (char c in s)
        {
            seq.Add(c);
        }
    }

    public void reset()
    {
        current_index = 0;
        num_correct = 0;
        num_wrong = 0;
    }

    public int get_num_correct()
    {
        return num_correct;
    }
    
    public int get_num_wrong()
    {
        return num_wrong;
    }

    public void inc_num_correct()
    {
        num_correct++;
    }

    public void inc_num_wrong()
    {
        num_wrong++;
    }

    public void start_time()
    {
        time_in = Time.unscaledTime;
    }

    public bool on_beat()
    {
        //Debug.Log(current_index);
        if (time_in < 0.0f)
        {
            current_index++;
            return true;
        }

        float delta = Time.unscaledTime - time_in;
        float current_step = durations_arr[current_index];

        if ((delta > (current_step - MARGIN_OF_ERROR)) && (delta < (current_step + MARGIN_OF_ERROR))) {
            current_index++;
            return true;
        } else if (delta > (current_step + MARGIN_OF_ERROR))
        {
            current_index++;
            return on_beat();
        }

        return false;
    }

    public float get_total_length()
    {
        return total_length;
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
                    tracking = add_to_sequence(beat_len, tracking, 0.25f);
                    break;
                case 'e':
                    tracking = add_to_sequence(beat_len, tracking, 0.5f);
                    break;
                case 'q':
                    tracking = add_to_sequence(beat_len, tracking, 1.0f);
                    break;
                case 'h':
                    tracking = add_to_sequence(beat_len, tracking, 2.0f);
                    break;
                case 'w':
                    tracking = add_to_sequence(beat_len, tracking, 4.0f);
                    break;
                default:
                    Debug.Log("Error in switch in init_durations");
                    break;
            }
        }
        total_length = tracking;
        durations_arr = durations.ToArray();
    }

    private float add_to_sequence(float beat_len, float tracking, float scaled_len)
    {
        tracking += beat_len * scaled_len;
        durations.Add(tracking);
        total_notes++;
        return tracking;
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
    private score_rhythm sr;
    private backing_track bt;
    private float beat, beat_len;
    private bool initialized;
    private Dictionary<char, float> note_divisions;

	// Use this for initialization
	void Start () {
        finished_playing = false;
        initialized = false;
        bt = GameObject.Find("Player").GetComponent<backing_track>();
        sr = GameObject.Find("Player").GetComponent<score_rhythm>();
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
                    current_rhythm.inc_num_correct();
                } else
                {
                    StartCoroutine(notification(on_rhythm_notification, "Not quite!", false, 0));
                    current_rhythm.inc_num_wrong();
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
        for(int i = 4; i > 1; i--)
        {
            countdown_notification.text = i.ToString();
            yield return new WaitForSecondsRealtime(beat_len);
        }

        // to count being on beat even before timer is completely done
        countdown_notification.text = "1";
        yield return new WaitForSecondsRealtime(beat_len - 0.1f);
        listening = true;
        yield return new WaitForSecondsRealtime(0.1f);

        current_rhythm.start_time();
        countdown_notification.text = "GO!";
        yield return new WaitForSecondsRealtime(0.4f);
        countdown_notification.text = "";
        yield return new WaitForSecondsRealtime(current_rhythm.get_total_length() - 0.3f);
        Debug.Log("Done waiting");
        listening = false;
        sr.SendMessage("receive_score", current_rhythm);
    }
}
