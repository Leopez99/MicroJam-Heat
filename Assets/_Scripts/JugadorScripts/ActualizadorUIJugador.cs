using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActualizadorUIJugador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    Jugador jugador;

    private void Awake() {
        jugador = GetComponent<Jugador>();
    }

    private void Update() {
        ActualizarBolsa();
    }

    private void ActualizarBolsa() {
        text.text = $"Coal {jugador.getCarbonesActuales()}/30";
    }

}
