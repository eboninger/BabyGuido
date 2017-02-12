using UnityEngine;
using System.Collections;



public class backing_track : MonoBehaviour {

    public AudioSource bt;
    public float sampling_rate;
    public const float BEAT_MOD = 0.5f;
    private float BEAT, BEAT_LO, BEAT_HI, OFFBEAT_LO, OFFBEAT_HI;
    public const float OFFBEAT = 4439.0f;
    public const float DIF = 0.04f;


    // Use this for initialization
    void Start()
    {
        sampling_rate = 1.0f / bt.clip.frequency;
        bt.Play();
        print("TIME: " + Time.time * 1.0f);     
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setBeat()
    {
        BEAT = (bt.timeSamples * sampling_rate) % BEAT_MOD;
        BEAT_LO = BEAT - DIF;
        BEAT_HI = BEAT + DIF;
        OFFBEAT_LO = OFFBEAT - DIF;
        OFFBEAT_HI = OFFBEAT + DIF;
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
        var ts_mod = bt.timeSamples % BEAT_MOD;
        if ((ts_mod > OFFBEAT_LO) && (ts_mod < OFFBEAT_HI))
        {
            return true;
        }

        return false;
    }
}


