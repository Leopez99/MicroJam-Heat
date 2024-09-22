using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITrain : MonoBehaviour
{
    [SerializeReference] Tren tren;
    StackCarbon stackCarbon;
    TextMeshPro textoCantidadCarbon;
    [SerializeReference] TextMeshPro textoTiempoDelTren;
    SpriteRenderer spriteCarbon;
    [SerializeField] Sprite[] spritesStackCarbon;

    private void Awake() {
        tren        = GetComponentInChildren<Tren>();
        stackCarbon = GetComponentInChildren<StackCarbon>();
        spriteCarbon = stackCarbon.GetComponent<SpriteRenderer>();
        textoCantidadCarbon = stackCarbon.GetComponentInChildren<TextMeshPro>();
        textoTiempoDelTren = tren.GetComponentInChildren<TextMeshPro>();
    }

    private void Update() {
        textoCantidadCarbon.text = $"{tren.carbonesDepositados}/{tren.cantidadMaximaDeCarbon}";
        CambiarSpriteStackDeCarbon();
        //textoTiempoDelTren.text = Mathf.Round(tren.contador).ToString();
        textoTiempoDelTren.text = Math.Round((decimal)tren.contador, 2).ToString();
    }

    private void CambiarSpriteStackDeCarbon() {
        if(tren.carbonesDepositados == 0) {
            spriteCarbon.sprite = spritesStackCarbon[0];
        }
        if (tren.carbonesDepositados == 15) {
            spriteCarbon.sprite = spritesStackCarbon[1];
        }
        if(tren.carbonesDepositados == 30) {
            spriteCarbon.sprite = spritesStackCarbon[2];
        }
    }
}
