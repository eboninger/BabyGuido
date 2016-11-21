using UnityEngine;
using System.Collections;

public class play_functionality : MonoBehaviour {
    hand_positions hp;

	// Use this for initialization
	void Start () {
        hp = GameObject.Find("Unhighlighted").GetComponent<hand_positions>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addToPlayPanel()
    {
        var pi = hp.soundObjects[hp.currentI].GetComponent<object_info>();

        GameObject newChild = Instantiate(pi.unhighlightedChildTemplate);
        newChild.transform.SetParent(transform);
        newChild.transform.position = transform.position;

        hp.soundObjects[hp.totalSoundObjects] = newChild;
        hp.add_possible_position(newChild);
    }
}
