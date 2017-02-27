using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmicSequence
{
    private List<char> seq;
    private List<float> durations;

    public RhythmicSequence(string s)
    {
        seq = new List<char>();
        foreach (char c in s)
        {
            seq.Add(c);
        }
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
    public bool finished_playing;
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
        print("ready to play");
        finished_playing = false;

        if (!initialized)
        {
            Debug.Log("No notion of beat yet");
            return;
        }

        var seq = rs.get_seq();
        rs.init_durations(beat_len);

        // loop until the music has reach an acceptable starting point for the sequence
        while (true)
        {
            if (bt.onStrongBeat())
            {
                break;
            }
        }
        StartCoroutine(play_from_seq(seq, beat_noise, listen_to_playback, rs.get_dur()));

    }


    // private: given a sequence of rhythms and an audio source, play that source in the given rhythm
    private IEnumerator play_from_seq(List<char> seq, AudioSource beat_noise, bool listen_to_playback, List<float> durations)
    {
        foreach (char single_note in seq)
        {
            var wait_time = note_divisions[single_note];
            beat_noise.Play();
            yield return new WaitForSecondsRealtime(wait_time);

        }

        listen(seq);
    }

    private void listen(List<char> seq)
    {
        while (true)
        {
            if (bt.fourBeatsBefore())
            {
                break;
            }
        }
    }
}
