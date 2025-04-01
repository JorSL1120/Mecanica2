using UnityEngine;

public class BounceBehaviour2D : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 direction, bounceVelocity;

    void Start()
    {
        StartMovement();
    }

    void Update()
    {
        direction = rb.linearVelocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Barrier2D"))
        {
            //Get geometrical info
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = (contact.normal).normalized;
            Vector2 tangent = RotateVector(normal, -90f);

            //Get bounve Velocity
            Vector2 velocity = speed * direction;
            float Vx = Vector2.Dot(tangent, velocity);
            float Vy = Vector2.Dot(normal, velocity);
            bounceVelocity = Vx * tangent - Vy * normal;
            rb.linearVelocity = bounceVelocity;
        }
    }

    Vector2 RotateVector(Vector2 vector, float angle)
    {
        angle *= Mathf.Deg2Rad;
        float Vx = vector.x;
        float Vy = vector.y;
        float newVx = Vx * Mathf.Cos(angle) - Vy * Mathf.Sin(angle);
        float newVy = Vx * Mathf.Sin(angle) + Vy * Mathf.Cos(angle);
        return new Vector2(newVx, newVy);
    }

    void StartMovement()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomVector = Random.onUnitSphere;
        rb.linearVelocity = speed * (randomVector.normalized);
    }
}
