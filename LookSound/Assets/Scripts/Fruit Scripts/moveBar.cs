using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBar : MonoBehaviour {

	private Vector3 startPos;
	public float moveRate = 1.2f;
	public float rightScreenEdge = 8.1f;

	void Start () {
		startPos = gameObject.transform.position;
	}
	
	void Update () {
		transform.Translate(Vector3.down * Time.deltaTime * moveRate);
		if(transform.position.x > rightScreenEdge){
			transform.position = startPos;
		}
	}
}
