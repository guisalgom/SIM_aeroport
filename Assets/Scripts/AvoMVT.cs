using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoMVT : MonoBehaviour {

    int porta;
    private GameObject controlador;
    private ControlPorta porta_id;
    private float porta_dec_x;
    private float porta_dec_z;
    public float speed = 10f;
    public int estat = 0;

    // Use this for initialization
    void Start () {

        controlador = GameObject.FindGameObjectWithTag("GameController");
        

        //porta = Mathf.RoundToInt(Random.Range(1, 12));
        

        //porta_id = GameObject.Find("Porta (" + porta.ToString() + ")").GetComponent<ControlPorta>();
        //porta_dec_x = Random.Range(-10, 10);
        //porta_dec_z = Random.Range(-10, 10);

    }
	
	// Update is called once per frame
	void Update () {
        
        print("estat " + estat);

        if (estat == 0) {
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
                    estat = 1;
            }
        }
        else if (estat == 1) {
            anar_a_la_porta();
			Vector3 porta_pos = porta_id.transform.position;
			Vector3 posicio_aparcar = new Vector3(porta_pos.x + porta_dec_x, transform.position.y, porta_pos.z + porta_dec_z);
			if (Vector3.Distance(transform.position, posicio_aparcar) < 1)
			{
				porta_id.pujar_passatgers(this);
				estat = 2;
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
}
