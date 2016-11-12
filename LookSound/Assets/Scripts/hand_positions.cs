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
    string current = "PalmtreeH";

    

	// Use this for initialization
	void Start () {
        possible_positions.Add("Palmtree", tree_coord);
        possible_positions.Add("Lightning", lightning_coord);
        possible_positions.Add("Wind", wind_coord);

        Transform hand = GameObject.Find("Hand").transform;
        var highlighted = GameObject.Find("PalmtreeH");

        Vector3 starting_pos = new Vector3(possible_positions["Palmtree"].x, possible_positions["Palmtree"].y);
        hand.position = starting_pos;
        //highlighted.renderer.

	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("right"))
        {

        }
	}
}
