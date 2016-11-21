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
	private Vector3 birdPos;
	private bool touched = false;
	private bool madeChild;
	private bool draggingChild;
	private Transform parent;
	private Transform child;
	private generateUIElement genUI;



	void Start (){
		//get initial position
		handPos = hand.transform.position;
		madeChild = false;
		draggingChild = false;
		genUI = GetComponent<generateUIElement>();
	}

	// Update is called once per frame
	void Update (){
		//If running game in editor
		#if UNITY_EDITOR
		//Using mouse
			mouseDrag();
		#else
		//using android phone
		//Unity shows this as commented out because we are looking at this while using the editor
			if (Input.touchCount >= 1){
				touchDrag();
			} else{
				touchEnd();
			}

		#endif
	}

	//FixedUpdate is called once per fixed time step
	void FixedUpdate(){
		if (touched){
			//now move the hand
			hand.transform.position = handPos;
			touched = false;
		}
	}


	void moveChild(Transform element, Vector3 handPos){
			element.transform.position = handPos;
	}

	//move the mouse to where the player touches
	void touchDrag() {
		touched = true;	
		Vector3 pos = Input.touches[0].position;
		pos.z = 10;

		Vector3 screenPos = Camera.main.ScreenToWorldPoint(pos);
		RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

		dragAndUpdate(hit);

		if(draggingChild){
			print("dragging child");
			child.position = screenPos;
			//print("child transform: " + screenPos.x + " " + screenPos.y);
		}
		handPos = screenPos;

	}



	//for debugging, use the mouse as input
	void mouseDrag(){
		if(Input.GetMouseButton(0)) {
			touched = true;	
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10;

			Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

			RaycastHit2D hit = Physics2D.Raycast(screenPos,Vector2.zero);

			dragAndUpdate(hit);

			if(draggingChild){
				//print("dragging child");
				child.position = screenPos;
				//print("child transform: " + screenPos.x + " " + screenPos.y);
			}
			handPos = screenPos;

		} else{
			touchEnd();
		}
	}


	void dragAndUpdate(RaycastHit2D hit){
		if(hit){
			print("hit: " + hit.transform.name);
			if(hit.transform.tag == "NotDraggable"){
				//got a parent object that does not move

				parent = hit.transform;
				draggingChild = true;
				if(parent.childCount > 0){
					print("new child assigned");
					child = parent.GetChild(0).transform;
				}
				//replace the drag bird we are now moving with the mouse
				if(!madeChild){
					generateChild genC = parent.GetComponent<generateChild>();
					if(genC != null){
						print("made new child for parent " + parent.name);
						genC.makeNewChild(0);
					}
					madeChild = true;
				}
				genUI.objectGrabbed(true);

			} 
			if(hit.transform.tag == "Staff"){
				print("Staff");
			}
		}
	}

	void touchEnd(){
		draggingChild = false;
		madeChild = false;
		if(genUI.grabbed){
			//we grabbed the item and are now releasing it
			genUI.readToDrop(child.tag);
		}
		genUI.objectGrabbed(false);

		if(child){
			Destroy(child.gameObject);
		}
	}


}