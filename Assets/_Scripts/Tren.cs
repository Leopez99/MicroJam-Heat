using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tren : MonoBehaviour
{
    public int carbonesDepositados;
    Jugador jugador;

    private void Awake() {
        jugador = FindAnyObjectByType<Jugador>();
    }

    public void AumentarCarbonesDepositados() {
        if(carbonesDepositados < 30) {
            carbonesDepositados++;
        }
    }
}
