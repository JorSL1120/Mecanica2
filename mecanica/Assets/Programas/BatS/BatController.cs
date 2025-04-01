using UnityEngine;
using UnityEngine.InputSystem;

public class BatController : MonoBehaviour
{
    public Key swingKey;

    private void Update()
    {
        if (Keyboard.current[swingKey].wasPressedThisFrame)
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
    }

    public void EnableDisableCollider()
    {
        bool state  = GetComponent<BoxCollider>().enabled;
        GetComponent<BoxCollider>().enabled = !state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            Vector3 velocity = new Vector3(0, 10, 10);
            other.gameObject.GetComponent<Rigidbody>().linearVelocity = velocity;
            other.gameObject.GetComponent<TrailRenderer>().emitting = true;
            Debug.Log("Contact");
        }
    }
}
