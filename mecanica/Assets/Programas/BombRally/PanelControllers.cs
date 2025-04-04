using UnityEngine;

public class PanelControllers : MonoBehaviour
{

    // En este codigo controlamos el panel del jugador que perdio

    public GameObject Panel;

    private void Start()
    {
        Panel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            Panel.SetActive(true);
        }
    }
}
