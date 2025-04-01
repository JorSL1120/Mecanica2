using UnityEngine;

public class PitchController : MonoBehaviour
{
    private float flyingtime;
    public Transform shootPoint, target;
    public GameObject ballPrefab;

    public void ThrowBall()
    {
        flyingtime = Random.Range(0.5f, 3f);
        GameObject ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        Vector3 g = Physics.gravity;
        Vector3 hitVelocity = (target.position - shootPoint.position) / flyingtime - 0.5f * g * flyingtime;
        ball.GetComponent<Rigidbody>().linearVelocity = hitVelocity;
        Destroy(ball, 10f);
    }
}
