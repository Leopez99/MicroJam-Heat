using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody2D rb2D;
    Vector2 input;
    [SerializeField] float velocidadDeMovimiento;
    private int carbonesActuales;

    private void Awake() {
        rb2D        = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate() {
        Movimiento();
    }

    private void Movimiento() {
        input = playerInput.actions["Movimiento"].ReadValue<Vector2>() * Time.deltaTime * velocidadDeMovimiento;
        rb2D.MovePosition(rb2D.position + input);
    }

    public void AgarrarCarbon() {
        if(hayEspacioEnBolsa()) {
            carbonesActuales++;
            Debug.Log($"El jugador tiene {carbonesActuales} carbones");
        }
        else {
            Debug.Log("No se pueden llevar mas carbones");
        }
    }

    public bool hayEspacioEnBolsa() {
        return carbonesActuales < 30;
    }
}
