using UnityEngine;
using System.Collections;

public class play_functionality : MonoBehaviour {
    hand_positions hp;
    public GameObject[] playObjects;
    public const int MAX_PLAY_OBJECTS = 20;
    public int total_play_objects;
    public int play_index;
    public GameObject currentHighlighted;

    // Use this for initialization
    void Start () {
        hp = GameObject.Find("Unhighlighted").GetComponent<hand_positions>();
        playObjects = new GameObject[MAX_PLAY_OBJECTS];
        total_play_objects = 0;
        play_index = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addToPlayPanel()
    {
        if (total_play_objects >= 10)
            return;

        var pi = hp.soundObjects[hp.currentI].GetComponent<object_info>();

        GameObject newChild = Instantiate(pi.unhighlightedChildTemplate);
        newChild.transform.SetParent(transform);
        newChild.transform.position = transform.position;

        GameObject newHChild = Instantiate(pi.highlightedChildTemplate);
        newHChild.transform.SetParent(transform);
        newHChild.transform.position = transform.position;
        newHChild.SetActive(false);

        playObjects[total_play_objects * 2] = newChild;
        playObjects[(total_play_objects * 2)     + 1] = newHChild;

        total_play_objects++;  
    }

    public void moveLeft()
    {
        unhighlight();
        play_index = ((play_index - 1) + total_play_objects) % total_play_objects;
        highlight();
    }

    public void moveRight()
    {
        unhighlight();
        play_index = (play_index + 1) % total_play_objects;
        highlight();
    }

    public void highlight()
    {
        if (total_play_objects < 1)
            return;

        playObjects[play_index * 2].SetActive(false);
        playObjects[(play_index * 2) + 1].SetActive(true);
    }

    public void unhighlight()
    {
        if (total_play_objects < 1)
            return;

        playObjects[(play_index * 2) + 1].SetActive(false);
        playObjects[(play_index * 2)].SetActive(true);
    }
}
