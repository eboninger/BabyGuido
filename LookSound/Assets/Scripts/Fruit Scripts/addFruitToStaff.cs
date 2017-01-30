using UnityEngine;
using System.Collections;

public class addFruitToStaff : MonoBehaviour {
	
	public GameObject apple;
	private Vector3[] locationsOnStaff = new [] { new Vector3(10f,13.33f,0f), 
												  new Vector3(1f,1f,1f) };
	// Use this for initialization
	void Start () {
	
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
				if(c == 'a'){
					//make a new apple
					GameObject newA = Instantiate(apple);
					//move it to the correct position on the staff
					newA.transform.position = locationsOnStaff[0];
					//make the apple fade over time
					fadeFruit ff = newA.GetComponent<fadeFruit>();
					if(ff != null){
						ff.turnOnFade();
					} else{
						print("Error: no fade fruit script attached to " + newA.name);
					}

				}
			}
		}
//		if (Input.GetKeyDown(KeyCode.A)){
//			
//		}
	}

	//check to see what key is pressed

	//add a fruit to the staff

	//see where the fruit needs to be placed
}
