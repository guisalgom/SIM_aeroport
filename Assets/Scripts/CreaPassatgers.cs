using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaPassatgers : MonoBehaviour {

	public float frequencia = 2;
	public int quantitat = 4;
	private float temps;
	private Object passatger;

	// Use this for initialization
	void Start () {

		temps = 0;

		passatger = Resources.Load("obj_Passatger");



	}
	
	// Update is called once per frame
	void Update () {

		temps += Time.deltaTime;
		if (temps > frequencia) { make_passatgers(); temps = 0; }

	}

	void make_passatgers()
	{

		for(int i = 0; i < quantitat; i++)
		{
			GameObject myRoadInstance = Instantiate(passatger, new Vector3(Random.Range(1.5f, 30.5f), 1.5f, -40), Quaternion.identity) as GameObject;
		}
	}
}
