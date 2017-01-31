using UnityEngine;
using System.Collections;

public class backing_track : MonoBehaviour {

    public AudioSource bt;
    public const float BEAT_MOD = 0.5f;
    public const float BEAT = 0.314f;
    public const float DIF = 0.1f;
    public const float BEAT_LO = BEAT - DIF;
    public const float BEAT_HI = BEAT + DIF;

    // Use this for initialization
    void Start()
    {
        bt.Play();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool checkOnBeat()
    {
        var ts_mod = bt.time % BEAT_MOD;
        print(ts_mod);
        if ((ts_mod > BEAT_LO) && (ts_mod < BEAT_HI))
        {
            return true;
        }

        return false;
    }
}
