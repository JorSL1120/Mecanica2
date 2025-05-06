using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tanque : MonoBehaviour
{
    public float velocidad = 5f;
    public string IzqDer, ArrAbj;
    private Rigidbody2D rb;
    private Vector2 movimiento;
    public string Bala;
    public string BalaOtra;


    public GameObject prefabBala;
    public Transform puntoDisparo;
    public KeyCode teclaDisparo;
    public float fuerzaDisparo = 10f;


    public int Vida = 3;
    public GameObject Vida1;
    public GameObject Vida2;
    public GameObject Vida3;
    public GameObject PanelGanador;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vida1.SetActive(true);
        Vida2.SetActive(true);
        Vida3.SetActive(true);
        PanelGanador.SetActive(false);
    }

    void Update()
    {
        movimiento = new Vector2(Input.GetAxisRaw(IzqDer), Input.GetAxisRaw(ArrAbj)).normalized;

        if (movimiento.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(-movimiento.y, -movimiento.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle - 90f);
        }

        if (Input.GetKeyDown(teclaDisparo))
        {
            Disparar();
        }

        VidaPlayer();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Este detecta si es bala de él mismo
        if (collision.collider.CompareTag(Bala))
        {
            Destroy(collision.gameObject);
        }

        // Este detecta la bala del otro tanque
        if (collision.collider.CompareTag(BalaOtra))
        {
            Vida--;
        }
    }

    void Disparar()
    {
        GameObject bala = Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
        V2_BounceBehaviour2D scriptBala = bala.GetComponent<V2_BounceBehaviour2D>();
        if (scriptBala != null)
        {
            scriptBala.SetDirection(puntoDisparo.up);
        }
    }

    void VidaPlayer()
    {
        if(Vida == 2)
        {
            Vida1.SetActive(false);
        }
        else if (Vida == 1)
        {
            Vida2.SetActive(false);
        }
        else if(Vida == 0)
        {
            Vida3.SetActive(false);
            PanelGanador.SetActive(true);
            Destroy(rb.gameObject);
        }
    }
}
