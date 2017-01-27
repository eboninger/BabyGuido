using UnityEngine;
using System.Collections;

public class generateChild : MonoBehaviour {
	public GameObject[] childTemplates;
	private Transform childTransform;

	//called by the stationary objects
	public void makeNewChild(int index){
		//create a new child
		GameObject newChild = Instantiate(childTemplates[0]);
		newChild.transform.SetParent(transform);
		newChild.transform.position = Vector3.zero;
		if(index == 0){
			newChild.transform.SetAsFirstSibling();
		} 

	}

	//called by the staff
	public void makeNewChild(int index, string name){
		//create a new child
		GameObject newChild;
		foreach(GameObject o in childTemplates){
			if(o.tag == name){
				newChild = Instantiate(o);
				newChild.transform.SetParent(transform);
				if(index == 0){
					newChild.transform.SetAsFirstSibling();
				} 
				newChild.transform.position = transform.position;
			}
		}

	}

}
