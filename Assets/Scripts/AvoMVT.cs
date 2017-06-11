using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoMVT : MonoBehaviour {

    int porta;
    private GameObject controlador;
    private GameObject manteniment;
    private GameObject hangar;
    private ControlPorta porta_id;
    private float temps;
    private float temps_manteniment;
    public float temps_manteniment_min;
    public float temps_manteniment_max;
    private float porta_dec_x;
    private float porta_dec_z;
    private float manteniment_dec_x;
    private float manteniment_dec_z;
    private float hangar_dec_x;
    private float hangar_dec_z;
    public float speed = 10f;
    public int estat = 0;

    // Use this for initialization
    void Start () {

        controlador = GameObject.FindGameObjectWithTag("GameController");
        manteniment = GameObject.FindGameObjectWithTag("Manteniment");
        hangar = GameObject.FindGameObjectWithTag("Hangar");

        temps = 0;
        temps_manteniment_min = 5f;
        temps_manteniment_max = 10f;
        temps_manteniment = Random.Range(temps_manteniment_min, temps_manteniment_max+1);

    }
	
	// Update is called once per frame
	void Update () {
        
        print("estat " + estat);
        if(estat == 0) { //Anar a Manteniment
            
            manteniment_dec_x = Random.Range(-30, 30);
            manteniment_dec_z = Random.Range(-30, 30);
            anar_a_manteniment();
            
        }

        else if (estat == 1) { //Rebre manteniment
            temps += Time.deltaTime*speed;
            if (temps >= temps_manteniment) estat = 2;
        }

        else if (estat == 2) { //Anar a Hangar

            hangar_dec_x = Random.Range(-50,50);
            hangar_dec_z = Random.Range(-5, 50);
            anar_a_hangar();
        }

        else if (estat == 3) { //Buscar porta
            porta = controlador.GetComponent<CreaAvions>().get_porta();
            print("porta " + porta);
            if (porta != -1) {
                porta_id = GameObject.Find("Porta (" + porta.ToString() + ")").GetComponent<ControlPorta>();
                if (porta > 6)
                {
                    porta_dec_x = 0;
                    porta_dec_z = 12;
                }
                else {
                    porta_dec_x = 12;
                    porta_dec_z = 0;

                }
                    estat = 4;
            }
        }

        else if (estat == 4) { //Anar a porta
            anar_a_la_porta();
			Vector3 porta_pos = porta_id.transform.position;
			Vector3 posicio_aparcar = new Vector3(porta_pos.x + porta_dec_x, transform.position.y, porta_pos.z + porta_dec_z);
			if (Vector3.Distance(transform.position, posicio_aparcar) < 1)
			{
				porta_id.pujar_passatgers(this);
				//estat = 5;
			}

        }

		///////////////////
		/// ///////////////
		/// LA FUNCIO PUJAR_PASSATGERS() porta_id.pujar_passatgers(this); ES CRIDA QUAN L'AVIO ESTIGUI LLEST PER QUE LA GENT HI PUJI
		/// LA IDEA ES QUE UN COP L'AVIO ARRIBI A LA PORTA D'EMBARCAMENT ESPERI UNA ESTONA ABANS D'ACEPTAR GENT.
		/// 
		/// 
		/// QUAN L'AVIO JA NO ADMETI MES PASSATGERS HA DE CRIDAR LA FUNCIO TANCAR_PORTA() porta_id.tancar_porta(); PER QUE LA GENT DEIXI DE PUJAR.
		/// LA IDEA ES QUE TANQUI LA PORTA I DESPRES VAGI A LA PISTA D'ENLAIRAMENT
		/// ///////////////
		/// ///////////////
       
	}

    void Ocupacio(int porta_desti) {
        porta = porta_desti;
    }

    void anar_a_la_porta()
    {
        Vector3 porta_pos = porta_id.transform.position;
        Vector3 my_pos = transform.position;

        if (porta_pos.x + porta_dec_x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (porta_pos.x + porta_dec_x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (porta_pos.z + porta_dec_z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed * Time.deltaTime); }
        if (porta_pos.z + porta_dec_z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed * Time.deltaTime); }
        transform.position = my_pos;
    }

    void anar_a_manteniment() {
        Vector3 manteniment_pos = manteniment.transform.position;
        Vector3 my_pos = transform.position;

        if (manteniment_pos.x + manteniment_dec_x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (manteniment_pos.x + manteniment_dec_x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (manteniment_pos.z + manteniment_dec_z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed * Time.deltaTime); }
        if (manteniment_pos.z + manteniment_dec_z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed * Time.deltaTime); }
        transform.position = my_pos;

        Vector3 posicio_aparcar = new Vector3(manteniment_pos.x + manteniment_dec_x, transform.position.y, manteniment_pos.z + manteniment_dec_z);
        if (Vector3.Distance(transform.position, posicio_aparcar) < 1) { estat = 1; }
    }

    void anar_a_hangar() {
        Vector3 hangar_pos = hangar.transform.position;
        Vector3 my_pos = transform.position;

        if (hangar_pos.x + hangar_dec_x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (hangar_pos.x + hangar_dec_x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed * Time.deltaTime, my_pos.y, my_pos.z); }
        if (hangar_pos.z + hangar_dec_z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed * Time.deltaTime); }
        if (hangar_pos.z + hangar_dec_z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed * Time.deltaTime); }
        transform.position = my_pos;

        Vector3 posicio_aparcar = new Vector3(hangar_pos.x + hangar_dec_x, transform.position.y, hangar_pos.z + hangar_dec_z);
        if (Vector3.Distance(transform.position, posicio_aparcar) < 1) { estat = 3; }
    }
}
