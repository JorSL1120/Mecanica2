using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

using Unity.Burst.CompilerServices;
using UnityEngine.Windows;

public class BallControl : MonoBehaviour
{
    public float forceMagnitude;
    public float drag;
    public float angularDrag;
    private Rigidbody rb;
    private Vector3 direction;

    private PlayerInput playerInput;
    private InputAction wasd;
    public string NameMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerInput = GetComponent<PlayerInput>();
        wasd = playerInput.actions.FindAction(NameMovement);
    }

    void Update()
    {
        Vector2 inputWasd = InputWASD();
        direction = new Vector3(inputWasd.x, 0, inputWasd.y).normalized;
    }

    void FixedUpdate()
    {
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        Vector3 force = forceMagnitude * direction;
        Vector3 torque = Vector3.Cross(Vector3.up, force);
        rb.AddTorque(torque, ForceMode.Force);
    }

    Vector2 InputWASD()
    {
        return wasd.ReadValue<Vector2>();
    }
}
