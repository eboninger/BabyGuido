using UnityEngine;
using System.Collections;

public class play_functionality : MonoBehaviour {
    hand_positions hp;
    public GameObject[] playObjects;
    public bool playing;
    public const int MAX_PLAY_OBJECTS = 16; // must be even
    public int total_play_objects;
    public int play_index;
    public GameObject currentHighlighted;
    public float[] widths;
    public Transform hand;
    public float screen_offset;

    // Use this for initialization
    void Start () {
        hp = GameObject.Find("Unhighlighted").GetComponent<hand_positions>();
        playObjects = new GameObject[MAX_PLAY_OBJECTS];
        widths = new float[MAX_PLAY_OBJECTS / 2];
        total_play_objects = 0;
        play_index = 0;
        playing = false;

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        screen_offset = (edgeVector.x * 2) * 0.1f;
        print("SCREEN OFFSET " + screen_offset);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addToPlayPanel()
    {
        if (total_play_objects >= (MAX_PLAY_OBJECTS / 2))
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

        var bc = newChild.GetComponent<BoxCollider2D>();
        widths[total_play_objects] = bc.size.x;

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
        var sum = 0.0f;
        for (int i = 0; i <= play_index; i++)
        {
            sum += widths[i];
        }


        var new_pos = new Vector3(sum + screen_offset, 1.0f);
        hand.position = new_pos;
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
        playing = true;
        for (int i = 0; i < total_play_objects; i++)
        {
            play_index = i;
            highlight();
            movehand();
            AudioSource audio = playObjects[(i * 2) + 1].GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSecondsRealtime(audio.clip.length);
            unhighlight();
            if (i == (total_play_objects-1))
            {
                if (hp.inPlay)
                {
                    movehand();
                }
                else
                {
                    hp.movehand();                    
                }
                playing = false;
            }
        }

    }
}
