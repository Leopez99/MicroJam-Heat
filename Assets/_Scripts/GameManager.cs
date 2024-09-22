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
    [SerializeReference] int minutoActual;
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

    Jugador jugador;

    private void Awake() {
        INS = this;
        segundoActual = 00;
        minutoActual = 3;
        jugador = FindObjectOfType<Jugador>();
        imagenDelPanel = panelVictoriaDerrota.GetComponent<Image>();
    }

    private void Start()
    {
        panelVictoriaDerrota.SetActive(false);
    }

    private void Update() {
        Contador();
        GameOver();
        Victory();
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
        textoScoreTrainBroken.text = $"Train Broken: {trenesExplotados}";
        textoScoreCoalGrabbed.text = $"Coal grabbed: {carbonRecogido}";
        textoScoreTrainSuccess.text = $"Train Success: {trenesLlenados}";
        textoScoreTotal.text = $"TOTAL: {TotalScore()}";
    }
}
