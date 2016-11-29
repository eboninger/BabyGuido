using UnityEngine;
using System.Collections;

public class play_functionality : MonoBehaviour {
    hand_positions hp;
    public GameObject[] playObjects;
    public int MAX_PLAY_OBJECTS = 10;
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

        playObjects[total_play_objects] = newChild;
        total_play_objects++;
        print("TOTAL PLAY OBECTS: " + total_play_objects);    
    }

    public void moveLeft()
    {

    }

    public void moveRight()
    {

    }

    public void highlight()
    {
        if (total_play_objects < 1)
            return;

        var pwh = playObjects[play_index].GetComponent<prefab_with_highlight>();
        var desired_index = playObjects[play_index].transform.GetSiblingIndex(); 

        GameObject newHighlighted = Instantiate(pwh.highlighted);
        newHighlighted.transform.SetParent(transform);
        newHighlighted.transform.position = transform.position;
        newHighlighted.transform.SetSiblingIndex(desired_index);
        Destroy(playObjects[play_index]);
        playObjects[play_index] = newHighlighted;

        // currentHighlighted = newHighlighted;
    }

    public void unhighlight()
    {
        if (total_play_objects < 1)
            return;

        var pwo = playObjects[play_index].GetComponent<prefab_wo_highlight>();
        var desired_index = playObjects[play_index].transform.GetSiblingIndex();

        GameObject newUnhighlighted = Instantiate(pwo.unhighlighted);
        newUnhighlighted.transform.SetParent(transform);
        newUnhighlighted.transform.position = transform.position;
        newUnhighlighted.transform.SetSiblingIndex(desired_index);
        Destroy(playObjects[play_index]);
        playObjects[play_index] = newUnhighlighted;
    }
}
