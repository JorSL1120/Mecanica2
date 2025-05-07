using UnityEngine;

public class FirstPersonRailCamera : MonoBehaviour
{
    public Transform cameraTransform;  // La cámara real (hija del CameraPivot)
    public float mouseSensitivity = 3f;
    public float verticalClamp = 80f;

    public GameObject startPanel;
    public GameObject WinPanel;
    public GameObject LosePanel;

    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Si el panel está activo, no mover la cámara
        if (startPanel != null && startPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else if(WinPanel != null && WinPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else if (LosePanel != null && LosePanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotación vertical (arriba y abajo)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalClamp, verticalClamp);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Rotación horizontal del carrito (gira el objeto que contiene este script)
        transform.Rotate(Vector3.up * mouseX);
    }
}
