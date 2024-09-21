using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCarbon : MonoBehaviour
{
    [SerializeField] Tren tren;

    private void Awake() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Jugador jugador = collision.GetComponent<Jugador>();
        if(collision != null) {
            jugador.estoyEnStackDeCarbon = true;
            Debug.Log($"El jugador entro al stack de carbon {gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Jugador jugador = collision.GetComponent<Jugador>();
        if (collision != null) {
            jugador.estoyEnStackDeCarbon = false;
            Debug.Log($"El jugador salio del stack de carbon {gameObject.name}");
        }
    }
}
