using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public Camera player1Camera;
    public Camera player2Camera;

    void Start()
    {
        // Verificar que las cámaras están asignadas
        if (player1Camera == null || player2Camera == null)
        {
            Debug.LogError("Cámaras no asignadas en el SplitScreenManager");
            return;
        }

        // Configura la pantalla dividida en vertical
        player1Camera.rect = new Rect(0, 0, 0.5f, 1); // Izquierda
        player2Camera.rect = new Rect(0.5f, 0, 0.5f, 1); // Derecha
    }
}
