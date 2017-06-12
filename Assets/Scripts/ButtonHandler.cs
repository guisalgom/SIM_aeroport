using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public string function;
	private bool pressed;
	private MoveCam cam;

	// Use this for initialization
	void Start () {
		pressed = false;
		cam = GameObject.Find("Main Camera").GetComponent<MoveCam>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		pressed = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (pressed)
		{
			Debug.Log("PULSADO!");
			if (function == "U") { cam.BUp(); }
			if (function == "D") { cam.BDown(); }
			if (function == "L") { cam.BLeft(); }
			if (function == "R") { cam.BRight(); }
			if (function == "Q") { cam.BQ(); }
			if (function == "W") { cam.BW(); }
		}
	}
}
