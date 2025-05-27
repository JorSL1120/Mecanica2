using UnityEngine;

public class OWSpaceShip : MonoBehaviour
{
    public float torqueMagnitude;
    [Range(0f, 5f)] public float angularDamping;

    public float forceMagnitude;
    [Range(0f, 5f)] public float linearDamping;

    private Rigidbody rb;
    private Vector2 inputRS, inputLS;
    private float valueRS, valueLS;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detecta el input del usuario
        inputRS = Get_RightStick_Input();
        inputLS = Get_LeftStick_Input();
        valueRS = Get_RightTrigger_Input();
        valueLS = Get_LeftTrigger_Input();
    }

    void FixedUpdate()
    {
        ModifyRBParameters();
        ApplyTorque();
        ApplyForce();
    }

    void ApplyTorque()
    {
        if (!Input.GetButton("LB"))
        {
            Vector3 normalizedTorque = (-inputRS.y * transform.right + inputRS.x * transform.up).normalized;
            Vector3 torque = torqueMagnitude * normalizedTorque;
            rb.AddTorque(torque, ForceMode.Force);
        }
        else
        {
            Vector3 normalizedTorque = -inputRS.x * transform.forward;
            Vector3 torque = torqueMagnitude * normalizedTorque;
            rb.AddTorque(torque, ForceMode.Force);
        }
    }

    void ApplyForce()
    {
        Vector3 leftRightDirecion = inputLS.x * transform.right;
        Vector3 frontRearDirection = inputLS.y * transform.forward;
        Vector3 upDownDirection = (valueRS - valueLS) * transform.up;
        Vector3 force = forceMagnitude * (leftRightDirecion + frontRearDirection +
        upDownDirection).normalized;
        rb.AddForce(force, ForceMode.Force);
    }

    void ModifyRBParameters()
    {
        rb.angularDamping = angularDamping;
        rb.linearDamping = linearDamping;
    }

    Vector2 Get_LeftStick_Input()
    {
        float x = Input.GetAxis("Horizontal-LS");
        float y = Input.GetAxis("Vertical-LS");
        return new Vector2(x, y);
    }

    Vector2 Get_RightStick_Input()
    {
        float x = Input.GetAxis("Horizontal-RS");
        float y = Input.GetAxis("Vertical-RS");
        return new Vector2(x, y);
    }

    float Get_LeftTrigger_Input()
    {
        float x = Input.GetAxis("LT");
        return x;
    }

    float Get_RightTrigger_Input()
    {
        float x = Input.GetAxis("RT");
        return x;
    }
}
