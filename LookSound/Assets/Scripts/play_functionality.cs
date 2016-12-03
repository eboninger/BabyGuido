using UnityEngine;
using System.Collections;

public class play_functionality : MonoBehaviour {
    hand_positions hp;
    public GameObject[] playObjects;
    public const int MAX_PLAY_OBJECTS = 20;
    public int total_play_objects;
    public int play_index;
    public GameObject currentHighlighted;
    public Transform hand;

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
        movehand();
    }

    public void moveRight()
    {
        unhighlight();
        play_index = (play_index + 1) % total_play_objects;
        highlight();
        movehand();
    }

    public void space()
    {
        unhighlight();
        Destroy(playObjects[play_index * 2]);
        Destroy(playObjects[(play_index * 2) + 1]);
        total_play_objects--;

        if (total_play_objects == 0)
        {
            hp.inPlay = false;
            hp.toEditMode();
            return;
        }
        else if (play_index == total_play_objects)
        {
            play_index--;
            highlight();
            return;
        }
        

        for (int i = play_index; i < total_play_objects; i++)
        {
            playObjects[i * 2] = playObjects[(i + 1) * 2];
            playObjects[(i * 2) + 1] = playObjects[((i + 1) * 2) + 1];
        }

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

    public void movehand()
    {
        //var rectTrans = playObjects[(play_index * 2) + 1].GetComponent<RectTransform>();
        //Vector3[] corners = new Vector3[4];
        //rectTrans.GetWorldCorners(corners);
        //print(corners[3]);
    }

    public void play()
    {
        unhighlight();
        var old_play_index = play_index;
        StartCoroutine(PlaySoundList());
        play_index = old_play_index;
        highlight();
    }

    IEnumerator PlaySoundList()
    {
        for (int i = 0; i < total_play_objects; i++)
        {
            play_index = i;
            highlight();
            AudioSource audio = playObjects[(i * 2) + 1].GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSecondsRealtime(audio.clip.length);
            unhighlight();
        }

    }
}
