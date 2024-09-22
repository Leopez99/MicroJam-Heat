using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float segundoActual;
    int minutoActual;
    public int camionesExplotados;
    public static GameManager INS;
    [SerializeField] TextMeshProUGUI textoTiempo;

    private void Awake() {
        INS = this;
        segundoActual = 00;
        minutoActual = 3;
    }

    private void Update() {
        Contador();
        GameOver();
    }

    private void Contador() {
        //if(segundosActuales > 0) {
        //    segundosActuales -= Time.deltaTime;
        //}
        ActualizarUITiempo();
    }

    private void ActualizarUITiempo() {
        ActualizarSegundos();
        if (segundoActual >= 0 && minutoActual >= 0) {
            textoTiempo.text = $"TIME: {minutoActual}:{Math.Round((decimal)segundoActual, 0)}";
        }
        //textoTiempo.text = $"TIME: {Math.Round((decimal)tiempoActual, 1)}";
    }

    private void ActualizarSegundos() {
        if (segundoActual > 0) {
            segundoActual -= Time.deltaTime;
        }
        if(segundoActual <= 0) {
            ActualizarMinutos();
        }
    }

    private void ActualizarMinutos() {
        minutoActual--;
        segundoActual = 60;
    }

    private void GameOver() {
        if(camionesExplotados >= 2) {
            Debug.Log("Game Over");
        }
    }
}
