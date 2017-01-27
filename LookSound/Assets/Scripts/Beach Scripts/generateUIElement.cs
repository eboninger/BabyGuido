using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class generateUIElement : MonoBehaviour{

	public GameObject staff;
	public bool grabbed = false;
	public bool inTheZone = false;

	//player grabbed an object, dragged it to the staff and then released
	public void readyToDrop(string name){
		if(inTheZone){
			//staff.GetComponent<Image>().color = Color.cyan;
			print("Dropped " + name);
			if(staff){
				generateChild gc = staff.GetComponent<generateChild>();
				if(gc){
					//if there are less than ten children
					if(staff.transform.childCount < 10){
						//make a new child at the end/back/right
						//staff.GetComponent<Image>().color = Color.cyan;
						gc.makeNewChild(1, name);
					}
				}
			}
		}
	}

	//called by Staff "on pointer enter" and "on pointer exit"
	//the former returns true, the latter false
	public void inDropZone(bool x){
		#if UNITY_EDITOR
		inTheZone = x;
		#else
		//need to see if user actually dropped the object in the zone
		//or moved finger out of the zone
		if(Input.touchCount >= 1){
			if(Input.GetTouch(0).phase != TouchPhase.Ended){
				inTheZone = x;
			}
			
		} 
		#endif

	}

	//called by touch hand when a stationary object is clicked/touched
	public void objectGrabbed(bool x){
		//print("object grabbed: " + x);
		grabbed = x;
	}


}