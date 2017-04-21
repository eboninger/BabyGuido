using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class fruitWithLocation{
	//the prefab/image for the fruit
	private GameObject o;
	//fruit's position in the game space
	private Vector3 pos;
	private float ypos;
	private float xpos;
	//offset from the begining of the music staff
	public float xoffset = 3.0f; 
	//scales how close or far the fruits are on the x axis
	public float xseparationScale = 2.50f; 
	//scales distance between half steps on the scale
	public float yseparationScale = .65f; 

	public fruitWithLocation(GameObject gameObj, float y, float order){
		o = gameObj;
		ypos = y + (order  * yseparationScale);
		xpos = xoffset + (order * xseparationScale);
		pos = new Vector3(xpos, ypos, 0f);
	}

	public GameObject getObject(){
		return o;
	}

	public Vector3 getPosition(){
		return pos;
	}
}

public class addFruitToStaff : MonoBehaviour {

	//all notes will be relative to the lowest (visually, not in world space) note on the staff
	public float lowestNoteYPos = 8.0f;
	public GameObject apple;
	public GameObject banana;
	public GameObject cherry;
	public GameObject dragonfruit;
	public GameObject eggplant;
	public GameObject fig;
	public GameObject grape;
	private Dictionary<char, fruitWithLocation> fruit;
	// Use this for initialization
	void Start () {
		fruit = new Dictionary<char, fruitWithLocation>();
		fruitWithLocation lowd = new fruitWithLocation(dragonfruit, lowestNoteYPos, 0);
		fruitWithLocation e = new fruitWithLocation(eggplant, lowestNoteYPos, 1);
		fruitWithLocation f = new fruitWithLocation(fig, lowestNoteYPos, 2);
		fruitWithLocation g = new fruitWithLocation(grape, lowestNoteYPos, 3);
		fruitWithLocation a = new fruitWithLocation(apple, lowestNoteYPos, 4);
		fruitWithLocation b = new fruitWithLocation(banana, lowestNoteYPos, 5);
		fruitWithLocation c = new fruitWithLocation(cherry, lowestNoteYPos, 6);
		fruitWithLocation highd = new fruitWithLocation(dragonfruit, lowestNoteYPos, 7.5f);
		fruitWithLocation highe = new fruitWithLocation(eggplant, lowestNoteYPos, 8.5f);
		fruitWithLocation highf = new fruitWithLocation(fig, lowestNoteYPos, 9.5f);
		fruit.Add('a', lowd);
		fruit.Add('s', e);
		fruit.Add('d', f);
		fruit.Add('f', g);
		fruit.Add('g', a);
		fruit.Add('h', b);
		fruit.Add('j', c);
		fruit.Add('k', highd);
		fruit.Add('l', highe);
		fruit.Add(';', highf);
	
	}
	
	// Update is called once per frame
	void Update () {
		keyPressed();
	}

	void keyPressed(){
		if (Input.anyKeyDown){
			foreach (char c in Input.inputString){
				createObject(c);
			}
		}
	}

	//instantiate a new fruit prefab based on the keyboard key the user pressed
	void createObject(char c){
		GameObject newObj;
		try {
			newObj = Instantiate(fruit[c].getObject());
			//move it to the correct position on the staff
			newObj.transform.position = fruit[c].getPosition();
			fadeOn(newObj);
		} catch (Exception e) {
			print("Error: no fruit associated with " + c.ToString());
		}  
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
