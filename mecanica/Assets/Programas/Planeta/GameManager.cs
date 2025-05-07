using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Referencias a los paneles de fin de juego
    public GameObject player1GameOverPanel;
    public GameObject player2GameOverPanel;

    // Contador global de objetos recolectados
    public static int totalObjectsCollected = 0;

    // Límite de objetos para terminar el juego
    public const int totalObjectsToCollect = 31;

    // Referencias a los scripts de los jugadores
    public SpherePlayer player1;
    public SpherePlayer player2;

    // Temporizador
    public TextMeshProUGUI timerText;
    private float timeRemaining = 60f;
    private bool timerRunning = true;

    void Start()
    {
        // Aseguramos que los paneles estén desactivados al inicio
        if (player1GameOverPanel != null)
        {
            player1GameOverPanel.SetActive(false);
        }
        if (player2GameOverPanel != null)
        {
            player2GameOverPanel.SetActive(false);
        }

        totalObjectsCollected = 0;
    }

    void Update()
    {
        // Actualiza el temporizador solo si el juego no ha terminado y el tiempo sigue corriendo
        if (timerRunning && totalObjectsCollected < totalObjectsToCollect)
        {
            timeRemaining -= Time.deltaTime;

            // Actualiza el texto del temporizador
            UpdateTimerText();

            // Si el tiempo se acaba, termina el juego
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                EndGame();
            }
        }

        // Verificamos si se alcanzaron los 30 objetos
        if (totalObjectsCollected >= totalObjectsToCollect)
        {
            EndGame();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Tiempo: {minutes:D2}:{seconds:D2}";
    }

    public void ObjectCollected()
    {
        totalObjectsCollected++;

        // Verifica si se alcanzaron los 30 objetos y termina el juego
        if (totalObjectsCollected >= totalObjectsToCollect)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        // Desactiva el control de los jugadores
        if (player1 != null)
            player1.enabled = false;
        if (player2 != null)
            player2.enabled = false;

        // Determina quién ha ganado (puedes hacerlo de varias maneras, aquí utilizamos la variable de nombre del jugador)
        if (player1 != null && player1.paintCount > player2.paintCount)
        {
            // Si el Player1 tiene más objetos recolectados, activa su panel
            if (player1GameOverPanel != null)
            {
                player1GameOverPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else if (player2 != null && player2.paintCount > player1.paintCount)
        {
            // Si el Player2 tiene más objetos recolectados, activa su panel
            if (player2GameOverPanel != null)
            {
                player2GameOverPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
