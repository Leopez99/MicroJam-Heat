using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarbonSpawn : MonoBehaviour
{
    [SerializeField] float tiempoParaSpawnearCarbon;
    private Carbon carbon;
    private bool contadorTrigger;
    private float tiempo;

    private void Awake() {
        carbon = GetComponentInChildren<Carbon>();
    }

    private void Update() {
        Timer();
        if(tiempo >= tiempoParaSpawnearCarbon) {
            carbon.gameObject.SetActive(true);
        }
    }

    private void Timer() {
        if (contadorTrigger) {
            tiempo += Time.deltaTime;
        }
    }

    public void ActivarContadorDeSpawn() {
        contadorTrigger = true;
        tiempo = 0;
    }
}
