using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private SpriteRenderer sR;
    public Color colorDaño = Color.red;
    public int vidaMax = 100;
    private int vidaActual;
    public VidaController barravida;


    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        vidaActual = vidaMax;
        barravida.setVidaMaxima(vidaActual);
    }

    void Update() { }

    void CambiarColor(Color color)
    {
        Material nuevoMaterial = new Material(sR.sharedMaterial);
        nuevoMaterial.color = color;
        sR.material = nuevoMaterial;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemigo/Ataque"))
        {
            RecibeDaño(10);
        }
    }

    public void RecibeDaño(int dañoRecibido)
    {
        Debug.Log("Player: " + vidaActual);

        if (vidaActual > 0)
        {
            vidaActual -= dañoRecibido;
            Debug.Log("Tamaño Slider: " + barravida.slider.maxValue);
            barravida.cambiarVida(vidaActual);
            CambiarColor(colorDaño);

            if (vidaActual <= 0)
            {
                Muerte();
            }
            else
            {
                Invoke("ResetColor", 0.2f);
            }
        }
    }

    void Muerte()
    {
        Debug.Log("Player muerto");
        gameObject.SetActive(false);
    }

    void ResetColor()
    {
        CambiarColor(Color.white);
    }
}
