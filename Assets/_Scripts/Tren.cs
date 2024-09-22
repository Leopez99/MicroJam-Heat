using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public int carbonesDepositados;
    [SerializeField] float velocidadMovimiento;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Color colorInicial;
    private bool activarEntrada;
    private bool activarSalida;
    private bool activarContador;
    private Vector3 posicionInicial;
    public float contador;

    private void OnEnable() {
        carbonesDepositados = 0;
        activarEntrada = true;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorInicial = spriteRenderer.color;
        posicionInicial = transform.localPosition;
        contador = 30;
    }


    public void AumentarCarbonesDepositados() {
        if(carbonesDepositados < 30) {
            carbonesDepositados++;
        }
    }

    private void Update() {
        EstoyLlenoDeCarbon();
        Entrar();
        Contador();
    }

    private void EstoyLlenoDeCarbon() {
        if(carbonesDepositados >= 30) {
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
        }
    }

    private void Salir() {
        if(transform.localPosition.x >= posicionInicial.x) {
            if(activarSalida)
                transform.localPosition = new Vector3 (transform.localPosition.x - velocidadMovimiento * Time.deltaTime, 0, 0);
        }
        else {
            activarSalida = false;
            gameObject.SetActive(false);

        }
        animator.SetBool("Saliendo", activarSalida);
    }

    private void Entrar() {
        if (transform.localPosition.x <= 0) {
            if(activarEntrada)
                transform.localPosition = new Vector3(transform.localPosition.x + velocidadMovimiento * 2 * Time.deltaTime, 0, 0);
        }
        else {
            activarEntrada = false;
            activarContador = true;
        }
        animator.SetBool("Entrando", activarEntrada);
    }


}
