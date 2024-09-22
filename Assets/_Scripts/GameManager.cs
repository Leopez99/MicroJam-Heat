using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager INS;
    [Header("Tiempo")]
    [SerializeReference] float segundoActual;
    [SerializeReference] public int minutoActual;
    [SerializeField] TextMeshProUGUI textoTiempo;

    [Header("Panel Victoria o derrota")]
    [SerializeField] GameObject panelVictoriaDerrota;
    [SerializeReference] Image imagenDelPanel;
    [SerializeField] Sprite[] spriteVictoryDefeat;

    [Header("Datos de score")]
    [SerializeField] TextMeshProUGUI textoScoreTotal;
    [SerializeField] TextMeshProUGUI textoScoreTrainBroken;
    [SerializeField] TextMeshProUGUI textoScoreCoalGrabbed;
    [SerializeField] TextMeshProUGUI textoScoreTrainSuccess;

    [Header("Datos reales para el score")]
    public int trenesExplotados;
    public int carbonRecogido;
    public int trenesLlenados;

    [SerializeReference] Jugador jugador;

    private void Awake() {
        INS = this;
        segundoActual = 00;
        minutoActual = 3;
        imagenDelPanel = panelVictoriaDerrota.GetComponent<Image>();
    }

    private void Start()
    {
        jugador = FindObjectOfType<Jugador>();
        panelVictoriaDerrota.SetActive(false);
        Time.timeScale= 1.0f;
    }

    private void Update() {
        Contador();
        GameOver();
        Victory();
        //ActualizarDificultad();
    }

    private void Contador() {
        ActualizarUITiempo();
    }

    private void ActualizarUITiempo() {
        ActualizarSegundos();
        if (segundoActual >= 0 && minutoActual >= 0) {
            textoTiempo.text = $"TIME: {minutoActual}:{Math.Round((decimal)segundoActual, 0)}";
        }
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
        if(trenesExplotados >= 2) {
            panelVictoriaDerrota.SetActive(true);
            //textoVictoryDefeat.text = "DEFEAT";
            imagenDelPanel.sprite = spriteVictoryDefeat[1];
            ActualizarPanelScore();
            Time.timeScale = 0;
            jugador.enabled = false;
        }
    }

    private void Victory() {
        if(!(segundoActual >= 0 && minutoActual >= 0)) {
            panelVictoriaDerrota.SetActive(true);
            //textoVictoryDefeat.text = "VICTORY";
            imagenDelPanel.sprite = spriteVictoryDefeat[0];
            ActualizarPanelScore();
            Time.timeScale = 0;
            jugador.enabled = false;
        }
    }

    private string TotalScore() {
        int penalizacion = trenesExplotados * 100;
        int suma = carbonRecogido * 100 + trenesLlenados * 2;
        return (suma - penalizacion).ToString();
    }

    private void ActualizarPanelScore() {
        textoScoreTrainBroken.text = $"Broken trains: {trenesExplotados}";
        textoScoreCoalGrabbed.text = $"Coal picked up: {carbonRecogido}";
        textoScoreTrainSuccess.text = $"Filled train: {trenesLlenados}";
        textoScoreTotal.text = $"TOTAL: {TotalScore()}";
    }

    private void ActualizarDificultad()
    {
        //if(minutoActual <= 2)
        //{
        //    cantidadCarbonAleatoria = UnityEngine.Random.Range(12, 15);
        //}

        //if(minutoActual <= 1)
        //{
        //    cantidadCarbonAleatoria = UnityEngine.Random.Range(12, 20);
        //}

        //if (minutoActual <= 1)
        //{
        //    cantidadCarbonAleatoria = UnityEngine.Random.Range(12, 25);
        //}
    }
}
