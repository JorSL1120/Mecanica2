using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bala1"))
        {
            EnemyCounter.instance.AddKill();
            Destroy(collision.gameObject); // Destruye la bala
            Destroy(gameObject);           // Destruye el enemigo
        }
    }
}
