using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackCarbon : MonoBehaviour
{
    [SerializeField] Tren tren;

    private void Awake() {

    }

    public void EnviarCarbonATren() {
        tren.AumentarCarbonesDepositados();
        Debug.Log("Envio desde el stack al tren");
        //Dato para el score
    }


    public bool ElTrenEstaEnEstacion() {
        return tren.gameObject.activeSelf == true;
    }
    
}
