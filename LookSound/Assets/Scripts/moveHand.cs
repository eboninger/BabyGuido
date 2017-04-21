using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveHand : MonoBehaviour {

	public GameObject hand;
	private RectTransform handPos;
	private Vector3 topButtonPos;
	private Vector3 middleButtonPos;
	private Vector3 bottomButtonPos;
	private Vector3[] posArray;
	private int currentPosition;
	const int MAX = 2;
	const int MIN = 0;

	// Use this for initialization
	void Start () {
		posArray = new Vector3[3];
		handPos = hand.GetComponent<RectTransform>();
		topButtonPos = new Vector3(140f, 120f, 0f);
		middleButtonPos = new Vector3(140f, -80f, 0f);
		bottomButtonPos = new Vector3(140f, -280f, 0f);
		posArray[0] = topButtonPos;
		posArray[1] = middleButtonPos;
		posArray[2] = bottomButtonPos;
		currentPosition = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S)){
			//move the hand up to the next button
			moveUp();
			handPos.localPosition = posArray[currentPosition];
			
		} else if(Input.GetKeyDown(KeyCode.A)){
			//move the hand down to the next button
			moveDown();
			handPos.localPosition = posArray[currentPosition];

		} else if(Input.GetKeyDown(KeyCode.Return)){
			//load the level the hand is pointing to
			Application.LoadLevel(currentPosition + 1);
			
		}
	}

	private void moveUp(){
		//change current position and wrap around so that we are 
		//within the array index bounds
		if(currentPosition == MIN){
			currentPosition = MAX;
		} else{
			currentPosition = currentPosition - 1;
		}

		
	}

	private void moveDown(){
		//change current position and wrap around so that we are 
		//within the array index bounds
		if(currentPosition == MAX){
			currentPosition = MIN;
		} else{
			currentPosition = currentPosition + 1;
		}
		
	}
}
