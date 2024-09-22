using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public int carbonesDepositados;
    [SerializeField] float velocidadMovimiento;
    Jugador jugador;
    Animator animator;
    private bool activarEntrada;
    private bool activarSalida;
    private Vector3 posicionInicial;

    private void OnEnable() {
        carbonesDepositados = 0;
        activarEntrada = true;
    }

    private void Awake() {
        jugador = FindAnyObjectByType<Jugador>();
        animator = GetComponent<Animator>();
        posicionInicial = transform.localPosition;
    }


    public void AumentarCarbonesDepositados() {
        if(carbonesDepositados < 30) {
            carbonesDepositados++;
        }
    }

    private void Update() {
        EstoyLlenoDeCarbon();
        Entrar();
    }

    private void EstoyLlenoDeCarbon() {
        if(carbonesDepositados >= 30) {
            //gameObject.SetActive(false);
            activarSalida = true;
            Salir();
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
    }

    private void Entrar() {
        if (transform.localPosition.x <= 0) {
            if(activarEntrada)
                transform.localPosition = new Vector3(transform.localPosition.x + velocidadMovimiento * 2 * Time.deltaTime, 0, 0);
        }
        else {
            activarEntrada = false;
        }
    }


}
