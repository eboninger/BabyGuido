using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class rhythmFruit : MonoBehaviour {

	public GameObject vertBar;
	public GameObject horzBar;
	public GameObject apple;
	public GameObject banana;
	public GameObject cherry;
	public GameObject dragonfruit;
	public GameObject eggplant;
	public GameObject fig;
	public GameObject grape;
	private float yAxis;

	private Dictionary<char, GameObject> fruit;

	void Start () {
		yAxis = horzBar.transform.position.y;
		fruit = new Dictionary<char, GameObject>();
		fruit.Add('a', dragonfruit);
		fruit.Add('s', eggplant);
		fruit.Add('d', fig);
		fruit.Add('f', grape);
		fruit.Add('g', apple);
		fruit.Add('h', banana);
		fruit.Add('j', cherry);
		fruit.Add('k', dragonfruit);
	}
	

    // simulate a key press with inputString of in_str
    public void handle_key_press(string in_str)
    {
        foreach (char c in in_str)
        {
            createFruit(c);
        }
    }

    void createFruit(char c){
		GameObject newObj;
		try {
			newObj = Instantiate(fruit[c]);
			//move it to the correct position on the staff
			newObj.transform.position = calculatePosition();
			fadeOn(newObj);
		} catch (Exception e) {
			print("Error: no fruit associated with " + c.ToString());
		}  
	}

	Vector3 calculatePosition(){
		return new Vector3(vertBar.transform.position.x, yAxis, 0f);

	}

	//make the fruit fade over time
	void fadeOn(GameObject newObj){
		fadeFruit ff = newObj.GetComponent<fadeFruit>();
		if(ff != null){
			ff.turnOnFade();
		} else{
			print("Error: no fade fruit script attached to " + newObj.name);
		}
	}
}
