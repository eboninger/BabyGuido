using UnityEngine;
using System.Collections;

public class touchscreen : MonoBehaviour {

	void Update () {

	}

	void OnGUI(){
		//Gives us information about input touches
		foreach(Touch touch in Input.touches){
			string message = "";
			message += "ID: " + touch.fingerId + "\n";
			message += "Phase: " + touch.phase.ToString() + "\n";
			message += "TapCount: " + touch.tapCount + "\n";
			message += "Pos X: " + touch.position.x + "\n";
			message += "Pos Y: " + touch.position.y + "\n";

			int num = touch.fingerId;
			GUI.Label(new Rect (0 + 130 * num, 0, 120, 100), message);
		}
	}
}
