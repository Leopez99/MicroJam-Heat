using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class BtnSiguienteEscena : MonoBehaviour {
    private Button button;
    [SerializeField] string nombreDeEscena;
    [SerializeField] bool permitirTeclaParaPresionarBoton;
    [SerializeField] KeyCode teclaParaPresionar = KeyCode.Return;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        button.onClick.AddListener(() => EscenaAMoverse());
    }

    private void Update() {
        // Si permitirTeclaParaPresionarBoton es true y se presiona la tecla asignada
        if (permitirTeclaParaPresionarBoton && Input.GetKeyDown(teclaParaPresionar)) {
            // Ejecuta el mismo método que el botón
            EscenaAMoverse();
        }
    }

    private void EscenaAMoverse() {
        SceneManager.LoadScene(nombreDeEscena);
        //EditorApplication.isPlaying = false;
    }

    public void SetNombreDeEscena(string nombreDeEscena) {
        this.nombreDeEscena = nombreDeEscena;
    }
}
