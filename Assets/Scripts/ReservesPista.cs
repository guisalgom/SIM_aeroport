using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReservesPista : MonoBehaviour {

	public bool reservat = false;

	// Use this for initialization
	void Start () {
		reservat = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reservar()
	{
		reservat = true;
	}

	public void lliberar()
	{
		reservat = false;
	}

	public bool is_free()
	{
		return !reservat;
	}
}
