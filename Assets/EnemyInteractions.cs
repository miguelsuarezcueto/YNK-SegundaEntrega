using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractions : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sR;
    private Animator anim;
    public Color colorDaño = Color.red;
    public int vidaMax = 50;
    private int vidaActual;
    private bool muerto = false;
    private bool desaparecido = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        vidaActual = vidaMax;
        rb.isKinematic = true;

    }

    void Update()
    { 
        anim.SetBool("muerto", muerto);
        anim.SetBool("desaparecido", desaparecido);
    }

    void CambiarColor(Color color)
    {
        Material nuevoMaterial = new Material(sR.sharedMaterial);
        nuevoMaterial.color = color;
        sR.material = nuevoMaterial;
    }

    public void RecibeDaño(int dañoRecibido)
    {
        Debug.Log("Enemigo: " + vidaActual);

        if (vidaActual > 0)
        {
            vidaActual -= dañoRecibido;
            CambiarColor(colorDaño);

            if (vidaActual <= 0)
            {
                Muerte();
                Invoke("ResetColor", 0.2f);
            }
            else
            {
                Invoke("ResetColor", 0.2f);
            }
        }
    }

    void Muerte()
    {
        muerto = true;
        anim.SetBool("muerto", true);
        Muerto();
    }

    void Muerto()
    {
        desaparecido = true;
        anim.SetBool("desaparecido", true);
        gameObject.SetActive(false);
    }

    void ResetColor()
    {
        CambiarColor(Color.white);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //CAMBIAR EL TAG AL ARMA, ASI ESTÁ PILLANDO LA HITBOX DEL PLAYER
        if (collision.gameObject.CompareTag("Player"))
        {
            RecibeDaño(10);
        }
    }
}
