using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveCam : MonoBehaviour {

	public int vel = 100;


	// Use this for initialization
	void Start () {
		
	}



	// Update is called once per frame
	void Update () {

		if (Input.GetKey(KeyCode.LeftArrow)) { BLeft(); }
		if (Input.GetKey(KeyCode.RightArrow)) { BRight(); }
		if (Input.GetKey(KeyCode.UpArrow)) { BUp(); }
		if (Input.GetKey(KeyCode.DownArrow)) { BDown(); }

		if (Input.GetKey(KeyCode.Q)) { BQ(); }
		if (Input.GetKey(KeyCode.W)) { BW(); }

	

	}

	public void BLeft()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime*vel/Time.timeScale);
	}

	public void BRight()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime*vel/Time.timeScale);
	}

	public void BUp()
	{
		transform.position = new Vector3(transform.position.x - Time.deltaTime*vel/Time.timeScale, transform.position.y, transform.position.z);
	}

	public void BDown()
	{
		transform.position = new Vector3(transform.position.x + Time.deltaTime*vel/Time.timeScale, transform.position.y, transform.position.z);
	}

	public void BQ()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime*vel/Time.timeScale, transform.position.z);
	}

	public void BW()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime*vel/Time.timeScale, transform.position.z);
	}
}
