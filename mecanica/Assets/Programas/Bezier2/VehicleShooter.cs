using System.Collections;
using UnityEngine;

public class VehicleShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float baseSpeed = 20f;
    public float speedMultiplier = 2f;

    private Vector3 lastPosition;  // Para calcular la velocidad manualmente

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        // Actualiza la última posición para el siguiente frame
        lastPosition = transform.position;
    }

    void Shoot()
    {
        // Calcula velocidad manual
        Vector3 delta = transform.position - lastPosition;
        float estimatedSpeed = delta.magnitude / Time.deltaTime;

        float projectileSpeed = baseSpeed + (estimatedSpeed * speedMultiplier);

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.forward * projectileSpeed;
        }

        Destroy(projectile, 4f);
    }
}
