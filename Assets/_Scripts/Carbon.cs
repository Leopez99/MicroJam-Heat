using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbon : MonoBehaviour, IClickeable
{
    int cantidadActual;
    bool puedeClickear;
    private Jugador jugador;
    [SerializeField] float distanciaClickeable;
    private CarbonSpawn carbonSpawn;

    private void Awake() {
        jugador = FindObjectOfType<Jugador>();
        carbonSpawn = GetComponentInParent<CarbonSpawn>();
    }

    private void OnEnable() {
        cantidadActual = 30;
    }

    private void OnMouseDown() {
        if (jugador.hayEspacioEnBolsa()) {
            RestarCantidad();
        }
        else {
            Debug.Log("No hay espacio en bolsa para mas carbon");
        }
    }

    public void RestarCantidad() {
        if (puedeClickear) {
            this.cantidadActual--;
            jugador.AgarrarCarbon();
            DestruirPorFaltaDeCarbon();

            Debug.Log($"En el objeto {gameObject.name} hay {cantidadActual} de carbones");
        }
        else {
            Debug.Log("No se puede clickear en este carbon");
        }
    }

    private void Update() {
        ComportamientoPorClick();
    }

    private void ComportamientoPorClick() {
        if (DistanciaAlJugador() < distanciaClickeable) {
            PermitirClick();
        }
        else {
            DesactivarClick();
        }
    }

    public void PermitirClick() {
        //Se cambia el estado del carbon para que se pueda hacer click y restar cantidad
        puedeClickear = true;
    }

    public void DesactivarClick() {
        puedeClickear = false;
    }

    private float DistanciaAlJugador() {
        return Vector2.Distance(this.transform.position, jugador.transform.position);
    }

    private void DestruirPorFaltaDeCarbon() {
        if(cantidadActual <= 0) {
            carbonSpawn.ActivarContadorDeSpawn();
            gameObject.SetActive(false);
        }
    }
}
