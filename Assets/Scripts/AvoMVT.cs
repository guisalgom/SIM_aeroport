using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoMVT : MonoBehaviour {

    int porta;
    private GameObject controlador;
    private GameObject manteniment;
    private GameObject hangar;
    private ControlPorta porta_id;
	private GameObject pistaE;
	private ReservesPista pistaE2;
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
    public int estat = -1;
    public int passatgers;
	public int temps_espera_porta;

	private CreaAvions creador;

	public float espera_hora_marxar = 0;
	public bool hora_de_marxar = false;

    // Use this for initialization
    void Start () {

		espera_hora_marxar = 0;
		hora_de_marxar = false;
        controlador = GameObject.FindGameObjectWithTag("GameController");
        manteniment = GameObject.FindGameObjectWithTag("Manteniment");
        hangar = GameObject.FindGameObjectWithTag("Hangar");
		pistaE = GameObject.Find("PistaEnlairament");
		pistaE2 = pistaE.GetComponent<ReservesPista>();
		creador = GameObject.Find("ControlGlobal").GetComponent<CreaAvions>();
		estat = -1;
        temps = 0;
        passatgers = 0;
        temps_manteniment_min = 5f;
        temps_manteniment_max = 10f;
		temps_manteniment = GameObject.Find("ControlGlobal").GetComponent<CreaAvions>().temps_manteniment;

		temps_espera_porta = GameObject.Find("ControlGlobal").GetComponent<CreaAvions>().temps_espera_porta;
			//Random.Range(temps_manteniment_min, temps_manteniment_max+1);

		manteniment_dec_x = Random.Range(-15, 15);
		manteniment_dec_z = Random.Range(-15, 15);

		hangar_dec_x = Random.Range(-15, 15);
		hangar_dec_z = Random.Range(-15, 15);
    }
	
	// Update is called once per frame
	void Update () {
        
        
		Vector3 old_pos = transform.position;

		if (estat == -1) //Aterrar
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed*Time.deltaTime);

			if (transform.position.y > 2)
			{
				transform.position = new Vector3(transform.position.x, transform.position.y - speed*Time.deltaTime*0.5f, transform.position.z);
			}

			if (transform.position.z  < 197)
			{
				estat = 0;
			}
		}
		else
        if(estat == 0) { //Anar a Manteniment
            
            
            anar_a_manteniment();
            
        }

        else if (estat == 1) { //Rebre manteniment
            temps += Time.deltaTime;
            if (temps >= temps_manteniment) estat = 2;
        }

        else if (estat == 2) { //Anar a Hangar

            
            anar_a_hangar();
        }

        else if (estat == 3) { //Buscar porta
            porta = controlador.GetComponent<CreaAvions>().get_porta();
            //print("porta " + porta);
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
			if (Vector3.Distance(transform.position, posicio_aparcar) < 1) {
				porta_id.pujar_passatgers(this);
				estat = 5;
            }

        }

        else if (estat == 5) { //Esperar a que pujin els passatgers

				espera_hora_marxar += Time.deltaTime;
				if (espera_hora_marxar > temps_espera_porta) { hora_de_marxar = true; }

				if (passatgers >= 150 || hora_de_marxar) {
					porta_id.tancar_porta();
					estat = 6;
				}
        }
			else if (estat == 6) //Esperar a que la pista d'enlairament estigui lliure
			{
				if (pistaE2.is_free())
				{
					estat = 7;
					pistaE2.reservar();
				}
			}
			else if (estat == 7) //Anar a la pista d'enlairament
			{
				anar_a_pistaE();
			}
			else if (estat == 8) //Enlairar-se
			{
				transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed*Time.deltaTime);

				if (transform.position.z  > 203)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y + speed*Time.deltaTime*0.5f, transform.position.z);
				}

				if (transform.position.y  > 24)
				{
					pistaE2.lliberar();
					creador.llibera_porta(porta);
					Destroy(gameObject);
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
       

		Vector3 new_pos = transform.position;

		float angle = Mathf.Atan2(old_pos.x - new_pos.x, old_pos.z - new_pos.z)*Mathf.Rad2Deg;
		transform.eulerAngles = new Vector3(270, angle + 90, 0);

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
        if (Vector3.Distance(transform.position, posicio_aparcar) < 5) { estat = 1; }
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
        if (Vector3.Distance(transform.position, posicio_aparcar) < 5) { estat = 3; }
    }

	void anar_a_pistaE() {
		Vector3 pista_pos = pistaE.transform.position;
		Vector3 pista_pos2 = new Vector3(pista_pos.x, pista_pos.y, 197);
		Vector3 my_pos = transform.position;

		if (pista_pos2.x < my_pos.x) { my_pos = new Vector3(my_pos.x - speed * Time.deltaTime, my_pos.y, my_pos.z); }
		if (pista_pos2.x > my_pos.x) { my_pos = new Vector3(my_pos.x + speed * Time.deltaTime, my_pos.y, my_pos.z); }
		if (pista_pos2.z < my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z - speed * Time.deltaTime); }
		if (pista_pos2.z > my_pos.z) { my_pos = new Vector3(my_pos.x, my_pos.y, my_pos.z + speed * Time.deltaTime); }
		transform.position = my_pos;

		Vector3 posicio_aparcar = new Vector3(pista_pos2.x, transform.position.y, pista_pos2.z);
		if (Vector3.Distance(transform.position, posicio_aparcar) < 5) { estat = 8; }
	}

    public void sumar_passatger() {
        passatgers++;
    }
}
