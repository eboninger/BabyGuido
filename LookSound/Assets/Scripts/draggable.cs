using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


	public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log ("OnBeginDrag");
	}
	public void OnDrag(PointerEventData eventData) {
		//Debug.Log ("OnDrag");
		this.transform.position = eventData.position;
	}
	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log ("OnEndDrag");
	}
		
}