using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPorta : MonoBehaviour {

	public bool hi_ha_avio; //Hi ha un avio al davant de la porta esperant a que pugin passatgers, o no
	public AvoMVT avio;
	public int npersones;

	// Use this for initialization
	void Start () {
		hi_ha_avio = false;
		npersones = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void pujar_passatgers(AvoMVT go_avio)
	{
		hi_ha_avio = true;
		avio = go_avio;
	}

	public void tancar_porta()
	{
		hi_ha_avio = false;
	}

	public void incrementar_persones()
	{
		npersones++;
	}

	public void decrementar_persones()
	{
		npersones--;
	}

	public int consultar_persones()
	{
		return npersones;
	}
}
