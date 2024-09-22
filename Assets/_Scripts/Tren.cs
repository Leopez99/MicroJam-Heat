using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public int carbonesDepositados;
    [SerializeField] float velocidadMovimiento;
    Jugador jugador;
    Animator animator;

    private void OnEnable() {
        carbonesDepositados = 0;
    }

    private void Awake() {
        jugador = FindAnyObjectByType<Jugador>();
        animator = GetComponent<Animator>();
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
            Salir();
        }
    }

    private void Salir() {
        transform.localPosition = new Vector3 (transform.localPosition.x - velocidadMovimiento * Time.deltaTime, 0, 0);
    }

    private void Entrar() {
        if(transform.localPosition.x <= 0)
            transform.localPosition = new Vector3(transform.localPosition.x + velocidadMovimiento * 2 * Time.deltaTime, 0, 0);
    }
}
