using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public GameObject PanelInicio;

    void Start()
    {
        PanelInicio.SetActive(true);
        Time.timeScale = 0f;
    }

    public void BotInicio()
    {
        PanelInicio.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BotReinicio()
    {
        SceneManager.LoadScene("Colisiones");
    }

    public void BotSalir()
    {
        Application.Quit();
    }
}
