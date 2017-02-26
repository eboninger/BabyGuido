using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class score_rhythm : MonoBehaviour
{

    public backing_track bt;
    public Dictionary<string, Note> notes;
    int user_score = 0;
    bool first_press = true;
    public Note input_note;
    public Rhythm rhythm;


    // Use this for initialization
    void Start()
    {
        rhythm = GameObject.Find("RhythmManager").GetComponent<Rhythm>();
        notes = new Dictionary<string, Note>();
        var sources = this.GetComponentsInParent<AudioSource>();

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
    void Update()
    {
        if (Input.anyKeyDown)
        {
            // to calibrate the rhythm
            if (first_press)
            {
                bt.setBeat(true);
                first_press = false;
            }

            // play each note corresponding to buttons pressed
            foreach (char c in Input.inputString)
            {
                input_note = notes[c.ToString()];
                input_note.sample.Play();
            }
        }

        if (Input.GetKeyDown("a"))
        {
            RhythmicSequence rs = new RhythmicSequence("hhqqq");
            rhythm.play_rhythm(rs, notes["a"].sample, true);
        }

    }


}