using TMPro;
using UnityEngine;

public class SpherePlayer : MonoBehaviour
{
    public float speed, rotSpeed;
    public Transform sphere;
    private float sphereRadius;

    public string NameMovimineto;

    public int paintCount = 0;
    public TextMeshProUGUI playerCounterText;

    public GameManager gameManager;

    void Start()
    {
        paintCount = 0;

        // Calcula el radio de la esfera
        sphereRadius = sphere.localScale.x / 2f;

        // Coloca al jugador en el polo norte o sur dependiendo de su tag
        Vector3 upDirection = Vector3.up;

        if (CompareTag("Player1"))
        {
            // Polo norte
            transform.position = sphere.position + upDirection * sphereRadius;
        }
        else if (CompareTag("Player2"))
        {
            // Polo sur (invertido)
            transform.position = sphere.position - upDirection * sphereRadius;
        }

        // Actualiza el contador inicial
        UpdateCounter();
    }

    void Update()
    {
        // Actualiza el radio de la esfera por si se modifica el tamaño de esta durante la ejecucion
        sphereRadius = sphere.localScale.x / 2f;

        SurfaceMovement();
    }

    void SurfaceMovement()
    {
        // Controla el giro izquierda/derecha del jugador
        float dt = Time.deltaTime;
        float hInput = Input.GetAxis(NameMovimineto);
        transform.Rotate(transform.up, rotSpeed * hInput * dt, Space.World);

        // Aqui mero esta la magia
        Vector3 newPosition = transform.position + transform.forward * dt * speed;
        Vector3 newUp = NormalToSurface(newPosition);
        float dotProduct = Vector3.Dot(transform.forward, newUp);
        Vector3 newForward = (transform.forward - dotProduct * newUp).normalized;
        // Constrye una nueva rotacion (orientacion) con los vectores newForward y NewUp. Y asignasela al player
        transform.rotation = Quaternion.LookRotation(newForward, newUp);
        // Un cuaternion es un objeto geometrico 4D con el que represntamos rotaciones.
        
        transform.position = sphere.position + sphereRadius * NormalToSurface(newPosition);
    }

    Vector3 NormalToSurface(Vector3 position)
    {
        Vector3 result = (position - sphere.position).normalized;
        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Collectable"))
        {
            // Obtén el componente Collectable del objeto
            Collectable collectable = other.GetComponent<Collectable>();

            // Si ya fue recogido, no hacer nada
            if (collectable != null && collectable.isCollected)
                return;

            // Marca el objeto como recogido
            collectable.isCollected = true;

            // Obtén el componente Renderer para cambiar el color
            Renderer objRenderer = other.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                // Cambia el color según el jugador que lo tocó
                if (CompareTag("Player1"))
                {
                    objRenderer.material.color = Color.green;
                }
                else if (CompareTag("Player2"))
                {
                    objRenderer.material.color = Color.red;
                }

                // Aumenta el contador y actualiza el texto
                paintCount++;

                // Llama al GameManager para aumentar el contador global
                if (gameManager != null)
                {
                    gameManager.ObjectCollected();
                }

                UpdateCounter();
            }
        }
    }

    void UpdateCounter()
    {
        if (playerCounterText != null)
        {
            playerCounterText.text = $"{(CompareTag("Player1") ? "Player 1" : "Player 2")}: {paintCount}";
        }
    }
}
