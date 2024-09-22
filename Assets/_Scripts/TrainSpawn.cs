using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpawn : MonoBehaviour
{
    Tren tren;
    [SerializeField] float tiempoActivarTren;

    private void Awake() {
        tren = GetComponentInChildren<Tren>();
    }

    private void Update() {
        if (!tren.gameObject.activeSelf) {
            StartCoroutine(ActivarTren());
        }
    }

    public IEnumerator ActivarTren() {
        yield return new WaitForSeconds(tiempoActivarTren);
        tren.gameObject.SetActive(true);
    }
}
