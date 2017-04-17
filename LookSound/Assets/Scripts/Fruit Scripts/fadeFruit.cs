using UnityEngine;
using System.Collections;

public class fadeFruit : MonoBehaviour {
	float minimum = 0.0f;
	float maximum = 1f;
	float duration = 4.0f;
	bool fade = false;
	float startTime;
	float t;
	float transparency;
	SpriteRenderer sprite;

	public void turnOnFade(){
		fade = true;
		startTime = Time.time;
		sprite = GetComponentInChildren<SpriteRenderer>();
	}

	public void turnOffFade(){
		fade = false;
	}
	// Update is called once per frame
	void Update () {
		if(fade){
			t = (Time.time - startTime) / duration;
			//fade the transparency over time
			transparency = Mathf.SmoothStep(maximum, minimum, t);
			sprite.color = new Color(1f, 1f, 1f, transparency);
			//when the object is no longer visible, destory it
			if(transparency <= 0){
				Destroy(gameObject);
			}

		}
	
	}
}
