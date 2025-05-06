using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{
    public GameObject PanelInicio;
    public string NameReinicio;

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
        SceneManager.LoadScene(NameReinicio);
    }

    public void BotMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BotSalir()
    {
        Application.Quit();
    }

    public void BotTanques()
    {
        SceneManager.LoadScene("Colision");
    }

    public void BotMontana()
    {
        SceneManager.LoadScene("Montana");
    }

    public void BotPlaneta()
    {
        SceneManager.LoadScene("Planeta");
    }
}
