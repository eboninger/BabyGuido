using UnityEngine;
using System.Collections;



public class backing_track : MonoBehaviour {

    public AudioSource bt;
    public float sampling_rate;
    public const int BEAT_MOD = 22008;
    public const int BEAT = 16244;
    public const int OFFBEAT = 4439;
    public const int DIF = 2000;
    public const int BEAT_LO = BEAT - DIF;
    public const int BEAT_HI = BEAT + DIF;
    public const int OFFBEAT_LO = OFFBEAT - DIF;
    public const int OFFBEAT_HI = OFFBEAT + DIF;

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

    public bool checkOnBeat()
    {
        var ts_mod = bt.timeSamples % BEAT_MOD;
        print(ts_mod);
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


