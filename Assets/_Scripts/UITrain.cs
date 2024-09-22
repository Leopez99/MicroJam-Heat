using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITrain : MonoBehaviour
{
    Tren tren;
    StackCarbon stackCarbon;
    TextMeshPro textoCantidadCarbon;
    TextMeshPro textoTiempoDelTren;
    SpriteRenderer spriteCarbon;
    [SerializeField] Sprite[] spritesStackCarbon;

    private void Awake() {
        tren        = GetComponentInChildren<Tren>();
        stackCarbon = GetComponentInChildren<StackCarbon>();
        spriteCarbon = stackCarbon.GetComponent<SpriteRenderer>();
        textoCantidadCarbon = stackCarbon.GetComponentInChildren<TextMeshPro>();
    }

    private void Update() {
        textoCantidadCarbon.text = tren.carbonesDepositados.ToString();
        CambiarSpriteStackDeCarbon();
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
