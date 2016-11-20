using UnityEngine;
using System.Collections;

public class generateChild : MonoBehaviour {
	public GameObject childTemplate;
	private Transform childTransform;

	public void makeNewChild(int index){
		//create a new child
		GameObject newChild = Instantiate(childTemplate);
		newChild.transform.SetParent(transform);
		if(index == 0){
			newChild.transform.SetAsFirstSibling();
		} 
		newChild.transform.position = transform.position;
	}

}
