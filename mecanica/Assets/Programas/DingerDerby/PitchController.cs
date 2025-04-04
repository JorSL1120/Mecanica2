using UnityEngine;

public class PitchController : MonoBehaviour
{
    // En este codigo se controla el lanzamiento de las bolas, lo que hace que las bolas siempre sean diferentes entre cada tiro es el random que altera el tiempo de vuelo de las bolas.

    private float flyingtime;
    public Transform shootPoint, target;
    public GameObject ballPrefab;

    public void ThrowBall()
    {
        flyingtime = Random.Range(0.5f, 2f);
        GameObject ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        Vector3 g = Physics.gravity;
        Vector3 hitVelocity = (target.position - shootPoint.position) / flyingtime - 0.5f * g * flyingtime;
        ball.GetComponent<Rigidbody>().linearVelocity = hitVelocity;
        Destroy(ball, 10f);
    }
}
