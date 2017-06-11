using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassatgerMVT : MonoBehaviour {

	// Use this for initialization

	private int guixeta_numero;
	private ControlGuixeta guixeta_id;
	public float speed = 10f;
	private float cua; //Distancia de la cua a la qual s'ha de posicionar
	public int ncua; //Posicio exacta dintre de l'ordre de la cua
	private float temps_atenent; //El temps que porta sent ates a la guixeta
	private bool pot_avansar; //Si desde la guixeta se li ha dit que pot avansar o no
	private int nova_pos_cua; //La nova posicio que adoptara en la cua quan hagi d'avansar

	private float esquiva_guixeta_x = 1;
	private float esquiva_guixeta_z = 3;

	public int porta_numero;
	private ControlPorta porta_id;
	private float porta_dec_x;
	private float porta_dec_z;

	private AvoMVT avio_id;

	public bool papers_en_regla; //True si te papers en regla. False si no.
	public int rate_papers_regla = 99; //% de gent que te els papers en regla

	public int estat = 0;
	//Estats:
	//0 -> anant cap a la seva guixeta
	//1 -> ha agafat lloc a la cua
	//2 -> esta esperant dintre de la cua
	//3 -> mentre espera la cua avanca una mica
	//4 -> esta sent ates per l'encarregat de la guixeta
	//5 -> Traspassar la guixeta
	//6 -> Anar a la porta d'embarcament
	//7 -> Esperar a que l'avio arribi a la porta d'embarcament i quan hi sigui i ho permeti, pujar-hi

	void Start () {

		guixeta_numero = Mathf.RoundToInt(Random.Range(1, 22));
		guixeta_id = GameObject.Find("Guixeta (" + guixeta_numero.ToString() + ")").GetComponent<ControlGuixeta>();
		temps_atenent = 0;
		pot_avansar = false;

		porta_numero = Mathf.RoundToInt(Random.Range(1, 12));
		porta_id = GameObject.Find("Porta (" + porta_numero.ToString() + ")").GetComponent<ControlPorta>();
		porta_dec_x = Random.Range(-10, 10);
		porta_dec_z = Random.Range(-10, 10);

		papers_en_regla = true;
		if (Random.Range(0,100) > rate_papers_regla) { papers_en_regla = false; }


	}
	
	// Update is called once per frame
	void Update () {

		Vector3 guixeta_pos = guixeta_id.transform.position;
		Vector3 my_pos = transform.position;

		if (estat == 0) //Anar a la seva guixeta
		{
			anar_a_la_cua(guixeta_id.cua); //Punt: La ultima posicio de la cua

			Vector3 pos_final = new Vector3(guixeta_pos.x, guixeta_pos.y, guixeta_pos.z - cua);
			if (Vector3.Distance(pos_final, my_pos) < 1 && ncua == guixeta_id.cua) { estat = 1; guixeta_id.aumenta_cua(gameObject); }
		}
		else
		if (estat == 1) //Agafar el seu lloc a la cua
		{
			transform.position = new Vector3(my_pos.x, my_pos.y, guixeta_pos.z - cua);
			estat = 2;
		}
		else
		if (estat == 2) //Esperant a la cua (quiet)
		{
			if (pot_avansar) //Si reb una crida desde la guixeta dient que pot avansar 1 posicio
			{
				pot_avansar = false;
				nova_pos_cua = ncua - 1;
				estat = 3; //Si la cua ha avançat, pasar a l'estat de caminar
			}

			if (ncua == 0)
			{
				estat = 4;
			}
		}
		else
		if (estat == 3) //Avançar 1 posicio dintre de la cua
		{
			anar_a_la_cua(nova_pos_cua); //Punt: Posicio actual menys la distancia guanyada ncua - (ncua - guixeta_id.cua)
			Vector3 pos_final = new Vector3(guixeta_pos.x, guixeta_pos.y, guixeta_pos.z - cua);
			if (Vector3.Distance(pos_final, my_pos) < 1) { estat = 2; } //Si ja has arribat al teu nou lloc, pasar a l'estat d'aturat
		}
		else
		if (estat == 4) //Sent ates per l'encarregat de la guixeta
		{
			temps_atenent += Time.deltaTime;
			if (temps_atenent > guixeta_id.temps_atencio)
			{
				guixeta_id.redueix_cua();

				if (papers_en_regla)
				{
					estat = 5;
				}
				else
				{
					estat = -1;
				}
							
			}
		}
		else
		if (estat == 5) //Traspassar la guixeta
		{
			//Destroy(gameObject);
			if (esquiva_guixeta_x > 0)
			{
				float ttt = Time.deltaTime*speed;
				esquiva_guixeta_x -= ttt;
				transform.position = new Vector3 (transform.position.x + ttt, transform.position.y, transform.position.z);
			}
			else
			{
				if (esquiva_guixeta_z > 0)
				{
					float ttt = Time.deltaTime*speed;
					esquiva_guixeta_z -= ttt;
					transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + ttt);
				}
				else
				{
					estat = 6;
				}
			}
		}
		else
		if (estat == 6) //Anar fins a la porta d'embarcament
		{
			//transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + Time.deltaTime*speed);
			anar_a_la_porta();
			Vector3 porta_pos = porta_id.transform.position;
			Vector3 posicio_aparcar = new Vector3(porta_pos.x + porta_dec_x, transform.position.y, porta_pos.z + porta_dec_z);
			if (Vector3.Distance(transform.position, posicio_aparcar) < 1) { estat = 7; }
		}
		else
		if (estat == 7) //Esperar a que arribi el seu avio i despres pujar-hi quan ho permeti
		{
			if (porta_id.hi_ha_avio)
			{
				avio_id = porta_id.avio;
				anar_a_lavio();
				if (Vector3.Distance(transform.position, avio_id.transform.position) < 1) { Destroy(gameObject); }
			}
		}
		else
		if (estat == -1)
		{
			if (esquiva_guixeta_x > 0)
			{
				float ttt = Time.deltaTime*speed;
				esquiva_guixeta_x -= ttt;
				transform.position = new Vector3 (transform.position.x + ttt, transform.position.y, transform.position.z);
			}
			else
			{
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - Time.deltaTime*speed);
				if (transform.position.z < -41) { Destroy(gameObject); }
			}
		}

	}

	void anar_a_la_cua(int valor_cua)
	{
		Vector3 guixeta_pos = guixeta_id.transform.position;
		Vector3 my_pos = transform.position;

		ncua = valor_cua; //Numero de posicio de la cua a la qual ens volem posicionar
		cua = ncua*2 + 2; //allargada de la cua en unitats de distancia (fins a la nostra posicio)
		if (guixeta_pos.x < my_pos.x)       { my_pos = new Vector3(my_pos.x - speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (guixeta_pos.x > my_pos.x)       { my_pos = new Vector3(my_pos.x + speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (guixeta_pos.z - cua < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed*Time.deltaTime); }
		if (guixeta_pos.z - cua > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed*Time.deltaTime); }
		transform.position = my_pos;
	}

	public void fer_que_avanci()
	{
		pot_avansar = true;
	}


	void anar_a_la_porta()
	{
		Vector3 porta_pos = porta_id.transform.position;
		Vector3 my_pos = transform.position;

		if (porta_pos.x + porta_dec_x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (porta_pos.x + porta_dec_x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (porta_pos.z + porta_dec_z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed*Time.deltaTime); }
		if (porta_pos.z + porta_dec_z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed*Time.deltaTime); }
		transform.position = my_pos;
	}

	void anar_a_lavio()
	{
		Vector3 avio_pos = avio_id.transform.position;
		Vector3 my_pos = transform.position;

		if (avio_pos.x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (avio_pos.x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed*Time.deltaTime, my_pos.y, my_pos.z); }
		if (avio_pos.z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed*Time.deltaTime); }
		if (avio_pos.z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed*Time.deltaTime); }
		transform.position = my_pos;
	}

}
