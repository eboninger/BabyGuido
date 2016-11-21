using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class generateUIElement : MonoBehaviour{

	public GameObject staff;
	public bool grabbed = false;
	public bool inTheZone = false;


	//player grabbed an object, dragged it to the staff and then released
	public void readToDrop(string name){
		if(inTheZone){
			print("Dropped " + name);
			if(staff){
				generateChild gc = staff.GetComponent<generateChild>();
				if(gc){
					//if there are less than ten children
					if(staff.transform.childCount < 10){
						//make a new child at the end/back/right
						gc.makeNewChild(1);
					}
				}
			}
		}
	}

	//called by Staff "on pointer enter" and "on pointer exit"
	//the former returns true, the latter false
	public void inDropZone(bool x){
		//print("mouse over staff: " + x);
		inTheZone = x;
	}

	//called by touch hand when a stationary object is clicked/touched
	public void objectGrabbed(bool x){
		//print("object grabbed: " + x);
		grabbed = x;
	}

}