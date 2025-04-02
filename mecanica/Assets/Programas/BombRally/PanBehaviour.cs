using UnityEngine;

public class PanBehaviour : MonoBehaviour
{
    public Transform Target1, Target2;
    public KeyCode SendLeft, SendRight;
    public float flyingTime;

    private Transform TargetSelected;

    void Update()
    {
        if(Input.GetKeyDown(SendLeft))
        {
            GetComponent<Animator>().SetTrigger("Move");
            TargetSelected = Target1;
        }
        else if (Input.GetKeyDown(SendRight))
        {
            GetComponent<Animator>().SetTrigger("Move");
            TargetSelected = Target2;
        }
    }

    void EnableCollider()
    {
        bool isEnabled = GetComponent<BoxCollider>().enabled;
        GetComponent<BoxCollider>().enabled = !isEnabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bomb"))
        {
            Vector3 P0 = other.transform.position;
            Vector3 Pf = TargetSelected.position;
            Vector3 g = Physics.gravity;
            float T = flyingTime;
            Vector3 hitVelocity = (Pf - P0) / T - 0.5f * g * T;

            Vector3 randomTorque = 100f * Random.onUnitSphere;
            other.GetComponent<Rigidbody>().linearVelocity = hitVelocity;
            other.GetComponent<Rigidbody>().AddTorque(randomTorque, ForceMode.Impulse);
        }
    }
}
