using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator anim;
    public bool estaSaltando = false;
    public bool enSuelo = true;
    public bool chocaParedIzq = false;
    public bool chocaParedDer = false;
    public bool estaCorriendo;

    [Header("Movimiento")]
    private float movimientoHorizontal = 0f;

    [SerializeField]
    private float fuerzaDeSalto;

    [SerializeField]
    private float velocidadDeMovimiento;

    private bool mirandoDerecha = true;

    public void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Manejo de entrada del jugador y actualizaciones visuales
        // Lógica para detectar la entrada y cambiar animaciones

        anim.SetBool("estaCorriendo", estaCorriendo);
        anim.SetBool("estaSaltando", estaSaltando);
        anim.SetBool("enSuelo", enSuelo);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (enSuelo && !estaSaltando)
            {
                enSuelo = false;
                anim.SetBool("enSuelo", false);
                estaSaltando = true;
                anim.SetBool("estaSaltando", true);

                rb2D.AddForce(Vector2.up * Mathf.Sqrt(2 * fuerzaDeSalto * Mathf.Abs(Physics2D.gravity.y)), ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        // Operaciones relacionadas con la física del jugador
        // Cálculos de movimiento, detección de colisiones, aplicar fuerzas al Rigidbody, etc.
        // Ideal para lógica asociada al movimiento físico.

        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        if (movimientoHorizontal == -1 || movimientoHorizontal == 1)
        {
            estaCorriendo = true;
            anim.SetBool("estaCorriendo", true);
            Mover(movimientoHorizontal * velocidadDeMovimiento * Time.fixedDeltaTime);
        } else
        {
            estaCorriendo = false;
            anim.SetBool("estaCorriendo", false);
        }
        
    }

    // Mueve al personaje segun el valor de mirandoDerecha en una direccion u otra
    private void Mover(float mover)
    {
        if (mover > 0 && !mirandoDerecha)
        {
            Girar();
        }
        else if (mover < 0 && mirandoDerecha)
        {
            Girar();
        }

        // Si no colisiona con paredes
        if (!chocaParedIzq && !chocaParedDer)
        {
            rb2D.gravityScale = 5.0f;
            rb2D.velocity = new Vector2(mover, rb2D.velocity.y);
        }
        else if (enSuelo && chocaParedIzq)
        {
            rb2D.gravityScale = 5.0f;
            rb2D.velocity = new Vector2(mover, -4.5f);
        }
        else if (enSuelo && chocaParedDer)
        {
            rb2D.gravityScale = 5.0f;
            rb2D.velocity = new Vector2(mover, -4.5f);
        }
    }

    // Da la vuelta al personaje
    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    // Leer cuando entra en contacto con elementos
    private void OnCollisionEnter2D(Collision2D colisionEntrada)
    {
        if (colisionEntrada.gameObject.CompareTag("Suelo") || colisionEntrada.gameObject.CompareTag("Enemigo/Ataque"))
        {
            enSuelo = true;
            anim.SetBool("enSuelo", true);
            estaSaltando = false;
            anim.SetBool("estaSaltando", false);
        }

        if (colisionEntrada.gameObject.CompareTag("Pared"))
        {
            Vector2 normal = colisionEntrada.contacts[0].normal;
            if (Vector2.Dot(normal, Vector2.left) > 0.01f)
            {
                chocaParedIzq = true;
            }

            if (Vector2.Dot(normal, Vector2.right) > 0.01f)
            {
                chocaParedDer = true;
            }
        }
    }

    // Leer cuando deja de estar en contacto con elementos
    private void OnCollisionExit2D(Collision2D colisionSalida)
    {
        if (colisionSalida.gameObject.CompareTag("Pared"))
        {
            chocaParedIzq = false;
            chocaParedDer = false;
        }
    }
}
