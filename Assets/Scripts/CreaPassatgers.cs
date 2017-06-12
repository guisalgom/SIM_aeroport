using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaPassatgers : MonoBehaviour {

	public float frequencia = 2;
	public int quantitat = 4;
	private float temps;
	private Object passatger;
	public int temps_atenciog = 2;

	private TimeCTRL timectrl;
	public int rate_papers_regla = 99;

	// Use this for initialization
	void Start () {

		temps = 0;

		passatger = Resources.Load("obj_Passatger");
		timectrl = GameObject.Find("Canvas").GetComponent<TimeCTRL>();



	}
	
	// Update is called once per frame
	void Update () {


		if (timectrl.get_aeroport_obert()) { temps += Time.deltaTime; }
		else { temps = 0; }

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
