using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Jugador : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody2D rb2D;
    Vector2 input;
    float velocidadDeMovimiento;
    [SerializeField] float velocidadDeCaminar;
    [SerializeField] float velocidadDeCorrer;
    private int carbonesActuales;
    private Animator animator;
    public bool estoyMinando;

    [Header("Estos son los idles que va a tener despues de moverse")]
    [SerializeField] Sprite arriba;
    [SerializeField] Sprite abajo;
    [SerializeField] Sprite izquierda;
    [SerializeField] Sprite derecha;
    Sprite spriteIdle;
    SpriteRenderer spriteRenderer;

    [Header("Stamina Settings")]
    public float maxStamina = 40f;                  // Máxima stamina
    [Tooltip("Tasa de regeneración por segundo")]
    public float staminaRegenerationRate = 10f;     // Tasa de regeneración por segundo
    [Tooltip("Tasa de agotamiento por segundo al correr")]
    public float staminaDepletionRate = 20f;        // Tasa de agotamiento por segundo al correr
    private float currentStamina;                   // Stamina actual
    private bool exausted;                          // Indica si el jugador termino su barra de stamina hasta el final
    private bool isRunning;                         // Indica si el jugador está corriendo
    public Slider staminaBar;                       // Stamina UI
    private StaminaSlider staminaSlider;

    private void Awake() {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        animator        = GetComponent<Animator>(); 
        rb2D            = GetComponent<Rigidbody2D>();
        playerInput     = GetComponent<PlayerInput>();
        staminaSlider   = staminaBar.GetComponent<StaminaSlider>();
        spriteIdle      = spriteRenderer.sprite;
        velocidadDeMovimiento = velocidadDeCaminar;
        currentStamina = maxStamina;
    }

    private void Update() {
        input = playerInput.actions["Movimiento"].ReadValue<Vector2>();
        CambiarReferenciaDeIdle();
        AnimacionesDeMovimiento();
        AnimacionMinar();
        HandleStamina();
        UpdateStaminaUI();
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

    public void Correr(InputAction.CallbackContext context) {
        //Solo dejarlo correr cuando no esta exausto
        if (context.performed && !exausted) {
            velocidadDeMovimiento = velocidadDeCorrer;
            isRunning = true;
        }

        if (context.canceled) {
            velocidadDeMovimiento = velocidadDeCaminar;
            isRunning = false;
        }
    }

    public void AgarrarCarbon() {
        if(hayEspacioEnBolsa()) {
            carbonesActuales++;
            estoyMinando = true;
            Debug.Log($"El jugador tiene {carbonesActuales} carbones");
        }
        else {
            Debug.Log("No se pueden llevar mas carbones");
        }
        Debug.Log("Estoy minando = " + estoyMinando);
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

    private void AnimacionMinar() {
        if (!hayEspacioEnBolsa()) {
            estoyMinando = false;
        }
        animator.SetBool("Minando", estoyMinando);
    }

    private void CambiarIdle() {
        if(input.x == 0 && input.y == 0 && !estoyMinando) {
            spriteRenderer.sprite = spriteIdle;
        }
    }

    public int getCarbonesActuales() {
        return this.carbonesActuales;
    }

    private void HandleStamina() {
        // Si el jugador está corriendo, agotamos stamina
        if (isRunning) {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }

        else {
            // Si no está corriendo regenerar stamina
            if (currentStamina < maxStamina) {
                //Debug.Log("Se esta recargando su estamina");
                currentStamina += staminaRegenerationRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
                    //Si termino de recargarse entonces dejarlo correr de nuevo
                    if(currentStamina >= maxStamina) {
                        exausted = false;
                        staminaSlider.VolverAColorOriginal();
                    }
            }
        }

        // No dejar que la stamina sea menor a 0
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        // Si la stamina se agota, el jugador no puede correr hasta que se recupere por completo
        if (currentStamina <= 0) {
            exausted = true;    // Indicamos que el jugador termino su barra de stamina
            isRunning = false;  // Forzamos a que el jugador deje de correr
            velocidadDeMovimiento = velocidadDeCaminar;
        }
    }

    private void UpdateStaminaUI() {
        if (staminaBar != null) {
            staminaBar.value = currentStamina / maxStamina;
        }
    }

}
