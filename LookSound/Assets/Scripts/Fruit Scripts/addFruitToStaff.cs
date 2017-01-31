using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class fruitWithLocation{
	private GameObject o;
	private Vector3 pos;

	public fruitWithLocation(GameObject gameObj, Vector3 location){
		o = gameObj;
		pos = location;
	}

	public GameObject getObject(){
		return o;
	}

	public Vector3 getPosition(){
		return pos;
	}
}

public class addFruitToStaff : MonoBehaviour {
	
	public GameObject apple;
	public GameObject banana;
	public GameObject cherry;
	public GameObject date;
	public GameObject eggplant;
	private Dictionary<char, fruitWithLocation> fruit;
	// Use this for initialization
	void Start () {
		fruit = new Dictionary<char, fruitWithLocation>();
		fruitWithLocation a = new fruitWithLocation(apple, new Vector3 (10f,13.33f,0f));
		fruitWithLocation b = new fruitWithLocation(banana, new Vector3 (8.6f,9.5f,0f));
		fruitWithLocation c = new fruitWithLocation(cherry, new Vector3 (11.6f,10f,0f));
		fruit.Add('a', a);
		fruit.Add('b', b);
		fruit.Add('c', c);
	
	}
	
	// Update is called once per frame
	void Update () {
		keyPressed();
	}

	void keyPressed(){
		if (Input.anyKeyDown)
		{
			foreach (char c in Input.inputString)
			{
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
		}
		catch (Exception e) {
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
