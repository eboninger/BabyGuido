using UnityEngine;
using System.Collections;

public class playSequence : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        var hand = GameObject.Find("Hand").transform;

        StartCoroutine(PlaySoundList(hand));
        //hand.GetChild(i).GetComponent<AudioSource>().Play();

    }

    IEnumerator PlaySoundList(Transform hand)
    {
        int numSounds = hand.childCount;
        for (int i = 0; i < numSounds; i++)
        {
            AudioSource audio = hand.GetChild(i).GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSecondsRealtime(audio.clip.length);
        }
        
    }
}
