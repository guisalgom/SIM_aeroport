using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaAvions : MonoBehaviour {

    public float frequencia = 5;
    private float temps;
    private Object avio;
    private bool[] disponibilitat_portes;
    private bool porta_lliure;

    // Use this for initialization
    void Start () {

        temps = 0;

        avio = Resources.Load("obj_avio");

        disponibilitat_portes = new bool[12] { false, false, false, false, false, false, false, false, false, false, false, false };

    }
	
	// Update is called once per frame
	void Update () {

        temps += Time.deltaTime;
        if (temps > frequencia) { make_avions(); temps = 0; }

    }

    void make_avions() {
        GameObject myRoadInstance = Instantiate(avio, new Vector3(Random.Range(130f, 9f), 2.5f, 188), Quaternion.identity) as GameObject;
    }

    bool comprova_disponibilitat() {
        for (int i = 0; i < disponibilitat_portes.Length; i++) {
            if (!disponibilitat_portes[i]) return true;
        }
        return false;
    }

    public int get_porta () {
        int porta_desti = -1;
        print ("disponibilitat " + comprova_disponibilitat());
        if (comprova_disponibilitat()) {
            print("lliure");
            bool trobat = false;
            while (!trobat) { 
                int porta = Mathf.RoundToInt(Random.Range(0, 12));
                if (!disponibilitat_portes[porta])
                {
                    disponibilitat_portes[porta] = true;
                    trobat = true;
                    porta_desti = porta+1;
                }
            }
        }
        return porta_desti;
    }
}
