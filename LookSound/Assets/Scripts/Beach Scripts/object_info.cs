using UnityEngine;
using System.Collections;

public class object_info : MonoBehaviour {
    public GameObject unhighlightedChildTemplate;
    public GameObject highlightedChildTemplate;
    public GameObject highlighted;
    public float sizeRatioX;
    public float sizeRatioY;

    // Use this for initialization
    void Start () {
        if (transform.name == "Palmtree")
        {
            sizeRatioX = 0.5f;
            sizeRatioY = 1.0f;
        } else if (transform.name == "Wave")
        {
            sizeRatioX = 1.0f;
            sizeRatioY = 1.0f;
        } else
        {
            sizeRatioX = 1.0f;
            sizeRatioY = 1.0f;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
