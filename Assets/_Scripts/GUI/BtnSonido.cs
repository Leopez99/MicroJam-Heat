using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnSonido : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => Accion());
    }

    private void Accion()
    {
        MusicManager.INS.seTocoUnaVez = !MusicManager.INS.seTocoUnaVez;
    }
}
