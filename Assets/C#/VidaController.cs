using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VidaController : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image colorBarra;
    
    public void setVidaMaxima(int vida){
        slider.maxValue = vida;
        slider.value = vida;

        colorBarra.color = gradient.Evaluate(1f);
    }

    public void cambiarVida(int vida){
        slider.value = vida;
        colorBarra.color = gradient.Evaluate(slider.normalizedValue);
    }
}
