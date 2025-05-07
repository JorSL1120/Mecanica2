using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI killCounterText;
    private int kills = 0;

    public static EnemyCounter instance;

    public GameObject PanelWin;
    public GameObject PanelLose;
    public int killsToWin = 5;
    private bool isFinished = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PanelWin.SetActive(false);
        PanelLose.SetActive(false);
    }

    public void AddKill()
    {
        if (isFinished) return;

        kills++;
        UpdateUI();
    }

    void UpdateUI()
    {
        killCounterText.text = "Kills: " + kills;
    }

    public void EndGame()
    {
        isFinished = true;

        if (kills >= killsToWin)
        {
            PanelWin.SetActive(true);
        }
        else
        {
            PanelLose.SetActive(true);
        }
    }
}
