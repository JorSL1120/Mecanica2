using UnityEngine;

public class BounceBehaviour3D : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    private Vector3 direction, bounceVelocity;

    void Start()
    {
        StartMovement();
    }

    void Update()
    {
        direction = rb.linearVelocity.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Barrier"))
        {
            // Get Geometrical Info
            ContactPoint contact = collision.GetContact(0);
            Vector3 normal = (contact.normal).normalized;
            Vector3 velocity = speed * direction;
            Vector3 tangent = velocity - Vector3.Dot(normal, velocity) * normal;
            tangent.Normalize();
            // Get bounce Velocity
            float Vt = Vector3.Dot(tangent, velocity);
            float Vn = Vector3.Dot(normal, velocity);
            bounceVelocity = Vt * tangent - Vn * normal;
            rb.linearVelocity = bounceVelocity;
        }
    }

    void StartMovement()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 randomVector = Random.onUnitSphere;
        rb.linearVelocity = speed * randomVector;
    }
}
