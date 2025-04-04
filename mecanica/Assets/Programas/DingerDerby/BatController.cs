using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatController : MonoBehaviour
{
    // Aqui controlamos el bateo de ambos jugadores, gracias a la variable "Key" podemos asignar la tecla que presiona desde el inspector.

    public Key swingKey;

    public TextMeshProUGUI TextoScore;
    private int Score = 0;

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

            Score++;
            TextoScore.text = "Score: " + Score;
        }
    }
}
