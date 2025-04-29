using UnityEngine;

public class SpherePlayer : MonoBehaviour
{
    public float speed, rotSpeed;
    public Transform sphere;
    private float sphereRadius;

    void Start()
    {
        // Una esfera de escala 1 tiene un radio de 1/2 = 0.5
        sphereRadius = sphere.localScale.x / 2f;
        // Pocisiones al jugador en el polo norte de la esfera
        transform.position = sphere.position + new Vector3(0, sphereRadius, 0);
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
        float hInput = Input.GetAxis("Horizontal");
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
}
