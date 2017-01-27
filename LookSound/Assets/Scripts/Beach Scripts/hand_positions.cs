using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coordinate
{
    public float x;
    public float y;

    public Coordinate(float new_x, float new_y)
    {
        x = new_x;
        y = new_y;
    }
}


public class hand_positions : MonoBehaviour {

    public int MAX_OBJECTS = 6; // total possible objects
    public Coordinate[] possible_positions;
    public GameObject[] soundObjects;  // array of all sound objects
    public int totalSoundObjects;  // number of sound objects in soundObjects array
    public play_functionality pf;
    public int currentI;     // index in array of current highlighted member
    public Transform hand;
    public bool inPlay;

    

	// Use this for initialization
	void Start () {
        totalSoundObjects = 0;
        soundObjects = new GameObject[MAX_OBJECTS];
        possible_positions = new Coordinate[MAX_OBJECTS];
        inPlay = false;

        int i = 0;

        // add all sound objects on screen to the array
        foreach (Transform child in transform)
        {
            soundObjects[i] = child.gameObject;          
            add_possible_position(child.gameObject);
            i++;
        }

        currentI = 0;
        pf = GameObject.Find("Panel").GetComponent<play_functionality>();

        //add_possible_positions();
        movehand();
        highlight();
	}

    public void add_possible_position(GameObject go)
    {
        var collider = go.GetComponent<BoxCollider2D>();
        var oi = go.GetComponent<object_info>();

        var x_pos = (go.transform.position.x * oi.sizeRatioX) - collider.offset.x + (collider.size.x / 2.0f);
        var y_pos = (go.transform.position.y * oi.sizeRatioY) + collider.offset.y - (collider.size.y / 2.0f);
        possible_positions[totalSoundObjects] = new Coordinate(x_pos, y_pos);
        totalSoundObjects++;
    }


    // move the hand to the position of 'current'
    public void movehand()
    {
        var new_pos = new Vector3(possible_positions[currentI].x, possible_positions[currentI].y);
        hand.position = new_pos;
    }

    // highlight the object at index 'currentI'
    void highlight()
    {
        if (currentI < MAX_OBJECTS)
        {
            var pre_inf = soundObjects[currentI].GetComponent<object_info>();
            pre_inf.highlighted.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        }
        
    }

    // unhighlight the object at index 'currentI'
    void unhighlight()
    {
        if (currentI < MAX_OBJECTS)
        {
            var pre_inf = soundObjects[currentI].GetComponent<object_info>();
            pre_inf.highlighted.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }
        
    }

    // update hand and mouse after adding offset to currentI to find next array position
    void updateHandAndMouse(int offset)
    {
        unhighlight();
        currentI = (currentI + offset + totalSoundObjects) % totalSoundObjects;
        print("CURRENTI = " + currentI + " and TOTALSOUNDOBJECTS = " + totalSoundObjects);
        highlight();
        movehand();
    }
	
	// Update is called once per frame
	void Update () {
        if (!pf.playing)
        {
            if (Input.GetKeyDown("right"))
            {
                if (inPlay)
                    pf.moveRight();
                else
                    updateHandAndMouse(1);
            }
            if (Input.GetKeyDown("left"))
            {
                if (inPlay)
                    pf.moveLeft();
                else
                    updateHandAndMouse(-1);
            }
            if (Input.GetKeyDown("space"))
            {
                if (inPlay)
                    pf.space();
                else
                    pf.addToPlayPanel();
            }
            if ((Input.GetKeyDown("down") || Input.GetKeyDown("up")) && (pf.total_play_objects > 0))
            {
                inPlay = !inPlay;
                if (inPlay)
                {
                    toPlayMode();
                }
                else
                {
                    toEditMode();
                }
            }
            if (Input.GetKeyDown("p"))
            {
                pf.play();
            }
        }        
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
	}

    public void toPlayMode()
    {
        unhighlight();
        //hand.gameObject.SetActive(false);
        pf.highlight();
        pf.movehand();
    }

    public void toEditMode()
    {
        pf.unhighlight();
        //hand.gameObject.SetActive(true);
        highlight();
        movehand();
    }


}
