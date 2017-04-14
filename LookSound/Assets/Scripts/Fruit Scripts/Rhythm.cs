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
        total_notes = 0;
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

        if (current_index >= durations_arr.Length)
        {
            current_index = 0;
            return on_beat();
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

    public int get_total_notes()
    {
        return total_notes;
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
    public bool finished_playing, listening, playing;
    public Text countdown_notification, on_rhythm_notification, score_display;
    private RhythmicSequence current_rhythm;
    private score_rhythm score_rhy;
    private backing_track back_track;
    private moveBar movbar;
    private float beat, beat_len;
    private bool initialized;
    private Dictionary<char, float> note_divisions;
    private int total_score;

	// Use this for initialization
	void Start () {
        finished_playing = false;
        total_score = 0;
        playing = false;
        initialized = false;
        back_track = GameObject.Find("Player").GetComponent<backing_track>();
        score_rhy = GameObject.Find("Player").GetComponent<score_rhythm>();
        movbar = GameObject.Find("v line").GetComponent<moveBar>();
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
                    update_score(5);
                    current_rhythm.inc_num_correct();
                } else
                {
                    StartCoroutine(notification(on_rhythm_notification, "Not quite!", false, 0));
                    update_score(-1);
                    current_rhythm.inc_num_wrong();
                }
            }
        }
	}

    private void update_score(int inc)
    {
        total_score += inc;
        
        if (total_score < 0)
        {
            total_score = 0;
        }

        score_display.text = "Score: " + total_score;
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
            if (back_track.onStrongBeat())
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

        // start the rhythm bar moving
        movbar.beginMovement();
        playing = true;

        foreach (char single_note in seq)
        {
            var wait_time = note_divisions[single_note];
            //beat_noise.Play();

            //simulate a key press in both scripts (one to make fruit appear, the other to play the sound)
            score_rhy.handle_key_press("a");

            // wait for the duration of the note
            yield return new WaitForSecondsRealtime(wait_time);

        }
        // stop the movement of the rhtyhm bar
        movbar.endMovement();
        playing = false;
        waitForCountdown();
        yield break;
    }

    // wait until we can count the user in to mimic the rhythm
    private void waitForCountdown()
    {
        while (true)
        {
            if (back_track.fourBeatsBefore())
            {
                break;
            }
        }
        StartCoroutine(countdown());
    }

    private IEnumerator countdown()
    {
        // count down from 4 to 2 with the time between numbers being beat_len
        for(int i = 4; i > 1; i--)
        {
            countdown_notification.text = i.ToString();
            yield return new WaitForSecondsRealtime(beat_len);
        }

        // to begin listening before the timer is completely done
        countdown_notification.text = "1";
        yield return new WaitForSecondsRealtime(beat_len - 0.1f);
        listening = true;
        yield return new WaitForSecondsRealtime(0.1f);

        // start checking against the current rhythmic sequence
        current_rhythm.start_time();
        countdown_notification.text = "GO!";

        // begin rhythm bar movement
        movbar.beginMovement();

        // wait to remove the "GO!" message
        yield return new WaitForSecondsRealtime(0.4f);
        countdown_notification.text = "";

        // wait for the duration of the rhythmic sequence
        yield return new WaitForSeconds(current_rhythm.get_total_length() - 0.3f);

        // end rhythm bar movement
        movbar.endMovement();

        // stop listenng and send stats to score_rhythm
        listening = false;
        score_rhy.SendMessage("receive_score", current_rhythm);
    }
}
