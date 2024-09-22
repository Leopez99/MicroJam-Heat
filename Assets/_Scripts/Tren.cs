using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public int carbonesDepositados;
    public float cantidadMaximaDeCarbon;
    [SerializeField] float velocidadMovimiento;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color colorInicial;
    private bool activarEntrada;
    private bool activarSalida;
    private bool activarContador;
    private Vector3 posicionInicial;
    public float contador;
    private AudioSource audioSource;

    private void OnEnable() {
        carbonesDepositados = 0;
        activarEntrada = true;
        contador = 30;
        cantidadMaximaDeCarbon = Random.Range(12, 15);   //Cambia segun el tiempo
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource= GetComponent<AudioSource>();
        colorInicial = spriteRenderer.color;
        posicionInicial = transform.localPosition;
        contador = 30;
    }

    private void Start()
    {
        ActualizarDificultad();
    }


    public void AumentarCarbonesDepositados() {
        if(carbonesDepositados < cantidadMaximaDeCarbon) {
            carbonesDepositados++;
            if (carbonesDepositados >= cantidadMaximaDeCarbon) {
                GameManager.INS.trenesLlenados++;
            }
        }
    }

    private void Update() {
        EstoyLlenoDeCarbon();
        Entrar();
        Contador();
    }

    private void EstoyLlenoDeCarbon() {
        if(carbonesDepositados >= cantidadMaximaDeCarbon) {
            activarSalida = true;
            Salir();
        }
    }

    private void Contador() {
        if (activarContador) {
            contador -= Time.deltaTime;
            RomperTren();
        }
    }

    private void RomperTren() {
        if (contador <= 0) {
            spriteRenderer.color = Color.red;
            this.enabled = false;
            GameManager.INS.trenesExplotados++;
        }
    }

    private void Salir() {
        if(transform.localPosition.x >= posicionInicial.x) {
            if (activarSalida) {
                transform.localPosition = new Vector3 (transform.localPosition.x - velocidadMovimiento * Time.deltaTime, 0, 0);
                activarContador = false;
                //StartCoroutine(playSonido());
            }
        }
        else {
            activarSalida = false;
            gameObject.SetActive(false);

        }
        animator.SetBool("Saliendo", activarSalida);
    }

    private void Entrar() {
        if (transform.localPosition.x <= 0) {
            if (activarEntrada)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + velocidadMovimiento * 2 * Time.deltaTime, 0, 0);
                //StartCoroutine(playSonido());
            }
        }
        else {
            activarEntrada = false;
            activarContador = true;
        }
        animator.SetBool("Entrando", activarEntrada);
    }

    IEnumerator playSonido()
    {
        yield return new WaitForEndOfFrame();
        audioSource.Play();
        StopCoroutine(playSonido());

    }

    private void ActualizarDificultad()
    {
        
        if(GameManager.INS.minutoActual <= 2)
        {
            cantidadMaximaDeCarbon = Random.Range(12, 15);
        }

        if (GameManager.INS.minutoActual <= 1)
        {
            cantidadMaximaDeCarbon = Random.Range(12, 20);
        }

        if (GameManager.INS.minutoActual <= 0)
        {
            cantidadMaximaDeCarbon = Random.Range(12, 22);
        }

        

    }

}
