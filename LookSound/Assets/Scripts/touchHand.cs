using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Moves the hand to where the player clicks/touches
public class touchHand : MonoBehaviour {
	//Public Variables
	public GameObject hand;
	public GameObject[] draggables;

	//Private Variables
	private Vector3 handPos;
	private bool touched = false;


	void Start (){
		//get initial position
		handPos = hand.transform.position;
		int i = 0;
		foreach(GameObject o in draggables){
			if(o != null){
				i++;
			}
		}
	}

	// Update is called once per frame
	void Update (){
		//If running game in editor
		#if UNITY_EDITOR
		//Using mouse
		mouseDrag();

		#endif
		//Using phone/tablet touch
		if (Input.touchCount >= 1){
			touchDrag();
		}
	}

	//FixedUpdate is called once per fixed time step
	void FixedUpdate(){
		if (touched){
			//now move the objects to their new position
			hand.transform.position = handPos;
			touched = false;
		}
	}


	//move the mouse to where the player touches
	void touchDrag() {
		Touch touch = Input.touches[0];
		Vector3 pos = touch.position;
		pos.z = 10;

		Vector3 screenPos = Camera.main.ScreenToWorldPoint(pos);

		RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

		if(hit){
			handPos = screenPos;
			touched = true;	
		}
	}

	//for debugging, use the mouse as input
	void mouseDrag(){
		
		if(Input.GetMouseButton(0)) {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10;

			Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

			RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

			if(hit){
				print (hit.collider.name);
			}
			handPos = screenPos;
		}
		touched = true;	
	}


}