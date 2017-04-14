using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBar : MonoBehaviour {

	private Vector3 startPos;
    private bool moving = false;
	private float moveRate = 3.5f;
	public float rightScreenEdge = 8.1f;

	void Start () {
		startPos = gameObject.transform.position;
	}
	
	void Update () {
        if (moving) {
            transform.Translate(Vector3.down * Time.deltaTime * moveRate);
            if (transform.position.x > rightScreenEdge) {
                transform.position = startPos;
            }
        }
	}

    // continue bar movement from whatever its current position is
    public void beginMovement() {
        moving = true;
    }

    // stop the bar's movement and return it to the beginning of staff
    public void endMovement() {
        moving = false;
        transform.position = startPos;
    }
}
