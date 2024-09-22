using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carbon : MonoBehaviour, IClickeable
{
    int cantidadActual;
    bool puedeClickear;
    private Jugador jugador;
    [SerializeField] float distanciaClickeable;
    private CarbonSpawn carbonSpawn;
    private SpriteRenderer spriteRenderer;
    private Color colorInicial;
    public bool contadorActivo;
    float tiempoDesdeUltimoClick;
    TextMeshPro text;
    AudioSource audioSource;

    private void Awake() {
        jugador = FindObjectOfType<Jugador>();
        carbonSpawn = GetComponentInParent<CarbonSpawn>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        audioSource= GetComponent<AudioSource>();
        colorInicial = spriteRenderer.color;
    }

    private void OnEnable() {
        cantidadActual = 30;
    }

    private void OnMouseDown() {
        if (jugador.hayEspacioEnBolsa()) {
            RestarCantidad();
            audioSource.Play();
        }
        else if (!puedeClickear) {
            Debug.Log("No se puede clickear aca");
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
            contadorActivo = true;
            Debug.Log($"En el objeto {gameObject.name} hay {cantidadActual} de carbones");
        }
        else {
            Debug.Log("No se puede clickear en este carbon");
            jugador.estoyMinando = false;
        }
    }

    private void Update() {
        ComportamientoPorClick();
        ActualizarUI();
    }

    private void LateUpdate() {
        CambiarColor();
        ResetearColor();
    }

    private void ComportamientoPorClick() {
        if (DistanciaAlJugador() < distanciaClickeable) {
            PermitirClick();
        }
        else {
            DesactivarClick();
        }
        ContadorDeClickIdle();
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
            jugador.estoyMinando = false;
            carbonSpawn.ActivarContadorDeSpawn();
            gameObject.SetActive(false);
        }
    }

    private void CambiarColor() {
        if(puedeClickear)
            spriteRenderer.color = new Color(1f, 1f, 1f, 100);
    }

    private void ResetearColor() {
        if (!puedeClickear)
            spriteRenderer.color = colorInicial;
    }

    private void ContadorDeClickIdle() {

        if (contadorActivo) {
            tiempoDesdeUltimoClick += Time.deltaTime;
            //Debug.Log(tiempoDesdeUltimoClick);
            if(tiempoDesdeUltimoClick >= 0.8) {
                contadorActivo = false;
                jugador.estoyMinando = false;
            }
        }
        else {
            tiempoDesdeUltimoClick = 0;
        }
    }

    private void ActualizarUI() {
        text.text = cantidadActual.ToString();
    }
}
