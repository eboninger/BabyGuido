using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coordinate : MonoBehaviour
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

    Dictionary<string, Coordinate> possible_positions = new Dictionary<string, Coordinate>();

    Coordinate tree_coord = new Coordinate(-5.428f, -2.46f);
    Coordinate lightning_coord = new Coordinate(5.2f, 0.71f);
    Coordinate wind_coord = new Coordinate(-1.38f, 0.61f);
    Coordinate wave_coord = new Coordinate(6.04f, -3.34f);
    Coordinate bird_coord = new Coordinate(-0.4f, -4.59f);
    string currentH;
    string current;
    int currentI;

    

	// Use this for initialization
	void Start () {
        possible_positions.Add("Palmtree", tree_coord);
        possible_positions.Add("Lightning", lightning_coord);
        possible_positions.Add("Wind", wind_coord);
        possible_positions.Add("Wave", wave_coord);
        possible_positions.Add("Bird", bird_coord);
        current = "Palmtree";
        currentH = "PalmtreeH";
        currentI = 0;

        movehand();
        highlight();
	}

    void movehand()
    {
        Transform hand = GameObject.Find("Hand").transform;
        Vector3 new_pos = new Vector3(possible_positions[current].x, possible_positions[current].y);
        hand.position = new_pos;
    }

    void highlight()
    {
        var highlighted = GameObject.Find(currentH);
        var layer = highlighted.GetComponent<SpriteRenderer>();
        layer.sortingLayerName = "Foreground";
    }

    void unhighlight()
    {
        var highlighted = GameObject.Find(currentH);
        var layer = highlighted.GetComponent<SpriteRenderer>();
        layer.sortingLayerName = "Default";
    }

    void updateHandAndMouse(int offset)
    {
        unhighlight();
        var highlightable = GameObject.Find("Unhighlighted").transform;
        currentI = (currentI + offset + highlightable.childCount) % highlightable.childCount;
        var nextTransform = highlightable.GetChild(currentI);
        current = nextTransform.name;
        currentH = current + "H";
        highlight();
        movehand();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("right"))
            updateHandAndMouse(1);           
        if (Input.GetKeyDown("left"))
            updateHandAndMouse(-1);
	}
}
