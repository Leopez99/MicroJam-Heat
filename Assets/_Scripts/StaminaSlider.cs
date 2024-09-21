using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSlider : MonoBehaviour
{
    Slider slider;
    [SerializeField] Image filled;
    Color colorOriginal;

    private void Awake() {
        slider = GetComponent<Slider>();
        colorOriginal = filled.color;
    }

    private void Update() {
        PonerseRojo();
    }

    private void PonerseRojo() {
        if(slider.value <= 0) {
            filled.color = Color.red;
        }
    }

    public void VolverAColorOriginal() {
        filled.color = colorOriginal;
    }
}
