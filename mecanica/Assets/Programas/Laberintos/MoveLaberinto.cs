using UnityEngine;
using UnityEngine.InputSystem;

public class MoveLaberinto : MonoBehaviour
{
    public float forceMagnitude = 10f;
    public float drag = 0.1f;
    public float angularDrag = 0.1f;

    private Rigidbody rb;
    private Vector3 direction;

    private PlayerInput playerInput;
    private InputAction wslr;
    public string NameMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();  // Agrega Rigidbody si no existe
        }

        rb.useGravity = false; // Desactiva la gravedad
        rb.isKinematic = false; // Asegura que el objeto sea afectado por la f�sica

        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;

        playerInput = GetComponent<PlayerInput>();
        wslr = playerInput.actions.FindAction(NameMovement);
    }

    void Update()
    {
        Vector2 inputWSLR = InputWSLR();
        direction = new Vector3(inputWSLR.x, 0, inputWSLR.y).normalized;
    }

    void FixedUpdate()
    {
        Vector3 force = forceMagnitude * direction;
        Vector3 torque = Vector3.Cross(Vector3.up, force);
        rb.AddTorque(torque, ForceMode.Force);
    }

    Vector2 InputWSLR()
    {
        return wslr.ReadValue<Vector2>();
    }
}
