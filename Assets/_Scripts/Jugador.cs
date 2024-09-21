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
    private Animator animator;

    [Header("Estos son los idles que va a tener despues de moverse")]
    [SerializeField] Sprite arriba;
    [SerializeField] Sprite abajo;
    [SerializeField] Sprite izquierda;
    [SerializeField] Sprite derecha;
    Sprite spriteIdle;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator    = GetComponent<Animator>(); 
        rb2D        = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        spriteIdle = spriteRenderer.sprite;
    }

    private void Update() {
        input = playerInput.actions["Movimiento"].ReadValue<Vector2>();
        CambiarReferenciaDeIdle();
        AnimacionesDeMovimiento();
    }

    private void FixedUpdate() {
        Movimiento();
    }

    private void LateUpdate() {
        CambiarIdle();
    }

    private void Movimiento() {
        Vector2 movimiento = input * Time.deltaTime * velocidadDeMovimiento;
        rb2D.MovePosition(rb2D.position + movimiento);
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

    private void AnimacionesDeMovimiento() {
        animator.SetFloat("x", input.x);
        animator.SetFloat("y", input.y);
        animator.SetFloat("Speed", input.sqrMagnitude);
    }

    private void CambiarReferenciaDeIdle() {
        if(input.x > 0) {
            spriteIdle = derecha;
        }
        if (input.x < 0) {
            spriteIdle = izquierda;
        }
        if (input.y > 0) {
            spriteIdle = arriba;
        }
        if (input.y < 0) {
            spriteIdle = abajo;
        }
    }

    private void CambiarIdle() {
        if(input.x == 0 && input.y == 0) {
            spriteRenderer.sprite = spriteIdle;
        }
    }
}
