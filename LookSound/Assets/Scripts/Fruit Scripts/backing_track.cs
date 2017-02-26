using UnityEngine;
using System;
using System.Collections;



public class backing_track : MonoBehaviour {

    public AudioSource bt;
    private Rhythm rhythm;
    public float sampling_rate;
    public const float BEAT_MOD = 0.5f;
    public const float MEASURE_MOD = 4.0f;
    private float BEAT, OFFBEAT, BEAT_LO, BEAT_HI, OFFBEAT_LO, OFFBEAT_HI, MEASURE, MEASURE_LO, MEASURE_HI;
    public const float DIF = 0.1f;


    // Use this for initialization
    void Start()
    {
        sampling_rate = 1.0f / bt.clip.frequency;
        bt.Play();
        rhythm = GameObject.Find("RhythmManager").GetComponent<Rhythm>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setBeat(bool on_rhythm)
    {
        BEAT = (bt.timeSamples * sampling_rate) % BEAT_MOD;
        MEASURE = (bt.timeSamples * sampling_rate) % MEASURE_MOD;
        MEASURE_LO = MEASURE - 0.05f;
        MEASURE_HI = MEASURE + 0.05f;
        BEAT_LO = BEAT - DIF;
        BEAT_HI = BEAT + DIF;
        OFFBEAT = Math.Abs(BEAT + 0.25f);
        OFFBEAT_LO = OFFBEAT - DIF;
        OFFBEAT_HI = OFFBEAT + DIF;
        print(BEAT);
        
        if (on_rhythm)
            rhythm.set_beat(BEAT, BEAT_MOD);
    }

    public bool checkOnBeat()
    {
        var ts_mod = (bt.timeSamples * sampling_rate) % BEAT_MOD;
        if ((ts_mod > BEAT_LO) && (ts_mod < BEAT_HI))
        {
            return true;
        }

        return false;
    }

    public bool checkOffBeat()
    {
        var ts_mod = (bt.timeSamples * sampling_rate) % BEAT_MOD;
        if ((ts_mod > OFFBEAT_LO) && (ts_mod < OFFBEAT_HI))
        {
            return true;
        }

        return false;
    }

    public bool onStrongBeat()
    {
        var ts_mod = (bt.timeSamples * sampling_rate) % MEASURE_MOD;
        
        if ((ts_mod > MEASURE_LO) && (ts_mod < MEASURE_HI))
        {
            return true;
        }

        return false;
    }

    public bool fourBeatsBefore()
    {
        var ts_mod = (bt.timeSamples * sampling_rate) % MEASURE_MOD;

        if ((ts_mod > (MEASURE_LO - (BEAT_MOD * 4))) && (ts_mod < (MEASURE_HI - (BEAT_MOD * 4))))
        {
            return true;
        }

        return false;
    }

    public void print_ts_mod()
    {
        print((bt.timeSamples * sampling_rate) % MEASURE_MOD);
    }
}


