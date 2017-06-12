using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaAvions : MonoBehaviour {

    public float frequencia = 30;
    private float temps;
    private Object avio;
    public bool[] disponibilitat_portes;
    private bool porta_lliure;

	public int temps_manteniment = 10;
	public int temps_espera_porta = 25;

	private TimeCTRL timectrl;

    // Use this for initialization
    void Start () {

        temps = 0;

        avio = Resources.Load("obj_avio");

        disponibilitat_portes = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

		timectrl = GameObject.Find("Canvas").GetComponent<TimeCTRL>();

    }
	
	// Update is called once per frame
	void Update () {

		if (timectrl.get_aeroport_obert()) { temps += Time.deltaTime; }
		else { temps = 0; }

        if (temps > frequencia & comprova_disponibilitat()) { make_avions(); temps = 0; }

    }

    void make_avions() {
		GameObject myRoadInstance = Instantiate(avio, new Vector3(170f, 23.5f, 282), Quaternion.Euler(270, 0, 0)) as GameObject;
    }

    bool comprova_disponibilitat() {
        for (int i = 0; i < disponibilitat_portes.Length; i++) {
            if (!disponibilitat_portes[i]) return true;
        }
        return false;
    }

    public int get_porta () {
        int porta_desti = -1;
        if (comprova_disponibilitat()) {

			bool trobat = false;
			int maxim = 0;

			for(int i = 0; i < 12; i++) //Prioritat quan hi ha molta gent esperant
			{
				int numero = GameObject.Find("Porta (" + (i + 1).ToString() +")").GetComponent<ControlPorta>().consultar_persones();
				if (!disponibilitat_portes[i] && numero > 50 && numero > maxim)
				{
					maxim = numero;
					trobat = true;
					porta_desti = i;

				}
			}

			if (!trobat) //Segona prioritat: aleatori
			{
				for(int i = 0; i<12; i++)
				{
					int porta = Mathf.RoundToInt(Random.Range(0, 11));
					if (!disponibilitat_portes[porta])
					{
						trobat = true;
						porta_desti = porta;
					}
				}
			}

			if (!trobat) //Tercera prioritat: Si aleatoriament no surt al cap de 12 intents, triar per ordre
			{
				for(int i = 0; i<12; i++)
				{
					if (!disponibilitat_portes[i])
					{
						trobat = true;
						porta_desti = i;
					}
				}
			}
			disponibilitat_portes[porta_desti] = true;
        }

        return porta_desti + 1;
    }

	public void llibera_porta(int n)
	{
		disponibilitat_portes[n - 1] = false;
	}
}
