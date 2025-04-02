using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public GameObject LoseBoth;

    public float InitialTime = 120f;
    private float RestTime;
    public TextMeshProUGUI Count;


    void Start()
    {
        Time.timeScale = 1;
        LoseBoth.SetActive(false);

        RestTime = InitialTime;
        if (Count != null)
        {
            Count.text = ActTime(RestTime);
        }
    }

    void Update()
    {
        if (RestTime > 0f)
        {
            RestTime -= Time.deltaTime;
            if (Count != null)
            {
                Count.text = ActTime(RestTime);
            }
        }
        else
        {
            if (Count != null)
            {
                Count.text = "0:00";
            }
            LoseBoth.SetActive(true);
            Time.timeScale = 0;
        }
    }

    string ActTime(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void Reiniciar(string nombreDeEscena)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nombreDeEscena);
    }
}
