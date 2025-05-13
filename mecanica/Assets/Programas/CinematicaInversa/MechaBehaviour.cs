using UnityEngine;

public class MechaBehaviour : MonoBehaviour
{
    public float forceMagnitude, torqueMagnitude;
    public float linearDrag, angularDrag;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    /*
    void FixedUpdate()
    {
        rb.drag = linearDrag;
        rb.angularDrag = angularDrag;
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        Vector3 torque = hInput * torqueMagnitude * transform.up;
        Vector3 force = vInput * forceMagnitude * transform.forward;

        rb.AddTorque(torque, ForceMode.Force);
        rb.AddForce(force, ForceMode.Force);
    }
    */
}
