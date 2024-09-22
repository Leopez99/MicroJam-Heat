using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnFullscreen : MonoBehaviour
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
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Fullscreen: " + Screen.fullScreen);
    }
}
