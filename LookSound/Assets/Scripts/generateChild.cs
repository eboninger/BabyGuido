using UnityEngine;
using System.Collections;

public class generateChild : MonoBehaviour {
	public GameObject childTemplate;

	private GameObject myChild;
	private Transform childTransform;

	// Use this for initialization
	void Start () {
		childTransform = transform.GetChild(0);
		myChild = childTransform.gameObject;
	}

	public void makeNewChild(){
		//create a new child
		GameObject newChild = Instantiate(childTemplate);
		newChild.transform.SetParent(transform);
	}
}
