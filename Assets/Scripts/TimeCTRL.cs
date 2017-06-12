using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCTRL : MonoBehaviour {

	public int any = 2017;
	public int mes = 1;
	public int dia = 1;

	public int hora = 0;
	public int minut = 0;

	public int hora_opertura = 6;
	public int hora_clausura = 24;

	private float timer;

	private Text fecha;
	private Text horaa;

	private Light llum;
	private GameObject parametros;
	bool param_activo = false;

	private CreaPassatgers creapas;
	private CreaAvions creaavs;

	public Slider sli1;
	public Slider sli2;
	public Slider sli3;
	public Slider sli4;
	public Slider sli5;
	public Slider sli6;
	public Slider sli7;

	public Text txtb1;
	public Text txtb2;
	public Text txtb3;
	public Text txtb4;
	public Text txtb5;
	public Text txtb6;
	public Text txtb7;

	// Use this for initialization
	void Start () {
		timer = 0;	
		fecha = GameObject.Find("CalData").GetComponent<Text>();
		horaa = GameObject.Find("CloData").GetComponent<Text>();

		llum = GameObject.Find("LlumFocDestruccio").GetComponent<Light>();
		parametros = GameObject.Find("Parametres"); //.GetComponent<Light>();
		param_activo = false;
		parametros.SetActive(param_activo);


		creapas = GameObject.Find("ControlGlobal").GetComponent<CreaPassatgers>();
		creaavs = GameObject.Find("ControlGlobal").GetComponent<CreaAvions>();


	}
	
	// Update is called once per frame
	void Update () {

		if (param_activo)
		{
			txtb1.text = "Cada " + sli1.value.ToString() + " min.";
			txtb2.text = sli2.value.ToString() + " cada cop.";
			txtb3.text = "Cada " + sli3.value.ToString() + " min.";
			txtb4.text = sli4.value.ToString() + " minuts.";
			txtb5.text = sli5.value.ToString() + "%";
			txtb6.text = sli6.value.ToString() + " minuts.";
			txtb7.text = sli7.value.ToString() + " minuts.";
		}


		timer += Time.deltaTime;
		if (timer >= 1) { timer = 0; minut++; }
		if (minut >= 60) { minut = 0; hora++; }
		if (hora >= 24) { hora = 0; dia++; }
		if (dia > dia_maxim(mes)) { dia = 1; mes++; }
		if (mes > 12) { mes = 1; any++; }

		string zeromes = "0"; if (mes >= 10) { zeromes = ""; }
		string zerodia = "0"; if (dia >= 10) { zerodia = ""; }
		string zerohora = "0"; if (hora >= 10) { zerohora = ""; }
		string zerominut = "0"; if (minut >= 10) { zerominut = ""; }

		fecha.text = any.ToString() + "/" + zeromes + mes.ToString() + "/" + zerodia + dia.ToString();
		horaa.text = zerohora + hora.ToString() + ":" + zerominut +minut.ToString();


		if (get_aeroport_obert()) { llum.enabled = true; }
		else { llum.enabled = false; }

	}

	int dia_maxim(int mes)
	{
		if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12) { return 31; }
		else if (mes == 2) { return 28; }
		else { return 30; }
	}

	public bool get_aeroport_obert()
	{
		if (hora >= hora_opertura && hora <= hora_clausura)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void aumenta_hora()
	{
		minut = 0;
		hora++;
	}

	public void aumenta_dia()
	{
		hora = 0;
		minut = 0;
		dia++;
	}

	public void mostrar_amagar_parametres()
	{
		if (param_activo) { param_activo = false; } else { param_activo = true; }
		parametros.SetActive(param_activo);

		if (param_activo)
		{
			sli1 = GameObject.Find("Slider").GetComponent<Slider>();
			sli2 = GameObject.Find("Slider (1)").GetComponent<Slider>();
			sli3 = GameObject.Find("Slider (2)").GetComponent<Slider>();
			sli4 = GameObject.Find("Slider (3)").GetComponent<Slider>();
			sli5 = GameObject.Find("Slider (4)").GetComponent<Slider>();
			sli6 = GameObject.Find("Slider (5)").GetComponent<Slider>();
			sli7 = GameObject.Find("Slider (6)").GetComponent<Slider>();

			txtb1 = GameObject.Find("Text (7)").GetComponent<Text>();
			txtb2 = GameObject.Find("Text (8)").GetComponent<Text>();
			txtb3 = GameObject.Find("Text (9)").GetComponent<Text>();
			txtb4 = GameObject.Find("Text (10)").GetComponent<Text>();
			txtb5 = GameObject.Find("Text (11)").GetComponent<Text>();
			txtb6 = GameObject.Find("Text (12)").GetComponent<Text>();
			txtb7 = GameObject.Find("Text (13)").GetComponent<Text>();
		}
	}

	public void barra1()
	{
		creapas.frequencia = GameObject.Find("Slider").GetComponent<Slider>().value;
	}

	public void barra2()
	{
		creapas.quantitat = Mathf.RoundToInt(GameObject.Find("Slider (1)").GetComponent<Slider>().value);
	}

	public void barra3()
	{
		creaavs.frequencia = Mathf.RoundToInt(GameObject.Find("Slider (2)").GetComponent<Slider>().value);
	}

	public void barra4()
	{
		creapas.temps_atenciog = Mathf.RoundToInt(GameObject.Find("Slider (3)").GetComponent<Slider>().value);
	}

	public void barra5()
	{
		creapas.rate_papers_regla = Mathf.RoundToInt(GameObject.Find("Slider (4)").GetComponent<Slider>().value);
	}

	public void barra6()
	{
		creaavs.temps_manteniment = Mathf.RoundToInt(GameObject.Find("Slider (5)").GetComponent<Slider>().value);
	}

	public void barra7()
	{
		creaavs.temps_espera_porta = Mathf.RoundToInt(GameObject.Find("Slider (6)").GetComponent<Slider>().value);
	}

	public void velocidadxminutos()
	{
		Time.timeScale = 1;
	}

	public void velocidadxhoras()
	{
		Time.timeScale = 4;
	}
}
