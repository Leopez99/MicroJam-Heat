using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCarbon : MonoBehaviour
{
    [SerializeField] Tren tren;
    [SerializeReference] private bool sePuedeClickear;
    [SerializeReference] Jugador jugador;

    private void Awake() {

    }

    private void OnMouseDown()
    {
        if(sePuedeClickear && jugador.carbonesActuales > 0 && ElTrenEstaEnEstacion() && !StackLleno())
        {
            EnviarCarbonATren();
            jugador.carbonesActuales--;
        }
    }

    public void EnviarCarbonATren() {
        tren.AumentarCarbonesDepositados();
        Debug.Log("Envio desde el stack al tren");
    }


    public bool ElTrenEstaEnEstacion() {
        return tren.gameObject.activeSelf == true;
    }

    public bool StackLleno()
    {
        return tren.carbonesDepositados >= tren.cantidadMaximaDeCarbon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        jugador = collision.GetComponent<Jugador>();

        if (jugador != null)
        {
            sePuedeClickear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        jugador = collision.GetComponent<Jugador>();

        if (jugador != null)
        {
            sePuedeClickear = false;
        }
        jugador = null;
    }
}
