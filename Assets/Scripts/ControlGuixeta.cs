using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGuixeta : MonoBehaviour {

	public int cua;
	public float temps_atencio;
	public Queue persones_esperant;

	// Use this for initialization
	void Start () {

		cua = 0;
		temps_atencio = 2;
		persones_esperant = new Queue();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void aumenta_cua(GameObject persona)
	{
		cua++;
		persones_esperant.Enqueue(persona);
	}

	public void redueix_cua()
	{
		persones_esperant.Dequeue();
		cua--;

		foreach (GameObject obj in persones_esperant)
		{
			obj.GetComponent<PassatgerMVT>().fer_que_avanci();
		}


	}
}
