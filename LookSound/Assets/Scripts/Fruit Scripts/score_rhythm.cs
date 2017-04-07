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
    public Text stats_display;
    private LinkedList<RhythmicSequence> sequences;
    private rhythmFruit rhy_fruit;


    // Use this for initialization
    void Start()
    {
        rhythm = GameObject.Find("RhythmManager").GetComponent<Rhythm>();
        notes = new Dictionary<string, Note>();
        rhy_fruit = GameObject.Find("FruitGenerator").GetComponent<rhythmFruit>();
        sequences = new LinkedList<RhythmicSequence>();
        var sources = this.GetComponentsInParent<AudioSource>();

        initialize_starting_params(sources);
    }

    private void initialize_starting_params(AudioSource[] sources)
    {
        notes.Add("a", new Note(sources[1], 0));
        notes.Add("s", new Note(sources[2], 1));
        notes.Add("d", new Note(sources[3], 2));
        notes.Add("f", new Note(sources[4], 3));
        notes.Add("g", new Note(sources[5], 4));
        notes.Add("h", new Note(sources[6], 5));
        notes.Add("j", new Note(sources[7], 6));
        notes.Add("k", new Note(sources[8], 7));

        sequences.AddLast(new RhythmicSequence("hhhh"));
        sequences.AddLast(new RhythmicSequence("hhqqq"));
        sequences.AddLast(new RhythmicSequence("wqqqq"));
        sequences.AddLast(new RhythmicSequence("hqqqee"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            handle_key_press(Input.inputString);
        }

    }

    public void handle_key_press(string in_str)
    {
        // to calibrate the rhythm
        if (first_press)
        {
            bt.setBeat(true);
            first_press = false;
            StartCoroutine(begin_challenge());
        }

        if (rhythm.listening || rhythm.playing)
        {
            rhy_fruit.handle_key_press(in_str);
        }

        // play each note corresponding to buttons pressed
        foreach (char c in in_str)
        {
            input_note = notes[c.ToString()];
            input_note.sample.Play();
        }
    }

    void receive_score(RhythmicSequence rs)
    {
        var num_correct = rs.get_num_correct();
        var num_wrong = rs.get_num_wrong();

        stats_display.text = ("On the last rhythm, you got\n" + num_correct.ToString() + " notes on the rhythm\n" + 
                               num_wrong.ToString() + " notes off the rhythm\n");

        if (((num_correct * 1.0f) / (rs.get_total_notes() * 1.0f)) > .85f)
        {
            sequences.RemoveFirst();
        }

        sequences.First.Value.reset();
        rhythm.play_rhythm(sequences.First.Value, notes["a"].sample, true);
    }

    IEnumerator begin_challenge()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        rhythm.play_rhythm(sequences.First.Value, notes["a"].sample, true);
    }


}